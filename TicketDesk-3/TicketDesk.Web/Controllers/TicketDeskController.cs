using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Breeze.WebApi2;
using Breeze.ContextProvider;
using Newtonsoft.Json.Linq;

using TicketDesk.Domain.Model;
using TicketDesk.Web.Helpers;
using TicketDesk.Web.Providers;

namespace TicketDesk.Web.Controllers
{
    /// <summary>
    /// Main controller retrieving information from the data store
    /// </summary>
    [BreezeController]
    public class TicketDeskController : ApiController
    {
        TicketDeskBreezeContext breezeContext;

        private UserManager<UserProfile> UserManager { get; set; }

        public TicketDeskController(TicketDeskBreezeContext ctx, UserManager<UserProfile> usermanager)
        {
            breezeContext = ctx;
            UserManager = usermanager;
        }

        [HttpGet]
        [Authorize(Roles = "TicketSubmitter")]
        public IQueryable<Ticket> Tickets()
        {
            var u = this.User;
            return breezeContext.Context.Tickets;
        }

            /// <summary>
        /// Save changes to data store
        /// </summary>
        /// <param name="saveBundle">The changes</param>
        /// <returns>Save result</returns>
        [HttpPost]
        [AllowAnonymous]
        public SaveResult SaveChanges(JObject saveBundle)
        {             
            return breezeContext.SaveChanges(saveBundle);
        }

        /// <summary>
        /// Get the lookups on client first app load
        /// </summary>
        /// <returns>The bundles</returns>
        [HttpGet]
        [AllowAnonymous]
        public LookupBundle Lookups()
        {
            return new LookupBundle
            {
                //Categories = breezeContext.Context.Categories.ToList()
            };
        }



    }
}
