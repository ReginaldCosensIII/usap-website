using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using USAP.Web.Models;
using USAP.Web.Services;

namespace USAP.Web.Pages.RequestAQuote;

public record HelpfulLinkItem(string Text, string Url);

public class ThankYouModel : PageModel
{
    private readonly ICtaContextResolver _contextResolver;

    public ThankYouModel(ICtaContextResolver contextResolver)
    {
        _contextResolver = contextResolver;
    }

    [TempData]
    public string? ReferenceNumber { get; set; }

    [TempData]
    public bool? IsGenuineSubmission { get; set; }

    [TempData]
    public string? SubmissionDisplayState { get; set; }

    [TempData]
    public string? SubmittedEmail { get; set; }

    [TempData]
    public bool? VisitorConfirmationSent { get; set; }

    [TempData]
    public string? ContextReason { get; set; }

    [TempData]
    public string? ContextFamily { get; set; }

    [TempData]
    public string? ContextGroup { get; set; }

    [TempData]
    public string? ContextDoc { get; set; }

    public bool HasDisplaySuccess => !string.IsNullOrEmpty(ReferenceNumber);

    public bool IsGenuine => IsGenuineSubmission == true;

    public bool HasActiveSubmission => HasDisplaySuccess;

    public IReadOnlyList<HelpfulLinkItem> HelpfulLinks { get; set; } = Array.Empty<HelpfulLinkItem>();

    public void OnGet()
    {
        if (HasActiveSubmission)
        {
            var context = _contextResolver.Resolve(ContextReason, ContextFamily, ContextGroup, ContextDoc);
            HelpfulLinks = BuildContextualHelpfulLinks(context);
        }
        else
        {
            HelpfulLinks = GetDefaultHelpfulLinks();
        }
    }

    private List<HelpfulLinkItem> BuildContextualHelpfulLinks(CtaContext context)
    {
        var links = new List<HelpfulLinkItem>();

        if (!string.IsNullOrWhiteSpace(context.DocumentSlug))
        {
            links.Add(new HelpfulLinkItem("Return to This Document", $"/technical-resources/document/{context.DocumentSlug}"));
            if (!string.IsNullOrWhiteSpace(context.FamilySlug))
            {
                links.Add(new HelpfulLinkItem(context.FamilyName ?? "Related Product Family", $"/products/{context.FamilySlug}"));
            }
            links.Add(new HelpfulLinkItem("Browse Technical Resources", "/technical-resources"));
            return links.Take(3).ToList();
        }

        if (!string.IsNullOrWhiteSpace(context.GroupId) && !string.IsNullOrWhiteSpace(context.FamilySlug))
        {
            links.Add(new HelpfulLinkItem(context.GroupName ?? "View Product Group", $"/products/{context.FamilySlug}#{context.GroupId}"));
            links.Add(new HelpfulLinkItem("Browse Technical Resources", "/technical-resources"));
            links.Add(new HelpfulLinkItem("Contact USAP", "/contact-us"));
            return links.Take(3).ToList();
        }

        if (!string.IsNullOrWhiteSpace(context.FamilySlug))
        {
            links.Add(new HelpfulLinkItem(context.FamilyName ?? "View Product Family", $"/products/{context.FamilySlug}"));
            links.Add(new HelpfulLinkItem("Browse Technical Resources", "/technical-resources"));
            links.Add(new HelpfulLinkItem("Contact USAP", "/contact-us"));
            return links.Take(3).ToList();
        }

        return GetDefaultHelpfulLinks();
    }

    private static List<HelpfulLinkItem> GetDefaultHelpfulLinks() => new()
    {
        new HelpfulLinkItem("Explore Products", "/products"),
        new HelpfulLinkItem("Technical Resources", "/technical-resources"),
        new HelpfulLinkItem("Contact Us", "/contact-us")
    };
}
