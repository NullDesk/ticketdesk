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
using System.Text;
using TicketDesk.Domain.Model;
using TicketDesk.Localization.Domain;

namespace TicketDesk.Domain
{
    public class TicketActionManager
    {

        //TODO: we should enforce required comments on the back-end

        public static TicketActionManager GetInstance(TdDomainSecurityProviderBase securityProvider)
        {
            return new TicketActionManager(securityProvider);
        }

        TdDomainSecurityProviderBase SecurityProvider { get; set; }
        internal TicketActionManager(TdDomainSecurityProviderBase securityProvider)
        {
            SecurityProvider = securityProvider;
        }

        /// <summary>
        /// Determines whether the ticket activity is valid for the specified ticket.
        /// </summary>
        /// <remarks>
        /// This is a convienience method to provide a more natual api for the client. 
        /// It just calls the equivalent method from the security provider.
        /// </remarks>
        /// <param name="ticket">The ticket.</param>
        /// <param name="activity">The activity.</param>
        /// <returns><c>true</c> if the ticket activity valid for the specified ticket; otherwise, <c>false</c>.</returns>
        public bool IsTicketActivityValid(Ticket ticket, TicketActivity activity)
        {
            return SecurityProvider.IsTicketActivityValid(ticket, activity);
        }

        /// <summary>
        /// Gets the valid ticket activities.
        /// </summary>
        /// <remarks>
        /// This is a convienience method to provide a more natual api for the client. 
        /// It just calls the equivalent method from the security provider.
        /// </remarks>
        /// <param name="ticket">The ticket.</param>
        /// <returns>TicketActivity.</returns>
        public TicketActivity GetValidTicketActivities(Ticket ticket)
        {
            return SecurityProvider.GetValidTicketActivities(ticket);
        }


        public Action<Ticket> AddComment(string comment)
        {
            const TicketActivity activity = TicketActivity.AddComment;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment);
                }
            };
        }

        public Action<Ticket> Assign(string comment, string assignTo, string priority)
        {
            return ChangeAssignment(comment, assignTo, priority, TicketActivity.Assign);
        }


        public Action<Ticket> CancelMoreInfo(string comment)
        {
            const TicketActivity activity = TicketActivity.CancelMoreInfo;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    ticket.TicketStatus = TicketStatus.Active;

                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment);
                }
            };
        }

        public Action<Ticket> Close(string comment)
        {
            const TicketActivity activity = TicketActivity.Close;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    ticket.TicketStatus = TicketStatus.Closed;
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment);
                }
            };
        }

        public Action<Ticket> EditTicketInfo(
            string comment,
//            int projectId,
//            string projectName,
            string title,
            string details,
            string priority,
            string ticketType,
            string category,
            string tagList,
            string subCategory,
            ApplicationSetting settings)
        {
            const TicketActivity activity = TicketActivity.EditTicketInfo;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    if (ticket.Title != title && title != "")
                    {
                        ticket.Title = title;
                    }
                    if (ticket.Details != details && details != "")
                    {
                        ticket.Details = details;
                    }
                    if ((SecurityProvider.IsTdHelpDeskUser || settings.Permissions.AllowInternalUsersToEditTags ) && ticket.TagList != tagList && tagList != "")
                    {
                        ticket.TagList = tagList;
                    }
                    if ((SecurityProvider.IsTdHelpDeskUser || settings.Permissions.AllowInternalUsersToEditPriority) && ticket.Priority != priority && priority != "")
                    {
                        ticket.Priority = priority;
                    }
                    if (ticket.TicketType != ticketType && ticketType != "")
                    {
                        ticket.TicketType = ticketType;
                    }
                    if (ticket.Category != category && category != "")
                    {
                        ticket.Category = category;
                    }
                    if (ticket.SubCategory != subCategory && subCategory != "")
                    {
                        ticket.SubCategory = subCategory;
                    }
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment);
                }
            };
        }

        public Action<Ticket> ForceClose(string comment)
        {
            const TicketActivity activity = TicketActivity.ForceClose;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    ticket.TicketStatus = TicketStatus.Closed;
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment);
                }
            };
        }

        public Action<Ticket> GiveUp(string comment)
        {
            const TicketActivity activity = TicketActivity.GiveUp;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    ticket.AssignedTo = null;
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment);

                }
            };
        }

        public Action<Ticket> ModifyAttachments(string comment)
        {
            //The vast majority of attachements management occur outside the business domain, so all we do here is log the activity event
            const TicketActivity activity = TicketActivity.ModifyAttachments;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment);
                }
            };
        }

        public Action<Ticket> Pass(string comment, string assignTo, string priority)
        {
            return ChangeAssignment(comment, assignTo, priority, TicketActivity.Pass);
        }

        public Action<Ticket> ReAssign(string comment, string assignTo, string priority)
        {
            return ChangeAssignment(comment, assignTo, priority, TicketActivity.ReAssign);
        }

        public Action<Ticket> RequestMoreInfo(string comment)
        {
            const TicketActivity activity = TicketActivity.RequestMoreInfo;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    ticket.TicketStatus = TicketStatus.MoreInfo;

                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment);
                }
            };
        }

        public Action<Ticket> ReOpen(string comment, bool assignToMe)
        {
            const TicketActivity activity = TicketActivity.ReOpen;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    ticket.AssignedTo = assignToMe ? SecurityProvider.CurrentUser.userName : null;
                    ticket.TicketStatus = TicketStatus.Active;
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment);
                }
            };
        }

        public Action<Ticket> Resolve(string comment)
        {
            const TicketActivity activity = TicketActivity.Resolve;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    ticket.TicketStatus = TicketStatus.Resolved;
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment);
                }
            };
        }

        public Action<Ticket> SupplyMoreInfo(string comment, bool reactivate)
        {
            const TicketActivity activity = TicketActivity.SupplyMoreInfo;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    if (reactivate)
                    {
                        ticket.TicketStatus = TicketStatus.Active;
                    }
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment);
                }
            };
        }

        public Action<Ticket> TakeOver(string comment, string priority)
        {
            const TicketActivity activity = TicketActivity.TakeOver;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    ticket.AssignedTo = SecurityProvider.CurrentUser.userName;
                    if (!string.IsNullOrEmpty(priority))
                    {
                        if (ticket.Priority == priority)
                        {
                            priority = null;
                        }
                        else
                        {
                            ticket.Priority = priority;
                        }
                    }
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment, priority, null);
                }

            };
        }

        private Action<Ticket> ChangeAssignment(string comment, string assignTo, string priority, TicketActivity activity)
        {
            return ticket =>
            {
                if (SecurityProvider.CurrentUser.userName == assignTo) //attempting to assign/reassign to self
                {
                    TakeOver(comment, priority)(ticket);
                }
                else
                {
                    if (CheckSecurity(ticket, activity))
                    {
                        ticket.AssignedTo = assignTo;
                        if (!string.IsNullOrEmpty(priority))
                        {
                            if (ticket.Priority == priority)
                            {
                                priority = null;
                            }
                            else
                            {
                                ticket.Priority = priority;
                            }
                        }

                        ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUser.userName, activity, comment, priority, SecurityProvider.CurrentUser.userName);
                    }
                }
            };
        }

        private bool CheckSecurity(Ticket ticket, TicketActivity activity)
        {
            if (!IsTicketActivityValid(ticket, activity))
            {
                throw new SecurityException(Strings.CheckSecurity_NotAuthorized);
            }
            return true;
        }

    }
}
