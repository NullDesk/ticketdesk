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
        public static UserDisplayInfo GetAssignedToInfo(this Ticket ticket)
        {
            return GetUserInfo(ticket.AssignedTo);
        }
        public static UserDisplayInfo GetCreatedByInfo(this Ticket ticket)
        {
            return GetUserInfo(ticket.CreatedBy);
        }
        public static UserDisplayInfo GetOwnerInfo(this Ticket ticket)
        {
            return GetUserInfo(ticket.Owner);
        }
        public static UserDisplayInfo GetLastUpdatedByInfo(this Ticket ticket)
        {
            return GetUserInfo(ticket.LastUpdateBy);
        }
        public static UserDisplayInfo GetCurrentStatusSetByInfo(this Ticket ticket)
        {
            return GetUserInfo(ticket.CurrentStatusSetBy);
        }

        private static UserDisplayInfo GetUserInfo(string userId)
        {
           
            var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            return userManager.InfoCache.GetUserInfo(userId);
        }
    }
}