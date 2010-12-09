using TicketDesk.Domain.Models;
using System.Web.Security;

namespace TicketDesk.Domain.Repositories
{
    public interface ISecurityRepository
    {

        /// <summary>
        /// Gets the users in a role.
        /// </summary>
        /// <param name="roleType">Name of the role type.</param>
        /// <returns></returns>
        UserInfo[] GetUsersInRole(string roleName);
        RoleProvider RoleSource { get; }
        /// <summary>
        /// Determines whether the the specified user name is in the role type .
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="roleType">Name of the role type.</param>
        /// <returns>
        /// 	<c>true</c> if the specified user name is in the role; otherwise, <c>false</c>.
        /// </returns>
        bool IsUserInRoleName(string userName, string roleName);

        /// <summary>
        /// Gets the display name for the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        string GetUserDisplayName(string userName);

        /// <summary>
        /// Formats the name of the user for use in TicketDesk.
        /// </summary>
        /// <param name="unformattedName">The unformatted username (usually from an authentication provider or IPrincipal).</param>
        /// <returns></returns>
        string FormatUserName(string unformattedName);

        string GetUserEmailAddress(string userName);

        void AddUserToRole(string userName, string roleName);
        void RemoveUserFromRole(string userName, string roleName);

    }
}
