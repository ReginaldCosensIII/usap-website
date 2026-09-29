using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using USAP.Web.Models;
using USAP.Web.Services;

namespace USAP.Web.Pages;

[EnableRateLimiting("InquirySubmission")]
public class ContactUsModel : InquiryPageModelBase
{

    public ContactUsModel(
        IInquirySubmissionService submissionService,
        ICtaContextResolver contextResolver)
        : base(submissionService, contextResolver)
    {
    }

    private static readonly HashSet<InquiryType> s_allowedContactTypes = new()
    {
        InquiryType.GeneralInquiry,
        InquiryType.ProductInformation,
        InquiryType.EngineeringSupport,
        InquiryType.TechnicalDocumentation
    };

    public IActionResult OnGet(string? reason, string? family, string? group, string? doc)
    {
        if (string.Equals(reason?.Trim(), "request-a-quote", StringComparison.OrdinalIgnoreCase))
        {
            var resolvedQuoteContext = _contextResolver.Resolve("request-a-quote", family, group, doc);
            var routeValues = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(resolvedQuoteContext.DocumentSlug))
            {
                routeValues["doc"] = resolvedQuoteContext.DocumentSlug;
            }
            else if (!string.IsNullOrEmpty(resolvedQuoteContext.GroupId))
            {
                routeValues["group"] = resolvedQuoteContext.GroupId;
            }
            else if (!string.IsNullOrEmpty(resolvedQuoteContext.FamilySlug))
            {
                routeValues["family"] = resolvedQuoteContext.FamilySlug;
            }
            return RedirectToPage("/RequestAQuote", routeValues);
        }

        Context = _contextResolver.Resolve(reason, family, group, doc);
        Input.Type = Context.Intent;

        Input.ContextReason = reason;
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

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Input.Type == InquiryType.RequestAQuote)
        {
            Context = _contextResolver.Resolve(Input.ContextReason, Input.ContextFamily, Input.ContextGroup, Input.ContextDoc);
            Input.SourceContextCategory = Context.DisplayCategory;
            Input.SourceContextTitle = Context.DisplayTitle;
            Input.SourceContextSummary = Context.ContextSummary;

            var safeType = s_allowedContactTypes.Contains(Context.Intent)
                ? Context.Intent
                : InquiryType.GeneralInquiry;
            Input.Type = safeType;

            ModelState.Remove($"{nameof(Input)}.{nameof(Input.Type)}");
            ModelState.AddModelError(
                $"{nameof(Input)}.{nameof(Input.Type)}",
                "Please use the Request a Quote form for quote requests.");

            return Page();
        }

        if (!s_allowedContactTypes.Contains(Input.Type) ||
            ModelState.GetFieldValidationState($"{nameof(Input)}.{nameof(Input.Type)}") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
        {
            Context = _contextResolver.Resolve(Input.ContextReason, Input.ContextFamily, Input.ContextGroup, Input.ContextDoc);
            Input.SourceContextCategory = Context.DisplayCategory;
            Input.SourceContextTitle = Context.DisplayTitle;
            Input.SourceContextSummary = Context.ContextSummary;

            var safeType = s_allowedContactTypes.Contains(Context.Intent)
                ? Context.Intent
                : InquiryType.GeneralInquiry;
            Input.Type = safeType;

            ModelState.Remove($"{nameof(Input)}.{nameof(Input.Type)}");
            ModelState.AddModelError(
                $"{nameof(Input)}.{nameof(Input.Type)}",
                "Please select a valid inquiry type.");

            return Page();
        }

        return await ProcessSubmissionAsync("An error occurred submitting your inquiry.", HttpContext.RequestAborted);
    }
}
