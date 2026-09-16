namespace USAP.Web.Models.Catalog;

public record ResponsiveHeroAsset(
    string DesktopImagePath,
    int DesktopWidth,
    int DesktopHeight,
    string MobileImagePath,
    int MobileWidth,
    int MobileHeight,
    string AltText,
    string AssetClassification,
    string ApprovalStatus
);
