namespace USAP.Web.Models;

public class SeoMetadata
{
    public const string ViewDataKey = "SeoMetadata";

    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? CanonicalPath { get; set; }
    public string? Robots { get; set; }
    public string? OpenGraphType { get; set; }
    public string? SocialImagePath { get; set; }
    public string? SocialImageAlt { get; set; }
    public bool OmitCanonical { get; set; }
}
