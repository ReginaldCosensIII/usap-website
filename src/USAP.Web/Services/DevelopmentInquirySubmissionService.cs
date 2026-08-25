namespace USAP.Web.Services;

public class DevelopmentInquirySubmissionService : IInquirySubmissionService
{
    private readonly IInquiryMessageComposer _composer;
    private readonly ILogger<DevelopmentInquirySubmissionService> _logger;

    public DevelopmentInquirySubmissionService(
        IInquiryMessageComposer composer,
        ILogger<DevelopmentInquirySubmissionService> logger)
    {
        _composer = composer;
        _logger = logger;
    }

    public Task<InquirySubmissionResult> SubmitAsync(Models.InquiryFormInput input, CancellationToken cancellationToken = default)
    {
        var referenceNumber = $"REQ-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";

        // Ensure composer is invoked so the architecture is validated, but do not log the result.
        var message = _composer.Compose(input);

        _logger.LogInformation("Development simulation: Inquiry submitted successfully. Reference: {ReferenceNumber}, Type: {Type}", referenceNumber, input.Type);

        return Task.FromResult(new InquirySubmissionResult
        {
            IsSuccess = true,
            ReferenceNumber = referenceNumber,
            ConfirmationMessage = "Your inquiry was successfully processed by the development simulator (no external email was sent)."
        });
    }
}
