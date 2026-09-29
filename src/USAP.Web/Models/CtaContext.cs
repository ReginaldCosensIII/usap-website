namespace USAP.Web.Models;

/// <summary>
/// Represents trusted, validated originating context passed from site CTAs.
/// Strictly separated from visitor-entered editable fields.
/// </summary>
public record CtaContext(
    InquiryType Intent,
    string? FamilySlug = null,
    string? FamilyName = null,
    string? GroupId = null,
    string? GroupName = null,
    string? DocumentSlug = null,
    string? DocumentTitle = null,
    string? ModelCode = null,
    string? DisplayCategory = null,
    string? DisplayTitle = null,
    string? SuggestedProductOfInterest = null,
    string? ContextSummary = null
)
{
    public bool HasContext => !string.IsNullOrWhiteSpace(DisplayTitle);
    public bool IsContextual => HasContext;

    public static CtaContext Empty(InquiryType intent = InquiryType.GeneralInquiry) =>
        new(intent);
}
