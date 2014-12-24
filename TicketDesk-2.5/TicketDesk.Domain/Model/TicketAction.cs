using System;
using System.Data.Entity;
using System.Security;
using System.Text;

namespace TicketDesk.Domain.Model
{
    public static class TicketAction
    {

        //TODO: we should enforce required comments on the back-end





        public static Action<TicketDeskContextSecurityProviderBase, Ticket> AddComment(string comment)
        {
            const TicketActivity activity = TicketActivity.AddComment;
            return (security, ticket) =>
            {
                if (CheckSecurity(security, ticket, activity))
                {
                    ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment);
                }
            };
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> Assign(string comment, string assignedTo, string assignToDisplayName, string priority)
        {
            return ChangeAssignment(comment, assignedTo, assignToDisplayName, priority, TicketActivity.Assign);
        }


        public static Action<TicketDeskContextSecurityProviderBase, Ticket> CancelMoreInfo(string comment)
        {
            const TicketActivity activity = TicketActivity.CancelMoreInfo;
            return (security, ticket) =>
            {
                if (CheckSecurity(security, ticket, activity))
                {
                    ticket.TicketStatus = TicketStatus.Active;

                    ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment);
                }
            };
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> Close(string comment)
        {
            const TicketActivity activity = TicketActivity.Close;
            return (security, ticket) =>
            {
                if (CheckSecurity(security, ticket, activity))
                {
                    ticket.TicketStatus = TicketStatus.Closed;
                    ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment);
                }
            };
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> EditTicketInfo(
            string comment,
            string title,
            string details,
            string priority,
            string ticketType,
            string category,
            string owner,
            Func<string, string> getUserDisplayName,
            string tagList,
            DbSet<Setting> settings)
        {
            const TicketActivity activity = TicketActivity.EditTicketInfo;
            return (security, ticket) =>
            {
                if (CheckSecurity(security, ticket, activity))
                {

                    var sb = new StringBuilder(comment);
                    sb.AppendLine();
                    sb.AppendLine("<pre>");
                    sb.AppendLine("Changes:");

                    //TODO: resource these strings!
                    if (ticket.Title != title)
                    {
                        sb.AppendLine(string.Format("    {0}", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.Title)));
                        ticket.Title = title;
                    }
                    if (ticket.Details != details)
                    {
                        sb.AppendLine(string.Format("    {0}", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.Details)));
                        ticket.Details = details;
                    }
                    if ((security.IsTdHelpDeskUser || settings.GetSettingValue("AllowSubmitterRoleToEditTags", false)) && ticket.TagList != tagList)
                    {
                        sb.AppendLine(string.Format("    {0}", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.TagList)));
                        ticket.TagList = tagList;
                    }
                    if ((security.IsTdHelpDeskUser || settings.GetSettingValue("AllowSubmitterRoleToEditPriority", false)) && ticket.Priority != priority)
                    {
                        sb.AppendLine(string.Format("    {0}: from \"{1}\" to \"{2}\"", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.Priority), ticket.Priority, priority));
                        ticket.Priority = priority;
                    }
                    if (ticket.TicketType != ticketType)
                    {
                        sb.AppendLine(string.Format("    {0}: from \"{1}\" to \"{2}\"", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.TicketType), ticket.TicketType, ticketType));
                        ticket.TicketType = ticketType;
                    }
                    if (ticket.Category != category)
                    {
                        sb.AppendLine(string.Format("    {0}: from \"{1}\" to \"{2}\"", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.Category), ticket.Category, category));
                        ticket.Category = category;
                    }
                    if (security.IsTdHelpDeskUser && ticket.Owner != owner)
                    {
                        sb.AppendLine(string.Format("    {0}: from \"{1}\" to \"{2}\"", PropertyUtility.GetPropertyDisplayString<Ticket>(p => p.Owner), getUserDisplayName(ticket.Owner), getUserDisplayName(owner)));
                        ticket.Owner = owner;
                    }
                    sb.AppendLine("</pre>");
                    comment = sb.ToString();
                    ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment);
                }
            };
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> ForceClose(string comment)
        {
            const TicketActivity activity = TicketActivity.ForceClose;
            return (security, ticket) =>
            {
                if (CheckSecurity(security, ticket, activity))
                {
                    ticket.TicketStatus = TicketStatus.Closed;
                    ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment);
                }
            };
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> GiveUp(string comment)
        {
            const TicketActivity activity = TicketActivity.GiveUp;
            return (security, ticket) =>
            {
                if (CheckSecurity(security, ticket, activity))
                {
                    ticket.AssignedTo = null;
                    ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment);

                }
            };
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> ModifyAttachments(string comment)
        {
            //The vast majority of attachements management occur outside the business domain, so all we do here is log the activity event
            const TicketActivity activity = TicketActivity.ModifyAttachments;
            return (security, ticket) =>
            {
                if (CheckSecurity(security, ticket, activity))
                {
                    ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment);
                }
            };
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> Pass(string comment, string assignedTo, string assignToDisplayName, string priority)
        {
            return ChangeAssignment(comment, assignedTo, assignToDisplayName, priority, TicketActivity.Pass);
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> ReAssign(string comment, string assignedTo, string assignToDisplayName, string priority)
        {
            return ChangeAssignment(comment, assignedTo, assignToDisplayName, priority, TicketActivity.ReAssign);
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> RequestMoreInfo(string comment)
        {
            const TicketActivity activity = TicketActivity.RequestMoreInfo;
            return (security, ticket) =>
            {
                if (CheckSecurity(security, ticket, activity))
                {
                    ticket.TicketStatus = TicketStatus.MoreInfo;

                    ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment);
                }
            };
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> ReOpen(string comment, bool assignToMe)
        {
            const TicketActivity activity = TicketActivity.ReOpen;
            return (security, ticket) =>
            {
                if (CheckSecurity(security, ticket, activity))
                {
                    ticket.AssignedTo = assignToMe ? security.CurrentUserId : null;
                    ticket.TicketStatus = TicketStatus.Active;
                    ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment);
                }
            };
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> Resolve(string comment)
        {
            const TicketActivity activity = TicketActivity.Resolve;
            return (security, ticket) =>
            {
                if (CheckSecurity(security, ticket, activity))
                {
                    ticket.TicketStatus = TicketStatus.Resolved;
                    ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment);
                }
            };
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> SupplyMoreInfo(string comment, bool reactivate)
        {
            const TicketActivity activity = TicketActivity.SupplyMoreInfo;
            return (security, ticket) =>
            {
                if (CheckSecurity(security, ticket, activity))
                {
                    if (reactivate)
                    {
                        ticket.TicketStatus = TicketStatus.Active;
                    }
                    ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment);
                }
            };
        }

        public static Action<TicketDeskContextSecurityProviderBase, Ticket> TakeOver(string comment, string priority)
        {
            const TicketActivity activity = TicketActivity.TakeOver;
            return (security, ticket) =>
            {
                if (CheckSecurity(security, ticket, activity))
                {
                    ticket.AssignedTo = security.CurrentUserId;
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
                    ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment, priority, null);
                }

            };
        }

        private static Action<TicketDeskContextSecurityProviderBase, Ticket> ChangeAssignment(string comment, string assignedTo, string assignToDisplayName, string priority, TicketActivity activity)
        {
            return (security, ticket) =>
            {
                if (security.CurrentUserId == assignedTo) //attempting to assign/reassign to self
                {
                    TakeOver(comment, priority)(security,ticket);
                }
                else
                {
                    if (CheckSecurity(security, ticket, activity))
                    {
                        ticket.AssignedTo = assignedTo;
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

                        ticket.TicketEvents.AddActivityEvent(security.CurrentUserId, activity, comment, priority, assignToDisplayName);
                    }
                }
            };
        }

        private static bool CheckSecurity(TicketDeskContextSecurityProviderBase securityProvider, Ticket ticket, TicketActivity activity)
        {
            if (!securityProvider.IsTicketActivityValid(ticket, activity))
            {
                throw new SecurityException("User is not authorized to perform this activity.");
            }
            return true;
        }

    }
}
