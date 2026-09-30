namespace USAP.Web.Configuration;

/// <summary>
/// Provider-neutral SMTP configuration options.
/// Secrets (such as Password) must NEVER be stored in source control;
/// use .NET User Secrets in development or IIS Environment Variables in production.
/// </summary>
public class SmtpOptions
{
    public const string SectionName = "Smtp";

    public bool Enabled { get; set; } = false;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string SecurityMode { get; set; } = "StartTls";
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string FromAddress { get; set; } = string.Empty;
    public string FromName { get; set; } = "USAP Web Inquiries";
    public string NotificationRecipient { get; set; } = string.Empty;

    public MailKit.Security.SecureSocketOptions ResolveSecureSocketOptions()
    {
        if (string.IsNullOrWhiteSpace(SecurityMode))
        {
            return MailKit.Security.SecureSocketOptions.StartTls;
        }

        return SecurityMode.Trim().ToLowerInvariant() switch
        {
            "starttls" => MailKit.Security.SecureSocketOptions.StartTls,
            "sslonconnect" or "ssl" => MailKit.Security.SecureSocketOptions.SslOnConnect,
            _ => throw new InvalidOperationException($"Invalid or insecure Smtp:SecurityMode '{SecurityMode}'. Strict transport security requires 'StartTls' or 'SslOnConnect'.")
        };
    }

    public bool IsValid(out string? error)
    {
        if (!Enabled)
        {
            error = null;
            return true;
        }

        if (string.IsNullOrWhiteSpace(Host))
        {
            error = "Smtp:Host is required when SMTP is enabled.";
            return false;
        }

        if (Port is < 1 or > 65535)
        {
            error = $"Smtp:Port '{Port}' is invalid. Must be between 1 and 65535.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Username))
        {
            error = "Smtp:Username is required when SMTP is enabled.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            error = "Smtp:Password is required when SMTP is enabled.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(FromAddress) ||
            !MimeKit.MailboxAddress.TryParse(FromAddress, out var fromMailbox) ||
            string.IsNullOrWhiteSpace(fromMailbox.Address) ||
            !fromMailbox.Address.Contains('@'))
        {
            error = "Smtp:FromAddress must be a valid mailbox address when SMTP is enabled.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(NotificationRecipient) ||
            !MimeKit.MailboxAddress.TryParse(NotificationRecipient, out var recipientMailbox) ||
            string.IsNullOrWhiteSpace(recipientMailbox.Address) ||
            !recipientMailbox.Address.Contains('@'))
        {
            error = "Smtp:NotificationRecipient must be a valid mailbox address when SMTP is enabled.";
            return false;
        }

        try
        {
            _ = ResolveSecureSocketOptions();
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }

        error = null;
        return true;
    }
}
