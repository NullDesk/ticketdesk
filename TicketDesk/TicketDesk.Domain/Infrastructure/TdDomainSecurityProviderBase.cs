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
using System.Security;
using TicketDesk.Domain.Model;

namespace TicketDesk.Domain
{
    /// <summary>
    /// Provides the minimal set of security services to support business logic 
    /// </summary>
    public abstract class TdDomainSecurityProviderBase
    {

        protected abstract Func<string, bool> GetIsTdHelpDeskUser { get; }

        protected abstract Func<string, bool> GetIsTdInternalUser { get; }

        protected abstract Func<string, bool> GetIsTdAdministrator { get; }

        protected abstract Func<string, bool> GetIsTdPendingUser { get; }

        public abstract Func<string, string> GetUserDisplayName { get; }

        public abstract string CurrentUserId { get; set; }

        public string CurrentUserDisplayName
        {
            get { return GetUserDisplayName(CurrentUserId); }
        }

        public bool IsTdHelpDeskUser
        {
            get { return GetIsTdHelpDeskUser(CurrentUserId); }
        }

        public bool IsTdInternalUser
        {
            get { return GetIsTdInternalUser(CurrentUserId); }
        }

        public bool IsTdAdministrator
        {
            get { return GetIsTdAdministrator(CurrentUserId); }
        }

        public bool IsTdPendingUser
        {
            get { return GetIsTdPendingUser(CurrentUserId); }
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
            var validTicketActivities = ticket.GetAvailableActivites(CurrentUserId);
            var allowedActivities = IsTdAdministrator
                ? GetAdministratorUserPermissions()
                : IsTdHelpDeskUser
                    ? GetHelpDeskUserPermissions()
                    : GetInternalUserPermissions();
            if (IsTdAdministrator)
            {
                //// Enables administrators to act upon ticket as the owner.
                validTicketActivities |= ticket.GetAvailableActivites(ticket.Owner);
            }

            return (validTicketActivities & allowedActivities);
        }

        public bool IsTicketActivityValid(Ticket ticket, TicketActivity activity)
        {
            return GetValidTicketActivities(ticket).HasFlag(activity);
        }
    }
}
