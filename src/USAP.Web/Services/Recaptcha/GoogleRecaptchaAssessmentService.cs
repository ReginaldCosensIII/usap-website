using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using USAP.Web.Configuration;
using USAP.Web.Models;

namespace USAP.Web.Services.Recaptcha;

/// <summary>
/// Google Cloud reCAPTCHA Enterprise / Fraud Defense v1 CreateAssessment client.
/// Evaluates score-based tokens using modern Google REST APIs with strict secret safety and fail-closed semantics.
/// </summary>
public class GoogleRecaptchaAssessmentService : IRecaptchaAssessmentService
{
    private readonly HttpClient _httpClient;
    private readonly RecaptchaOptions _options;
    private readonly ILogger<GoogleRecaptchaAssessmentService> _logger;

    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public GoogleRecaptchaAssessmentService(
        HttpClient httpClient,
        IOptions<RecaptchaOptions> options,
        ILogger<GoogleRecaptchaAssessmentService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<RecaptchaAssessmentResult> AssessAsync(
        string? token,
        string expectedAction,
        string? userIpAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled)
        {
            return RecaptchaAssessmentResult.DisabledResult();
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            _logger.LogWarning(
                "reCAPTCHA assessment failed: missing token. Action: {Action}",
                expectedAction);

            return RecaptchaAssessmentResult.Failure(
                RecaptchaFailureCategory.MissingToken,
                "Token was not provided.",
                action: expectedAction);
        }

        var endpoint = $"v1/projects/{Uri.EscapeDataString(_options.ProjectId)}/assessments";

        var requestPayload = new GoogleAssessmentRequest
        {
            Event = new GoogleAssessmentEvent
            {
                Token = token,
                SiteKey = _options.SiteKey,
                ExpectedAction = expectedAction,
                UserAgent = string.IsNullOrWhiteSpace(userAgent) ? null : userAgent,
                UserIpAddress = string.IsNullOrWhiteSpace(userIpAddress) ? null : userIpAddress
            }
        };

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(requestPayload, options: s_jsonOptions)
        };

        // Prefer standard Google REST API key header; never put API key in query string
        if (!string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            requestMessage.Headers.Add("x-goog-api-key", _options.ApiKey);
        }

        try
        {
            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Google Cloud reCAPTCHA API rejected assessment. StatusCode: {StatusCode}, Action: {Action}",
                    (int)response.StatusCode, expectedAction);

