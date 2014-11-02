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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using TicketDesk.Domain.Utilities.Pagination;
using TicketDesk.Domain.Models;
using System.Data.Objects;

namespace TicketDesk.Domain.Repositories
{
    public class TicketEventArgs : EventArgs { public Ticket Ticket { get; set; } }

    [Export(typeof(ITicketRepository))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public class TicketRepository : ITicketRepository
    {
        

        public event EventHandler<TicketEventArgs> Saving;


        private TicketDeskEntities ctx = new TicketDeskEntities();



        #region ITicketRepository Members

        /// <summary>
        /// Gets a specific ticket.
        /// </summary>
        /// <param name="ticketId">The ticket id to fetch.</param>
        /// <returns></returns>
        public Ticket GetTicket(int ticketId)
        {
            return ctx.Tickets.SingleOrDefault(t => t.TicketId == ticketId);
            
        }

        /// <summary>
        /// Get a paged lists of tickets.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortColumns">The sort columns.</param>
        /// <param name="filterColumns">The filter columns.</param>
        /// <returns></returns>
        public IPagination<Ticket> ListTickets(int pageIndex, int pageSize, List<TicketListSortColumn> sortColumns, List<TicketListFilterColumn> filterColumns, bool includeComments)
        {
            ObjectQuery<Ticket> tq = (includeComments) ? ctx.Tickets.Include("TicketComments") : ctx.Tickets;
          
            string wString = null;
            string kString = null;
            if (filterColumns != null && filterColumns.Count > 0)
            {
                string[] fkeys = new string[filterColumns.Count];
                for (var i = 0; i < filterColumns.Count; i++)
                {
                    var filterColumn = filterColumns[i];

                    string colVal = (filterColumn.ColumnValue == null) ? "null" : string.Format("\"{0}\"", filterColumn.ColumnValue);
                    string optr = null;

                    if (colVal == "null")
                    {
                        optr = (filterColumn.UseEqualityComparison.HasValue && !filterColumn.UseEqualityComparison.Value) ? "IS NOT" : "IS";
                    }
                    else
                    {
                        optr = (filterColumn.UseEqualityComparison.HasValue && !filterColumn.UseEqualityComparison.Value) ? "!=" : "=";
                    }


                    fkeys[i] = string.Format("it.{0} {1} {2}", filterColumn.ColumnName, optr, colVal);

                }
                wString = string.Join(" and ", fkeys);
            }

            if (sortColumns != null && sortColumns.Count > 0)
            {

                string[] skeys = new string[sortColumns.Count];
                for (var i = 0; i < sortColumns.Count; i++)
                {
                    var sortColumn = sortColumns[i];
                    string appd = (sortColumn.SortDirection == ColumnSortDirection.Ascending) ? string.Empty : "DESC";
                    skeys[i] = string.Format("it.{0} {1}", sortColumn.ColumnName, appd);
                }

                kString = string.Join(",", skeys);

            }

            
            
            if (!string.IsNullOrEmpty(wString))
            {
                tq = tq.Where(wString);
            }
            if (string.IsNullOrEmpty(kString))
            {
                kString = "it.TicketId DESC";
            }

            return tq.OrderBy(kString).AsPagination(pageIndex, pageSize);
        }

        public IEnumerable<Ticket> ListTickets(SortedList<int, int> orderedTicketList, bool includeComments)
        {
            ObjectQuery<Ticket> tickets = (includeComments)? ctx.Tickets.Include("TicketComments") : ctx.Tickets;
            var orderedTickets = from i in orderedTicketList
                                 join t in tickets
                                 on i.Value equals t.TicketId
                                 orderby i.Key
                                 select t;
            
            return orderedTickets;

        }

        /// <summary>
        /// Creates a new ticket.
        /// </summary>
        /// <param name="newTicket">The new ticket.</param>
        /// <param name="commit">if set to <c>true</c> save new ticket to DB.</param>
        /// <returns>
        /// 	<c>true</c> if the ticket is created successfully.
        /// </returns>
        public bool CreateTicket(Ticket newTicket, bool commit)
        {
            ctx.Tickets.AddObject(newTicket);
            if (commit)
            {
                ctx.SaveChanges();
                SavingTicketChanges(newTicket);

            }
            return true;
        }

        /// <summary>
        /// Updates the ticket.
        /// </summary>
        /// <remarks>
        /// In the EF implementation we don't need the ticket entity passed as a parameter, but we pass it anyway as other repositories may not work the same way
        /// </remarks>
        /// <param name="ticket">The ticket to update.</param>
        /// <returns></returns>
        public bool UpdateTicket(Ticket ticket)
        {
            ctx.SaveChanges();
            SavingTicketChanges(ticket);
            return true;
        }

        /// <summary>
        /// Removes the attachment.
        /// </summary>
        /// <param name="attachment">The attachment to remove.</param>
        /// <param name="commit">if set to <c>true</c> save change to DB.</param>
        /// <returns></returns>
        public bool RemoveAttachment(TicketAttachment attachment, bool commit)
        {
            ctx.TicketAttachments.DeleteObject(attachment);
            if (commit)
            {

                ctx.SaveChanges();
                if (attachment.Ticket != null)//new tickets page can add and remove attachments before the ticket physically exists in the DB
                {
                    SavingTicketChanges(attachment.Ticket);
                }
            }
            return true;
        }

        /// <summary>
        /// Adds a pending attachment.
        /// </summary>
        /// <param name="attachment">The attachment to add.</param>
        /// <param name="commit">if set to <c>true</c> save pending attachment to DB.</param>
        /// <returns></returns>
        public bool AddPendingAttachment(TicketAttachment attachment, bool commit)
        {
            ctx.TicketAttachments.AddObject(attachment);
            if (commit)
            {
                ctx.SaveChanges();
                if (attachment.Ticket != null)//new tickets page can add and remove attachments before the ticket physically exists in the DB
                {
                    SavingTicketChanges(attachment.Ticket);
                }
            }
            return true;
        }

        /// <summary>
        /// Gets the pending attachment.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public TicketAttachment GetPendingAttachment(int fileId)
        {
            return ctx.TicketAttachments.SingleOrDefault(ta => ta.FileId == fileId && ta.IsPending);
        }

        /// <summary>
        /// Gets the ticket attachment.
        /// </summary>
        /// <param name="fileId">The file id of the attachment.</param>
        /// <returns></returns>
        public TicketAttachment GetTicketAttachment(int fileId)
        {
            return ctx.TicketAttachments.SingleOrDefault(ta => ta.FileId == fileId && !ta.IsPending);
        }

        /// <summary>
        /// Cleans up pending attachments that were not committed to a ticket in a timely manner.
        /// </summary>
        /// <param name="hoursOld">The number of hours old a pending attachment should be before being removed.</param>
        /// <returns></returns>
        public bool CleanUpDerelictAttachments(int hoursOld)
        {
            var ud = DateTime.Now.AddHours(hoursOld * -1);
            var killAttachments = ctx.TicketAttachments.Where(ta => ta.IsPending && ta.UploadedDate < ud);
            foreach (var att in killAttachments)
            {
                ctx.TicketAttachments.DeleteObject(att);
            }
            ctx.SaveChanges();
            return true;
        }

        /// <summary>
        /// Gets the list of changes for a ticket.
        /// </summary>
        /// <param name="modifiedTicket">The modified ticket.</param>
        /// <returns>
        /// A dictionary containing the fields and their original values for field that have been changed.
        /// </returns>
        public Dictionary<string, object> GetTicketChanges(Ticket modifiedTicket)
        {
            Dictionary<string, object> changes = new Dictionary<string, object>();

            var ose = ctx.ObjectStateManager.GetObjectStateEntry(modifiedTicket);

            var modProperties = ose.GetModifiedProperties();

            foreach (var p in modProperties)
            {
                if (!ose.OriginalValues[p].Equals(ose.CurrentValues[p]))
                {
                    changes.Add(p, ose.OriginalValues[p]);
                }
            }

            // THIS IS THE LINQ to SQL version - keeping for reference

            //var originalTicket = ctx.Tickets.GetOriginalEntityState(modifiedTicket);
            //ModifiedMemberInfo[] minfo = ctx.Tickets.GetModifiedMembers(modifiedTicket);
            //foreach (var info in minfo)
            //{
            //    if (info.CurrentValue != info.OriginalValue)
            //    {
            //        changes.Add(info.Member.Name, info.OriginalValue);
            //    }
            //}
            return changes;
        }



        /// <summary>
        /// Clears the tags of a ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="commit">if set to <c>true</c> deletes the tags from the DB.</param>
        /// <returns></returns>
        public bool ClearTags(Ticket ticket, bool commit)
        {
            var tagsToKill = new List<TicketTag>();
            foreach (var tag in ticket.TicketTags)
            {
                tagsToKill.Add(tag);

            }
            foreach (var tag in tagsToKill)
            {
                ctx.TicketTags.DeleteObject(tag);
            }

            if (commit)
            {
                ctx.SaveChanges();
                SavingTicketChanges(ticket);
            }
            return true;
        }

        /// <summary>
        /// Gets the distinct tags starting with a specified value.
        /// </summary>
        /// <param name="textToCheck">The starting text from which to locate tags.</param>
        /// <param name="maxTagsToReturn">The max number of possible matches to return.</param>
        /// <returns></returns>
        public List<string> GetDistinctTagsStartingWith(string textToCheck, int maxTagsToReturn)
        {
            if (maxTagsToReturn == 0)
            {
                maxTagsToReturn = 10;
            }
            var q = (from tag in ctx.TicketTags
                     orderby tag.TagName
                     where tag.TagName.StartsWith(textToCheck)
                     select tag.TagName).Distinct().Take(maxTagsToReturn);
            return q.ToList();
        }

        #endregion

        public List<String> GetChangedProperties(object a, object b)
        {
            Type type = a.GetType();
            List<String> differences = new List<String>();
            foreach (PropertyInfo p in type.GetProperties())
            {
                object aValue = p.GetValue(a, null);
                object bValue = p.GetValue(b, null);

                if (p.PropertyType.IsPrimitive || p.PropertyType == typeof(string))
                {
                    if (!aValue.Equals(bValue))
                    {
                        differences.Add(p.Name);
                    }
                }
                else
                {
                    differences.AddRange(GetChangedProperties(aValue, bValue));
                }
            }

            return differences;
        }

        private void SavingTicketChanges(Ticket ticket)
        {
            if (Saving != null)
            {
                Saving(this, new TicketEventArgs() { Ticket = ticket });
            }
        }
    }
}
