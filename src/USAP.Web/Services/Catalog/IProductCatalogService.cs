namespace USAP.Web.Services.Catalog;

using USAP.Web.Models.Catalog;

public interface IProductCatalogService
{
    IReadOnlyList<ProductFamilyRecord> GetFamilies();
    ProductFamilyRecord? GetFamilyBySlug(string slug);
    ProductFamilyRecord? GetFamilyById(string id);
    IReadOnlyList<ProductGroupRecord> GetProductGroupsByFamily(string familyId);
    ProductGroupRecord? GetProductGroupById(string id);
    CatalogAsset GetLandingHeroAsset();
    IReadOnlyList<ProductFamilyRecord> GetFeaturedFamilies();
}
