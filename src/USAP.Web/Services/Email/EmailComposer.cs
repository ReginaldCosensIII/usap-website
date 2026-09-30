using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using USAP.Web.Configuration;
using USAP.Web.Models;

namespace USAP.Web.Services.Email;

/// <summary>
/// Composes internal notification and visitor confirmation emails
/// using strict controlled subjects, refined USAP brand design (dark navy header,
/// white typographic mark, red divider, navy footer, canonical domain), and compliant MIME multipart/alternative structures.
/// </summary>
public class EmailComposer : IEmailComposer
{
    private const string BrandNavy = "#0d1b2e";
    private const string BrandNavyBorder = "#1e3450";
    private const string BrandRed = "#c8102e";
    private const string NeutralSurface = "#f8fafc";
    private const string NeutralBorder = "#e2e8f0";
    private const string TextDark = "#1e293b";
    private const string TextMuted = "#64748b";

    private readonly SmtpOptions _options;
    private readonly IWebHostEnvironment _environment;

    public EmailComposer(
        IOptions<SmtpOptions> options,
        IWebHostEnvironment environment)
    {
        _options = options.Value;
        _environment = environment;
    }

    public EmailMessage ComposeInternalNotification(InquiryFormInput input, string referenceNumber)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentException.ThrowIfNullOrWhiteSpace(referenceNumber);

        var isProduction = _environment.IsProduction();
        var envMarker = isProduction ? string.Empty : "[DEV] ";
        var inquiryTypeName = GetInquiryTypeName(input.Type);

        // Subject: Controlled taxonomy + optional canonical context + reference.
        // NEVER include arbitrary visitor text in the subject.
        var contextTag = string.Empty;
        if (!string.IsNullOrWhiteSpace(input.SourceContextTitle))
        {
            var cleanContext = Regex.Replace(input.SourceContextTitle, @"[\r\n]", string.Empty).Trim();
            if (cleanContext.Length > 50)
            {
                cleanContext = cleanContext[..47] + "...";
            }
            contextTag = $" — {cleanContext}";
        }

        var subject = $"{envMarker}USAP Website — {inquiryTypeName}{contextTag} — {referenceNumber}";

        var safeVisitorName = !string.IsNullOrWhiteSpace(input.Name)
            ? Regex.Replace(input.Name, @"[\r\n]", string.Empty).Trim()
            : string.Empty;

        var htmlBody = BuildInternalHtml(input, referenceNumber, inquiryTypeName, isProduction);
        var plainTextBody = BuildInternalPlainText(input, referenceNumber, inquiryTypeName, isProduction);

