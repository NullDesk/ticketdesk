using System;
using System.Globalization;
using System.Threading.Tasks;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.IO;

namespace TicketDesk.Web.Client.Models
{
    public class TicketCreateViewModel
    {
        public Ticket Ticket { get; set; }
 
        private TicketDeskContext Context { get; set; }

        public TicketCreateViewModel(Ticket ticket,  TicketDeskContext context)
        {
            Ticket = ticket;
            Context = context;
            TempId = Guid.NewGuid();
           
        }

        public async Task<bool> CreateTicketAsync()
        {
            //TODO: still need to store attachment info to DB?
            Context.Tickets.Add(Ticket);
            await Context.SaveChangesAsync();
            var attachments = TicketDeskFileStore.ListAttachmentInfo(TempId.ToString(), true);

            foreach (var attachment in attachments)
            {
                TicketDeskFileStore.MoveFile(
                    attachment.Name, 
                    TempId.ToString(), 
                    Ticket.TicketId.ToString(CultureInfo.InvariantCulture), 
                    true, 
                    false);
            }
            

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
    }
}