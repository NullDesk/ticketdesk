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
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        [HttpGet]
        [Route("")]
        public CPUUser userID()
        {
            var employeeManager = new EmployeeInformationManager.EmployeeManager();
            var userId = System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace(@"clarkpud\", string.Empty);
            var employee = employeeManager.GetADUserByLogin(userId);
            CPUUser user = new CPUUser();
            user.FirstName = employee.FirstName;
            user.LastName = employee.LastName;
            user.Phone = employee.Phone;
            user.Email = employee.Email;
            user.ID = employee.Id.ToString();
            var mygroup = employee.Groups;
            user.Groups = new List<string>();

            foreach (var group in mygroup)
            {
                user.Groups.Add(group.Name);
            }
            
            return user;
        }
        [HttpGet]
        [Route("permissions")]
        public int GetPermission()
        {
            var employeeManager = new EmployeeInformationManager.EmployeeManager();
            var userId = System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace(@"clarkpud\", string.Empty);
            var user = employeeManager.GetADUserByLogin(userId);
            var permission = user.Groups;


            return 0;
        }

    }
}
