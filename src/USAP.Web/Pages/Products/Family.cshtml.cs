using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace USAP.Web.Pages.Products;

public class FamilyModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string FamilySlug { get; set; } = string.Empty;

    public void OnGet(string familySlug)
    {
        FamilySlug = familySlug;
    }
}
