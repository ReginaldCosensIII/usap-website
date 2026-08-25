using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace USAP.Web.Models.Validation;

public class PlausiblePhoneNumberAttribute : ValidationAttribute
{
    private static readonly Regex s_phoneRegex = new Regex(
        @"^(?<primary>\+?[\d\s\-\.\(\)]+)(?<ext>(?:\s+)?(?:x|ext\.?|extension)\s*\d{1,6})?$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public PlausiblePhoneNumberAttribute()
    {
        ErrorMessage = "Enter a valid phone number with 7 to 15 digits.";
    }

    public override bool IsValid(object? value)
    {
        var input = value as string;
        if (string.IsNullOrWhiteSpace(input))
        {
            return true;
        }

        var match = s_phoneRegex.Match(input);
        if (!match.Success)
        {
            return false;
        }

        // Count digits in primary part
        var primaryPart = match.Groups["primary"].Value;
        int digitCount = 0;
        foreach (char c in primaryPart)
        {
            if (char.IsDigit(c))
            {
                digitCount++;
            }
        }

        return digitCount >= 7 && digitCount <= 15;
    }
}
