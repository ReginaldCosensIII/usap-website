namespace USAP.Web.Services;

public class InquirySubmissionResult
{
    public bool IsSuccess { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ConfirmationMessage { get; set; }
}

public interface IInquirySubmissionService
{
    Task<InquirySubmissionResult> SubmitAsync(Models.InquiryFormInput input, CancellationToken cancellationToken = default);
}
