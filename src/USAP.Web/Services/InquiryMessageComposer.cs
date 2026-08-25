using System.Text;
using System.Text.RegularExpressions;
using USAP.Web.Models;

namespace USAP.Web.Services;

public class InquiryMessageComposer : IInquiryMessageComposer
{
    public InquiryMessage Compose(InquiryFormInput input)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"New Website Inquiry: {input.Type}");
        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine($"Name: {input.Name}");
        sb.AppendLine($"Email: {input.Email}");
        if (!string.IsNullOrWhiteSpace(input.Organization))
        {
            sb.AppendLine($"Organization: {input.Organization}");
        }

        if (!string.IsNullOrWhiteSpace(input.Phone))
        {
            sb.AppendLine($"Phone: {input.Phone}");
        }

        if (!string.IsNullOrWhiteSpace(input.ProductOfInterest))
        {
            sb.AppendLine($"Product of Interest: {input.ProductOfInterest}");
        }

        if (input.PreferredContactMethod.HasValue)
        {
            sb.AppendLine($"Preferred Contact Method: {input.PreferredContactMethod.Value}");
        }

        if (input.Type == InquiryType.RequestAQuote)
        {
            sb.AppendLine();
            sb.AppendLine("Quote Details:");
            if (!string.IsNullOrWhiteSpace(input.EstimatedQuantity))
            {
                sb.AppendLine($"Estimated Quantity: {input.EstimatedQuantity}");
            }

            if (!string.IsNullOrWhiteSpace(input.DesiredTimeline))
            {
                sb.AppendLine($"Desired Timeline: {input.DesiredTimeline}");
            }

            if (!string.IsNullOrWhiteSpace(input.IntendedApplication))
            {
                sb.AppendLine("Intended Application:");
                sb.AppendLine(input.IntendedApplication);
            }
        }

        sb.AppendLine();
        sb.AppendLine("Message / Project Requirements:");
        sb.AppendLine(input.Message);

        // Strip carriage returns and line feeds from name to prevent header injection
        var safeName = Regex.Replace(input.Name, @"[\r\n]", string.Empty).Trim();

        return new InquiryMessage
        {
            Subject = $"Website Inquiry: {input.Type} from {safeName}",
            Body = sb.ToString()
        };
    }
}
