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
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Web.Client.Controllers;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Services;
using TicketDesk.Web.Client.Helpers;
using TicketDesk.Domain.Utilities.Pagination;
using TicketDesk.Domain.Models;
using System.Web.Security;
using TicketDesk.Web.Client.Areas.Admin.Models;
using TicketDesk.Web.Client.Models;
using System.Configuration;

namespace TicketDesk.Web.Client.Areas.Admin.Controllers
{
    [Export("SecurityManagement", typeof(IController))]
    [AuthorizeAdminOnly]
    [SqlSecurityOnly]
    public partial class SecurityManagementController : ApplicationController
    {
        [ImportingConstructor]
        public SecurityManagementController(ISecurityService security)
            : base(security)
        {

        }

        public virtual ActionResult Index()
        {
            
            return View();
        }
        public virtual ActionResult UsersList(int page = 1)
        {
            var repository = Security.Repository as TicketDesk.Domain.Repositories.SqlSecurityRepository;
            var membership = repository.MembershipSource;
            int totalRecs;
            var m = membership.GetAllUsers(page - 1, 25, out totalRecs);
            List<SecurityManagementUserViewModel> model = new List<SecurityManagementUserViewModel>();
            foreach (var u in m.Cast<MembershipUser>())
            {
                model.Add(CreateViewModelFromUser(u));
            }


            var cp = new CustomPagination<SecurityManagementUserViewModel>(model, page, 25, totalRecs);

            return View(cp);
        }

        private SecurityManagementUserViewModel CreateViewModelFromUser(MembershipUser u)
        {
            return new SecurityManagementUserViewModel()
            {
                DisplayName = u.Comment ?? string.Empty,
                Email = u.Email,
                UserName = u.UserName,
                IsAdmin = Security.IsTdAdmin(u.UserName),
                IsStaff = Security.IsTdStaff(u.UserName),
                IsSubmitter = Security.IsTdSubmitter(u.UserName),
                IsApproved = u.IsApproved,
                IsLockedOut = u.IsLockedOut
            };

        }

        public virtual ActionResult EditUser(string userName)
        {
            var repository = Security.Repository as TicketDesk.Domain.Repositories.SqlSecurityRepository;
            var membership = repository.MembershipSource;
            var m = membership.GetUser(userName, false);
            if (m != null)
            {
                return View(CreateViewModelFromUser(m));
            }
            else
            {
                return RedirectToAction(MVC.Admin.SecurityManagement.UsersList());
            }
        }

        [HttpPost]
        [ValidateOnlyIncomingValues]
        public virtual ActionResult EditUser(SecurityManagementUserViewModel user)
        {
            var repository = Security.Repository as TicketDesk.Domain.Repositories.SqlSecurityRepository;
            var membership = repository.MembershipSource;
            var m = membership.GetUser(user.UserName, false);
            if (ModelState.IsValid)
            {
                m.Comment = user.DisplayName;
                m.Email = user.Email;
                m.IsApproved = user.IsApproved;
                if(m.IsLockedOut && !user.IsLockedOut)
                {
                    m.UnlockUser();
                }
                
                membership.UpdateUser(m);

                if (Security.IsTdAdmin(user.UserName) != user.IsAdmin)
                {
                    if (user.IsAdmin)
                    {
                        Security.AddUserToTdAdmin(user.UserName);
                    }
                    else
                    {
                        Security.RemoveUserFromTdAdmin(user.UserName);
                    }
                }
                if (Security.IsTdStaff(user.UserName) != user.IsStaff)
                {
                    if (user.IsStaff)
                    {
                        Security.AddUserToTdStaff(user.UserName);
                    }
                    else
                    {
                        Security.RemoveUserFromTdStaff(user.UserName);
                    }
                }
                if (Security.IsTdSubmitter(user.UserName) != user.IsSubmitter)
                {
                    if (user.IsSubmitter)
                    {
                        Security.AddUserToTdSubmitter(user.UserName);
                    }
                    else
                    {
                        Security.RemoveUserFromTdSubmitter(user.UserName);
                    }
                }
                return RedirectToAction(MVC.Admin.SecurityManagement.UsersList());
            }
            else
            {
                return View(user);
            }
        }

        public virtual ActionResult CreateUser()
        {
            var model = new SecurityManagementUserViewModel() { IsApproved = true, IsSubmitter = true };
            return View(model);
        }

        [HttpPost]
        [ValidateOnlyIncomingValues]
        public virtual ActionResult CreateUser(SecurityManagementUserViewModel user)
        {
            var repository = Security.Repository as TicketDesk.Domain.Repositories.SqlSecurityRepository;
            var membership = repository.MembershipSource;

            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                var u = membership.CreateUser(user.UserName, user.Password, user.Email, null, null, true, null, out createStatus);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    u.Comment = user.DisplayName;
                    membership.UpdateUser(u);
                    if (Security.IsTdAdmin(user.UserName) != user.IsAdmin)
                    {
                        if (user.IsAdmin)
                        {
                            Security.AddUserToTdAdmin(user.UserName);
                        }
                        else
                        {
                            Security.RemoveUserFromTdAdmin(user.UserName);
                        }
                    }
                    if (Security.IsTdStaff(user.UserName) != user.IsStaff)
                    {
                        if (user.IsStaff)
                        {
                            Security.AddUserToTdStaff(user.UserName);
                        }
                        else
                        {
                            Security.RemoveUserFromTdStaff(user.UserName);
                        }
                    }
                    if (Security.IsTdSubmitter(user.UserName) != user.IsSubmitter)
                    {
                        if (user.IsSubmitter)
                        {
                            Security.AddUserToTdSubmitter(user.UserName);
                        }
                        else
                        {
                            Security.RemoveUserFromTdSubmitter(user.UserName);
                        }
                    }
                    return RedirectToAction(MVC.Admin.SecurityManagement.UsersList());
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));

                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public virtual ActionResult DeleteUser(string userName)
        {

            var repository = Security.Repository as TicketDesk.Domain.Repositories.SqlSecurityRepository;
            var membership = repository.MembershipSource;
            var m = membership.GetUser(userName, false);
            if (m != null)
            {
                return View(CreateViewModelFromUser(m));
            }
            else
            {
                return RedirectToAction(MVC.Admin.SecurityManagement.UsersList());
            }
        }


        [HttpPost]
        [ValidateOnlyIncomingValues]
        public virtual ActionResult DeleteUser(SecurityManagementUserViewModel user)
        {
            var repository = Security.Repository as TicketDesk.Domain.Repositories.SqlSecurityRepository;
            var membership = repository.MembershipSource;

            if (!membership.DeleteUser(user.UserName, true))
            {
                ModelState.AddModelError("deleteUser", "Unable to delete user account");
                return View(user);
            }
            else
            {
                return RedirectToAction(MVC.Admin.SecurityManagement.UsersList());
            }
        }

        public virtual ActionResult RolesList()
        {
            return new EmptyResult();
        }
    }
}
