using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using PagedList;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Domain.Models;
using TicketDesk.Web.Client.Models;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("Tickets")]
    [Authorize]
    public class TicketCenterController : Controller
    {



        private TicketDeskContext Context { get; set; }
        public TicketCenterController(TicketDeskContext context)
        {
            Context = context;
        }




        // GET: TicketCenter
        [Route("{listName=opentickets}/{page:int?}")]
        public async Task<ActionResult> Index(int? page, string listName)
        {
            var pageNumber = page ?? 1;

            var viewModel = await TicketCenterListViewModel.GetViewModelAsync(pageNumber, listName, Context, User.Identity.GetUserId());//new TicketCenterListViewModel(listName, model, Context, User.Identity.GetUserId());

            return View(viewModel);
        }

        [Route("PageList/{listName=opentickets}/{page:int?}")]
        public async Task<ActionResult> PageList(int? page, string listName)
        {
            return await GetTicketListPartial(page, listName);
        }


        [Route("FilterList/{listName=opentickets}/{page:int?}")]
        public async Task<PartialViewResult> FilterList(
            int? page,
            string listName,
            int pageSize,
            string currentStatus,
            string owner,
            string assignedTo)
        {
            var uId = User.Identity.GetUserId();
            var userSetting = Context.UserSettings.GetUserSetting(uId);

            var currentListSetting = userSetting.GetUserListSettingByName(listName);

            currentListSetting.ModifyFilterSettings(pageSize, currentStatus, owner, assignedTo);

            await Context.SaveChangesAsync();

            return await GetTicketListPartial(page, listName);

        }

        [Route("SortList/{listName=opentickets}/{page:int?}")]
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

        //// GET: TicketCenter/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Ticket ticket = await db.Tickets.FindAsync(id);
        //    if (ticket == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticket);
        //}

        //// GET: TicketCenter/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: TicketCenter/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "TicketId,TicketType,Category,Title,Details,IsHtml,TagList,CreatedBy,CreatedDate,Owner,AssignedTo,TicketStatus,CurrentStatusDate,CurrentStatusSetBy,LastUpdateBy,LastUpdateDate,Priority,AffectsCustomer,Version")] Ticket ticket)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Tickets.Add(ticket);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(ticket);
        //}

        //// GET: TicketCenter/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Ticket ticket = await db.Tickets.FindAsync(id);
        //    if (ticket == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticket);
        //}

        //// POST: TicketCenter/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "TicketId,TicketType,Category,Title,Details,IsHtml,TagList,CreatedBy,CreatedDate,Owner,AssignedTo,TicketStatus,CurrentStatusDate,CurrentStatusSetBy,LastUpdateBy,LastUpdateDate,Priority,AffectsCustomer,Version")] Ticket ticket)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(ticket).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(ticket);
        //}

        //// GET: TicketCenter/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Ticket ticket = await db.Tickets.FindAsync(id);
        //    if (ticket == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticket);
        //}

        //// POST: TicketCenter/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Ticket ticket = await db.Tickets.FindAsync(id);
        //    db.Tickets.Remove(ticket);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        Context.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
