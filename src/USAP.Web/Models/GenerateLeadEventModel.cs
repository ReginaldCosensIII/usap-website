namespace USAP.Web.Models;

/// <summary>
/// Server-controlled model for rendering the GA4 generate_lead conversion event.
/// Strictly excludes inquiry business data, reference numbers, and visitor PII.
/// </summary>
public record GenerateLeadEventModel(bool ShouldTrack, string LeadSource)
{
    public const string ContactLeadSource = "website_contact_form";
    public const string QuoteLeadSource = "website_quote_request";
}
