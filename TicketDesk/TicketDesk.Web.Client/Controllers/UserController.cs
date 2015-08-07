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
    [Authorize]
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
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("register")]
        public async Task<ActionResult> Register(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new TicketDeskUser { UserName = model.Email, Email = model.Email, DisplayName = model.DisplayName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    await UserManager.AddToRolesAsync(user.Id, DomainContext.TicketDeskSettings.SecuritySettings.DefaultNewUserRoles.ToArray());
                    HostingEnvironment.QueueBackgroundWorkItem(ct =>
                    {
                        using
                        (
                           var notificationContext = DependencyResolver.Current.GetService<TdPushNotificationContext>()
                        )
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
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("sign-in")]
        public async Task<ActionResult> SignIn(UserSignInViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                default:
                    ModelState.AddModelError("", Strings.InvalidLoginAttempt);
                    return View(model);
            }
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