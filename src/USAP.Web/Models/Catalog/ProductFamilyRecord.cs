namespace USAP.Web.Models.Catalog;

public record ProductFamilyRecord(
    string Id,
    string Slug,
    string Name,
    int DisplayOrder,
    string CardEyebrow,
    string CardTitle,
    string CardSummary,
    CatalogAsset CardAsset,
    CatalogAsset? HeroAsset,
    string Route,
    IReadOnlyList<string> CurrentSourceUrls,
    IReadOnlyList<string> LegacySourceUrls,
    string ApprovalStatus,
    string SourceNotes,
    IReadOnlyList<string> ConflictHolds,
    IReadOnlyList<string> ProductGroupIds,
    ResponsiveHeroAsset? ResponsiveHero = null,
    string? SectionEyebrow = null,
    string? SectionHeading = null,
    string? SectionIntro = null,
    string? PageTitle = null,
    string? MetaDescription = null
)
{
    public string DisplayTitle =>
        string.IsNullOrWhiteSpace(CardTitle) ? Name : CardTitle;
}
