using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Models
{
    public class ProjectListViewModelItem
    {
        public Project Project
        {
            get;
            set;
        }
        public int NumberOfTickets { get; set; }
    }
}