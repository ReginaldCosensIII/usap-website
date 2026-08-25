using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using USAP.Web.Models;
using USAP.Web.Services;

namespace USAP.Web.Pages;

public abstract class InquiryPageModelBase : PageModel
{
    protected readonly IInquirySubmissionService _submissionService;

    protected InquiryPageModelBase(IInquirySubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [BindProperty]
    public InquiryFormInput Input { get; set; } = new();

    protected async Task<IActionResult> ProcessSubmissionAsync(string defaultErrorMessage, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(Input.Website))
        {
            // Honeypot triggered
            TempData["ConfirmationMessage"] = "Your inquiry has been processed.";
            return RedirectToPage("/ThankYou");
        }

        if (Input.PreferredContactMethod == ContactMethod.Phone &&
            string.IsNullOrWhiteSpace(Input.Phone))
        {
            ModelState.TryAddModelError(
                $"{nameof(Input)}.{nameof(Input.Phone)}",
                "Phone number is required when Phone is selected as the preferred contact method.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _submissionService.SubmitAsync(Input, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["ReferenceNumber"] = result.ReferenceNumber;
            TempData["ConfirmationMessage"] = result.ConfirmationMessage;
            return RedirectToPage("/ThankYou");
        }

        ModelState.AddModelError(string.Empty, result.ErrorMessage ?? defaultErrorMessage);
        return Page();
    }
}
