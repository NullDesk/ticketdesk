using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("admin/users")]
    [Route("{action=index}")]
    [Authorize(Roles = "TdAdministrators")]
    public class UserAdministrationController : Controller
    {
        private TicketDeskUserManager UserManager { get; set; }
        private TicketDeskRoleManager RoleManager { get; set; }
        public UserAdministrationController(TicketDeskUserManager userManager, TicketDeskRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        
        public async Task<ActionResult> Index(int page = 1)
        {
            ViewBag.AllRolesList = await RoleManager.Roles.ToListAsync();

            var users = await UserManager.Users
                .OrderBy(u => u.DisplayName)
                .Select(u => new UserRoleInfo
                {
                    DisplayName = u.DisplayName,
                    Email = u.Email,
                    Roles = u.Roles.Select(r => r.RoleId)
                })
                .ToPagedListAsync(page, 100);
            return View(users);
        }
    }

    public class UserRoleInfo
    {
        public string DisplayName { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<string> GetRoleNames(IEnumerable<TicketDeskRole> allRolesList)
        {
            return allRolesList.Where(ar => Roles.Any(r => r == ar.Id)).Select(ar => ar.DisplayName);
        }

    }
}