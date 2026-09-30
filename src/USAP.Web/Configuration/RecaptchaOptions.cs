namespace USAP.Web.Configuration;

/// <summary>
/// Google Cloud reCAPTCHA score-based enterprise configuration options.
/// ApiKey is SECRET and must never be committed to source control.
/// </summary>
public class RecaptchaOptions
{
    public const string SectionName = "Recaptcha";

    public bool Enabled { get; set; } = false;
    public string ProjectId { get; set; } = string.Empty;
    public string SiteKey { get; set; } = string.Empty;
    public string? ApiKey { get; set; }
    public float MinimumScore { get; set; } = 0.5f;

    /// <summary>
    /// Sole authoritative expected hostname for reCAPTCHA token verification.
    /// Required when reCAPTCHA is enabled. Must be a bare hostname (e.g. "localhost" or "www.usantennaproducts.com").
    /// </summary>
    public string ExpectedHostname { get; set; } = string.Empty;

    public bool IsValid(out string? error)
    {
        if (!Enabled)
        {
            error = null;
            return true;
        }

        if (string.IsNullOrWhiteSpace(ProjectId))
        {
            error = "Recaptcha:ProjectId is required when reCAPTCHA is enabled.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(SiteKey))
        {
            error = "Recaptcha:SiteKey is required when reCAPTCHA is enabled.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            error = "Recaptcha:ApiKey is required when reCAPTCHA is enabled.";
            return false;
        }

        if (MinimumScore is < 0.0f or > 1.0f)
        {
            error = "Recaptcha:MinimumScore must be between 0.0 and 1.0 inclusive.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(ExpectedHostname))
        {
            error = "Recaptcha:ExpectedHostname is required when reCAPTCHA is enabled.";
            return false;
        }

        var trimmedHost = ExpectedHostname.Trim();
        if (trimmedHost.Contains("://", StringComparison.Ordinal) ||
            trimmedHost.Contains('/', StringComparison.Ordinal) ||
            trimmedHost.Contains('\\', StringComparison.Ordinal) ||
            trimmedHost.Contains(':', StringComparison.Ordinal) ||
            trimmedHost.Contains('?', StringComparison.Ordinal) ||
            trimmedHost.Contains('#', StringComparison.Ordinal) ||
            Uri.CheckHostName(trimmedHost) == UriHostNameType.Unknown)
        {
            error = "Recaptcha:ExpectedHostname must be a valid bare hostname without scheme, port, path, query, or fragment.";
            return false;
        }

        error = null;
        return true;
    }
}
