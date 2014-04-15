using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TicketDesk.Web.Models
{
    //AccountController parameter models
    public class AddExternalLoginBindingModel
    {
        [Required]        
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]        
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} characters should be greater or equal to {2}", MinimumLength = 6)]
        [DataType(DataType.Password)]        
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password and confirmation should match")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required]        
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} characters should be greater or equal to {2}", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Password and confirmation should match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]        
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]        
        public string LoginProvider { get; set; }

        [Required]        
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} characters should be greater or equal to {2}", MinimumLength = 6)]
        [DataType(DataType.Password)]        
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]        
        [Compare("NewPassword", ErrorMessage = "New password and confirmation should match")]
        public string ConfirmPassword { get; set; }
    }
}