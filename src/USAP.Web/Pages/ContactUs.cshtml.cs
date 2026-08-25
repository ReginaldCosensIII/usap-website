using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using USAP.Web.Models;
using USAP.Web.Services;

namespace USAP.Web.Pages;

[EnableRateLimiting("InquirySubmission")]
public class ContactUsModel : InquiryPageModelBase
{
    public ContactUsModel(IInquirySubmissionService submissionService)
        : base(submissionService)
    {
    }

    public void OnGet(string? reason)
    {
        Input.Type = reason?.ToLowerInvariant() switch
        {
            "product-information" => InquiryType.ProductInformation,
            "engineering-support" => InquiryType.EngineeringSupport,
            "technical-documentation" => InquiryType.TechnicalDocumentation,
            "request-a-quote" => InquiryType.RequestAQuote,
            _ => InquiryType.GeneralInquiry
        };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        return await ProcessSubmissionAsync("An error occurred submitting your inquiry.", HttpContext.RequestAborted);
    }
}
