using System.ComponentModel.DataAnnotations;

namespace USAP.Web.Models;

public enum ContactMethod
{
    [Display(Name = "No Preference")]
    NoPreference = 0,

    [Display(Name = "Email")]
    Email = 1,

    [Display(Name = "Phone")]
    Phone = 2
}
