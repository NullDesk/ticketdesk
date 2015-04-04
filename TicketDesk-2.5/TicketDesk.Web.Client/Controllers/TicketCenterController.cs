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

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Web.Client.Models;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("tickets")]
    [Route("{action=index}")]
    [Authorize]
    public class TicketCenterController : Controller
    {
        private TdContext Context { get; set; }
        public TicketCenterController(TdContext context)
        {
            Context = context;
        }

        // GET: TicketCenter
        [Route("{listName=mytickets}/{page:int?}")]
        public async Task<ActionResult> Index(int? page, string listName)
        {
            var pageNumber = page ?? 1;

            var viewModel = await TicketCenterListViewModel.GetViewModelAsync(pageNumber, listName, Context, User.Identity.GetUserId());//new TicketCenterListViewModel(listName, model, Context, User.Identity.GetUserId());

            return View(viewModel);
        }

        [Route("pageList/{listName=mytickets}/{page:int?}")]
        public async Task<ActionResult> PageList(int? page, string listName)
        {
            return await GetTicketListPartial(page, listName);
        }


        [Route("filterList/{listName=opentickets}/{page:int?}")]
        public async Task<PartialViewResult> FilterList(
            string listName,
            int pageSize,
            string ticketStatus,
            string owner,
            string assignedTo)
        {
            var uId = User.Identity.GetUserId();
            var userSetting = Context.UserSettings.GetUserSetting(uId);

            var currentListSetting = userSetting.GetUserListSettingByName(listName);

            currentListSetting.ModifyFilterSettings(pageSize, ticketStatus, owner, assignedTo);
            
            await Context.SaveChangesAsync();

            return await GetTicketListPartial(null, listName);

        }

        [Route("sortList/{listName=opentickets}/{page:int?}")]
        public async Task<PartialViewResult> SortList(
            int? page,
            string listName,
            string columnName,
            bool isMultiSort = false)
        {
            var uId = User.Identity.GetUserId();
            var userSetting = Context.UserSettings.GetUserSetting(uId);
            var currentListSetting = userSetting.GetUserListSettingByName(listName);

            var sortCol = currentListSetting.SortColumns.SingleOrDefault(sc => sc.ColumnName == columnName);

            if (isMultiSort)
            {
                if (sortCol != null)// column already in sort, remove from sort
                {
                    if (currentListSetting.SortColumns.Count > 1)//only remove if there are more than one sort
                    {
                        currentListSetting.SortColumns.Remove(sortCol);
                    }
                }
                else// column not in sort, add to sort
                {
                    currentListSetting.SortColumns.Add(new UserTicketListSortColumn(columnName, ColumnSortDirection.Ascending));
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
                    currentListSetting.SortColumns.Clear();
                    currentListSetting.SortColumns.Add(new UserTicketListSortColumn(columnName, ColumnSortDirection.Ascending));
                }
            }

            await Context.SaveChangesAsync();

            return await GetTicketListPartial(page, listName);
        }


        private async Task<PartialViewResult> GetTicketListPartial(int? page, string listName)
        {
            var pageNumber = page ?? 1;

            var viewModel = await TicketCenterListViewModel.GetViewModelAsync(pageNumber, listName, Context, User.Identity.GetUserId());//new TicketCenterListViewModel(listName, model, Context, User.Identity.GetUserId());
            return PartialView("_TicketList", viewModel);

        }

       
    }
}
