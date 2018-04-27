using System;
using System.Collections.Generic;
using EmployeeInformationManager;

namespace TicketDesk.Domain.Model
{
    public class CPUUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public List<string> Groups { get; set; }
        public string ID { get; set; }

        public CPUUser()
        {
            return;
        }
        public CPUUser(string userName)
        {
            /*This creates an instance of a CPUUser given a username.*/
            var employeeManager = new EmployeeManager();
            var employee = employeeManager.GetADUserByLogin(userName);

            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Phone = employee.Phone;
            Email = employee.Email;
            UserName = employee.UserName.ToLower();
            ID = employee.Id.ToString();
            var mygroup = employee.Groups;
            Groups = new List<string>();

            foreach (var group in mygroup)
            {
                Groups.Add(group.Name);
            }
        }
    }
}