                return RecaptchaAssessmentResult.Failure(
                    RecaptchaFailureCategory.ApiRejected,
                    $"Google API returned HTTP {(int)response.StatusCode}",
                    action: expectedAction);
            }

            var responseModel = await response.Content.ReadFromJsonAsync<GoogleAssessmentResponse>(s_jsonOptions, cancellationToken);

            if (responseModel?.TokenProperties == null)
            {
                _logger.LogError(
                    "Google Cloud reCAPTCHA response was malformed or missing token properties. Action: {Action}",
                    expectedAction);

                return RecaptchaAssessmentResult.Failure(
                    RecaptchaFailureCategory.MalformedResponse,
                    "Malformed Google response.",
                    action: expectedAction);
            }

            var tokenProps = responseModel.TokenProperties;
            var returnedAction = tokenProps.Action;
            var returnedHostname = tokenProps.Hostname;
            var assessmentName = responseModel.Name;

            // Gate 1: Token Validity
            if (!tokenProps.Valid)
            {
                _logger.LogWarning(
                    "reCAPTCHA token invalid. Reason: {InvalidReason}, Action: {Action}, Hostname: {Hostname}",
                    tokenProps.InvalidReason, returnedAction ?? expectedAction, returnedHostname);

                return RecaptchaAssessmentResult.Failure(
                    RecaptchaFailureCategory.InvalidToken,
                    $"Token invalid: {tokenProps.InvalidReason}",
                    action: returnedAction ?? expectedAction,
                    hostname: returnedHostname,
                    assessmentName: assessmentName);
            }

            // Gate 2: Action Verification
            if (!string.Equals(returnedAction, expectedAction, StringComparison.Ordinal))
            {
                _logger.LogWarning(
                    "reCAPTCHA action mismatch. Expected: {ExpectedAction}, Received: {ReceivedAction}",
                    expectedAction, returnedAction);

                return RecaptchaAssessmentResult.Failure(
                    RecaptchaFailureCategory.ActionMismatch,
                    $"Action mismatch: expected '{expectedAction}', got '{returnedAction}'",
                    action: returnedAction,
                    hostname: returnedHostname,
                    assessmentName: assessmentName);
            }

            // Gate 3: Hostname Verification (RecaptchaOptions.ExpectedHostname is the sole authoritative expected hostname)
            var normConfigured = NormalizeHostname(_options.ExpectedHostname);
            var normReturned = NormalizeHostname(returnedHostname);

            var hostnameMatches = !string.IsNullOrEmpty(normConfigured) &&
                                  string.Equals(normReturned, normConfigured, StringComparison.OrdinalIgnoreCase);

            if (!hostnameMatches)
            {
                _logger.LogWarning(
                    "reCAPTCHA hostname mismatch. Configured Expected: {ExpectedHostname}, Received: {ReceivedHostname}",
                    _options.ExpectedHostname, returnedHostname);

                return RecaptchaAssessmentResult.Failure(
                    RecaptchaFailureCategory.HostnameMismatch,
                    $"Hostname mismatch: expected '{_options.ExpectedHostname}', got '{returnedHostname}'",
                    action: returnedAction,
                    hostname: returnedHostname,
                    assessmentName: assessmentName);
            }

            // Gate 4: Score Verification
            if (responseModel.RiskAnalysis?.Score == null)
            {
                _logger.LogError(
                    "reCAPTCHA risk score missing in response. Action: {Action}",
                    expectedAction);

                return RecaptchaAssessmentResult.Failure(
                    RecaptchaFailureCategory.MalformedResponse,
                    "Risk score missing in response.",
                    action: returnedAction,
                    hostname: returnedHostname,
                    assessmentName: assessmentName);
            }

            var score = responseModel.RiskAnalysis.Score.Value;
            var reasons = responseModel.RiskAnalysis.Reasons;

            if (score < _options.MinimumScore)
            {
                _logger.LogWarning(
                    "reCAPTCHA score below minimum threshold. Score: {Score:F2}, Minimum: {MinimumScore:F2}, Action: {Action}, Hostname: {Hostname}",
                    score, _options.MinimumScore, returnedAction, returnedHostname);

                return RecaptchaAssessmentResult.Failure(
                    RecaptchaFailureCategory.BelowThreshold,
                    $"Score {score:F2} below threshold {_options.MinimumScore:F2}",
                    score: score,
                    action: returnedAction,
                    hostname: returnedHostname,
                    assessmentName: assessmentName,
                    reasons: reasons);
            }

            // Gate 5: All checks passed
            _logger.LogInformation(
                "reCAPTCHA assessment accepted. Score: {Score:F2}, Action: {Action}, Hostname: {Hostname}",
                score, returnedAction, returnedHostname);

            return RecaptchaAssessmentResult.Accepted(
                score,
                returnedAction,
                returnedHostname,
                assessmentName,
                reasons);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(
                "Google Cloud reCAPTCHA transport error. Stage: {Stage}, ErrorCategory: {ErrorCategory}, Action: {Action}",
                "HttpRequest", ex.GetType().Name, expectedAction);

            return RecaptchaAssessmentResult.Failure(
                RecaptchaFailureCategory.ServiceUnavailable,
                "reCAPTCHA service request failed.",
                action: expectedAction);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(
                "Google Cloud reCAPTCHA request timed out. Stage: {Stage}, ErrorCategory: {ErrorCategory}, Action: {Action}",
                "Timeout", ex.GetType().Name, expectedAction);

            return RecaptchaAssessmentResult.Failure(
                RecaptchaFailureCategory.ServiceUnavailable,
                "reCAPTCHA service timed out.",
                action: expectedAction);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(
                "Google Cloud reCAPTCHA JSON deserialization error. Stage: {Stage}, ErrorCategory: {ErrorCategory}, Action: {Action}",
                "JsonParsing", ex.GetType().Name, expectedAction);

            return RecaptchaAssessmentResult.Failure(
                RecaptchaFailureCategory.MalformedResponse,
                "reCAPTCHA response parsing error.",
                action: expectedAction);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Google Cloud reCAPTCHA unexpected assessment error. Stage: {Stage}, ErrorCategory: {ErrorCategory}, Action: {Action}",
                "Unexpected", ex.GetType().Name, expectedAction);

            return RecaptchaAssessmentResult.Failure(
                RecaptchaFailureCategory.ServiceUnavailable,
                "Unexpected reCAPTCHA error.",
                action: expectedAction);
        }
    }

    private static string NormalizeHostname(string? host)
    {
        if (string.IsNullOrWhiteSpace(host))
        {
            return string.Empty;
        }

        var trimmed = host.Trim();
        var colonIdx = trimmed.IndexOf(':');
        if (colonIdx >= 0)
        {
            trimmed = trimmed[..colonIdx];
        }

        return trimmed.ToLowerInvariant();
    }

    // Google Cloud v1 CreateAssessment JSON Contracts
    internal sealed class GoogleAssessmentRequest
    {
        [JsonPropertyName("event")]
        public GoogleAssessmentEvent Event { get; set; } = new();
    }

    internal sealed class GoogleAssessmentEvent
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("siteKey")]
        public string SiteKey { get; set; } = string.Empty;

        [JsonPropertyName("expectedAction")]
        public string ExpectedAction { get; set; } = string.Empty;

        [JsonPropertyName("userAgent")]
        public string? UserAgent { get; set; }

        [JsonPropertyName("userIpAddress")]
        public string? UserIpAddress { get; set; }
    }

    internal sealed class GoogleAssessmentResponse
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("event")]
        public GoogleAssessmentEvent? Event { get; set; }

        [JsonPropertyName("riskAnalysis")]
        public GoogleRiskAnalysis? RiskAnalysis { get; set; }

        [JsonPropertyName("tokenProperties")]
        public GoogleTokenProperties? TokenProperties { get; set; }
    }

    internal sealed class GoogleRiskAnalysis
    {
        [JsonPropertyName("score")]
        public float? Score { get; set; }

        [JsonPropertyName("reasons")]
        public List<string>? Reasons { get; set; }
    }

    internal sealed class GoogleTokenProperties
    {
        [JsonPropertyName("valid")]
        public bool Valid { get; set; }

        [JsonPropertyName("invalidReason")]
        public string? InvalidReason { get; set; }

        [JsonPropertyName("hostname")]
        public string? Hostname { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; }

        [JsonPropertyName("createTime")]
        public string? CreateTime { get; set; }
    }
}