        return new EmailMessage
        {
            Subject = subject,
            FromAddress = _options.FromAddress,
            FromName = _options.FromName,
            ToAddress = _options.NotificationRecipient,
            ToName = "USAP Web Inquiries",
            ReplyToAddress = input.Email,
            ReplyToName = safeVisitorName,
            HtmlBody = htmlBody,
            PlainTextBody = plainTextBody
        };
    }

    public EmailMessage ComposeVisitorConfirmation(InquiryFormInput input, string referenceNumber)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentException.ThrowIfNullOrWhiteSpace(referenceNumber);

        var isProduction = _environment.IsProduction();
        var envMarker = isProduction ? string.Empty : "[DEV] ";
        var isQuote = input.Type == InquiryType.RequestAQuote;

        var subject = isQuote
            ? $"{envMarker}United States Antenna Products — Quote Request Received — {referenceNumber}"
            : $"{envMarker}United States Antenna Products — Inquiry Received — {referenceNumber}";

        var safeVisitorName = !string.IsNullOrWhiteSpace(input.Name)
            ? Regex.Replace(input.Name, @"[\r\n]", string.Empty).Trim()
            : string.Empty;

        var htmlBody = isQuote
            ? BuildVisitorQuoteHtml(input, referenceNumber, isProduction)
            : BuildVisitorContactHtml(input, referenceNumber, isProduction);

        var plainTextBody = isQuote
            ? BuildVisitorQuotePlainText(input, referenceNumber, isProduction)
            : BuildVisitorContactPlainText(input, referenceNumber, isProduction);

        return new EmailMessage
        {
            Subject = subject,
            FromAddress = _options.FromAddress,
            FromName = _options.FromName,
            ToAddress = input.Email,
            ToName = safeVisitorName,
            ReplyToAddress = _options.FromAddress,
            ReplyToName = _options.FromName,
            HtmlBody = htmlBody,
            PlainTextBody = plainTextBody
        };
    }

    private static string GetInquiryTypeName(InquiryType type) => type switch
    {
        InquiryType.GeneralInquiry => "General Inquiry",
        InquiryType.ProductInformation => "Product Information",
        InquiryType.RequestAQuote => "Request a Quote",
        InquiryType.EngineeringSupport => "Engineering & Requirements Support",
        InquiryType.TechnicalDocumentation => "Technical Documentation",
        _ => "Inquiry"
    };

    private static string BuildInternalHtml(InquiryFormInput input, string referenceNumber, string inquiryTypeName, bool isProduction)
    {
        var isQuote = input.Type == InquiryType.RequestAQuote;
        var subtitle = isQuote ? "New Quote Request" : "New Website Inquiry";
        var devBanner = isProduction
            ? string.Empty
            : $"<div style=\"background-color: #fef3c7; border: 1px solid #f59e0b; color: #92400e; font-size: 11px; font-weight: 700; padding: 4px 10px; border-radius: 4px; display: inline-block; text-transform: uppercase; letter-spacing: 0.5px;\">DEVELOPMENT / TEST NOTIFICATION</div>";

        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang=\"en\">");
        sb.AppendLine("<head><meta charset=\"utf-8\"><title>Website Submission</title></head>");
        sb.AppendLine("<body style=\"margin: 0; padding: 24px 0; background-color: #f4f5f7; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; color: #1e293b; line-height: 1.5;\">");
        sb.AppendLine("  <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
        sb.AppendLine("    <tr>");
        sb.AppendLine("      <td align=\"center\">");
        sb.AppendLine($"        <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"620\" style=\"max-width: 620px; width: 100%; background-color: #ffffff; border-radius: 4px; overflow: hidden; border: 1px solid {NeutralBorder}; box-shadow: 0 1px 3px rgba(0,0,0,0.06);\">");

        // Refined Dark Navy Branded Header with Red Eyebrow & Bottom Red Divider
        sb.AppendLine("          <tr>");
        sb.AppendLine($"            <td style=\"background-color: {BrandNavy}; padding: 26px 32px 22px 32px; border-bottom: 3px solid {BrandRed};\">");
        sb.AppendLine($"              <div style=\"font-size: 11px; font-weight: 700; color: {BrandRed}; text-transform: uppercase; letter-spacing: 1.2px; line-height: 1.2; margin-bottom: 6px;\">USAP WEBSITE</div>");
        sb.AppendLine($"              <div style=\"font-size: 21px; font-weight: 800; color: #ffffff; letter-spacing: 1.2px; line-height: 1.2; text-transform: uppercase;\">UNITED STATES<br />ANTENNA PRODUCTS</div>");
        sb.AppendLine($"              <div style=\"font-size: 12px; font-weight: 600; color: #cbd5e1; text-transform: uppercase; letter-spacing: 0.8px; margin-top: 8px;\">{subtitle.ToUpperInvariant()}</div>");
        if (!string.IsNullOrEmpty(devBanner))
        {
            sb.AppendLine($"              <div style=\"margin-top: 12px;\">{devBanner}</div>");
        }
        sb.AppendLine("            </td>");
        sb.AppendLine("          </tr>");

        // Content
        sb.AppendLine("          <tr>");
        sb.AppendLine("            <td style=\"padding: 32px;\">");

        // Reference Box with red accent border and red labels
        sb.AppendLine($"              <div style=\"background-color: {NeutralSurface}; border: 1px solid {NeutralBorder}; border-left: 4px solid {BrandRed}; border-radius: 4px; padding: 16px 20px; margin-bottom: 24px;\">");
        sb.AppendLine("                <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
        sb.AppendLine("                  <tr>");
        sb.AppendLine($"                    <td style=\"font-size: 11px; font-weight: 700; text-transform: uppercase; color: {BrandRed}; letter-spacing: 0.6px;\">Reference Number</td>");
        sb.AppendLine($"                    <td style=\"font-size: 11px; font-weight: 700; text-transform: uppercase; color: {BrandRed}; letter-spacing: 0.6px;\">Inquiry Type</td>");
        sb.AppendLine("                  </tr>");
        sb.AppendLine("                  <tr>");
        sb.AppendLine($"                    <td style=\"font-size: 16px; font-weight: 700; color: {BrandNavy}; font-family: monospace; padding-top: 4px;\">{referenceNumber}</td>");
        sb.AppendLine($"                    <td style=\"font-size: 15px; font-weight: 600; color: {TextDark}; padding-top: 4px;\">{inquiryTypeName}</td>");
        sb.AppendLine("                  </tr>");
        sb.AppendLine("                </table>");
        sb.AppendLine("              </div>");

        // Canonical Context Section
        if (!string.IsNullOrWhiteSpace(input.SourceContextTitle))
        {
            sb.AppendLine("              <div style=\"margin-bottom: 24px;\">");
            sb.AppendLine($"                <div style=\"font-size: 11px; font-weight: 700; text-transform: uppercase; color: {BrandRed}; letter-spacing: 0.6px; border-bottom: 1px solid {NeutralBorder}; padding-bottom: 6px; margin-bottom: 12px;\">Canonical Website Context</div>");
            sb.AppendLine("                <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 14px;\">");
            sb.AppendLine($"                  <tr><td style=\"width: 140px; color: {TextMuted}; padding: 4px 0;\">Category:</td><td style=\"color: {TextDark}; font-weight: 600;\">{WebUtility.HtmlEncode(input.SourceContextCategory ?? "Context")}</td></tr>");
            sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Context Title:</td><td style=\"color: {BrandNavy}; font-weight: 600;\">{WebUtility.HtmlEncode(input.SourceContextTitle)}</td></tr>");
            if (!string.IsNullOrWhiteSpace(input.SourceContextSummary) && input.SourceContextSummary != input.SourceContextTitle)
            {
                sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Summary:</td><td style=\"color: #334155;\">{WebUtility.HtmlEncode(input.SourceContextSummary)}</td></tr>");
            }
            sb.AppendLine("                </table>");
            sb.AppendLine("              </div>");
        }

        // Visitor Contact Details
        sb.AppendLine("              <div style=\"margin-bottom: 24px;\">");
        sb.AppendLine($"                <div style=\"font-size: 11px; font-weight: 700; text-transform: uppercase; color: {BrandRed}; letter-spacing: 0.6px; border-bottom: 1px solid {NeutralBorder}; padding-bottom: 6px; margin-bottom: 12px;\">Visitor Contact Details</div>");
        sb.AppendLine("                <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 14px;\">");
        sb.AppendLine($"                  <tr><td style=\"width: 140px; color: {TextMuted}; padding: 4px 0;\">Name:</td><td style=\"color: {TextDark}; font-weight: 600;\">{WebUtility.HtmlEncode(input.Name)}</td></tr>");
        sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Email:</td><td><a href=\"mailto:{WebUtility.HtmlEncode(input.Email)}\" style=\"color: {BrandNavy}; font-weight: 600; text-decoration: underline;\">{WebUtility.HtmlEncode(input.Email)}</a></td></tr>");
        if (!string.IsNullOrWhiteSpace(input.Organization))
        {
            sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Organization:</td><td style=\"color: {TextDark};\">{WebUtility.HtmlEncode(input.Organization)}</td></tr>");
        }
        if (!string.IsNullOrWhiteSpace(input.Phone))
        {
            sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Phone:</td><td style=\"color: {TextDark};\">{WebUtility.HtmlEncode(input.Phone)}</td></tr>");
        }
        if (input.PreferredContactMethod.HasValue)
        {
            sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Contact Method:</td><td style=\"color: {TextDark};\">{input.PreferredContactMethod.Value}</td></tr>");
        }
        sb.AppendLine("                </table>");
        sb.AppendLine("              </div>");

        // Visitor Request Details
        var hasRequestDetails = !string.IsNullOrWhiteSpace(input.ProductOfInterest) || isQuote;
        if (hasRequestDetails)
        {
            sb.AppendLine("              <div style=\"margin-bottom: 24px;\">");
            sb.AppendLine($"                <div style=\"font-size: 11px; font-weight: 700; text-transform: uppercase; color: {BrandRed}; letter-spacing: 0.6px; border-bottom: 1px solid {NeutralBorder}; padding-bottom: 6px; margin-bottom: 12px;\">Visitor-Provided Request Details</div>");
            sb.AppendLine("                <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 14px;\">");
            if (!string.IsNullOrWhiteSpace(input.ProductOfInterest))
            {
                sb.AppendLine($"                  <tr><td style=\"width: 140px; color: {TextMuted}; padding: 4px 0;\">Product/Model:</td><td style=\"color: {TextDark};\">{WebUtility.HtmlEncode(input.ProductOfInterest)} <span style=\"font-size: 11px; color: {TextMuted}; font-weight: normal;\">(Visitor-Provided)</span></td></tr>");
            }
            if (isQuote)
            {
                if (!string.IsNullOrWhiteSpace(input.EstimatedQuantity))
                {
                    sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Estimated Quantity:</td><td style=\"color: {TextDark};\">{WebUtility.HtmlEncode(input.EstimatedQuantity)}</td></tr>");
                }
                if (!string.IsNullOrWhiteSpace(input.DesiredTimeline))
                {
                    sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Desired Timeline:</td><td style=\"color: {TextDark};\">{WebUtility.HtmlEncode(input.DesiredTimeline)}</td></tr>");
                }
                if (!string.IsNullOrWhiteSpace(input.IntendedApplication))
                {
                    var encodedApp = WebUtility.HtmlEncode(input.IntendedApplication)
                        .Replace("\r\n", "<br />")
                        .Replace("\n", "<br />");
                    sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0; vertical-align: top;\">Application:</td><td style=\"color: {TextDark}; line-height: 1.5;\">{encodedApp}</td></tr>");
                }
            }
            sb.AppendLine("                </table>");
            sb.AppendLine("              </div>");
        }

        // Message Section — normalized without double-spacing
        sb.AppendLine("              <div>");
        sb.AppendLine($"                <div style=\"font-size: 11px; font-weight: 700; text-transform: uppercase; color: {BrandRed}; letter-spacing: 0.6px; border-bottom: 1px solid {NeutralBorder}; padding-bottom: 6px; margin-bottom: 12px;\">Message / Project Requirements</div>");
        var encodedMsg = WebUtility.HtmlEncode(input.Message)
            .Replace("\r\n", "<br />")
            .Replace("\n", "<br />");
        sb.AppendLine($"                <div style=\"background-color: {NeutralSurface}; border: 1px solid {NeutralBorder}; border-radius: 4px; padding: 16px; font-size: 14px; color: {TextDark}; line-height: 1.6; word-break: break-word;\">{encodedMsg}</div>");
        sb.AppendLine("              </div>");

        sb.AppendLine("            </td>");
        sb.AppendLine("          </tr>");

        // Refined Navy Company Footer with Red Accent Rule & Public Contact Details
        sb.AppendLine("          <tr>");
        sb.AppendLine($"            <td style=\"background-color: {BrandNavy}; border-top: 3px solid {BrandRed}; padding: 24px 32px; font-size: 12px; color: #94a3b8; text-align: center; line-height: 1.6;\">");
        sb.AppendLine("              <div style=\"font-size: 14px; font-weight: 700; color: #ffffff; letter-spacing: 0.5px; margin-bottom: 6px;\">United States Antenna Products, LLC</div>");
        sb.AppendLine("              <div style=\"color: #cbd5e1; margin-bottom: 4px;\">5263 Agro Drive, Frederick, MD 21703</div>");
        sb.AppendLine("              <div style=\"color: #cbd5e1; margin-bottom: 12px;\">Phone: <a href=\"tel:+12403417120\" style=\"color: #ffffff; text-decoration: none;\">240-341-7120</a> &nbsp;|&nbsp; Fax: 240-371-4980 &nbsp;|&nbsp; <a href=\"https://www.usantennaproducts.com/\" style=\"color: #ffffff; text-decoration: underline;\">www.usantennaproducts.com</a></div>");
        sb.AppendLine($"              <div style=\"font-size: 11px; color: #94a3b8; border-top: 1px solid {BrandNavyBorder}; padding-top: 12px; margin-top: 12px;\">This notification was generated by the United States Antenna Products website for reference <strong style=\"color: #ffffff;\">{referenceNumber}</strong>.</div>");
        if (!isProduction)
        {
            sb.AppendLine("              <div style=\"color: #f59e0b; font-weight: 700; font-size: 11px; margin-top: 6px; text-transform: uppercase; letter-spacing: 0.5px;\">DEVELOPMENT / TEST ENVIRONMENT — Internal Notification</div>");
        }
        sb.AppendLine("            </td>");
        sb.AppendLine("          </tr>");

        sb.AppendLine("        </table>");
        sb.AppendLine("      </td>");
        sb.AppendLine("    </tr>");
        sb.AppendLine("  </table>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return sb.ToString();
    }

    private static string BuildInternalPlainText(InquiryFormInput input, string referenceNumber, string inquiryTypeName, bool isProduction)
    {
        var isQuote = input.Type == InquiryType.RequestAQuote;
        var sb = new StringBuilder();

        sb.AppendLine("==================================================");
        sb.AppendLine("USAP WEBSITE");
        sb.AppendLine("UNITED STATES ANTENNA PRODUCTS");
        sb.AppendLine(isQuote ? "NEW QUOTE REQUEST" : "NEW WEBSITE INQUIRY");
        if (!isProduction)
        {
            sb.AppendLine("[DEVELOPMENT / TEST NOTIFICATION]");
        }
        sb.AppendLine("==================================================");
        sb.AppendLine($"Reference Number : {referenceNumber}");
        sb.AppendLine($"Inquiry Type     : {inquiryTypeName}");

        if (!string.IsNullOrWhiteSpace(input.SourceContextTitle))
        {
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("CANONICAL WEBSITE CONTEXT:");
            sb.AppendLine($"  Category       : {input.SourceContextCategory ?? "Context"}");
            sb.AppendLine($"  Title          : {input.SourceContextTitle}");
            if (!string.IsNullOrWhiteSpace(input.SourceContextSummary) && input.SourceContextSummary != input.SourceContextTitle)
            {
                sb.AppendLine($"  Summary        : {input.SourceContextSummary}");
            }
        }

        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine("VISITOR CONTACT DETAILS:");
        sb.AppendLine($"  Name           : {input.Name}");
        sb.AppendLine($"  Email          : {input.Email}");
        if (!string.IsNullOrWhiteSpace(input.Organization))
        {
            sb.AppendLine($"  Organization   : {input.Organization}");
        }
        if (!string.IsNullOrWhiteSpace(input.Phone))
        {
            sb.AppendLine($"  Phone          : {input.Phone}");
        }
        if (input.PreferredContactMethod.HasValue)
        {
            sb.AppendLine($"  Contact Method : {input.PreferredContactMethod.Value}");
        }

        if (!string.IsNullOrWhiteSpace(input.ProductOfInterest) || isQuote)
        {
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("REQUEST DETAILS (VISITOR-PROVIDED):");
            if (!string.IsNullOrWhiteSpace(input.ProductOfInterest))
            {
                sb.AppendLine($"  Product/Model  : {input.ProductOfInterest} (Visitor-Provided)");
            }
            if (isQuote)
            {
                if (!string.IsNullOrWhiteSpace(input.EstimatedQuantity))
                {
                    sb.AppendLine($"  Estimated Qty  : {input.EstimatedQuantity}");
                }
                if (!string.IsNullOrWhiteSpace(input.DesiredTimeline))
                {
                    sb.AppendLine($"  Timeline       : {input.DesiredTimeline}");
                }
                if (!string.IsNullOrWhiteSpace(input.IntendedApplication))
                {
                    sb.AppendLine($"  Application    : {input.IntendedApplication}");
                }
            }
        }

        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine("MESSAGE / PROJECT REQUIREMENTS:");
        sb.AppendLine(input.Message);
        sb.AppendLine("==================================================");
        sb.AppendLine("UNITED STATES ANTENNA PRODUCTS, LLC");
        sb.AppendLine("5263 Agro Drive, Frederick, MD 21703");
        sb.AppendLine("Phone: 240-341-7120 | Fax: 240-371-4980");
        sb.AppendLine("Website: https://www.usantennaproducts.com/");
        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine($"Generated by USAP Website for reference {referenceNumber}");
        if (!isProduction)
        {
            sb.AppendLine("DEVELOPMENT / TEST ENVIRONMENT — Internal Notification");
        }

        return sb.ToString();
    }

    private static string BuildVisitorContactHtml(InquiryFormInput input, string referenceNumber, bool isProduction)
    {
        var safeName = WebUtility.HtmlEncode(input.Name);
        var devNotice = isProduction
            ? string.Empty
            : "<div style=\"background-color: #fef3c7; border: 1px solid #f59e0b; color: #92400e; font-size: 11px; font-weight: 700; padding: 4px 10px; border-radius: 4px; display: inline-block; text-transform: uppercase; letter-spacing: 0.5px;\">DEVELOPMENT / TEST EMAIL — NO PRODUCTION CUSTOMER COMMUNICATION</div>";

        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang=\"en\">");
        sb.AppendLine("<head><meta charset=\"utf-8\"><title>Inquiry Received</title></head>");
        sb.AppendLine("<body style=\"margin: 0; padding: 24px 0; background-color: #f4f5f7; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; color: #1e293b; line-height: 1.5;\">");
        sb.AppendLine("  <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
        sb.AppendLine("    <tr>");
        sb.AppendLine("      <td align=\"center\">");
        sb.AppendLine($"        <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"600\" style=\"max-width: 600px; width: 100%; background-color: #ffffff; border-radius: 4px; overflow: hidden; border: 1px solid {NeutralBorder}; box-shadow: 0 1px 3px rgba(0,0,0,0.06);\">");

        // Refined Dark Navy Branded Header with Red Eyebrow & Bottom Red Divider
        sb.AppendLine("          <tr>");
        sb.AppendLine($"            <td style=\"background-color: {BrandNavy}; padding: 26px 32px 22px 32px; border-bottom: 3px solid {BrandRed};\">");
        sb.AppendLine($"              <div style=\"font-size: 11px; font-weight: 700; color: {BrandRed}; text-transform: uppercase; letter-spacing: 1.2px; line-height: 1.2; margin-bottom: 6px;\">USAP WEBSITE</div>");
        sb.AppendLine($"              <div style=\"font-size: 21px; font-weight: 800; color: #ffffff; letter-spacing: 1.2px; line-height: 1.2; text-transform: uppercase;\">UNITED STATES<br />ANTENNA PRODUCTS</div>");
        sb.AppendLine($"              <div style=\"font-size: 12px; font-weight: 600; color: #cbd5e1; text-transform: uppercase; letter-spacing: 0.8px; margin-top: 8px;\">INQUIRY CONFIRMATION</div>");
        if (!string.IsNullOrEmpty(devNotice))
        {
            sb.AppendLine($"              <div style=\"margin-top: 12px;\">{devNotice}</div>");
        }
        sb.AppendLine("            </td>");
        sb.AppendLine("          </tr>");

        // Content
        sb.AppendLine("          <tr>");
        sb.AppendLine("            <td style=\"padding: 32px;\">");
        sb.AppendLine($"              <h1 style=\"font-size: 20px; font-weight: 700; color: {BrandNavy}; margin: 0 0 16px 0;\">We received your inquiry</h1>");
        sb.AppendLine($"              <p style=\"font-size: 15px; color: #334155; margin: 0 0 16px 0;\">Hello {safeName},</p>");
        sb.AppendLine("              <p style=\"font-size: 15px; color: #334155; margin: 0 0 24px 0;\">Thank you for contacting United States Antenna Products. Your inquiry has been received for review.</p>");

        // Reference Card with red accent border and red label
        sb.AppendLine($"              <div style=\"background-color: {NeutralSurface}; border: 1px solid {NeutralBorder}; border-left: 4px solid {BrandRed}; border-radius: 4px; padding: 16px 20px; margin-bottom: 24px;\">");
        sb.AppendLine($"                <div style=\"font-size: 11px; font-weight: 700; text-transform: uppercase; color: {BrandRed}; letter-spacing: 0.6px;\">REFERENCE NUMBER</div>");
        sb.AppendLine($"                <div style=\"font-size: 18px; font-weight: 700; color: {BrandNavy}; font-family: monospace; padding-top: 4px;\">{referenceNumber}</div>");
        sb.AppendLine($"                <div style=\"font-size: 13px; color: {TextMuted}; margin-top: 6px;\">Please keep this reference number for your records.</div>");
        sb.AppendLine("              </div>");

        // Submission Summary with red section eyebrow
        sb.AppendLine("              <div style=\"margin-bottom: 24px;\">");
        sb.AppendLine($"                <div style=\"font-size: 11px; font-weight: 700; text-transform: uppercase; color: {BrandRed}; letter-spacing: 0.8px; border-bottom: 1px solid {NeutralBorder}; padding-bottom: 6px; margin-bottom: 12px;\">SUBMISSION SUMMARY</div>");
        sb.AppendLine("                <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 14px;\">");
        sb.AppendLine($"                  <tr><td style=\"width: 140px; color: {TextMuted}; padding: 4px 0;\">Inquiry Type:</td><td style=\"color: {TextDark}; font-weight: 600;\">{GetInquiryTypeName(input.Type)}</td></tr>");
        if (!string.IsNullOrWhiteSpace(input.SourceContextTitle))
        {
            sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Context:</td><td style=\"color: {TextDark};\">{WebUtility.HtmlEncode(input.SourceContextTitle)}</td></tr>");
        }
        if (!string.IsNullOrWhiteSpace(input.ProductOfInterest))
        {
            sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Product of Interest:</td><td style=\"color: {TextDark};\">{WebUtility.HtmlEncode(input.ProductOfInterest)} <span style=\"font-size: 11px; color: {TextMuted}; font-weight: normal;\">(Visitor-Provided)</span></td></tr>");
        }
        sb.AppendLine("                </table>");
        sb.AppendLine("              </div>");

        sb.AppendLine($"              <p style=\"font-size: 14px; color: {TextMuted}; margin: 0;\">USAP will review your message and follow up using the contact information you provided.</p>");
        sb.AppendLine("            </td>");
        sb.AppendLine("          </tr>");

        // Refined Navy Company Footer with Red Accent Rule & Public Contact Details
        sb.AppendLine("          <tr>");
        sb.AppendLine($"            <td style=\"background-color: {BrandNavy}; border-top: 3px solid {BrandRed}; padding: 24px 32px; font-size: 12px; color: #94a3b8; text-align: center; line-height: 1.6;\">");
        sb.AppendLine("              <div style=\"font-size: 14px; font-weight: 700; color: #ffffff; letter-spacing: 0.5px; margin-bottom: 6px;\">United States Antenna Products, LLC</div>");
        sb.AppendLine("              <div style=\"color: #cbd5e1; margin-bottom: 4px;\">5263 Agro Drive, Frederick, MD 21703</div>");
        sb.AppendLine("              <div style=\"color: #cbd5e1; margin-bottom: 12px;\">Phone: <a href=\"tel:+12403417120\" style=\"color: #ffffff; text-decoration: none;\">240-341-7120</a> &nbsp;|&nbsp; Fax: 240-371-4980 &nbsp;|&nbsp; <a href=\"https://www.usantennaproducts.com/\" style=\"color: #ffffff; text-decoration: underline;\">www.usantennaproducts.com</a></div>");
        sb.AppendLine($"              <div style=\"font-size: 11px; color: #94a3b8; border-top: 1px solid {BrandNavyBorder}; padding-top: 12px; margin-top: 12px;\">This is an automated confirmation of a submission made through the United States Antenna Products website.</div>");
        if (!isProduction)
        {
            sb.AppendLine("              <div style=\"color: #f59e0b; font-weight: 700; font-size: 11px; margin-top: 6px; text-transform: uppercase; letter-spacing: 0.5px;\">DEVELOPMENT / TEST EMAIL — NO PRODUCTION CUSTOMER COMMUNICATION</div>");
        }
        sb.AppendLine("            </td>");
        sb.AppendLine("          </tr>");

        sb.AppendLine("        </table>");
        sb.AppendLine("      </td>");
        sb.AppendLine("    </tr>");
        sb.AppendLine("  </table>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return sb.ToString();
    }

    private static string BuildVisitorContactPlainText(InquiryFormInput input, string referenceNumber, bool isProduction)
    {
        var sb = new StringBuilder();
        sb.AppendLine("==================================================");
        sb.AppendLine("USAP WEBSITE");
        sb.AppendLine("UNITED STATES ANTENNA PRODUCTS");
        sb.AppendLine("INQUIRY CONFIRMATION");
        if (!isProduction)
        {
            sb.AppendLine("[DEVELOPMENT / TEST EMAIL — NO PRODUCTION CUSTOMER COMMUNICATION]");
        }
        sb.AppendLine("==================================================");
        sb.AppendLine("We received your inquiry");
        sb.AppendLine();
        sb.AppendLine($"Hello {input.Name},");
        sb.AppendLine();
        sb.AppendLine("Thank you for contacting United States Antenna Products. Your inquiry has been received for review.");
        sb.AppendLine();
        sb.AppendLine($"Reference Number: {referenceNumber}");
        sb.AppendLine("Please keep this reference number for your records.");
        sb.AppendLine();
        sb.AppendLine("Submission Summary:");
        sb.AppendLine($"  Inquiry Type        : {GetInquiryTypeName(input.Type)}");
        if (!string.IsNullOrWhiteSpace(input.SourceContextTitle))
        {
            sb.AppendLine($"  Context             : {input.SourceContextTitle}");
        }
        if (!string.IsNullOrWhiteSpace(input.ProductOfInterest))
        {
            sb.AppendLine($"  Product of Interest : {input.ProductOfInterest} (Visitor-Provided)");
        }
        sb.AppendLine();
        sb.AppendLine("USAP will review your message and follow up using the contact information you provided.");
        sb.AppendLine("==================================================");
        sb.AppendLine("UNITED STATES ANTENNA PRODUCTS, LLC");
        sb.AppendLine("5263 Agro Drive, Frederick, MD 21703");
        sb.AppendLine("Phone: 240-341-7120 | Fax: 240-371-4980");
        sb.AppendLine("Website: https://www.usantennaproducts.com/");
        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine("This is an automated confirmation of a submission made through the United States Antenna Products website.");
        if (!isProduction)
        {
            sb.AppendLine("DEVELOPMENT / TEST EMAIL — NO PRODUCTION CUSTOMER COMMUNICATION");
        }

        return sb.ToString();
    }

    private static string BuildVisitorQuoteHtml(InquiryFormInput input, string referenceNumber, bool isProduction)
    {
        var safeName = WebUtility.HtmlEncode(input.Name);
        var devNotice = isProduction
            ? string.Empty
            : "<div style=\"background-color: #fef3c7; border: 1px solid #f59e0b; color: #92400e; font-size: 11px; font-weight: 700; padding: 4px 10px; border-radius: 4px; display: inline-block; text-transform: uppercase; letter-spacing: 0.5px;\">DEVELOPMENT / TEST EMAIL — NO PRODUCTION CUSTOMER COMMUNICATION</div>";

        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang=\"en\">");
        sb.AppendLine("<head><meta charset=\"utf-8\"><title>Quote Request Received</title></head>");
        sb.AppendLine("<body style=\"margin: 0; padding: 24px 0; background-color: #f4f5f7; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; color: #1e293b; line-height: 1.5;\">");
        sb.AppendLine("  <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
        sb.AppendLine("    <tr>");
        sb.AppendLine("      <td align=\"center\">");
        sb.AppendLine($"        <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"600\" style=\"max-width: 600px; width: 100%; background-color: #ffffff; border-radius: 4px; overflow: hidden; border: 1px solid {NeutralBorder}; box-shadow: 0 1px 3px rgba(0,0,0,0.06);\">");

        // Refined Dark Navy Branded Header with Red Eyebrow & Bottom Red Divider
        sb.AppendLine("          <tr>");
        sb.AppendLine($"            <td style=\"background-color: {BrandNavy}; padding: 26px 32px 22px 32px; border-bottom: 3px solid {BrandRed};\">");
        sb.AppendLine($"              <div style=\"font-size: 11px; font-weight: 700; color: {BrandRed}; text-transform: uppercase; letter-spacing: 1.2px; line-height: 1.2; margin-bottom: 6px;\">USAP WEBSITE</div>");
        sb.AppendLine($"              <div style=\"font-size: 21px; font-weight: 800; color: #ffffff; letter-spacing: 1.2px; line-height: 1.2; text-transform: uppercase;\">UNITED STATES<br />ANTENNA PRODUCTS</div>");
        sb.AppendLine($"              <div style=\"font-size: 12px; font-weight: 600; color: #cbd5e1; text-transform: uppercase; letter-spacing: 0.8px; margin-top: 8px;\">QUOTE REQUEST CONFIRMATION</div>");
        if (!string.IsNullOrEmpty(devNotice))
        {
            sb.AppendLine($"              <div style=\"margin-top: 12px;\">{devNotice}</div>");
        }
        sb.AppendLine("            </td>");
        sb.AppendLine("          </tr>");

        // Content
        sb.AppendLine("          <tr>");
        sb.AppendLine("            <td style=\"padding: 32px;\">");
        sb.AppendLine($"              <h1 style=\"font-size: 20px; font-weight: 700; color: {BrandNavy}; margin: 0 0 16px 0;\">We received your quote request</h1>");
        sb.AppendLine($"              <p style=\"font-size: 15px; color: #334155; margin: 0 0 16px 0;\">Hello {safeName},</p>");
        sb.AppendLine("              <p style=\"font-size: 15px; color: #334155; margin: 0 0 24px 0;\">Thank you for submitting a quote request to United States Antenna Products. Your request has been received for review.</p>");

        // Reference Card with red accent border and red label
        sb.AppendLine($"              <div style=\"background-color: {NeutralSurface}; border: 1px solid {NeutralBorder}; border-left: 4px solid {BrandRed}; border-radius: 4px; padding: 16px 20px; margin-bottom: 24px;\">");
        sb.AppendLine($"                <div style=\"font-size: 11px; font-weight: 700; text-transform: uppercase; color: {BrandRed}; letter-spacing: 0.6px;\">QUOTE REQUEST REFERENCE</div>");
        sb.AppendLine($"                <div style=\"font-size: 18px; font-weight: 700; color: {BrandNavy}; font-family: monospace; padding-top: 4px;\">{referenceNumber}</div>");
        sb.AppendLine($"                <div style=\"font-size: 13px; color: {TextMuted}; margin-top: 6px;\">Please keep this reference number for your records.</div>");
        sb.AppendLine("              </div>");

        // Submission Summary with red section eyebrow
        sb.AppendLine("              <div style=\"margin-bottom: 24px;\">");
        sb.AppendLine($"                <div style=\"font-size: 11px; font-weight: 700; text-transform: uppercase; color: {BrandRed}; letter-spacing: 0.8px; border-bottom: 1px solid {NeutralBorder}; padding-bottom: 6px; margin-bottom: 12px;\">SUBMISSION SUMMARY</div>");
        sb.AppendLine("                <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 14px;\">");
        sb.AppendLine($"                  <tr><td style=\"width: 140px; color: {TextMuted}; padding: 4px 0;\">Inquiry Type:</td><td style=\"color: {TextDark}; font-weight: 600;\">Request a Quote</td></tr>");
        if (!string.IsNullOrWhiteSpace(input.Organization))
        {
            sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Organization:</td><td style=\"color: {TextDark};\">{WebUtility.HtmlEncode(input.Organization)}</td></tr>");
        }
        if (!string.IsNullOrWhiteSpace(input.SourceContextTitle))
        {
            sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Context:</td><td style=\"color: {TextDark};\">{WebUtility.HtmlEncode(input.SourceContextTitle)}</td></tr>");
        }
        if (!string.IsNullOrWhiteSpace(input.ProductOfInterest))
        {
            sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Product of Interest:</td><td style=\"color: {TextDark};\">{WebUtility.HtmlEncode(input.ProductOfInterest)} <span style=\"font-size: 11px; color: {TextMuted}; font-weight: normal;\">(Visitor-Provided)</span></td></tr>");
        }
        if (!string.IsNullOrWhiteSpace(input.EstimatedQuantity))
        {
            sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Estimated Quantity:</td><td style=\"color: {TextDark};\">{WebUtility.HtmlEncode(input.EstimatedQuantity)}</td></tr>");
        }
        if (!string.IsNullOrWhiteSpace(input.DesiredTimeline))
        {
            sb.AppendLine($"                  <tr><td style=\"color: {TextMuted}; padding: 4px 0;\">Desired Timeline:</td><td style=\"color: {TextDark};\">{WebUtility.HtmlEncode(input.DesiredTimeline)}</td></tr>");
        }
        sb.AppendLine("                </table>");
        sb.AppendLine("              </div>");

        sb.AppendLine($"              <p style=\"font-size: 14px; color: {TextMuted}; margin: 0;\">USAP will review the information you provided and follow up if clarification or additional information is needed.</p>");
        sb.AppendLine("            </td>");
        sb.AppendLine("          </tr>");

        // Refined Navy Company Footer with Red Accent Rule & Public Contact Details
        sb.AppendLine("          <tr>");
        sb.AppendLine($"            <td style=\"background-color: {BrandNavy}; border-top: 3px solid {BrandRed}; padding: 24px 32px; font-size: 12px; color: #94a3b8; text-align: center; line-height: 1.6;\">");
        sb.AppendLine("              <div style=\"font-size: 14px; font-weight: 700; color: #ffffff; letter-spacing: 0.5px; margin-bottom: 6px;\">United States Antenna Products, LLC</div>");
        sb.AppendLine("              <div style=\"color: #cbd5e1; margin-bottom: 4px;\">5263 Agro Drive, Frederick, MD 21703</div>");
        sb.AppendLine("              <div style=\"color: #cbd5e1; margin-bottom: 12px;\">Phone: <a href=\"tel:+12403417120\" style=\"color: #ffffff; text-decoration: none;\">240-341-7120</a> &nbsp;|&nbsp; Fax: 240-371-4980 &nbsp;|&nbsp; <a href=\"https://www.usantennaproducts.com/\" style=\"color: #ffffff; text-decoration: underline;\">www.usantennaproducts.com</a></div>");
        sb.AppendLine($"              <div style=\"font-size: 11px; color: #94a3b8; border-top: 1px solid {BrandNavyBorder}; padding-top: 12px; margin-top: 12px;\">This is an automated confirmation of a submission made through the United States Antenna Products website.</div>");
        if (!isProduction)
        {
            sb.AppendLine("              <div style=\"color: #f59e0b; font-weight: 700; font-size: 11px; margin-top: 6px; text-transform: uppercase; letter-spacing: 0.5px;\">DEVELOPMENT / TEST EMAIL — NO PRODUCTION CUSTOMER COMMUNICATION</div>");
        }
        sb.AppendLine("            </td>");
        sb.AppendLine("          </tr>");

        sb.AppendLine("        </table>");
        sb.AppendLine("      </td>");
        sb.AppendLine("    </tr>");
        sb.AppendLine("  </table>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return sb.ToString();
    }

    private static string BuildVisitorQuotePlainText(InquiryFormInput input, string referenceNumber, bool isProduction)
    {
        var sb = new StringBuilder();
        sb.AppendLine("==================================================");
        sb.AppendLine("USAP WEBSITE");
        sb.AppendLine("UNITED STATES ANTENNA PRODUCTS");
        sb.AppendLine("QUOTE REQUEST CONFIRMATION");
        if (!isProduction)
        {
            sb.AppendLine("[DEVELOPMENT / TEST EMAIL — NO PRODUCTION CUSTOMER COMMUNICATION]");
        }
        sb.AppendLine("==================================================");
        sb.AppendLine("We received your quote request");
        sb.AppendLine();
        sb.AppendLine($"Hello {input.Name},");
        sb.AppendLine();
        sb.AppendLine("Thank you for submitting a quote request to United States Antenna Products. Your request has been received for review.");
        sb.AppendLine();
        sb.AppendLine($"Quote Request Reference: {referenceNumber}");
        sb.AppendLine("Please keep this reference number for your records.");
        sb.AppendLine();
        sb.AppendLine("Submission Summary:");
        sb.AppendLine("  Inquiry Type        : Request a Quote");
        if (!string.IsNullOrWhiteSpace(input.Organization))
        {
            sb.AppendLine($"  Organization        : {input.Organization}");
        }
        if (!string.IsNullOrWhiteSpace(input.SourceContextTitle))
        {
            sb.AppendLine($"  Context             : {input.SourceContextTitle}");
        }
        if (!string.IsNullOrWhiteSpace(input.ProductOfInterest))
        {
            sb.AppendLine($"  Product of Interest : {input.ProductOfInterest} (Visitor-Provided)");
        }
        if (!string.IsNullOrWhiteSpace(input.EstimatedQuantity))
        {
            sb.AppendLine($"  Estimated Quantity  : {input.EstimatedQuantity}");
        }
        if (!string.IsNullOrWhiteSpace(input.DesiredTimeline))
        {
            sb.AppendLine($"  Desired Timeline    : {input.DesiredTimeline}");
        }
        sb.AppendLine();
        sb.AppendLine("USAP will review the information you provided and follow up if clarification or additional information is needed.");
        sb.AppendLine("==================================================");
        sb.AppendLine("UNITED STATES ANTENNA PRODUCTS, LLC");
        sb.AppendLine("5263 Agro Drive, Frederick, MD 21703");
        sb.AppendLine("Phone: 240-341-7120 | Fax: 240-371-4980");
        sb.AppendLine("Website: https://www.usantennaproducts.com/");
        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine("This is an automated confirmation of a submission made through the United States Antenna Products website.");
        if (!isProduction)
        {
            sb.AppendLine("DEVELOPMENT / TEST EMAIL — NO PRODUCTION CUSTOMER COMMUNICATION");
        }

        return sb.ToString();
    }
}
