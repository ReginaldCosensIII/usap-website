namespace USAP.Web.Services.Email;

/// <summary>
/// Provider-neutral email message payload.
/// </summary>
public class EmailMessage
{
    public required string Subject { get; init; }
    public required string FromAddress { get; init; }
    public string? FromName { get; init; }
    public required string ToAddress { get; init; }
    public string? ToName { get; init; }
    public string? ReplyToAddress { get; init; }
    public string? ReplyToName { get; init; }
    public required string PlainTextBody { get; init; }
    public required string HtmlBody { get; init; }
}
