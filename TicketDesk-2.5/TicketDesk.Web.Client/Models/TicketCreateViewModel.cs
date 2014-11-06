using System.Collections.Generic;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Web.Client.Models.Extensions;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client.Models
{
    public class TicketCreateViewModel
    {

        public Ticket Ticket { get; set; }
        private string UserId { get; set; }
        private TicketDeskUserManager UserManager { get; set; }
        private TicketDeskRoleManager RoleManager { get; set; }
        private TicketDeskContext Context { get; set; }

        public TicketCreateViewModel(Ticket ticket, string userId, TicketDeskContext context)
        {
            Ticket = ticket;
            UserManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            RoleManager = DependencyResolver.Current.GetService<TicketDeskRoleManager>();
            UserId = userId;
            Context = context;
        }

        public bool DisplayUserSelects
        {
            get { return UserManager.IsTdHelpDeskUser(UserId); }
        }

        

        public bool DisplayTags
        {
            get
            {
                return UserManager.IsTdHelpDeskUser(UserId) || Context.Settings.GetSettingValue("AllowSubmitterRoleToEditTags", false);
            }
        }

        public bool DisplayPriorityList
        {
            get
            {
                return UserManager.IsTdHelpDeskUser(UserId) || Context.Settings.GetSettingValue("AllowSubmitterRoleToEditPriority", false);
            }
        }

        public SelectList PriorityList
        {
            get { return Context.Settings.GetPriorityList(true, string.Empty); }
        }

        public SelectList CategoryList
        {
            get { return Context.Settings.GetCategoryList(true, string.Empty); }
        }

        public SelectList TicketTypeList
        {
            get { return Context.Settings.GetTicketTypeList(true, string.Empty); }
        }

        public SelectList OwnersList
        {
            get { return RoleManager.GetTdInternalUsers(UserManager).ToSelectList(false, UserId); }
        }

        public SelectList AssignedToList
        {
            get { return RoleManager.GetTdHelpDeskUsers(UserManager).ToSelectList(Ticket.AssignedTo, "-- unassigned --"); }
        }
    }
}