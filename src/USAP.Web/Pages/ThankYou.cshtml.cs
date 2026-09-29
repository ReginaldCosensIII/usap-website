using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace USAP.Web.Pages;

public class ThankYouModel : PageModel
{
    [TempData]
    public string? ReferenceNumber { get; set; }

    [TempData]
    public string? ConfirmationMessage { get; set; }

    public IActionResult OnGet()
    {
        return RedirectToPage("/ContactUs/ThankYou");
    }
}
