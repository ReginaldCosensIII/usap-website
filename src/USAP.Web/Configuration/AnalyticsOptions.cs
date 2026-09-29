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
}
