namespace USAP.Web.Services;

/// <summary>
/// Delivery-aware inquiry submission outcome.
/// </summary>
public class InquirySubmissionResult
{
    public bool IsSuccess { get; set; }
    public bool InquiryAccepted { get; set; }
    public bool InternalNotificationSent { get; set; }
    public bool VisitorConfirmationSent { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ConfirmationMessage { get; set; }
}

public interface IInquirySubmissionService
{
    Task<InquirySubmissionResult> SubmitAsync(Models.InquiryFormInput input, CancellationToken cancellationToken = default);
}
