using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeInformationManager;
using ngWebClientAPI.Models;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace ngWebClientAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        [HttpGet]
        [Route("{userName?}")]
        public CPUUser userID(string userName = "")
        {
            var employeeManager = new EmployeeInformationManager.EmployeeManager();
            var id = (String.IsNullOrEmpty(userName)) 
              ? System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace(@"clarkpud\", string.Empty)
              : userName;
            var employee = employeeManager.GetADUserByLogin(id);
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
        public JObject GetPermission()
        {
            var employeeManager = new EmployeeInformationManager.EmployeeManager();
            var userId = System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace(@"clarkpud\", string.Empty);
            var user = employeeManager.GetADUserByLogin(userId);
            var groups = user.Groups;

            var highestPermission = "TD_User";
            JObject result = new JObject();

            foreach (var group in groups)
            {
                if (group.Name.Equals("TD_Admin"))
                {
                    highestPermission = group.Name;
                    break;
                }
                if (group.Name.Equals("TD_Resolver"))
                {
                    highestPermission = group.Name;
                }
            }

            result.Add("UserPermissions", highestPermission);

            return result;
        }

    }
}
