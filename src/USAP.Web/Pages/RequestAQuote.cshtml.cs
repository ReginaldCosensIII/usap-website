using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using USAP.Web.Models;
using USAP.Web.Services;

namespace USAP.Web.Pages;

[EnableRateLimiting("InquirySubmission")]
public class RequestAQuoteModel : InquiryPageModelBase
{
    public RequestAQuoteModel(
        IInquirySubmissionService submissionService,
        ICtaContextResolver contextResolver)
        : base(submissionService, contextResolver)
    {
    }

    public override bool IsQuoteWorkflow => true;

    public void OnGet(string? family, string? group, string? doc)
    {
        Context = _contextResolver.Resolve("request-a-quote", family, group, doc);
        Input.Type = InquiryType.RequestAQuote;

        Input.ContextReason = "request-a-quote";
        Input.ContextFamily = Context.FamilySlug;
        Input.ContextGroup = Context.GroupId;
        Input.ContextDoc = Context.DocumentSlug;

        if (string.IsNullOrEmpty(Input.ProductOfInterest) && !string.IsNullOrEmpty(Context.SuggestedProductOfInterest))
        {
            Input.ProductOfInterest = Context.SuggestedProductOfInterest;
        }

        if (Context.HasContext)
        {
            Input.SourceContextCategory = Context.DisplayCategory;
            Input.SourceContextTitle = Context.DisplayTitle;
            Input.SourceContextSummary = Context.ContextSummary;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Enforce the type for this page
        Input.Type = InquiryType.RequestAQuote;

        // Organization is strictly required for commercial quote requests
        if (string.IsNullOrWhiteSpace(Input.Organization))
        {
            ModelState.TryAddModelError(
                $"{nameof(Input)}.{nameof(Input.Organization)}",
                "Organization/Company is required for quote requests.");
        }

        return await ProcessSubmissionAsync("An error occurred submitting your quote request.", HttpContext.RequestAborted);
    }
}
