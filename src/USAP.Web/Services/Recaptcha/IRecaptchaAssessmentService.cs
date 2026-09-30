using USAP.Web.Models;

namespace USAP.Web.Services.Recaptcha;

/// <summary>
/// Service contract for assessing score-based Google Cloud reCAPTCHA tokens.
/// </summary>
public interface IRecaptchaAssessmentService
{
    Task<RecaptchaAssessmentResult> AssessAsync(
        string? token,
        string expectedAction,
        string? userIpAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default);
}
