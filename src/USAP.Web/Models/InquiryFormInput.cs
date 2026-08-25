using System.ComponentModel.DataAnnotations;
using USAP.Web.Models.Validation;

namespace USAP.Web.Models;

public class InquiryFormInput
{
    private string _name = string.Empty;
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name
    {
        get => _name;
        set => _name = value?.Trim() ?? string.Empty;
    }

    private string _email = string.Empty;
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters.")]
    public string Email
    {
        get => _email;
        set => _email = value?.Trim() ?? string.Empty;
    }

    [Required(ErrorMessage = "Please select an inquiry type.")]
    [EnumDataType(typeof(InquiryType), ErrorMessage = "Invalid inquiry type selected.")]
    public InquiryType Type { get; set; } = InquiryType.GeneralInquiry;

    private string _message = string.Empty;
    [Required(ErrorMessage = "Message is required.")]
    [StringLength(4000, ErrorMessage = "Message cannot exceed 4000 characters.")]
    public string Message
    {
        get => _message;
        set => _message = value?.Trim() ?? string.Empty;
    }

    private string? _organization;
    [StringLength(150, ErrorMessage = "Organization cannot exceed 150 characters.")]
    public string? Organization
    {
        get => _organization;
        set => _organization = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private string? _phone;
    [StringLength(40, ErrorMessage = "Phone cannot exceed 40 characters.")]
    [PlausiblePhoneNumber]
    public string? Phone
    {
        get => _phone;
        set => _phone = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    [EnumDataType(typeof(ContactMethod), ErrorMessage = "Invalid contact method selected.")]
    public ContactMethod? PreferredContactMethod { get; set; }

    private string? _productOfInterest;
    [StringLength(150, ErrorMessage = "Product/Model cannot exceed 150 characters.")]
    public string? ProductOfInterest
    {
        get => _productOfInterest;
        set => _productOfInterest = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private string? _estimatedQuantity;
    [StringLength(50, ErrorMessage = "Quantity cannot exceed 50 characters.")]
    public string? EstimatedQuantity
    {
        get => _estimatedQuantity;
        set => _estimatedQuantity = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private string? _intendedApplication;
    [StringLength(1000, ErrorMessage = "Intended application cannot exceed 1000 characters.")]
    public string? IntendedApplication
    {
        get => _intendedApplication;
        set => _intendedApplication = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private string? _desiredTimeline;
    [StringLength(100, ErrorMessage = "Timeline cannot exceed 100 characters.")]
    public string? DesiredTimeline
    {
        get => _desiredTimeline;
        set => _desiredTimeline = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    // Honeypot field
    [StringLength(200)]
    public string? Website { get; set; }
}
