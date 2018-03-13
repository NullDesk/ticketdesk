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

namespace ngWebClientAPI.Controllers
{
    [RoutePrefix("tickets")]
    [Route("{action=index}")]
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
        [HttpGet]
        [Route("{listName?}/{page:int?}")]
        public Task<TicketCenterListViewModel> Index(int? page, string listName)
        {
            Task<TicketCenterListViewModel> stuff = ticketCenterController.Index(page, listName);
            return stuff;
        }

        [HttpGet]
        [Route("pageList/{listName=mytickets}/{page:int?}")]
        public void PageList(int? page, string listName)
        {
            var stuff = ticketCenterController.PageList(page, listName);
            return;
        }

        [HttpGet]
        [Route("cook")]
        public void Stuff()
        {
            Console.WriteLine("hello");
        }
    }
}
