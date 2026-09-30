using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using USAP.Web.Models;
using USAP.Web.Services;

namespace USAP.Web.Pages;

public abstract class InquiryPageModelBase : PageModel
{
    protected readonly IInquirySubmissionService _submissionService;
    protected readonly ICtaContextResolver _contextResolver;

    protected InquiryPageModelBase(
        IInquirySubmissionService submissionService,
        ICtaContextResolver contextResolver)
    {
        _submissionService = submissionService;
        _contextResolver = contextResolver;
    }

    [BindProperty]
    public InquiryFormInput Input { get; set; } = new();

    public CtaContext Context { get; set; } = CtaContext.Empty();

    public virtual bool IsQuoteWorkflow => false;

    public override void OnPageHandlerExecuting(Microsoft.AspNetCore.Mvc.Filters.PageHandlerExecutingContext context)
    {
        ViewData["IsQuoteWorkflow"] = IsQuoteWorkflow;
        base.OnPageHandlerExecuting(context);
    }

    protected async Task<IActionResult> ProcessSubmissionAsync(string defaultErrorMessage, CancellationToken cancellationToken)
    {
        var targetThankYou = IsQuoteWorkflow
            ? "/RequestAQuote/ThankYou"
            : "/ContactUs/ThankYou";

        // Re-establish canonical context server-side from submitted identifiers.
        // Client-posted display strings are untrusted and discarded.
        Context = _contextResolver.Resolve(Input.ContextReason, Input.ContextFamily, Input.ContextGroup, Input.ContextDoc);

        // Overwrite presentation properties server-side strictly from the canonical resolver
        Input.SourceContextCategory = Context.DisplayCategory;
        Input.SourceContextTitle = Context.DisplayTitle;
        Input.SourceContextSummary = Context.ContextSummary;

        if (!string.IsNullOrEmpty(Input.Website))
        {
            // Honeypot triggered — silent diversion without sending email or invoking backend
            var syntheticRef = Services.InquiryReferenceGenerator.Generate();
            TempData["ReferenceNumber"] = syntheticRef;
            TempData["IsGenuineSubmission"] = false;
            TempData["SubmissionDisplayState"] = "HoneypotDiversion";
            if (Context.HasContext)
            {
                TempData["ContextReason"] = Input.ContextReason;
                TempData["ContextFamily"] = Context.FamilySlug;
                TempData["ContextGroup"] = Context.GroupId;
                TempData["ContextDoc"] = Context.DocumentSlug;
            }
            return RedirectToPage(targetThankYou);
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
            TempData["IsGenuineSubmission"] = true;
            TempData["SubmissionDisplayState"] = "GenuineSuccess";
            TempData["SubmittedEmail"] = Input.Email;
            TempData["VisitorConfirmationSent"] = result.VisitorConfirmationSent;
            if (Context.HasContext)
            {
                TempData["ContextReason"] = Input.ContextReason;
                TempData["ContextFamily"] = Context.FamilySlug;
                TempData["ContextGroup"] = Context.GroupId;
                TempData["ContextDoc"] = Context.DocumentSlug;
            }
            return RedirectToPage(targetThankYou);
        }

        ModelState.AddModelError(string.Empty, result.ErrorMessage ?? defaultErrorMessage);
        return Page();
    }
}
