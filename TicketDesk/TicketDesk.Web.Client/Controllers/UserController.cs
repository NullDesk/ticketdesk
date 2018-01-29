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

using System;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TicketDesk.Domain;
using TicketDesk.PushNotifications;
using TicketDesk.PushNotifications.Model;
using TicketDesk.Web.Client.Models;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;
using TicketDesk.Localization.Controllers;

namespace TicketDesk.Web.Client.Controllers
{
    [TdAuthorize]
    [RoutePrefix("user")]
    [Route("{action}")]
    public class UserController : Controller
    {
        private TicketDeskUserManager UserManager { get; set; }
        private TicketDeskSignInManager SignInManager { get; set; }
        private TdDomainContext DomainContext { get; set; }

        public UserController(
            TicketDeskUserManager userManager,
            TicketDeskSignInManager signInManager,
            TdDomainContext domainContext)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            DomainContext = domainContext;
        }

        [AllowAnonymous]
        [Route("register")]
        public ActionResult Register()
        {
            if (ApplicationConfig.RegisterEnabled)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("register")]
        public async Task<ActionResult> Register(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new TicketDeskUser { UserName = model.UserName, Email = model.Email, DisplayName = model.DisplayName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRolesAsync(user.Id, DomainContext.TicketDeskSettings.SecuritySettings.DefaultNewUserRoles.ToArray());
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    HostingEnvironment.QueueBackgroundWorkItem(ct =>
                    {
                        using(var notificationContext = new TdPushNotificationContext())
                        {
                            notificationContext.SubscriberPushNotificationSettingsManager.AddSettingsForSubscriber(
                                new SubscriberNotificationSetting
                                {
                                    SubscriberId = user.Id,
                                    IsEnabled = true,
                                    PushNotificationDestinations = new[]
                                    {
                                        new PushNotificationDestination()
                                        {
                                            DestinationType = "email",
                                            DestinationAddress = user.Email,
                                            SubscriberName = user.DisplayName
                                        }
                                    }
                                });
                            notificationContext.SaveChanges();
                        }
                    });

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        [Route("sign-in")]
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IsDemoMode = (ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"] ?? "false").Equals("true", StringComparison.InvariantCultureIgnoreCase);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("sign-in")]
        public async Task<ActionResult> SignIn(UserSignInViewModel model, string returnUrl)
        {
            ViewBag.IsDemoMode = (ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"] ?? "false").Equals("true", StringComparison.InvariantCultureIgnoreCase);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.UserNameOrEmail, model.Password, model.RememberMe, true);
            if (result != SignInStatus.Success && model.UserNameOrEmail.Contains("@"))
            {
                var user = await UserManager.FindByEmailAsync(model.UserNameOrEmail);
                if (user!=null)
                {
                    result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, true);
                }
            }

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
            }

            var domain = (DomainContext.TicketDeskSettings.SecuritySettings.DefaultLogonDomain ?? string.Empty).Trim();
            if (!string.IsNullOrEmpty(domain))
            {
                var user = await UserManager.FindByNameAsync(model.UserNameOrEmail);
                if (user == null)
                {
                    user = await UserManager.FindByNameAsync(model.UserNameOrEmail + "@" + domain);
                }

                if (user != null)
                {
                    var dsEntry = new DirectoryEntry("LDAP://" + domain)
                    {
                        Username = user.UserName,
                        Password = model.Password
                    };
                    var dsSearch = new DirectorySearcher(dsEntry);
                    dsSearch.Filter = "(objectclass=user)";
                    var OK = false;
                    try
                    {
                        OK = dsSearch.FindOne() != null;
                    }
                    catch { }

                    if (OK)
                    {
                        await UserManager.ResetAccessFailedCountAsync(user.Id);
                        await SignInManager.SignInAsync(user, model.RememberMe, true);
                        return RedirectToLocal(returnUrl);
                    }
                }
            }

            ModelState.AddModelError("", Strings.InvalidLoginAttempt);
            return View(model);
        }

        [Route("sign-out")]
        public ActionResult SignOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}