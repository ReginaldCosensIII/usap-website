namespace USAP.Web.Models;

public class SiteSettings
{
    public const string SectionName = "SiteSettings";

    public string SiteName { get; set; } = string.Empty;
    public string OrganizationName { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string DefaultTitle { get; set; } = string.Empty;
    public string DefaultDescription { get; set; } = string.Empty;
    public string DefaultSocialImagePath { get; set; } = string.Empty;
    public string DefaultSocialImageAlt { get; set; } = string.Empty;
    public string Locale { get; set; } = "en_US";
    public string ThemeColor { get; set; } = "#0f203c";
    public string TitleSeparator { get; set; } = "—";
    public string TwitterCard { get; set; } = "summary_large_image";
}
