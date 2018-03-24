using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

using ngWebClientAPI.Models;
using System.Threading.Tasks;


using TicketDesk.Web.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Model;

using Newtonsoft.Json.Linq;
using X.PagedList;

namespace ngWebClientAPI.Controllers
{
    [RoutePrefix("api/tickets")]
    public class TicketCenterAPIController : ApiController
    {
        private TicketCenterController ticketCenterController;

        public TicketCenterAPIController()
        {
            TdIdentityContext context = new TdIdentityContext();
            var userStore = new UserStore<TicketDeskUser>(context);
            var roleStore = new RoleStore<TicketDeskRole>(context);
            var userManager = new TicketDeskUserManager(userStore);
            var roleManager = new TicketDeskRoleManager(roleStore);

            roleManager.EnsureDefaultRolesExist();

            TicketDeskUser user = userManager.FindByName("admin@example.com");
            if (user == null)
            {
                user = new TicketDeskUser
                {
                    Id = "64165817-9cb5-472f-8bfb-6a35ca54be6a",
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    DisplayName = "Admin User"
                };
                userManager.Create(user, "123456");
                userManager.AddToRole(user.Id, "TdAdministrators");
                userManager.AddToRole(user.Id, "TdHelpDeskUsers");
                userManager.AddToRole(user.Id, "TdInternalUsers");
                context.SaveChanges();
            }
            TicketDeskContextSecurityProvider secur = new TicketDeskContextSecurityProvider();
            ticketCenterController = new TicketCenterController(new TdDomainContext(secur));
        }

        [Route("reset-user-lists")]
        public async Task<IPagedList<Ticket>> ResetUserLists()
        {
            IPagedList<Ticket> ticketList = await ticketCenterController.ResetUserLists();
            return ticketList;
        }

        [HttpGet]
        [Route("{listName?}/{page:int?}")]
        public async Task<IPagedList<Ticket>> Index(JObject data)
        {
            int? page = data["page"].ToObject<int?>();
            string listName = data["listName"].ToObject<string>();

            IPagedList<Ticket> ticketList = await ticketCenterController.Index(page, listName);
            return ticketList;
        }

        [HttpGet]
        [Route("pageList")]
        public async Task<IPagedList<Ticket>> PageList(JObject data)
        {
            int? page = data["page"].ToObject<int?>();
            string listName = data["listName"].ToObject<string>();
            IPagedList<Ticket> ticketList = await ticketCenterController.PageList(page, listName);
            return ticketList;
        }

        [Route("filterList")]
        public async Task<IPagedList<Ticket>> filterlist(JObject data)
        {
            string listName = data["listName"].ToObject<string>();
            int pageSize = data["pageSize"].ToObject<int>();
            string ticketStatus = data["ticketStatus"].ToObject<string>();
            string owner = data["owner"].ToObject<string>();
            string assignedTo = data["assignedTo"].ToObject<string>();

            IPagedList<Ticket> ticketList = await ticketCenterController.FilterList(listName, pageSize, ticketStatus, owner, assignedTo);
            return ticketList;
        }

        [Route("sortList")]
        public async Task<IPagedList<Ticket>> SortList(JObject data)
        {
            int? page = data["page"].ToObject<int?>();
            string listName = data["listName"].ToObject<string>();
            string columnName = data["columnName"].ToObject<string>();
            bool isMultiSort = data["isMultiSort"].ToObject<bool>();

            IPagedList<Ticket> ticketList = await ticketCenterController.SortList(page, listName, columnName, isMultiSort);
            return ticketList;
        }
    }
}
