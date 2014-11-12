using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Web.Client.Models.Extensions;

namespace TicketDesk.Web.Client.Models
{
    public class TicketCreateViewModel
    {

        public Ticket Ticket { get; set; }
       
        private TicketDeskUserManager UserManager { get; set; }
        private TicketDeskRoleManager RoleManager { get; set; }
        private TicketDeskContext Context { get; set; }

        public TicketCreateViewModel(Ticket ticket,  TicketDeskContext context)
        {
            Ticket = ticket;
           
            Context = context;
            TempId = Guid.NewGuid();
            RoleManager = DependencyResolver.Current.GetService<TicketDeskRoleManager>();
            UserManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
        }

        public async Task<bool> CreateTicketAsync()
        {

            //TODO: Scan for any pending attachments

            Context.Tickets.Add(Ticket);
            await Context.SaveChangesAsync();//savechanges will drive populating the rest of the initial set of fields
           
            //TODO: move attachments from pending state 

            return Ticket.TicketId != default(int);

        }

        public bool DisplayUserSelects
        {
            get { return Context.SecurityProvider.IsTdHelpDeskUser; }
        }


        public Guid TempId { get; set; }

        public bool DisplayTags
        {
            get
            {
                return Context.SecurityProvider.IsTdHelpDeskUser || Context.Settings.GetSettingValue("AllowSubmitterRoleToEditTags", false);
            }
        }

        public bool DisplayPriorityList
        {
            get
            {
                return Context.SecurityProvider.IsTdHelpDeskUser || Context.Settings.GetSettingValue("AllowSubmitterRoleToEditPriority", false);
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
            get { return RoleManager.GetTdInternalUsers(UserManager).ToSelectList(false, Context.SecurityProvider.CurrentUserId); }
        }

        public SelectList AssignedToList
        {
            get { return RoleManager.GetTdHelpDeskUsers(UserManager).ToSelectList(Ticket.AssignedTo, "-- unassigned --"); }
        }
    }
}