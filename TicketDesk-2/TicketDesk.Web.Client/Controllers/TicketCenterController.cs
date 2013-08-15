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

using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using TicketDesk.Domain.Models;
using TicketDesk.Domain.Services;
using TicketDesk.Web.Client.Models;
using TicketDesk.Web.Client.Helpers;

namespace TicketDesk.Web.Client.Controllers
{

    [HandleError]
    [NoCache]
    [Export("TicketCenter", typeof(IController))]
    public partial class TicketCenterController : ApplicationController
    {
        private ITicketService Tickets { get; set; }
        private SettingsService Settings { get; set; }

        [ImportingConstructor]
        public TicketCenterController(ITicketService ticketService, ISecurityService security, SettingsService settings)
            : base(security)
        {
            Tickets = ticketService;
            Settings = settings;
        }


        /// <summary>
        /// Lists the specified page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns></returns>
        [Authorize]
        public virtual ActionResult List(int? page, string listName)
        {
            var dp = Settings.UserSettings.GetDisplayPreferences();

            int p = page ?? 1;

            if (string.IsNullOrEmpty(listName) || !dp.TicketCenterListPreferences.Any(pr => pr.ListName == listName))
            {
                var defaultTicketCenterListPreferences = dp.TicketCenterListPreferences.OrderBy(ls => ls.ListMenuDisplayOrder).FirstOrDefault();
                if (defaultTicketCenterListPreferences != null)
                {
                    listName = defaultTicketCenterListPreferences.ListName;
                }
            }




            var lp = dp.GetPreferencesForList(listName);
            TempData["TicketCenterList"] = listName;
            TempData["TicketCenterPage"] = p;
            TempData["TicketCenterListDisplayName"] = lp.ListDisplayName;

            if (p < 1) //prevent negative pageIndex, redirect to page 1(index 0);
            {
                return RedirectToAction(MVC.TicketCenter.List(1, listName));
            }
            var model = new TicketCenterListViewModel(listName, Tickets.ListTickets(p, lp), Settings, Security);

            if (p > model.Tickets.TotalPages && p > 1)//total pages is 0 when no rows returned, so we only do this when requested page is not page 1.
            {
                return RedirectToAction(MVC.TicketCenter.List(model.Tickets.TotalPages, listName));
            }

            if (IsItReallyRedirectFromAjax())
            {
                return PartialView(MVC.TicketCenter.Views.Controls.TicketList, model);
            }

            return View(MVC.TicketCenter.Views.TicketCenter, model);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult FilterList(string listName, int pageSize, string currentStatus, string owner, string assignedTo)
        {
            var dp = Settings.UserSettings.GetDisplayPreferences();

            var lp = dp.GetPreferencesForList(listName);

            lp.ItemsPerPage = pageSize;

            if (!lp.DisabledFilterColumNames.Contains("CurrentStatus"))
            {
                lp.ChangeCurrentStatusFilter(currentStatus);
            }
            if (!lp.DisabledFilterColumNames.Contains("Owner"))
            {
                lp.ChangeOwnerFilter(owner);
            }
            if (!lp.DisabledFilterColumNames.Contains("AssignedTo"))
            {
                lp.ChangeAssignedFilter(assignedTo);
            }

            Settings.UserSettings.SaveDisplayPreferences(dp);

            TempData["IsRedirectFromAjax"] = IsItReallyRedirectFromAjax();// some browsers don't correctly send headers necessary for IsAjaxRequest after a redirect, so we are making out own indicator

            return RedirectToAction(MVC.TicketCenter.List(null, listName));
        }

        [Authorize]
        public virtual ActionResult SortList(string listName, string columnName, bool isMultiSort = false)
        {
            var dp = Settings.UserSettings.GetDisplayPreferences();

            var lp = dp.GetPreferencesForList(listName);

            TicketListSortColumn sortCol = lp.SortColumns.SingleOrDefault(sc => sc.ColumnName == columnName);

            if (isMultiSort)
            {
                if (sortCol != null)// column already in sort, remove from sort
                {
                    if (lp.SortColumns.Count > 1)//only remove if there are more than one sort
                    {
                        lp.SortColumns.Remove(sortCol);
                    }
                    //TODO: should we revert to flipping direction if this is the only column in the sort? same behavior as if the user weren't holding shift?
                }
                else// column not in sort, add to sort
                {
                    lp.SortColumns.Add(new TicketListSortColumn(columnName, ColumnSortDirection.Ascending));
                }
            }
            else
            {
                if (sortCol != null)// column already in sort, just flip direction
                {
                    sortCol.SortDirection = (sortCol.SortDirection == ColumnSortDirection.Ascending) ? ColumnSortDirection.Descending : ColumnSortDirection.Ascending;
                }
                else // column not in sort, replace sort with new simple sort for column
                {
                    lp.SortColumns.Clear();
                    lp.SortColumns.Add(new TicketListSortColumn(columnName, ColumnSortDirection.Ascending));
                }
            }
            Settings.UserSettings.SaveDisplayPreferences(dp);

            TempData["IsRedirectFromAjax"] = IsItReallyRedirectFromAjax();// some browsers don't correctly send headers necessary for IsAjaxRequest after a redirect, so we are making out own indicator
            return RedirectToAction(MVC.TicketCenter.List(null, listName));
        }

    }
}
