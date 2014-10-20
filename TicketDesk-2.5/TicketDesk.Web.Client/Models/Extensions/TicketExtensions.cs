using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TicketDesk.Domain.Model;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client.Models.Extensions
{
    public static class TicketExtensions
    {
        public static TicketDeskUser GetAssignedToUser(this Ticket ticket)
        {
            return GetUser(ticket.AssignedTo);
        }
        public static TicketDeskUser GetCreatedByUser(this Ticket ticket)
        {
            return  GetUser(ticket.CreatedBy);
        }
        public static TicketDeskUser GetOwnerUser(this Ticket ticket)
        {
            return  GetUser(ticket.Owner);
        }
        public static TicketDeskUser GetLastUpdatedByUser(this Ticket ticket)
        {
            return  GetUser(ticket.LastUpdateBy);
        }
        public static TicketDeskUser GetCurrentStatusSetByUser(this Ticket ticket)
        {
            return  GetUser(ticket.CurrentStatusSetBy);
        }

        private static TicketDeskUser GetUser(string userId)
        {
            //TODO: this is a lot of query for every ticket, need to implement serious caching
            var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            return userManager.FindById(userId);
        }
    }
}