using System;
using System.Web.Http;
using System.Web;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using System.Threading.Tasks;
using TicketDesk.Web.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Model;
using Newtonsoft.Json.Linq;

namespace ngWebClientAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/actions")]
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

        [HttpGet]
        [Route("activity-buttons/{ticketId}")]
        public JObject ActivityButtons(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.Name;

            var activities = ticketActivityController.ActivityButtons(ticketId);
            var jsonActivity = new JObject();
            jsonActivity.Add("actionPermissions", (int) activities);
            return jsonActivity;
        }

        [HttpPost]
        [Route("resolve")]
        public Task<Ticket> Resolve([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.Resolve(ticketId, comment);
            return ticket;
        }

        [HttpPost]
        [Route("add-comment")]
        public Task<Ticket> AddComment([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.AddComment(ticketId, comment);
            return ticket;
        }

        [HttpPost]
        [Route("assign")]
        public Task<Ticket> Assign([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            string assignedTo = data["assignedTo"].ToObject<string>();
            string priority = data["priority"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.Assign(ticketId, comment, assignedTo, priority);
            return ticket;
        }

        [HttpPost]
        [Route("cancel-more-info")]
        public Task<Ticket> CancelMoreInfo([FromBody] JObject data) {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.CancelMoreInfo(ticketId, comment);
            return ticket;
        }

        [HttpPost]
        [Route("edit-ticket-info")]
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

        [HttpPost]
        [Route("force-close")]
        public async Task<string> ForceClose([FromBody]JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            var stuff = await ticketActivityController.ForceClose(ticketId, comment);
            return "Successfully Forced Closed Ticket";
        }

        [HttpPost]
        [Route("give-up")]
        public Task<Ticket> GiveUp([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.GiveUp(ticketId, comment);
            return ticket;
        }

        [HttpPost]
        [Route("pass")]
        public Task<Ticket> Pass([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            string priority = data["priority"].ToObject<string>();
            string assignedTo = data["assignedTo"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.Pass(ticketId, comment, assignedTo, priority);
            return ticket;
        }

        [HttpPost]
        [Route("reassign")]
        public Task<Ticket> ReAssign([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            string assignedTo = data["assignedTo"].ToObject<string>();
            string priority = data["priority"].ToObject<string>();

            Task<Ticket> ticket = ticketActivityController.ReAssign(ticketId, comment, assignedTo, priority);
            return ticket;
        }

        [HttpPost]
        [Route("request-more-info")]
        public Task<Ticket> RequestMoreInfo([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.RequestMoreInfo(ticketId, comment);
            return ticket;
        }

        [HttpPost]
        [Route("reopen")]
        public Task<Ticket> ReOpen([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            bool assignToMe = data["assignToMe"].ToObject<bool>();
      
            Task<Ticket> ticket = ticketActivityController.ReOpen(ticketId, comment, assignToMe);
            return ticket;
        }

        [HttpPost]
        [Route("supply-more-info")]
        public Task<Ticket> SupplyMoreInfo([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            bool reactive = data["reactive"].ToObject<bool>();

            Task<Ticket> ticket = ticketActivityController.SupplyMoreInfo(ticketId, comment, reactive);
            return ticket;
        }

        [HttpPost]
        [Route("take-over")]
        public Task<Ticket> TakeOver([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            string priority = data["priority"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.TakeOver(ticketId, comment, priority);

            return ticket;
        }

    }
}
