using TicketDesk.Domain.Models;
using System;

namespace TicketDesk.Domain.Services
{
    public interface ISecurityService
    {

        /// <summary>
        /// Gets the submitter users.
        /// </summary>
        /// <returns></returns>
        UserInfo[] GetTdSubmitterUsers();

        /// <summary>
        /// Gets the staff users.
        /// </summary>
        /// <returns></returns>
        UserInfo[] GetTdStaffUsers();

        /// <summary>
        /// Gets the admin users.
        /// </summary>
        /// <returns></returns>
        UserInfo[] GetTdAdminUsers();

        /// <summary>
        /// Determines whether user is in the staff role.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if user in staff role; otherwise, <c>false</c>.
        /// </returns>
        bool IsTdStaff();


        /// <summary>
        /// Determines whether user is in the staff role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// 	<c>true</c> if user in staff role; otherwise, <c>false</c>.
        /// </returns>
        bool IsTdStaff(string userName);

        /// <summary>
        /// Determines whether user is in the submitter role.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if user in submitter role; otherwise, <c>false</c>.
        /// </returns>
        bool IsTdSubmitter();

        /// <summary>
        /// Determines whether user is in the submitter role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// 	<c>true</c> if user in submitter role; otherwise, <c>false</c>.
        /// </returns>
        bool IsTdSubmitter(string userName);

        /// <summary>
        /// Determines whether user is in the admin role.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if user is in admin role; otherwise, <c>false</c>.
        /// </returns>
        bool IsTdAdmin();


        /// <summary>
        /// Determines whether user is in the admin role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// 	<c>true</c> if user is in admin role; otherwise, <c>false</c>.
        /// </returns>
        bool IsTdAdmin(string userName);


        /// <summary>
        /// Determines whether user is in a valid td role.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if user is in valid td role; otherwise, <c>false</c>.
        /// </returns>
        bool IsInValidTdUserRole();

        /// <summary>
        /// Determines whether user is in a valid td role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// 	<c>true</c> if user is in valid td role; otherwise, <c>false</c>.
        /// </returns>
        bool IsInValidTdUserRole(string userName);

        /// <summary>
        /// Gets the display name of the current user.
        /// </summary>
        /// <returns></returns>
        string GetUserDisplayName();

        string CurrentUserName { get; }
        Func<string> GetCurrentUserName { get; set; }

        /// <summary>
        /// Gets the display name of the specified user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        string GetUserDisplayName(string userName);


        string GetUserEmailAddress(string userName);
    }
}
