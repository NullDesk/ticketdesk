using System;
using TicketDesk.Domain.Model;

namespace TicketDesk.Domain
{
    /// <summary>
    /// Provides the minimal set of security services to support business logic 
    /// </summary>
    public abstract class TicketDeskContextSecurityProviderBase
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
            get { return GetIsTdHelpDeskUser(GetCurrentUserId()); }
        }

        public bool IsTdAdministrator
        {
            get { return GetIsTdHelpDeskUser(GetCurrentUserId()); }
        }

        public bool IsTdPendingUser
        {
            get { return GetIsTdHelpDeskUser(GetCurrentUserId()); }
        }

        public virtual bool IsTicketActivityValid(Ticket ticket, TicketActivity activity)
        {
            //TODO: This mixes pure security with checks against checks for ticket's general state --I don't like the mix of concerns
            bool isAllowed = false;
            if (!string.IsNullOrEmpty(GetCurrentUserId()) && (IsTdAdministrator || IsTdHelpDeskUser || IsTdInternalUser))
            {
                bool isAssignedToMe = (!string.IsNullOrEmpty(ticket.AssignedTo) &&
                                       ticket.AssignedTo == GetCurrentUserId());
                bool isOwnedByMe = (ticket.Owner == GetCurrentUserId());
                bool isMoreInfo = (ticket.TicketStatus == TicketStatus.MoreInfo);
                bool isResolved = (ticket.TicketStatus == TicketStatus.Resolved);


                switch (activity)
                {
                    case TicketActivity.NoChange:
                        isAllowed = true;
                        break;
                    case TicketActivity.GetTicketInfo:
                        isAllowed = true;
                        break;
                    case TicketActivity.Create:
                        isAllowed = true;
                        break;
                    case TicketActivity.CreateOnBehalfOf:
                        //TODO: This doesn't look right, was there a bug in TD2.1x?
                        isAllowed = true;
                        break;
                    case TicketActivity.ModifyAttachments:
                        isAllowed = ticket.IsOpen;
                        break;
                    case TicketActivity.EditTicketInfo:
                        isAllowed = ticket.IsOpen && (IsTdHelpDeskUser || isOwnedByMe);
                        break;
                    case TicketActivity.AddComment:
                        isAllowed = ticket.IsOpen && !isMoreInfo;
                        break;
                    case TicketActivity.SupplyMoreInfo:
                        isAllowed = isMoreInfo;
                        break;
                    case TicketActivity.Resolve:
                        isAllowed = ticket.IsOpen && !isMoreInfo && isAssignedToMe;
                        break;
                    case TicketActivity.RequestMoreInfo:
                        isAllowed = ticket.IsOpen && !isMoreInfo && isAssignedToMe;
                        break;
                    case TicketActivity.CancelMoreInfo:
                        isAllowed = isMoreInfo && isAssignedToMe;
                        break;
                    case TicketActivity.Close:
                        isAllowed = isResolved && isOwnedByMe;
                        break;
                    case TicketActivity.ReOpen:
                        isAllowed = !ticket.IsOpen;
                        break;
                    case TicketActivity.TakeOver:
                        isAllowed = (ticket.IsOpen || isResolved) && !isAssignedToMe && IsTdHelpDeskUser;
                        break;
                    case TicketActivity.TakeOverWithPriority:
                        isAllowed = (ticket.IsOpen || isResolved) && !isAssignedToMe && IsTdHelpDeskUser;
                        break;
                    case TicketActivity.Assign:
                        isAllowed = (ticket.IsOpen || isResolved) && IsTdHelpDeskUser && !ticket.IsAssigned;
                        break;
                    case TicketActivity.AssignWithPriority:
                        isAllowed = (ticket.IsOpen || isResolved) && IsTdHelpDeskUser && !ticket.IsAssigned;
                        break;
                    case TicketActivity.ReAssign:
                        isAllowed = (ticket.IsOpen || isResolved) && IsTdHelpDeskUser && ticket.IsAssigned &&
                                    !isAssignedToMe;
                        break;
                    case TicketActivity.ReAssignWithPriority:
                        isAllowed = (ticket.IsOpen || isResolved) && IsTdHelpDeskUser && ticket.IsAssigned &&
                                    !isAssignedToMe;
                        break;
                    case TicketActivity.Pass:
                        isAllowed = (ticket.IsOpen || isResolved) && IsTdHelpDeskUser && isAssignedToMe;
                        break;
                    case TicketActivity.PassWithPriority:
                        isAllowed = (ticket.IsOpen || isResolved) && IsTdHelpDeskUser && isAssignedToMe;
                        break;
                    case TicketActivity.GiveUp:
                        isAllowed = (ticket.IsOpen || isResolved) && isAssignedToMe;
                        break;
                    case TicketActivity.ForceClose:
                        isAllowed = (ticket.IsOpen || isResolved) && (isAssignedToMe || isOwnedByMe) &&
                                    !(isResolved && isOwnedByMe);
                        break;
                }
            }
            return isAllowed;
        }
    }
}
