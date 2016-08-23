using System.Linq;
using System.Web.Mvc;
using Postal;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Models
{
    public class TicketEmail : Email
    {
        public Ticket Ticket { get; set; }

        public string SiteRootUrl { get; set; }

        public bool IsMultiProject
        {
            get
            {
                var context = DependencyResolver.Current.GetService<TdDomainContext>();
                return context.Projects.Count() > 1;
            }
        }
    }
}