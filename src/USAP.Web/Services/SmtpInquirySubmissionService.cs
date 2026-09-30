using Microsoft.Extensions.Options;
using USAP.Web.Configuration;
using USAP.Web.Models;
using USAP.Web.Services.Email;

namespace USAP.Web.Services;

/// <summary>
/// Delivery-aware inquiry submission service orchestrating internal notification
/// and subsequent visitor confirmation via ISmtpEmailSender.
/// </summary>
public class SmtpInquirySubmissionService : IInquirySubmissionService
{
    private readonly ISmtpEmailSender _emailSender;
    private readonly IEmailComposer _emailComposer;
    private readonly SmtpOptions _options;
    private readonly ILogger<SmtpInquirySubmissionService> _logger;

    public SmtpInquirySubmissionService(
        ISmtpEmailSender emailSender,
        IEmailComposer emailComposer,
        IOptions<SmtpOptions> options,
        ILogger<SmtpInquirySubmissionService> logger)
    {
        _emailSender = emailSender;
        _emailComposer = emailComposer;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<InquirySubmissionResult> SubmitAsync(InquiryFormInput input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        // 1. Generate genuine inquiry reference number using canonical generator
        var referenceNumber = InquiryReferenceGenerator.Generate();

        // 2. Compose internal notification
        var internalEmail = _emailComposer.ComposeInternalNotification(input, referenceNumber);

        // 3. Send internal notification
        try
        {
            await _emailSender.SendAsync(internalEmail, cancellationToken);
            _logger.LogInformation(
                "Internal notification sent successfully. Reference: {ReferenceNumber}, Type: {InquiryType}",
                referenceNumber, input.Type);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            // CASE C: Internal notification failed
            // Do NOT show ordinary success state, do NOT send visitor confirmation, do NOT tell visitor inquiry was received.
            _logger.LogError(
                "Internal notification delivery failed. Reference: {ReferenceNumber}, Type: {InquiryType}, Stage: {DeliveryStage}, ErrorCategory: {ErrorCategory}",
                referenceNumber, input.Type, "InternalNotification", ex.GetType().Name);

            return new InquirySubmissionResult
            {
                IsSuccess = false,
                InquiryAccepted = false,
                InternalNotificationSent = false,
                VisitorConfirmationSent = false,
                ReferenceNumber = null,
                ErrorMessage = "We couldn't send your request at this time. Please try again."
            };
        }

        // Internal notification succeeded -> Inquiry is accepted
        var visitorConfirmationSent = false;

        // 4. ONLY if internal notification succeeds: compose and attempt visitor confirmation
        try
        {
            var visitorEmail = _emailComposer.ComposeVisitorConfirmation(input, referenceNumber);
            await _emailSender.SendAsync(visitorEmail, cancellationToken);
            visitorConfirmationSent = true;
            _logger.LogInformation(
                "Visitor confirmation email sent successfully. Reference: {ReferenceNumber}, Type: {InquiryType}",
                referenceNumber, input.Type);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            // CASE B: Internal notification succeeded BUT visitor confirmation failed
            // DO NOT lose or reject the accepted inquiry.
            _logger.LogWarning(
                "Visitor confirmation email delivery failed for accepted inquiry. Reference: {ReferenceNumber}, Type: {InquiryType}, Stage: {DeliveryStage}, ErrorCategory: {ErrorCategory}",
                referenceNumber, input.Type, "VisitorConfirmation", ex.GetType().Name);

            visitorConfirmationSent = false;
        }

        // Return delivery-aware result
        return new InquirySubmissionResult
        {
            IsSuccess = true,
            InquiryAccepted = true,
            InternalNotificationSent = true,
            VisitorConfirmationSent = visitorConfirmationSent,
            ReferenceNumber = referenceNumber,
            ConfirmationMessage = visitorConfirmationSent
                ? "Your inquiry has been submitted and a confirmation email has been sent."
                : "Your submission has been received. Please keep this reference number for your records."
        };
    }
}
