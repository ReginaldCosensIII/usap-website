using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using USAP.Web.Configuration;

namespace USAP.Web.Services.Email;

/// <summary>
/// MailKit implementation of ISmtpEmailSender.
/// Dispatches MIME multipart/alternative emails with full certificate validation.
/// </summary>
public class MailKitSmtpEmailSender : ISmtpEmailSender
{
    private readonly SmtpOptions _options;
    private readonly ILogger<MailKitSmtpEmailSender> _logger;

    public MailKitSmtpEmailSender(
        IOptions<SmtpOptions> options,
        ILogger<MailKitSmtpEmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (!_options.IsValid(out var validationError))
        {
            throw new InvalidOperationException($"Cannot send email because SMTP configuration is invalid: {validationError}");
        }

        var mimeMessage = new MimeMessage();

        var fromName = !string.IsNullOrWhiteSpace(message.FromName)
            ? message.FromName
            : _options.FromName;
        mimeMessage.From.Add(new MailboxAddress(fromName, message.FromAddress));

        var toName = message.ToName ?? string.Empty;
        mimeMessage.To.Add(new MailboxAddress(toName, message.ToAddress));

        if (!string.IsNullOrWhiteSpace(message.ReplyToAddress))
        {
            var replyToName = message.ReplyToName ?? string.Empty;
            mimeMessage.ReplyTo.Add(new MailboxAddress(replyToName, message.ReplyToAddress));
        }

        mimeMessage.Subject = message.Subject;

        var builder = new BodyBuilder
        {
            TextBody = message.PlainTextBody,
            HtmlBody = message.HtmlBody
        };

        mimeMessage.Body = builder.ToMessageBody();

        using var client = new SmtpClient();

        // Standard strict certificate validation — do not disable or weaken checks
        client.CheckCertificateRevocation = true;

        var secureSocketOptions = _options.ResolveSecureSocketOptions();

        await client.ConnectAsync(_options.Host, _options.Port, secureSocketOptions, cancellationToken);

        if (!string.IsNullOrWhiteSpace(_options.Username) && !string.IsNullOrWhiteSpace(_options.Password))
        {
            await client.AuthenticateAsync(_options.Username, _options.Password, cancellationToken);
        }

        await client.SendAsync(mimeMessage, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}
