// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.
using System;
using System.Linq;
using TicketDesk.Engine.ListView;

namespace TicketDesk.Engine.Linq
{
    /// <summary>
    /// Container class for extension methods related to <see cref="IQueryable<Ticket>"/>
    /// </summary>
    public static class TicketListQueryExtensions
    {
        /// <summary>
        /// Applies the filters, sorts, and pages for a specific user's list view settings.
        /// </summary>
        /// <param name="ticketQuery">The ticket query.</param>
        /// <param name="listViewSettings">The list view settings containing the filters and sorts to apply.</param>
        /// <returns></returns>
        public static IQueryable<Ticket> ApplyListViewSettings(this IQueryable<Ticket> ticketQuery, ListViewSettings listViewSettings, int startRowIndex)
        {
            IQueryable<Ticket> newQuery = null;
            newQuery = ticketQuery.ApplyFilters(listViewSettings);
            if(newQuery != null)
            {
                newQuery = newQuery.ApplySorts(listViewSettings);
            }
            else
            {
                newQuery = ticketQuery.ApplySorts(listViewSettings);
            }
            return newQuery.Take(listViewSettings.ItemsPerPage).Skip(startRowIndex);
        }


        /// <summary>
        /// Applies the sorts from a specific user's list view settings.
        /// </summary>
        /// <param name="ticketQuery">The ticket query.</param>
        /// <param name="listViewSettings">The list view settings to sort by.</param>
        /// <returns></returns>
        private static IQueryable<Ticket> ApplySorts(this IQueryable<Ticket> ticketQuery, ListViewSettings listViewSettings)
        {
            IQueryable<Ticket> newQuery = ticketQuery;

            //sorts have to be applied in reverse order as the last applied is the first in the SQL query generated
            for(int x = listViewSettings.SortColumns.Count() - 1; x >= 0; x--)
            {
                ListViewSortColumn column = listViewSettings.SortColumns[x];
                bool isDescending = (column.SortDirection == ColumnSortDirection.Descending);
                newQuery = newQuery.ApplySort(column.ColumnName, isDescending);
            }
            return newQuery;
        }


        /// <summary>
        /// Applies a sort to the query.
        /// </summary>
        /// <param name="ticketQuery">The ticket query for which to apply the sort operation.</param>
        /// <param name="fieldName">Name of the field to sort.</param>
        /// <param name="sortDescending">if set to <c>true</c> sorts descending.</param>
        /// <returns></returns>
        private static IQueryable<Ticket> ApplySort(this IQueryable<Ticket> ticketQuery, string fieldName, bool sortDescending)
        {
            IQueryable<Ticket> newQuery = null;
            switch(fieldName)
            {
                case "LastUpdateDate":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.LastUpdateDate) : ticketQuery.OrderBy(t => t.LastUpdateDate);
                    break;
                case "TicketId":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.TicketId) : ticketQuery.OrderBy(t => t.TicketId);
                    break;
                case "Type":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.Type) : ticketQuery.OrderBy(t => t.Type);
                    break;
                case "Category":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.Category) : ticketQuery.OrderBy(t => t.Category);
                    break;
                case "Title":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.Title) : ticketQuery.OrderBy(t => t.Title);
                    break;
                case "CreatedBy":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.CreatedBy) : ticketQuery.OrderBy(t => t.CreatedBy);
                    break;
                case "CreatedDate":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.CreatedDate) : ticketQuery.OrderBy(t => t.CreatedDate);
                    break;
                case "Owner":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.Owner) : ticketQuery.OrderBy(t => t.Owner);
                    break;
                case "AssignedTo":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.AssignedTo) : ticketQuery.OrderBy(t => t.AssignedTo);
                    break;
                case "CurrentStatus":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.CurrentStatus) : ticketQuery.OrderBy(t => t.CurrentStatus);
                    break;
                case "LastUpdateBy":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.LastUpdateBy) : ticketQuery.OrderBy(t => t.LastUpdateBy);
                    break;
                case "Priority":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.Priority) : ticketQuery.OrderBy(t => t.Priority);
                    break;
                case "AffectsCustomer":
                    newQuery = (sortDescending) ? ticketQuery.OrderByDescending(t => t.AffectsCustomer) : ticketQuery.OrderBy(t => t.AffectsCustomer);
                    break;
                
                default:
                    break;
            }

            if(newQuery == null)
            {
                throw new ApplicationException(string.Format("A sort was specified, but the field ({0}) is not supported by the TicketListQueryExtensions.ApplySort() method.", fieldName));
            }
            return newQuery;
        }


        /// <summary>
        /// Applies the filters for a specific user's list view settings.
        /// </summary>
        /// <param name="ticketQuery">The ticket query.</param>
        /// <param name="listViewSettings">The list view settings to filder by.</param>
        /// <returns></returns>
        private static IQueryable<Ticket> ApplyFilters(this IQueryable<Ticket> ticketQuery, ListViewSettings listViewSettings)
        {
            IQueryable<Ticket> newQuery = ticketQuery;
            foreach(ListViewFilterColumn column in listViewSettings.FilterColumns)
            {
                newQuery = newQuery.ApplyFilter(column.ColumnName, column.ColumnValue, column.EqualityComparison);
            }
            return newQuery;
        }

        /// <summary>
        /// Applies a filter to a query.
        /// </summary>
        /// <param name="ticketQuery">The ticket query for which to add the where condition.</param>
        /// <param name="fieldName">Name of the field to filter by.</param>
        /// <param name="fieldValue">The field value to filter for.</param>
        /// <param name="equalityComparison">if set to <c>true</c> performs an equality test, otherwise a not equals test.</param>
        /// <returns></returns>
        private static IQueryable<Ticket> ApplyFilter(this IQueryable<Ticket> ticketQuery, string fieldName, string fieldValue, bool? equalityComparison)
        {
            IQueryable<Ticket> newQuery = null;
            switch(fieldName)
            {
                case "CurrentStatus":
                    newQuery = ticketQuery.Where(t => t.CurrentStatus == fieldValue == equalityComparison);
                    break;
                case "Owner":
                    newQuery = ticketQuery.Where(t => t.Owner == fieldValue == equalityComparison);
                    break;
                case "AssignedTo":
                    if(!equalityComparison.HasValue)//special case for nullable value here
                    {
                        //TODO: retest this in .NET 3.5 SP1 RTM, or next major version of .NET to see if the behavior is improved any
                        // For some reason, if you try and test the column against a null string variable, it 
                        //  doesn't translate to SQL as an "is null" test correctly. Instead we have to explicity use the null keyword.
                        newQuery = ticketQuery.Where(t => t.AssignedTo == null);
                    }
                    else
                    {
                        newQuery = ticketQuery.Where(t => t.AssignedTo == fieldValue == equalityComparison);
                    }
                    break;
                default:
                    break;
            }

            if(newQuery == null)
            {
                throw new ApplicationException(string.Format("A filter was specified, but the field ({0}) is not supported by TicketListQueryExtensions.ApplyFilter() method.", fieldName));
            }
            return newQuery;
        }
    }
}
