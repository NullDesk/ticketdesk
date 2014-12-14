using System.Web;
using System.Web.Mvc;
using StackExchange.DataExplorer.Helpers;
using TicketDesk.Web.Client;
using TicketDesk.Web.Client.Models;


namespace TicketDesk.Domain.Model
{
    public static class TicketExtensions
    {
        public static UserDisplayInfo GetAssignedToInfo(this Ticket ticket)
        {
            return UserDisplayInfo.GetUserInfo(ticket.AssignedTo);
        }

        public static UserDisplayInfo GetCreatedByInfo(this Ticket ticket)
        {
            return UserDisplayInfo.GetUserInfo(ticket.CreatedBy);
        }

        public static UserDisplayInfo GetOwnerInfo(this Ticket ticket)
        {
            return UserDisplayInfo.GetUserInfo(ticket.Owner);
        }

        public static UserDisplayInfo GetLastUpdatedByInfo(this Ticket ticket)
        {
            return UserDisplayInfo.GetUserInfo(ticket.LastUpdateBy);
        }

        public static UserDisplayInfo GetCurrentStatusSetByInfo(this Ticket ticket)
        {
            return UserDisplayInfo.GetUserInfo(ticket.CurrentStatusSetBy);
        }

        public static HtmlString HtmlDetails(this Ticket ticket)
        {
            var content = (ticket.IsHtml) ? ticket.Details : ticket.Details.HtmlFromMarkdown();
            return new HtmlString(HtmlUtilities.Safe(content));
        }

        public static SelectList GetPriorityList(this Ticket ticket)
        {
            var context = DependencyResolver.Current.GetService<TicketDeskContext>();
            return context.Settings.GetPriorityList(true, ticket.Priority);
        }

        public static SelectList GetCategoryList(this Ticket ticket)
        {
            var context = DependencyResolver.Current.GetService<TicketDeskContext>();
            return context.Settings.GetCategoryList(true, ticket.Category);
        }

        public static SelectList GetTicketTypeList(this Ticket ticket)
        {
            var context = DependencyResolver.Current.GetService<TicketDeskContext>();
            return context.Settings.GetTicketTypeList(true, ticket.TicketType);
        }

        public static SelectList GetOwnersList(this Ticket ticket)
        {
            var roleManager = DependencyResolver.Current.GetService<TicketDeskRoleManager>();
            var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            return roleManager.GetTdInternalUsers(userManager).ToUserSelectList(false, ticket.Owner);
        }


        public static SelectList GetAssignedToList(this Ticket ticket)
        {
            var roleManager = DependencyResolver.Current.GetService<TicketDeskRoleManager>();
            var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            return roleManager.GetTdHelpDeskUsers(userManager).ToUserSelectList(ticket.AssignedTo, "-- unassigned --");
        }
    }

}