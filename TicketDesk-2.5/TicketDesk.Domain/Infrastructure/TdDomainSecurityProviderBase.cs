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
using System.Security;
using TicketDesk.Domain.Model;

namespace TicketDesk.Domain
{
    /// <summary>
    /// Provides the minimal set of security services to support business logic 
    /// </summary>
    public abstract class TdDomainSecurityProviderBase
    {
        protected abstract Func<string> GetCurrentUserId { get; }

        protected abstract Func<string, bool> GetIsTdHelpDeskUser { get; }

        protected abstract Func<string, bool> GetIsTdInternalUser { get; }

        protected abstract Func<string, bool> GetIsTdAdministrator { get; }

        protected abstract Func<string, bool> GetIsTdPendingUser { get; }

        public abstract Func<string, string> GetUserDisplayName { get; }

        public string CurrentUserId
        {
            get { return GetCurrentUserId(); }
        }

        public string CurrentUserDisplayName
        {
            get { return GetUserDisplayName(GetCurrentUserId()); }
        }

        public bool IsTdHelpDeskUser
        {
            get { return GetIsTdHelpDeskUser(GetCurrentUserId()); }
        }

        public bool IsTdInternalUser
        {
            get { return GetIsTdInternalUser(GetCurrentUserId()); }
        }

        public bool IsTdAdministrator
        {
            get { return GetIsTdAdministrator(GetCurrentUserId()); }
        }

        public bool IsTdPendingUser
        {
            get { return GetIsTdPendingUser(GetCurrentUserId()); }
        }

        internal TicketActivity GetInternalUserPermissions()
        {
            return
                TicketActivity.ModifyAttachments |
                TicketActivity.EditTicketInfo |
                TicketActivity.SupplyMoreInfo |
                TicketActivity.ReOpen |
                TicketActivity.Create |
                TicketActivity.AddComment |
                TicketActivity.Close |
                TicketActivity.ForceClose;
        }

        internal TicketActivity GetHelpDeskUserPermissions()
        {
            return GetInternalUserPermissions() |
                TicketActivity.RequestMoreInfo |
                TicketActivity.CancelMoreInfo |
                TicketActivity.CreateOnBehalfOf |
                TicketActivity.TakeOver |
                TicketActivity.Assign |
                TicketActivity.ReAssign |
                TicketActivity.Pass |
                TicketActivity.GiveUp |
                TicketActivity.Resolve;
        }

        internal TicketActivity GetAdministratorUserPermissions()
        {
            return GetInternalUserPermissions() | GetHelpDeskUserPermissions();
        }

        public TicketActivity GetValidTicketActivities(Ticket ticket)
        {
            if (!(IsTdAdministrator || IsTdHelpDeskUser || IsTdInternalUser))
            {
                throw new SecurityException("User is not authorized to read ticket data.");
            }
            var validTicketActivities = ticket.GetAvailableActivites(GetCurrentUserId());
            var allowedActivities = IsTdAdministrator
                ? GetAdministratorUserPermissions()
                : IsTdHelpDeskUser
                    ? GetHelpDeskUserPermissions()
                    : GetInternalUserPermissions();
            return (validTicketActivities & allowedActivities);
        }

        public bool IsTicketActivityValid(Ticket ticket, TicketActivity activity)
        {
            return GetValidTicketActivities(ticket).HasFlag(activity);
        }
    }
}
