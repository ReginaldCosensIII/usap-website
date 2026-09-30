using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using USAP.Web.Configuration;
using USAP.Web.Models;
using USAP.Web.Services;
using USAP.Web.Services.Recaptcha;

namespace USAP.Web.Pages;

public abstract class InquiryPageModelBase : PageModel
{
    protected readonly IInquirySubmissionService _submissionService;
    protected readonly ICtaContextResolver _contextResolver;
    protected readonly IRecaptchaAssessmentService _recaptchaService;
    protected readonly RecaptchaOptions _recaptchaOptions;

    protected InquiryPageModelBase(
        IInquirySubmissionService submissionService,
        ICtaContextResolver contextResolver,
        IRecaptchaAssessmentService recaptchaService,
        IOptions<RecaptchaOptions> recaptchaOptions)
    {
        _submissionService = submissionService;
        _contextResolver = contextResolver;
        _recaptchaService = recaptchaService;
        _recaptchaOptions = recaptchaOptions.Value;
    }

    [BindProperty]
    public InquiryFormInput Input { get; set; } = new();

    /// <summary>
    /// reCAPTCHA Enterprise execution token obtained client-side at form submission time.
    /// This property is isolated from inquiry business data and cleared on any redisplay.
    /// </summary>
    [BindProperty]
    public string? RecaptchaToken { get; set; }

    public CtaContext Context { get; set; } = CtaContext.Empty();

    public virtual bool IsQuoteWorkflow => false;

    /// <summary>
    /// Server-controlled reCAPTCHA action identifier for this workflow.
    /// </summary>
    public abstract string RecaptchaAction { get; }

    public override void OnPageHandlerExecuting(Microsoft.AspNetCore.Mvc.Filters.PageHandlerExecutingContext context)
    {
        ViewData["IsQuoteWorkflow"] = IsQuoteWorkflow;
        ViewData["RecaptchaEnabled"] = _recaptchaOptions.Enabled;
        ViewData["RecaptchaSiteKey"] = _recaptchaOptions.SiteKey;
        ViewData["RecaptchaAction"] = RecaptchaAction;
        base.OnPageHandlerExecuting(context);
    }

    public void ClearRecaptchaToken()
    {
        RecaptchaToken = null;
        ModelState.Remove(nameof(RecaptchaToken));
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
            // Honeypot triggered — silent diversion without sending email or invoking Google assessment
            ClearRecaptchaToken();
            var syntheticRef = InquiryReferenceGenerator.Generate();
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

        // Server-side form validation runs before any external assessment
        if (!ModelState.IsValid)
        {
            ClearRecaptchaToken();
            return Page();
        }

        // Score-based reCAPTCHA Enterprise verification
        if (_recaptchaOptions.Enabled)
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();

            var assessment = await _recaptchaService.AssessAsync(
                RecaptchaToken,
                RecaptchaAction,
                clientIp,
                userAgent,
                cancellationToken);

            ClearRecaptchaToken();

            if (!assessment.IsAccepted)
            {
                // Generic visitor-facing error; internal details/scores are strictly excluded
                ModelState.AddModelError(string.Empty, "We couldn't verify your submission. Please try again.");
                return Page();
            }
        }
        else
        {
            ClearRecaptchaToken();
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
