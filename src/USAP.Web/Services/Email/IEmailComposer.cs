using USAP.Web.Models;

namespace USAP.Web.Services.Email;

/// <summary>
/// Composes internal notification and visitor confirmation emails with controlled subjects and HTML/plain-text bodies.
/// </summary>
public interface IEmailComposer
{
    EmailMessage ComposeInternalNotification(InquiryFormInput input, string referenceNumber);
    EmailMessage ComposeVisitorConfirmation(InquiryFormInput input, string referenceNumber);
}
