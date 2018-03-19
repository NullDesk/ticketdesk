using System;
using System.Web.Http;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using System.Threading.Tasks;
using TicketDesk.Web.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Model;
using Newtonsoft.Json.Linq;
using ngWebClientAPI.Models;
using System.Web.Mvc;
using System.Net;

/*
 * This needs refactor....badly
 * Need to refactor it to return success/depending on whether or not the action could or could not
 * be completed.  This will involve HTTPstatusCodes being passed to the front end w/messages
 * indicating what happened.  For success not much is needed other than like 200 or somehting close.
 * Failure needs more detailed messages and catch conditions....need better error checking
 */

namespace ngWebClientAPI.Controllers
{
    [System.Web.Http.RoutePrefix("api/actions")]
    public class ActionsController : ApiController
    {
        TicketActivityController ticketActivityController;
        public ActionsController()
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
            ticketActivityController = new TicketActivityController(new TdDomainContext(secur));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("activity-buttons/{ticketId}")]
        public TicketActivity ActivityButtons(int ticketId)
        {
            //convert ticketid - assuming id is semantically numbered
            var activities = ticketActivityController.ActivityButtons(ticketId);
            return activities;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("resolve")]
        public async HttpStatusCodeResult Resolve([FromBody] JObject data)
        {
            HttpStatusCodeResult result;
            //convert data to comment and ID
            try
            {
                InfoObject info = APIActionsConversion.ConvertInfo(data);
                int ticketId = data["ticketId"].ToObject<int>();
                string comment = data["comment"].ToObject<string>();
                Ticket ticket = await ticketActivityController.Resolve(info.ticketId, info.comment);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            return result;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("add-comment")]
        public async Task<JObject> AddComment([FromBody] JObject data)
        {
            //convert all things to front end ticket fffffffffffffffffffffffffffffffffffff
            InfoObject info = APIActionsConversion.ConvertInfo(data);
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            Ticket ticket = await ticketActivityController.AddComment(info.ticketId, info.comment);
            JObject test = APITicketConversion.ConvertGETTicket(ticket);
            //return APITicketConversion.ConvertGETTicket(ticket);
            return test;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("assign")]
        public async Task<JObject> Assign([FromBody] JObject data)
        {
            //convertAssign
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            string assignedTo = data["assignedTo"].ToObject<string>();
            string priority = data["priority"].ToObject<string>();
            Ticket ticket = await ticketActivityController.Assign(ticketId, comment, assignedTo, priority);
            return APITicketConversion.ConvertGETTicket(ticket);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("cancel-more-info")]
        public Task<Ticket> CancelMoreInfo([FromBody] JObject data) {
            //convertInfo
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.CancelMoreInfo(ticketId, comment);
            return ticket;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("edit-ticket-info")]
        public Task<Ticket> EditTicketInfo([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            int projectId = data["projectId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            string title = data["title"].ToObject<string>();
            string details = data["details"].ToObject<string>();
            string priority = data["priority"].ToObject<string>();
            string ticketType = data["ticketType"].ToObject<string>();
            string category = data["category"].ToObject<string>();
            string owner = data["owner"].ToObject<string>();
            string tagList = data["ticketId"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.EditTicketInfo(ticketId, projectId, comment, title, details, priority, ticketType, category, owner, tagList);
            return ticket;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("force-close")]
        public async Task<string> ForceClose([FromBody]JObject data)
        {
            //convertInfo
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            var stuff = await ticketActivityController.ForceClose(ticketId, comment);
            return "Successfully Forced Closed Ticket";
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("give-up")]
        public async Task<Ticket> GiveUp([FromBody] JObject data)
        {
            //convertInfo
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            Ticket ticket = await ticketActivityController.GiveUp(ticketId, comment);
            return ticket;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("pass")]
        public async Task<Ticket> Pass([FromBody] JObject data)
        {
            //convertAssign
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            string priority = data["priority"].ToObject<string>();
            string assignedTo = data["assignedTo"].ToObject<string>();
            Ticket ticket = await ticketActivityController.Pass(ticketId, comment, assignedTo, priority);
            return ticket;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("reassign")]
        public Task<Ticket> ReAssign([FromBody] JObject data)
        {
            //convertAssign
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            string assignedTo = data["assignedTo"].ToObject<string>();
            string priority = data["priority"].ToObject<string>();

            Task<Ticket> ticket = ticketActivityController.ReAssign(ticketId, comment, assignedTo, priority);
            return ticket;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("request-more-info")]
        public Task<Ticket> RequestMoreInfo([FromBody] JObject data)
        {
            //convertinfo
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.RequestMoreInfo(ticketId, comment);
            return ticket;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("reopen")]
        public Task<Ticket> ReOpen([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            bool assignToMe = data["assignToMe"].ToObject<bool>();
      
            Task<Ticket> ticket = ticketActivityController.ReOpen(ticketId, comment, assignToMe);
            return ticket;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("supply-more-info")]
        public Task<Ticket> SupplyMoreInfo([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            bool reactive = data["reactive"].ToObject<bool>();

            Task<Ticket> ticket = ticketActivityController.SupplyMoreInfo(ticketId, comment, reactive);
            return ticket;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("take-over")]
        public Task<Ticket> TakeOver([FromBody] JObject data)
        {
            //convertAssign
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            string priority = data["priority"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.TakeOver(ticketId, comment, priority);

            return ticket;
        }

    }
}
