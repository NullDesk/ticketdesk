using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Search;

namespace TicketDesk.Domain.Model
{
    public static class TicketExtensions
    {
        public static IEnumerable<SearchQueueItem> ToSeachQueueItems(this IEnumerable<Ticket> tickets)
        {
            return tickets.Select(t => new SearchQueueItem
            {
                Id = t.TicketId,
                Title = t.Title,
                Details = t.Details,
                Status = t.TicketStatus.ToString(),
                LastUpdateDate = t.LastUpdateDate,
                Tags = t.TagList.Split(','),
                //not null comments only, otherwise we end up indexing empty array item, or blowing up azure required field
                Comments = t.TicketComments.Where(c => !string.IsNullOrEmpty(c.Comment)).Select(c => c.Comment).ToArray()
            });
        }

        public static TicketActivity GetValidActivitesForTicket(this Ticket ticket, string userId)
        {
            var isOwnedByMe = (ticket.Owner == userId);
            var isMoreInfo = (ticket.TicketStatus == TicketStatus.MoreInfo);
            var isAssignedToMe = (!string.IsNullOrEmpty(ticket.AssignedTo) &&
                                       ticket.AssignedTo == userId);
            var isResolved = ticket.TicketStatus == TicketStatus.Resolved;

            var validActivities = TicketActivity.None;

            if (ticket.TicketId == default(int))
            {
                validActivities |= TicketActivity.Create | TicketActivity.CreateOnBehalfOf;
            }

            if (ticket.IsOpen)
            {
                validActivities |= TicketActivity.ModifyAttachments;
            }

            if (ticket.IsOpen)
            {
                if (isOwnedByMe)
                {
                    validActivities |= TicketActivity.EditTicketInfo;
                }
                if (isMoreInfo)
                {
                    validActivities |= TicketActivity.SupplyMoreInfo;
                    if (isAssignedToMe)
                    {
                        validActivities |= TicketActivity.CancelMoreInfo;
                    }
                }
                else //!moreInfo
                {
                    validActivities |= TicketActivity.AddComment;
                    if (isAssignedToMe)
                    {
                        validActivities |= TicketActivity.Resolve | TicketActivity.RequestMoreInfo;
                    }
                }
            }
            else //not open (resolved or closed)
            {
                validActivities |= TicketActivity.ReOpen;
            }
            if (isResolved)
            {
                if (isOwnedByMe)
                {
                    validActivities |= TicketActivity.Close;
                }
            }
            if (ticket.IsOpen || isResolved)
            {
                if (ticket.IsAssigned)
                {
                    if (!isAssignedToMe)
                    {
                        validActivities |= TicketActivity.ReAssign;
                    }
                }
                else//!assigned
                {
                    validActivities |= TicketActivity.Assign;
                }

                if ((isAssignedToMe || isOwnedByMe) && !(isResolved && isOwnedByMe))
                {
                    validActivities |= TicketActivity.ForceClose;
                }

                if (isAssignedToMe)
                {
                    validActivities |= TicketActivity.Pass | TicketActivity.GiveUp;
                }
                else//!isAssignedToMe
                {
                    validActivities |= TicketActivity.TakeOver ;
                }
            }
            return validActivities;
        }
    }
}
