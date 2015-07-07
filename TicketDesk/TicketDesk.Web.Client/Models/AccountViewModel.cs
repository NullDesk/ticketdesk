// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client.Models
{
    public class AccountPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AccountProfileViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
    }

    public class UserAccountInfoViewModel
    {
        public UserAccountInfoViewModel()
        {
            Roles = new string[] { };
        }

        public UserAccountInfoViewModel(TicketDeskUser user, IEnumerable<string> roles)
        {
            var lockDate = (user.LockoutEndDateUtc ?? DateTime.MinValue).ToUniversalTime();
            User = user;
            IsLocked = lockDate > DateTime.UtcNow && lockDate < DateTime.MaxValue.ToUniversalTime();
            IsDisabled = lockDate == DateTime.MaxValue.ToUniversalTime();
            Roles = roles ?? new string[] {};
        }

        public TicketDeskUser User { get; set; }

        [Display(Name = "Locked", Prompt = "Locked")]
        [Description(
            "Determines if the account has been locked. Temporary locks are created by invalid login attempts.")]
        public bool IsLocked { get; set; }

        [Display(Name = "Disabled", Prompt = "Disabled")]
        [Description("Determines if the account has been disabled by an administrator.")]
        public bool IsDisabled { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<string> GetRoleNames(IEnumerable<TicketDeskRole> allRolesList)
        {
            return allRolesList.Where(ar => Roles.Any(r => r == ar.Id)).Select(ar => ar.DisplayName);
        }

        public MultiSelectList UserRolesList
        {
            get
            {
                var roleManager = DependencyResolver.Current.GetService<TicketDeskRoleManager>();

                return roleManager.Roles.ToMultiSelectList(
                    r => r.Id,
                    r => r.DisplayName,
                    Roles.ToArray());
            }
        }


    }
}