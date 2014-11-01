// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Web;
using System.Web.Security;
using TicketDesk.Web.Client.Controllers;
using TicketDesk.Domain.Services;
using TicketDesk.Web.Client.Areas.Admin.Controllers;

namespace TicketDesk.Web.Client
{
    public class MefManager
    {
        

        [Export("RuntimeSecurityMode")]
        public string RuntimeSecurityMode() { return ConfigurationManager.AppSettings["SecurityMode"]; }

        [Export("ActiveDirectoryDomain")]
        public string ActiveDirectoryDomain() { return ConfigurationManager.AppSettings["ActiveDirectoryDomain"]; }

        [Export("ActiveDirectoryUser")]
        public string ActiveDirectoryUser() { return ConfigurationManager.AppSettings["ActiveDirectoryUser"]; }

        [Export("ActiveDirectoryUserPassword")]
        public string ActiveDirectoryUserPassword() { return ConfigurationManager.AppSettings["ActiveDirectoryUserPassword"]; }

        [Export("CurrentUserNameMethod")]
        public string GetMembershipUserFromContext()
        {
            string user = null;
            if (HttpContext.Current != null && HttpContext.Current.User != null & HttpContext.Current.User.Identity != null)
            {
                user = HttpContext.Current.User.Identity.Name;
            }
            return user;
        }

        [Export(typeof(MembershipProvider))]
        public MembershipProvider MembersProvider { get { return (Membership.Providers.Count > 0) ? Membership.Provider : null; } }

        [Export(typeof(RoleProvider))]
        public RoleProvider RolesProvider { get { return Roles.Provider; } }

        [Export("StaffRoleName")]
        public string StaffRoleName { get { return ConfigurationManager.AppSettings["HelpDeskStaffRoleName"]; } }

        [Export("SubmitterRoleName")]
        public string SubmitterRoleName { get { return ConfigurationManager.AppSettings["TicketSubmittersRoleName"]; } }

        [Export("AdminRoleName")]
        public string AdminRoleName { get { return ConfigurationManager.AppSettings["AdministrativeRoleName"]; } }

        [Export("TicketNotificationHtmlEmailContent")]
        protected string TicketNotificationHtmlEmailContent(TicketDesk.Domain.Models.TicketEventNotification notification, int firstUnsentCommentId)
        {
            var controller = new EmailTemplateController();
            return controller.GenerateTicketNotificationHtmlEmailBody(notification, firstUnsentCommentId);
        }

        [Export("TicketNotificationTextEmailContent")]
        protected string TicketNotificationTexxtEmailContent(TicketDesk.Domain.Models.TicketEventNotification notification, int firstUnsentCommentId)
        {
            var controller = new EmailTemplateController();
            return controller.GenerateTicketNotificationTextEmailBody(notification, firstUnsentCommentId);
        }

        [Export("LuceneDirectory")]
        public string LuceneDirectory
        {
            get
            {
                var appSettings = MefHttpApplication.ApplicationContainer.GetExportedValue<IApplicationSettingsService>();
                var rawDir = appSettings.LuceneDirectory;
                return (string.Equals(rawDir, "ram", StringComparison.InvariantCultureIgnoreCase)) ? rawDir : System.Web.Hosting.HostingEnvironment.MapPath(rawDir);
            }
        }

    }
}