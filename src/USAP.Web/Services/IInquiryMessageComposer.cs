using USAP.Web.Models;

namespace USAP.Web.Services;

public interface IInquiryMessageComposer
{
    InquiryMessage Compose(InquiryFormInput input);
}
