using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeInformationManager;
using ngWebClientAPI.Models;
using System.Diagnostics;

namespace ngWebClientAPI.Controllers
{
    //[Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        [HttpGet]
        [Route("userId")]
        public CPUUser userID()
        {
            var employeeManager = new EmployeeInformationManager.EmployeeManager();
            //var userId = System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace(@"clarkpud\", string.Empty);
            var userId = "NStelmakh";
            var employee = employeeManager.GetADUserByLogin(userId);
            CPUUser user = new CPUUser();
            user.FirstName = employee.FirstName;
            user.LastName = employee.LastName;
            user.Phone = employee.Phone;
            user.Email = employee.Email;
            user.ID = employee.Id;
            var mygroup = employee.Groups;
            user.Groups = new List<string>();

            foreach (var group in mygroup)
            {
                user.Groups.Add(group.Name);
            }

            return user;
        }

    }
}
