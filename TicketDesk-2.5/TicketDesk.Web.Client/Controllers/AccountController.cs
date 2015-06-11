using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using TicketDesk.Web.Client.Models;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client.Controllers
{
    [Authorize]
    [RoutePrefix("account")]
    [Route("{action=manage}")]
    public class AccountController : Controller
    {
        private TicketDeskUserManager UserManager { get; set; }
        private TicketDeskSignInManager SignInManager { get; set; }

        public AccountController(TicketDeskUserManager userManager, TicketDeskSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        [Route("manage")]
        public ActionResult Manage(AccountMessageId? message)
        {
            ViewBag.StatusMessage =
               message == AccountMessageId.ChangePasswordSuccess ? "Your password has been changed."
               : message == AccountMessageId.Error ? "An error has occurred."
               : message == AccountMessageId.ProfileSaveSuccess ? "Your profile has been updated."
               : "";
            return View();
        }

        [Route("change-password")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Route("change-password")]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(AccountPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Manage", new { Message = AccountMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        [Route("edit-profile")]
        public ActionResult EditProfile()
        {
            var model = new AccountProfileViewModel {DisplayName = User.Identity.GetUserDisplayName(), Email = User.Identity.GetUserName()};
            return View(model);
        }

        [Route("edit-profile")]
        [HttpPost]
        public async Task<ActionResult> EditProfile(AccountProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            user.UserName = model.Email;
            user.Email = model.Email;
            user.DisplayName = model.DisplayName;
            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                AuthenticationManager.SignOut();
                await SignInManager.SignInAsync(user, false, false);//.SignIn(new AuthenticationProperties { IsPersistent = false }, 
                //await user.GenerateUserIdentityAsync(UserManager);
                return RedirectToAction("Manage", new { Message = AccountMessageId.ProfileSaveSuccess });
            }
            return View();
        }

        public enum AccountMessageId
        {
            ChangePasswordSuccess,
            ProfileSaveSuccess,
            Error
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(TicketDeskUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}