namespace USAP.Web.Models.Catalog;

/// <summary>
/// Represents a canonical public-facing technical document record.
/// Contains display data, classification, and associations with zero internal approval or conflict tokens.
/// </summary>
public record ProductResourceRecord(
    string Id,
    string Title,
    string LocalPdfPath,
    string FamilySlug,
    string FamilyName,
    IReadOnlyList<string> ProductGroupIds,
    IReadOnlyList<string> ModelCodes,
    string DocumentType,
    string Description,
    int PageCount,
    int SortOrder,
    IReadOnlyList<string> LegacySourceUrls,
    string SearchText
)
{
    /// <summary>
    /// URL-friendly stable slug derived from the local canonical filename without extension.
    /// </summary>
    public string Slug => System.IO.Path.GetFileNameWithoutExtension(LocalPdfPath);

    /// <summary>
    /// Preferred branded HTML document-detail route.
    /// </summary>
    public string DetailUrl => $"/technical-resources/document/{Slug}";

    /// <summary>
    /// Backwards-compatible convenience accessor for single primary product group.
    /// </summary>
    public string ProductGroupId => ProductGroupIds.Count > 0 ? ProductGroupIds[0] : string.Empty;

    /// <summary>
    /// Backwards-compatible alias for DocumentType.
    /// </summary>
    public string ResourceType => DocumentType;

    /// <summary>
    /// Backwards-compatible alias for LocalPdfPath.
    /// </summary>
    public string CurrentSourceUrl => LocalPdfPath;
}
