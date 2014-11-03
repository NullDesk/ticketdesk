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
using System.ComponentModel.Composition;
using TicketDesk.Domain.Models;
using TicketDesk.Domain.Repositories;
using System.Reflection;

namespace TicketDesk.Domain.Services
{
    [Export(typeof(ISecurityService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SecurityService : ISecurityService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TdSecurityService"/> class.
        /// </summary>
        /// <param name="securityRepository">The security repository.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="staffRoleName">Name of the staff role.</param>
        /// <param name="submitterRoleName">Name of the submitter role.</param>
        /// <param name="adminRoleName">Name of the admin role.</param>
        [ImportingConstructor]
        public SecurityService
            (
                [ImportMany(typeof(ISecurityRepository))] ISecurityRepository[] repositories,
                [ImportMany("RefreshSecurityCache")] Action[] refreshSecurityCacheMethods,
                [Import("RefreshSecurityCacheMinutes")] int refreshSecurityCacheMinutes,
                [Import("RuntimeSecurityMode")] Func<string> getSecurityModeMethod,
                [Import("CurrentUserNameMethod")] Func<string> getCurrentUserNameMethod,
                [Import("StaffRoleName")] string staffRoleName,
                [Import("SubmitterRoleName")] string submitterRoleName,
                [Import("AdminRoleName")] string adminRoleName
            )
        {
            Repositories = repositories;
            RefreshSecurityCacheMethods = refreshSecurityCacheMethods;
            RefreshSecurityCacheMinutes = refreshSecurityCacheMinutes;
            GetCurrentUserName = getCurrentUserNameMethod;
            GetSecurityMode = getSecurityModeMethod;
            TdStaffRoleName = staffRoleName;
            TdSubmittersRoleName = submitterRoleName;
            TdAdminRoleName = adminRoleName;
        }

        public Func<string> GetCurrentUserName { get; set; }
        private Func<string> GetSecurityMode { get; set; }
        private int RefreshSecurityCacheMinutes { get; set; }

        private Action[] RefreshSecurityCacheMethods { get; set; }
        private Action RefreshSecurityCacheMethod
        {
            get
            {
                Action meth = null;
                var sec = GetSecurityMode();
                foreach (var r in RefreshSecurityCacheMethods)
                {

                    foreach (Attribute attribute in r.Method.GetCustomAttributes(false))
                    {
                        ExportMetadataAttribute emAttribute = attribute as ExportMetadataAttribute;
                        if (emAttribute != null && emAttribute.Name == "SecurityMode" && emAttribute.Value.Equals(sec))
                        {
                            meth = r;
                            break;
                        }
                    }
                    if (meth != null)
                    {
                        break;
                    }
                }
                return meth;
            }
        }


        public ISecurityRepository[] Repositories { get; private set; }
        public ISecurityRepository Repository
        {
            get
            {
                ISecurityRepository repos = null;
                var sec = GetSecurityMode();
                foreach (var r in Repositories)
                {
                    foreach (Attribute attribute in r.GetType().GetCustomAttributes(false))
                    {
                        ExportMetadataAttribute emAttribute = attribute as ExportMetadataAttribute;
                        if (emAttribute != null && emAttribute.Name == "SecurityMode" && emAttribute.Value.Equals(sec))
                        {
                            repos = r;
                            break;
                        }
                    }
                    if (repos != null)
                    {
                        break;
                    }
                }
                return repos;
            }
        }


        private string TdStaffRoleName { get; set; }
        private string TdSubmittersRoleName { get; set; }
        private string TdAdminRoleName { get; set; }

        #region ISecurityService Members

        private System.Timers.Timer CacheRefreshTimer = null;
        /// <summary>
        /// Initializes the security cache refresh timer. The caller should maintain a reference to the timer object to prevent it from disposing.
        /// </summary>
        /// <returns>The timer instance responsible for refreshing the cache, null if no timer is created/needed.</returns>
        public System.Timers.Timer InitializeSecurityCacheRefreshTimer()
        {

            if (RefreshSecurityCacheMethod != null)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() => RefreshSecurityCacheMethod());//go ahead and spin out a run of the cache system right now.

                CacheRefreshTimer = new System.Timers.Timer();
                int CacheRefreshInterval = RefreshSecurityCacheMinutes * 60 * 1000;
                CacheRefreshTimer.Elapsed += new System.Timers.ElapsedEventHandler(CacheRefreshTimer_Elapsed);
                CacheRefreshTimer.Interval = CacheRefreshInterval;
                CacheRefreshTimer.AutoReset = false;//we'll manually restart the timer

                CacheRefreshTimer.Start();
            }
            return CacheRefreshTimer;
        }

        private void CacheRefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (RefreshSecurityCacheMethod != null)
            {
                CacheRefreshTimer.Stop();
                try
                {
                    RefreshSecurityCacheMethod();
                }
                catch (Exception ex) { throw ex; }
                finally
                {
                    CacheRefreshTimer.Start();
                }
            }
        }


