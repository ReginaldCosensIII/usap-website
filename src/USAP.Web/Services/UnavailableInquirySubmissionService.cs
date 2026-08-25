namespace USAP.Web.Services;

public class UnavailableInquirySubmissionService : IInquirySubmissionService
{
    private readonly ILogger<UnavailableInquirySubmissionService> _logger;

    public UnavailableInquirySubmissionService(ILogger<UnavailableInquirySubmissionService> logger)
    {
        _logger = logger;
    }

    public Task<InquirySubmissionResult> SubmitAsync(Models.InquiryFormInput input, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("Inquiry submission failed: Submission service is currently unavailable in this environment.");

        return Task.FromResult(new InquirySubmissionResult
        {
            IsSuccess = false,
            ErrorMessage = "The submission service is temporarily unavailable. Please try again later or contact us directly."
        });
    }
}
