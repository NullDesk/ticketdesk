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
using TicketDesk.Domain.Model;

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
            try
            {
                var employeeManager = new EmployeeManager();
                var id = (String.IsNullOrEmpty(userName) && userName != "undefined")
                  ? System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace(@"clarkpud\", string.Empty)
                  : userName;
                var employee = employeeManager.GetADUserByLogin(id);
                CPUUser user = new CPUUser();
                user.firstName = employee.FirstName;
                user.lastName = employee.LastName;
                user.phoneNumber = employee.Phone;
                user.email = employee.Email;
                user.id = employee.Id.ToString();
                user.userName = employee.UserName;
                var mygroup = employee.Groups;
                user.groups = new List<string>();

                foreach (var group in mygroup)
                {
                    user.groups.Add(group.Name);
                }

                return user;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("permissions")]
        public JObject getPermission()
        {
            JObject result = new JObject();
            try
            {
                var employeeManager = new EmployeeInformationManager.EmployeeManager();
                var userId = System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace(@"clarkpud\", string.Empty);
                var user = employeeManager.GetADUserByLogin(userId);
                var groups = user.Groups;

                var highestPermission = "TD_User";


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

                result.Add("userPermissions", highestPermission);
            }

            catch(Exception ex)
            {
                result.Add("userPermissions", ex.ToString());
            }
            return result;
        }

    }
}
