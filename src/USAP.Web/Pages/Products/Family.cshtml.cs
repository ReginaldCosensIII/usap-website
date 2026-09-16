using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using USAP.Web.Models.Catalog;
using USAP.Web.Services.Catalog;

namespace USAP.Web.Pages.Products;

public class FamilyModel : PageModel
{
    private readonly IProductCatalogService _catalogService;

    public FamilyModel(IProductCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [BindProperty(SupportsGet = true)]
    public string FamilySlug { get; set; } = string.Empty;

    public ProductFamilyRecord? Family { get; private set; }
    public IReadOnlyList<ProductGroupRecord> ProductGroups { get; private set; } = Array.Empty<ProductGroupRecord>();

    public IActionResult OnGet(string familySlug)
    {
        FamilySlug = familySlug;
        Family = _catalogService.GetFamilyBySlug(familySlug);

        if (Family == null)
        {
            return NotFound();
        }

        ProductGroups = _catalogService.GetProductGroupsByFamily(Family.Id);
        return Page();
    }

    public IReadOnlyList<ProductResourceRecord> GetApprovedResources(string groupId)
    {
        return _catalogService.GetApprovedResourcesForGroup(groupId);
    }
}
