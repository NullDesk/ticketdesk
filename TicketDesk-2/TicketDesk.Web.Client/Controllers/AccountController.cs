// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using TicketDesk.Web.Client.Models;
using TicketDesk.Domain.Services;
using System.ComponentModel.Composition;

namespace TicketDesk.Web.Client.Controllers
{

    [HandleError]
    [Export("Account", typeof(IController))]
    public partial class AccountController : ApplicationController
    {
        
        public SettingsService Settings { get; set; }

        [ImportingConstructor]
        public AccountController(ISecurityService security, SettingsService settings):base(security)
        {
            Settings = settings;
        }
        
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public virtual ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName.Trim(), model.Password.Trim()))
                {
                    FormsService.SignIn(model.UserName.Trim().ToLower(), model.RememberMe);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction(MVC.Home.Index());
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public virtual ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction(MVC.Home.Index());
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public virtual ActionResult Register()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public virtual ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName.Trim(), model.DisplayName.Trim(), model.Password.Trim(), model.Email.Trim(), Security, Settings.ApplicationSettings);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName.Trim(), false /* createPersistentCookie */);
                    return RedirectToAction(MVC.Home.Index());
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }


        [Authorize]
        public virtual ActionResult MyAccount()
        {
            return View();
        }

        [Authorize]
        public virtual ActionResult ChangePreferences()
        {
            var model = new AccountPreferencesModel();
            model.DisplayName = ViewData["UserDisplayName"] as string;
            model.EmailAddress = Security.GetUserEmailAddress(Security.CurrentUserName);
            model.OpenEditorWithPreview = Settings.UserSettings.OpenEditorWithPreview;
            return View(model);
        }

        [Authorize]
        public virtual ActionResult ChangePreferencesSuccess()
        {

            return View();
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult ChangePreferences(AccountPreferencesModel model)
        {
            if (ModelState.IsValid)
            {

                if (MembershipService.ChangeUserPreferences(User.Identity.Name, model.DisplayName.Trim(), model.EmailAddress.Trim(), model.OpenEditorWithPreview, Settings))
                    {
                        return RedirectToAction(MVC.Account.ChangePreferencesSuccess());
                    }
                   
                
            }
                ModelState.AddModelError("", "Unable to save preferences, unknown error.");
                    return View(model);
                
        }






        [Authorize]
        public virtual ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword.Trim(), model.NewPassword.Trim()))
                {
                    return RedirectToAction(MVC.Account.ChangePasswordSuccess());
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public virtual ActionResult ChangePasswordSuccess()
        {
            return View();
        }

    }
}
