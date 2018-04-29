using System;
using System.Collections.Generic;
using EmployeeInformationManager;

namespace TicketDesk.Domain.Model
{
    public class CPUUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public List<string> groups { get; set; }
        public string id { get; set; }

        public CPUUser()
        {
            return;
        }
        public CPUUser(string userName)
        {
            /*This creates an instance of a CPUUser given a username.*/
            var employeeManager = new EmployeeManager();
            var employee = employeeManager.GetADUserByLogin(userName);

            firstName = employee.FirstName;
            lastName = employee.LastName;
            phoneNumber = employee.Phone;
  
            email = employee.Email;
            this.userName = employee.UserName.ToLower();
            id = employee.Id.ToString();
            var mygroup = employee.Groups;
            groups = new List<string>();

            foreach (var group in mygroup)
            {
                groups.Add(group.Name);
            }
        }
    }
}