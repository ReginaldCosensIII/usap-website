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
}
