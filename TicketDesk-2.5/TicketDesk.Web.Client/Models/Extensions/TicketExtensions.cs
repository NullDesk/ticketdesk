using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackExchange.DataExplorer.Helpers;
using TicketDesk.IO;
using TicketDesk.Web.Client;
using TicketDesk.Web.Client.Models;
using TicketDesk.Web.Identity.Model;


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

        public static SelectList GetOwnersList(this Ticket ticket, bool excludeCurrentUser = false, bool excludeCurrentOwner = false)
        {
            var roleManager = DependencyResolver.Current.GetService<TicketDeskRoleManager>();
            var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            var sec = DependencyResolver.Current.GetService<TicketDeskContextSecurityProvider>();
            IEnumerable<TicketDeskUser> all = roleManager.GetTdInternalUsers(userManager);
            if (excludeCurrentUser)
            {
                all = all.Where(u => u.Id != sec.CurrentUserId);
            }
            if (excludeCurrentOwner)
            {
                all = all.Where(u => u.Id != ticket.Owner);
            }
            return all.ToUserSelectList(false, ticket.Owner);
        }


        public static SelectList GetAssignedToList(this Ticket ticket, bool excludeCurrentUser = false, bool excludeCurrentAssignedTo = false, bool includeEmptyText = true)
        {
            var roleManager = DependencyResolver.Current.GetService<TicketDeskRoleManager>();
            var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            var sec = DependencyResolver.Current.GetService<TicketDeskContextSecurityProvider>();
            IEnumerable<TicketDeskUser> all = roleManager.GetTdHelpDeskUsers(userManager);
            if (excludeCurrentUser)
            {
                all = all.Where(u => u.Id != sec.CurrentUserId);
            }
            if (excludeCurrentAssignedTo)
            {
                all = all.Where(u => u.Id != ticket.AssignedTo);
            }
            return includeEmptyText ? all.ToUserSelectList(ticket.AssignedTo, "-- unassigned --") : all.ToUserSelectList(false, ticket.AssignedTo);
        }

        public static bool AllowEditTags(this Ticket ticket)
        {
            //TODO: is this the best place to put this check?
            var context = DependencyResolver.Current.GetService<TicketDeskContext>();
            return context.SecurityProvider.IsTdHelpDeskUser || context.Settings.GetSettingValue("AllowSubmitterRoleToEditTags", false);
        }

        public static bool AllowSetOwner(this Ticket ticket)
        {
            //TODO: is this the best place to put this check? Is this one even worth the extension?
            var context = DependencyResolver.Current.GetService<TicketDeskContext>();
            return context.SecurityProvider.IsTdHelpDeskUser;
        }

        public static bool AllowEditPriorityList(this Ticket ticket)
        {
            //TODO: is this the best place to put this check?
            var context = DependencyResolver.Current.GetService<TicketDeskContext>();
            return context.SecurityProvider.IsTdHelpDeskUser || context.Settings.GetSettingValue("AllowSubmitterRoleToEditPriority", false);
        }

        /// <summary>
        /// Commits any pending attachments for the ticket in the file store.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="tempId">The temporary identifier for pending attachments.</param>
        /// <returns>IEnumerable&lt;System.String&gt;. The list of filenames for all attachments saved</returns>
        public static IEnumerable<string> CommitPendingAttachments(this Ticket ticket, Guid tempId)
        {
            var attachments = TicketDeskFileStore.ListAttachmentInfo(tempId.ToString(), true).ToArray();
            foreach (var attachment in attachments)
            {
                TicketDeskFileStore.MoveFile(
                    attachment.Name,
                    tempId.ToString(),
                    ticket.TicketId.ToString(CultureInfo.InvariantCulture),
                    true,
                    false);
            }
            return attachments.Select(a => a.Name);
        }
    }

}