        /// <summary>
        /// Gets the submitter users.
        /// </summary>
        /// <returns></returns>
        public UserInfo[] GetTdSubmitterUsers()
        {
            return Repository.GetUsersInRole(TdSubmittersRoleName);
        }

        /// <summary>
        /// Gets the staff users.
        /// </summary>
        /// <returns></returns>
        public UserInfo[] GetTdStaffUsers()
        {
            return Repository.GetUsersInRole(TdStaffRoleName);
        }

        /// <summary>
        /// Gets the admin users.
        /// </summary>
        /// <returns></returns>
        public UserInfo[] GetTdAdminUsers()
        {
            return Repository.GetUsersInRole(TdAdminRoleName);
        }

        /// <summary>
        /// Determines whether user is in the staff role.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if user in staff role; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTdStaff()
        {
            return IsTdStaff(CurrentUserName);
        }

        /// <summary>
        /// Determines whether user is in the staff role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// 	<c>true</c> if user in staff role; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTdStaff(string userName)
        {
            return Repository.IsUserInRoleName(userName, TdStaffRoleName);
        }

        /// <summary>
        /// Determines whether user is in the submitter role.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if user in submitter role; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTdSubmitter()
        {
            return IsTdSubmitter(CurrentUserName);
        }

        /// <summary>
        /// Determines whether user is in the submitter role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// 	<c>true</c> if user in submitter role; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTdSubmitter(string userName)
        {
            return Repository.IsUserInRoleName(userName, TdSubmittersRoleName);
        }

        /// <summary>
        /// Determines whether user is in the admin role.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if user is in admin role; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTdAdmin()
        {
            return IsTdAdmin(CurrentUserName);
        }

        /// <summary>
        /// Determines whether user is in the admin role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// 	<c>true</c> if user is in admin role; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTdAdmin(string userName)
        {
            return Repository.IsUserInRoleName(userName, TdAdminRoleName);
        }

        /// <summary>
        /// Gets the display name of the current user.
        /// </summary>
        /// <returns></returns>
        public string GetUserDisplayName()
        {
            return GetUserDisplayName(CurrentUserName);
        }

        /// <summary>
        /// Gets the formatted name of the current user.
        /// </summary>
        /// <value>The name of the current user.</value>
        public string CurrentUserName
        {
            get
            {
                return Repository.FormatUserName(GetCurrentUserName());
            }
        }


        /// <summary>
        /// Gets the display name of the specified user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public string GetUserDisplayName(string userName)
        {
            return Repository.GetUserDisplayName(userName);
        }


        /// <summary>
        /// Determines whether user is in a valid td role.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if user is in valid td role; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInValidTdUserRole()
        {
            return (IsTdAdmin() || IsTdStaff() || IsTdSubmitter());
        }

        /// <summary>
        /// Determines whether user is in a valid td role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// 	<c>true</c> if user is in valid td role; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInValidTdUserRole(string userName)
        {
            return (IsTdAdmin(userName) || IsTdStaff(userName) || IsTdSubmitter(userName));

        }

        #endregion

      

        public string GetUserEmailAddress(string userName)
        {
            return Repository.GetUserEmailAddress(userName);
        }

        


        public void AddUserToTdAdmin(string userName)
        {
            Repository.AddUserToRole(userName, TdAdminRoleName);
        }

        public void AddUserToTdSubmitter(string userName)
        {
            Repository.AddUserToRole(userName, TdSubmittersRoleName);
        }

        public void AddUserToTdStaff(string userName)
        {
            Repository.AddUserToRole(userName, TdStaffRoleName);
        }

        public void RemoveUserFromTdAdmin(string userName)
        {
            Repository.RemoveUserFromRole(userName, TdAdminRoleName);
        }

        public void RemoveUserFromTdSubmitter(string userName)
        {
            Repository.RemoveUserFromRole(userName, TdSubmittersRoleName);
        }

        public void RemoveUserFromTdStaff(string userName)
        {
            Repository.RemoveUserFromRole(userName, TdStaffRoleName);
        }
    }
}
