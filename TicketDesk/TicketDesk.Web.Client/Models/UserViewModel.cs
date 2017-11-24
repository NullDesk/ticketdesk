// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.ComponentModel.DataAnnotations;
using TicketDesk.Localization;
using TicketDesk.Localization.Models;

namespace TicketDesk.Web.Client.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [StringLength(100, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation))]
        [Display(Name = "UserName", ResourceType = typeof(Strings))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Validation))]
        [Display(Name = "Email", ResourceType = typeof(Strings))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [StringLength(100, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation))]
        [Display(Name = "DisplayName", ResourceType = typeof(Strings))]
        public string DisplayName { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [StringLength(100, ErrorMessageResourceName = "FieldMinimumLength", ErrorMessageResourceType = typeof(Validation), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Strings))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Strings))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmationDoNotMatch", ErrorMessageResourceType = typeof(Strings))]
        public string ConfirmPassword { get; set; }
    }

    public class UserSignInViewModel
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [Display(Name = "UserNameOrEmail", ResourceType = typeof(Strings))]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Strings))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Strings))]
        public bool RememberMe { get; set; }
    }
}