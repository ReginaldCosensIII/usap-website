namespace USAP.Web.Models;

public enum RecaptchaFailureCategory
{
    None = 0,
    MissingToken,
    InvalidToken,
    ActionMismatch,
    HostnameMismatch,
    BelowThreshold,
    ApiRejected,
    ServiceUnavailable,
    MalformedResponse
}

/// <summary>
/// Result of a Google Cloud reCAPTCHA assessment verification.
/// Contains internal diagnostic metadata; sensitive visitor PII, tokens, and API keys are never stored here.
/// </summary>
public sealed class RecaptchaAssessmentResult
{
    public bool IsAccepted { get; init; }
    public float? Score { get; init; }
    public string? Action { get; init; }
    public string? Hostname { get; init; }
    public RecaptchaFailureCategory FailureCategory { get; init; }
    public string? AssessmentName { get; init; }
    public IReadOnlyList<string>? Reasons { get; init; }
    public string? DiagnosticMessage { get; init; }

    public static RecaptchaAssessmentResult Accepted(
        float score,
        string? action,
        string? hostname,
        string? assessmentName,
        IReadOnlyList<string>? reasons = null)
    {
        return new RecaptchaAssessmentResult
        {
            IsAccepted = true,
            Score = score,
            Action = action,
            Hostname = hostname,
            FailureCategory = RecaptchaFailureCategory.None,
            AssessmentName = assessmentName,
            Reasons = reasons
        };
    }

    public static RecaptchaAssessmentResult Failure(
        RecaptchaFailureCategory category,
        string? diagnosticMessage = null,
        float? score = null,
        string? action = null,
        string? hostname = null,
        string? assessmentName = null,
        IReadOnlyList<string>? reasons = null)
    {
        return new RecaptchaAssessmentResult
        {
            IsAccepted = false,
            Score = score,
            Action = action,
            Hostname = hostname,
            FailureCategory = category,
            AssessmentName = assessmentName,
            Reasons = reasons,
            DiagnosticMessage = diagnosticMessage
        };
    }

    public static RecaptchaAssessmentResult DisabledResult()
    {
        return new RecaptchaAssessmentResult
        {
            IsAccepted = true,
            FailureCategory = RecaptchaFailureCategory.None,
            DiagnosticMessage = "reCAPTCHA protection is disabled by configuration."
        };
    }
}
