namespace USAP.Web.Models.Catalog;

/// <summary>
/// Represents a compact, public-display product or configuration model record.
/// Contains only approved display data with zero internal governance or hold metadata.
/// </summary>
public record ProductModelRecord(
    string ModelCode,
    string? DisplayName = null,
    string? Description = null,
    IReadOnlyList<ProductCharacteristic>? Characteristics = null
);
