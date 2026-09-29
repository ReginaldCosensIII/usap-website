using System.ComponentModel.DataAnnotations;

namespace USAP.Web.Models;

public enum InquiryType
{
    [Display(Name = "General Inquiry")]
    GeneralInquiry = 1,

    [Display(Name = "Request a Quote")]
    RequestAQuote = 2,

    [Display(Name = "Product Information")]
    ProductInformation = 3,

    [Display(Name = "Engineering & Requirements Support")]
    EngineeringSupport = 4,

    [Display(Name = "Technical Documentation")]
    TechnicalDocumentation = 5
}
