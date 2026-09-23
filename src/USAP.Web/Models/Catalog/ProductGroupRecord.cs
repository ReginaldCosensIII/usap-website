namespace USAP.Web.Models.Catalog;

/// <summary>
/// Represents a public-facing product group within a product family.
/// Contains only approved display data with zero internal governance or hold metadata.
/// </summary>
public record ProductGroupRecord(
    string Id,
    string FamilyId,
    string Name,
    int DisplayOrder,
    string SectionAnchor,
    string ShortDescription,
    string ExpandedIntroduction,
    IReadOnlyList<ProductModelRecord> Models,
    IReadOnlyList<ProductCharacteristic>? OverviewCharacteristics = null,
    IReadOnlyList<ProductCharacteristic>? DetailedCharacteristics = null,
    CatalogAsset? AssociatedAsset = null,
    bool HasPublishedModelNumber = true,
    string? ModelNote = null
)
{
    public IReadOnlyList<ProductCharacteristic> AllGroupCharacteristics =>
        (OverviewCharacteristics ?? Array.Empty<ProductCharacteristic>())
        .Concat(DetailedCharacteristics ?? Array.Empty<ProductCharacteristic>())
        .ToList();
}
