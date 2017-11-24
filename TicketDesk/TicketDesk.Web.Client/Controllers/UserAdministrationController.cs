using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using TicketDesk.Domain;
using TicketDesk.Localization.Controllers;
using TicketDesk.PushNotifications;
using TicketDesk.PushNotifications.Model;
using TicketDesk.Web.Client.Models;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("admin")]
    [Route("{action=index}")]
    [TdAuthorize(Roles = "TdAdministrators")]
    public class UserAdministrationController : Controller
    {
        private TicketDeskUserManager UserManager { get; set; }
        private TicketDeskRoleManager RoleManager { get; set; }
        private TdDomainContext DomainContext { get; set; }

        public UserAdministrationController(
            TicketDeskUserManager userManager, 
            TicketDeskRoleManager roleManager,
            TdDomainContext domainContext)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            DomainContext = domainContext;
        }

        [Route("users/{page:int?}")]
        public async Task<ActionResult> Index(int? page)
        {
            ViewBag.AllRolesList = await RoleManager.Roles.ToListAsync();
            var users = await GetUsersForListAsync(page ?? 1);
            return View(users);
        }

        [Route("users/page/{page:int?}")]
        public async Task<ActionResult> PageList(int? page = 1)
        {
            ViewBag.AllRolesList = await RoleManager.Roles.ToListAsync();
            var users = await GetUsersForListAsync(page ?? 1);
            return PartialView("_UserList",users);

        }

        [Route("user/delete/{id?}")]
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            //does user to delete exist?
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            //checks for current user
            var currentUser = await UserManager.FindByIdAsync(this.User.Identity.GetUserId());
            if (user == currentUser)
            {
                ModelState.AddModelError("", Strings.CannotRemoveCurrentUser);
            }
            else
            {
                try
                {
                    UserManager.Delete(user);
                    return RedirectToAction("Index");
                }
                catch { }
                ModelState.AddModelError("", Strings.UnableToRemoveUser);
            }

            return RedirectToAction("Edit", new { id });
        }

        [Route("user/{id?}")]
        public async Task<ActionResult> Edit(string id)
        {
            //ViewBag.AllRolesList = await RoleManager.Roles.ToListAsync();
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            var model = new UserAccountInfoViewModel(user, user.Roles.Select(r => r.RoleId));
            var tdPendingRole = RoleManager.Roles.FirstOrDefault(r => r.Name == "TdPendingUsers");
            
            ViewBag.TdPendingUsersRoleId = tdPendingRole == null ? "" : tdPendingRole.Id; 
            return View(model);
        }

        [Route("user/{id?}")]
        [HttpPost]
        public async Task<ActionResult> Edit(UserAccountInfoViewModel model)
        {
            var tdPendingRole = RoleManager.Roles.FirstOrDefault(r => r.Name == "TdPendingUsers");
            ViewBag.TdPendingUsersRoleId = tdPendingRole == null ? "" : tdPendingRole.Id;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(model.User.Id);
            var demoMode = (ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"] ?? "false").Equals("true", StringComparison.InvariantCultureIgnoreCase);
            if (demoMode && model.User.Email.EndsWith("@example.com", StringComparison.InvariantCultureIgnoreCase))
            {
                ModelState.AddModelError("Profile", Strings.UnableToChangeDemoUser);
            }
            else
            {
                //deal with locks first
                if (await SetLockout(model, user))
                {
                    //do user info updates
                    if (await UpdateUserInfo(user, model))
                    {

                        //if any roles other than pending are selected, make sure we remove the pending role from the model first
                        if (tdPendingRole != null)
                        {
                            if (model.Roles.Any(r => r != tdPendingRole.Id))
                            {
                                var mRoles = model.Roles.ToList();
                                mRoles.Remove(tdPendingRole.Id);
                                model.Roles = mRoles;
                            }
                        }

                        //get role changes
                        var roleIdsToRemove = user.Roles.Select(ur => ur.RoleId).Except(model.Roles).ToList();

                        if (User.Identity.GetUserId() == user.Id)
                        {
                            //stop current user from removing their own admin role
                            roleIdsToRemove = roleIdsToRemove.Except(RoleManager.Roles.Where(r => r.Name == "tdAdministrators").Select(r => r.Id)).ToList();
                        }


                        var roleIdsToAdd = model.Roles.Except(user.Roles.Select(ur => ur.RoleId)).ToList();
                        //do role removes
                        if (await RemoveRoles(roleIdsToRemove, user))
                        {
                            //do role adds
                            if (await AddRoles(roleIdsToAdd, user))
                            {
                                //everything worked, return to index (if anything failed, will return to the edit user view)
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }               
            }
            //Since the above operations could be partially committed before a failure, 
            //  we will re-get the user so we are sure the screen will accurately show 
            //  the current data. Model errors will have been added by the respective 
            //  update operations that failed
            user = await UserManager.FindByIdAsync(model.User.Id);
            model = new UserAccountInfoViewModel(user, user.Roles.Select(r => r.RoleId));
            return View(model);
        }

        [Route("new")]
        public ActionResult New()
        {
            return View();
        }

        [Route("new")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new TicketDeskUser { UserName = model.UserName, Email = model.Email, DisplayName = model.DisplayName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRolesAsync(user.Id, DomainContext.TicketDeskSettings.SecuritySettings.DefaultNewUserRoles.ToArray());
                    HostingEnvironment.QueueBackgroundWorkItem(ct =>
                    {
                        using (var notificationContext = new TdPushNotificationContext())
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

                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private async Task<IPagedList<UserAccountInfoViewModel>> GetUsersForListAsync(int page)
        {
            var users = await UserManager.Users
                .OrderBy(u => u.DisplayName)
                .Select(u => new UserAccountInfoViewModel
                {
                    User = u,
                    Roles = u.Roles.Select(r => r.RoleId),
                    IsLocked = (u.LockoutEndDateUtc ?? DateTime.MinValue) > DateTime.UtcNow &&
                               (u.LockoutEndDateUtc ?? DateTime.MinValue) < DateTime.MaxValue,
                    IsDisabled = (u.LockoutEndDateUtc ?? DateTime.MinValue) == DateTime.MaxValue
                })
                .ToPagedListAsync(page, 25);
            return users;
        }

        private async Task<bool> UpdateUserInfo(TicketDeskUser user, UserAccountInfoViewModel model)
        {
            user.UserName = model.User.UserName;
            user.Email = model.User.Email;
            user.DisplayName = model.User.DisplayName;
            var result = await UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                AddErrors(result);
            }
            return result.Succeeded;
        }

        private async Task<bool> AddRoles(IEnumerable<string> roleIdsToAdd, TicketDeskUser user)
        {
            var roleNamesToAdd = RoleManager.Roles.Where(r => roleIdsToAdd.Any(ri => ri == r.Id)).Select(r => r.Name);
            if (roleNamesToAdd.Any())
            {
                var result = await UserManager.AddToRolesAsync(user.Id, await roleNamesToAdd.ToArrayAsync());
                if (!result.Succeeded)
                {
                    AddErrors(result);
                }
                return result.Succeeded;
            }
            return true;
        }

        private async Task<bool> RemoveRoles(IEnumerable<string> roleIdsToRemove, TicketDeskUser user)
        {
           
            var roleNamesToRemove = RoleManager.Roles.Where(r => roleIdsToRemove.Any(ri => ri == r.Id)).Select(r => r.Name);
            if (roleNamesToRemove.Any())
            {
                var result = await UserManager.RemoveFromRolesAsync(user.Id, await roleNamesToRemove.ToArrayAsync());
                if (!result.Succeeded)
                {
                    AddErrors(result);
                }
                return result.Succeeded;
            }
            return true;
        }

        private async Task<bool> SetLockout(UserAccountInfoViewModel model, TicketDeskUser user)
        {
            var currentlyLocked = UserManager.IsLockedOut(model.User.Id);
            var currentlyDisabled = currentlyLocked && (user.LockoutEndDateUtc ?? DateTime.MinValue).ToUniversalTime() == DateTime.MaxValue.ToUniversalTime();

            if (currentlyDisabled != model.IsDisabled || currentlyLocked != model.IsLocked)
            {
                DateTimeOffset newLockoutDate = DateTimeOffset.MinValue;
                if (model.IsDisabled)
                {
                    newLockoutDate = DateTimeOffset.MaxValue;
                }
                else if (model.IsLocked)
                {
                    newLockoutDate = DateTimeOffset.Now.Add(UserManager.DefaultAccountLockoutTimeSpan);
                }

                var result = await UserManager.SetLockoutEndDateAsync(model.User.Id, newLockoutDate);
                if (!result.Succeeded)
                {
                    AddErrors(result);
                }
                return result.Succeeded;
            }
            return true;
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