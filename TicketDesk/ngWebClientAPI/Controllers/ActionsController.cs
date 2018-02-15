using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketDesk.Domain;
using System.Threading.Tasks;
using TicketDesk.Web.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Model;
using Newtonsoft.Json.Linq;

namespace ngWebClientAPI.Controllers
{

    [RoutePrefix("api/actions")]
    public class ActionsController : ApiController
    {
        TicketDeskContextSecurityProvider secur;
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
            secur = new TicketDeskContextSecurityProvider(userManager, user.Id);
        }
        [HttpPost]
        [Route("force-close")]
        public async Task<string> Close([FromBody]JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            TicketActivityController ticketActivityController = new TicketActivityController(new TdDomainContext(secur));
            var stuff = await ticketActivityController.ForceClose(ticketId, comment);
            return "Successfully Forced Closed Ticket";
        }

        [HttpPost]
        [Route("reopen")]
        public async Task<string> ReOpen([FromBody] JObject data)
        {
            int ticketId = data["ticketId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            bool assignedToMe = data["assignedToMe"].ToObject<bool>();;
            TicketActivityController ticketActivityController = new TicketActivityController(new TdDomainContext(secur));
            var stuff = await ticketActivityController.ReOpen(ticketId, comment, assignedToMe);
            return "Successfully ReOpened Ticket";
        }

    }
}
