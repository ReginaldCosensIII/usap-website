namespace USAP.Web.Services;

/// <summary>
/// Generates canonical public reference numbers for inquiries and quote requests.
/// Ensures identical visible shape and entropy across genuine and honeypot submissions.
/// </summary>
public static class InquiryReferenceGenerator
{
    public const string Prefix = "REQ";

    public static string Generate()
    {
        return $"{Prefix}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";
    }
}
