using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Web;
using System.Web.Security;

namespace TicketDesk.Web.Client
{
    public class MefManager
    {
        //TODO: Almost all of this needs to be relocated to DB configuration in the model. 
        //      For now, just using as MEF exports to satisfy the model; sort of a hackish 
        //      way to allow circular assembly references.

        [Export("EmailNotificationsEnabled")]
        public bool EmailNotificationsEnabled() { return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailNotifications"]); }

        [Export("EmailNotificationsInitialDelayMinutes")]
        public double EmailNotificationInitialDelayMinutes() { return Convert.ToDouble(ConfigurationManager.AppSettings["EmailNotificationInitialDelayMinutes"]); }


        [Export("RuntimeSecurityMode")]
        public string RuntimeSecurityMode() { return ConfigurationManager.AppSettings["SecurityMode"]; }

        [Export("ActiveDirectoryDomain")]
        public string ActiveDirectoryDomain() { return ConfigurationManager.AppSettings["ActiveDirectoryDomain"]; }

        [Export("ActiveDirectoryUser")]
        public string ActiveDirectoryUser() { return ConfigurationManager.AppSettings["ActiveDirectoryUser"]; }

        [Export("ActiveDirectoryUserPassword")]
        public string ActiveDirectoryUserPassword() { return ConfigurationManager.AppSettings["ActiveDirectoryUserPassword"]; }

        [Export("CurrentUserNameMethod")]
        public string GetMembershipUserFromContext() { return HttpContext.Current.User.Identity.Name; }

        [Export(typeof(MembershipProvider))]
        public MembershipProvider MembersProvider
        {
            get
            {
                return (Membership.Providers.Count > 0) ? Membership.Provider : null;
            }
        }

        [Export(typeof(RoleProvider))]
        public RoleProvider RolesProvider { get { return Roles.Provider; } }

        [Export("StaffRoleName")]
        public string StaffRoleName { get { return ConfigurationManager.AppSettings["HelpDeskStaffRoleName"]; } }

        [Export("SubmitterRoleName")]
        public string SubmitterRoleName { get { return ConfigurationManager.AppSettings["TicketSubmittersRoleName"]; } }


        [Export("AdminRoleName")]
        public string AdminRoleName { get { return ConfigurationManager.AppSettings["AdministrativeRoleName"]; } }

    }
}