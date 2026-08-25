using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using USAP.Web.Models;
using USAP.Web.Services;

namespace USAP.Web.Pages;

[EnableRateLimiting("InquirySubmission")]
public class RequestAQuoteModel : InquiryPageModelBase
{
    public RequestAQuoteModel(IInquirySubmissionService submissionService)
        : base(submissionService)
    {
    }

    public void OnGet()
    {
        Input.Type = InquiryType.RequestAQuote;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Enforce the type for this page
        Input.Type = InquiryType.RequestAQuote;

        return await ProcessSubmissionAsync("An error occurred submitting your quote request.", HttpContext.RequestAborted);
    }
}
