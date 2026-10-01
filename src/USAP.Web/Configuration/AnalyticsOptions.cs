namespace USAP.Web.Configuration;

/// <summary>
/// Direct Google Analytics 4 (gtag.js) configuration options.
/// MeasurementId is public and non-secret.
/// </summary>
public class AnalyticsOptions
{
    public const string SectionName = "Analytics";

    public bool Enabled { get; set; } = false;
    public string? MeasurementId { get; set; }
    public bool DebugMode { get; set; } = false;

    public bool IsValid(out string? error)
    {
        if (!Enabled)
        {
            error = null;
            return true;
        }

        if (string.IsNullOrWhiteSpace(MeasurementId))
        {
            error = "Analytics:MeasurementId is required when Analytics is enabled.";
            return false;
        }

        var trimmed = MeasurementId.Trim();
        if (trimmed.Length < 4 ||
            !trimmed.StartsWith("G-", StringComparison.OrdinalIgnoreCase) ||
            !trimmed[2..].All(char.IsLetterOrDigit))
        {
            error = "Analytics:MeasurementId must be a valid GA4 Measurement ID starting with 'G-' followed by alphanumeric characters.";
            return false;
        }

        error = null;
        return true;
    }
}
