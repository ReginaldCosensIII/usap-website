namespace USAP.Web.Services.Email;

/// <summary>
/// Provider-neutral SMTP email transport interface.
/// </summary>
public interface ISmtpEmailSender
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
