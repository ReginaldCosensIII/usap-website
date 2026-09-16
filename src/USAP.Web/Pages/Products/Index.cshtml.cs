using Microsoft.AspNetCore.Mvc.RazorPages;
using USAP.Web.Models.Catalog;
using USAP.Web.Services.Catalog;

namespace USAP.Web.Pages.Products;

public class IndexModel : PageModel
{
    private readonly IProductCatalogService _catalogService;

    public IndexModel(IProductCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public IReadOnlyList<ProductFamilyRecord> Families { get; private set; } = Array.Empty<ProductFamilyRecord>();
    public CatalogAsset HeroAsset { get; private set; } = null!;

    public void OnGet()
    {
        Families = _catalogService.GetFamilies();
        HeroAsset = _catalogService.GetLandingHeroAsset();
    }
}
