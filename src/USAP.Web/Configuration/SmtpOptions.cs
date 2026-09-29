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
}
