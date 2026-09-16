namespace USAP.Web.Models.Catalog;

public record CatalogAsset(
    string ImagePath,
    string AltText,
    bool IsPlaceholder = false,
    string? BadgeText = null,
    string? Caption = null
);
