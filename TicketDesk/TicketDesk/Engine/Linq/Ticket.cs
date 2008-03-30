using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Net.Mail;

namespace TicketDesk.Engine.Linq
{
    public partial class Ticket
    {
        /// <summary>
        /// Adds old assigned user to the notification list
        /// </summary>
        /// <param name="value"></param>
        partial void OnAssignedToChanging(string value)
        {
            AddNotificationEmailAddress(AssignedTo);
        }

        /// <summary>
        /// Adds old owner to the notification list
        /// </summary>
        /// <param name="value"></param>
        partial void OnOwnerChanging(string value)
        {
            AddNotificationEmailAddress(Owner);
        }

        private Dictionary<string, MailAddress> notificationAddresses = new Dictionary<string, MailAddress>();

        /// <summary>
        /// Gets notification email addresses for users that will be notified about the most recent change to the ticket.
        /// </summary>
        /// <remarks>
        /// Will always add the current owner and assigned to user plus any specified additional users. Users
        /// will not be added multiple times, and will not be added if they were the user that made the most
        /// recent change.
        /// </remarks>
        /// <param name="additionalUsers">Additional users that should be notified</param>
        /// <returns></returns>
        public List<MailAddress> GetNotificationEmailAddressesForUsers(params string[] additionalUsers)
        {
            List<MailAddress> addresses = null;
            AddNotificationEmailAddress(Owner);
            AddNotificationEmailAddress(AssignedTo);
            foreach(string user in additionalUsers)
            {
                AddNotificationEmailAddress(user);
            }
            if(notificationAddresses.Count > 0)
            {
                addresses = notificationAddresses.Values.ToList();
            }
            return addresses;
        }

        private void AddNotificationEmailAddress(string user)
        {
            if(!string.IsNullOrEmpty(user) && !notificationAddresses.ContainsKey(user) && user != LastUpdateBy)
            {
                string email = SecurityManager.GetUserEmailAddress(user);
                if(!string.IsNullOrEmpty(email))
                {
                    MailAddress addy = new MailAddress(email, SecurityManager.GetUserDisplayName(user));
                    notificationAddresses.Add(user, addy);
                }
            }
        }
    }
}
