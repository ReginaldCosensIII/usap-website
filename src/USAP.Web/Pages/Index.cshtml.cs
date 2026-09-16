using Microsoft.AspNetCore.Mvc.RazorPages;
using USAP.Web.Models.Catalog;
using USAP.Web.Services.Catalog;

namespace USAP.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IProductCatalogService _catalogService;

    public IndexModel(IProductCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public IReadOnlyList<ProductFamilyRecord> FeaturedFamilies { get; private set; } = Array.Empty<ProductFamilyRecord>();

    public void OnGet()
    {
        FeaturedFamilies = _catalogService.GetFeaturedFamilies();
    }
}
