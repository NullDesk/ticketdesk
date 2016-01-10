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

using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Search.Common;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("admin/search-manager")]
    [Route("{action=index}")]
    [TdAuthorize(Roles = "TdAdministrators")]
    public class SearchManagerController : Controller
    {
        
        private TdDomainContext Context { get; set; }
        public SearchManagerController(TdDomainContext context)
        {
          
            Context = context;
        }

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("remove-index")]
        public async Task<ActionResult> RemoveIndex()
        {
            await TdSearchContext.Current.IndexManager.RemoveIndexAsync();
            ViewBag.IndexRemoved = true;
            return View("Index");
        }
       
        [Route("populate-index")]
        public async Task<ActionResult> PopulateIndex()
        {
            
            await TdSearchContext.Current.IndexManager.RunIndexMaintenanceAsync();
            var searchItems = Context.Tickets.Include("TicketEvents").ToSeachIndexItems();
            await TdSearchContext.Current.IndexManager.AddItemsToIndexAsync(searchItems);
            ViewBag.IndexPopulated = true;
            return View("Index");
        }
    }
}