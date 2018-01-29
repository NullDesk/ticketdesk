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
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment);
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

                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment);
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
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment);
                }
            };
        }

        public Action<Ticket> EditTicketInfo(
            string comment,
            int projectId,
            string projectName,
            string title,
            string details,
            string priority,
            string dueDateAsString,
            string ticketType,
            string category,
            string owner,
            string tagList,
            decimal? estimatedDuration,
            decimal? actualDuraion,
            string targetDateAsString,
            string resolutionDateAsString,
            ApplicationSetting settings)
        {
            const TicketActivity activity = TicketActivity.EditTicketInfo;
            return ticket =>
            {
                if (CheckSecurity(ticket, activity))
                {
                    
                    var sb = new StringBuilder(comment);
                    sb.AppendLine();
                   
                    sb.AppendLine("<dl><dt>");
                    sb.AppendLine(Strings.Changes_Title);
                    sb.AppendLine("</dt>");

                    //TODO: resource these strings!
                    if (ticket.Title != title)
                    {
                        sb.AppendLine(string.Format("<dd>    {0}</dd>", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.Title)));
                        ticket.Title = title;
                    }
                    if (ticket.ProjectId != projectId)
                    {
                        sb.AppendLine(string.Format("<dd>    " + Strings.Changes_From_To + "</dd>", 
                            PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.ProjectId),
                            ticket.Project.ProjectName,
                            projectName));
                        ticket.ProjectId = projectId;
                    }
                    if (ticket.Details != details)
                    {
                        sb.AppendLine(string.Format("<dd>    {0}</dd>", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.Details)));
                        ticket.Details = details;
                    }
                    if ((SecurityProvider.IsTdHelpDeskUser || settings.Permissions.AllowInternalUsersToEditTags ) && ticket.TagList != tagList)
                    {
                        sb.AppendLine(string.Format("<dd>    {0}</dd>", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.TagList)));
                        ticket.TagList = tagList;
                    }
                    if ((SecurityProvider.IsTdHelpDeskUser || settings.Permissions.AllowInternalUsersToEditPriority) && ticket.Priority != priority)
                    {
                        sb.AppendLine(string.Format("<dd>    " + Strings.Changes_From_To + "</dd>", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.Priority), ticket.Priority, priority));
                        ticket.Priority = priority;
                    }
                    if (ticket.DueDateAsString != dueDateAsString)
                    {
                        sb.AppendLine(
                            string.Format("<dd>    " + 
                            Strings.Changes_From_To + 
                            "</dd>", 
                            PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.DueDate), ticket.DueDateAsString, dueDateAsString));
                        ticket.DueDateAsString = dueDateAsString;
                    }
                    if (ticket.TargetDateAsString != targetDateAsString)
                    {
                        sb.AppendLine(
                            string.Format("<dd>    " +
                            Strings.Changes_From_To +
                            "</dd>",
                            PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.TargetDate), ticket.TargetDateAsString, targetDateAsString));
                        ticket.TargetDateAsString = targetDateAsString;
                    }
                    if (ticket.ResolutionDateAsString != resolutionDateAsString)
                    {
                        sb.AppendLine(
                            string.Format("<dd>    " +
                            Strings.Changes_From_To +
                            "</dd>",
                            PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.ResolutionDate), ticket.ResolutionDateAsString, resolutionDateAsString));
                        ticket.ResolutionDateAsString = resolutionDateAsString;
                    }
                    if (ticket.TicketType != ticketType)
                    {
                        sb.AppendLine(string.Format("<dd>    " + Strings.Changes_From_To + "</dd>", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.TicketType), ticket.TicketType, ticketType));
                        ticket.TicketType = ticketType;
                    }
                    if (ticket.Category != category)
                    {
                        sb.AppendLine(string.Format("<dd>    " + Strings.Changes_From_To + "</dd>", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.Category), ticket.Category, category));
                        ticket.Category = category;
                    }
                    if (ticket.EstimatedDuration != estimatedDuration)
                    {
                        sb.AppendLine(string.Format("<dd>    " + Strings.Changes_From_To + "</dd>", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.EstimatedDuration), ticket.EstimatedDuration, estimatedDuration));
                        ticket.EstimatedDuration = estimatedDuration;
                    }
                    if (ticket.ActualDuration != actualDuraion)
                    {
                        sb.AppendLine(string.Format("<dd>    " + Strings.Changes_From_To + "</dd>", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.ActualDuration), ticket.ActualDuration, actualDuraion));
                        ticket.ActualDuration = actualDuraion;
                    }
                    if (SecurityProvider.IsTdHelpDeskUser && ticket.Owner != owner)
                    {
                        sb.AppendLine(string.Format("<dd>    " + Strings.Changes_From_To + "</dd>", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.Owner), SecurityProvider.GetUserDisplayName(ticket.Owner), SecurityProvider.GetUserDisplayName(owner)));
                        ticket.Owner = owner;
                    }
                    sb.AppendLine("</dl>");
                    comment = sb.ToString();
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment);
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
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment);
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
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment);

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
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment);
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

                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment);
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
                    ticket.AssignedTo = assignToMe ? SecurityProvider.CurrentUserId : null;
                    ticket.TicketStatus = TicketStatus.Active;
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment);
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
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment);
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
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment);
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
                    ticket.AssignedTo = SecurityProvider.CurrentUserId;
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
                    ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment, priority, null);
                }

            };
        }

        private Action<Ticket> ChangeAssignment(string comment, string assignTo, string priority, TicketActivity activity)
        {
            return ticket =>
            {
                if (SecurityProvider.CurrentUserId == assignTo) //attempting to assign/reassign to self
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

                        ticket.TicketEvents.AddActivityEvent(SecurityProvider.CurrentUserId, activity, comment, priority, SecurityProvider.GetUserDisplayName(assignTo));
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
