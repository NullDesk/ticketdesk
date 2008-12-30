// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.
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
    [SortableFields("TicketId", "Type", "Category", "Title", "CreatedBy", "CreatedDate", "Owner",
                    "AssignedTo", "CurrentStatus", "LastUpdateBy", "LastUpdateDate", "Priority",
                    "AffectsCustomer")]
    public partial class Ticket
    {
        private List<string> additionalUsersForNotification = new List<string>();



        /// <summary>
        /// Adds old assigned user to the notification list
        /// </summary>
        /// <param name="value"></param>
        partial void OnAssignedToChanging(string value)
        {
            if (!string.IsNullOrEmpty(AssignedTo))
            {
                additionalUsersForNotification.Add(AssignedTo);
            }
        }

        /// <summary>
        /// Adds old owner to the notification list
        /// </summary>
        /// <param name="value"></param>
        partial void OnOwnerChanging(string value)
        {
            if (!string.IsNullOrEmpty(Owner))
            {
                additionalUsersForNotification.Add(Owner);
            }
        }


       

        public List<TicketEventNotification> CreateTicketEventNotificationsForComment(int commentId, string commentBy)
        {
            List<TicketEventNotification> eventNotes = new List<TicketEventNotification>();
            
            Dictionary<string, string> usersToAdd = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(Owner))
            {
                usersToAdd.Add(Owner, "Owner");
            }
            foreach (string user in additionalUsersForNotification)
            {
                if (!usersToAdd.ContainsKey(user))
                {
                    usersToAdd.Add(user, "Subscriber");
                }
            }
            if (AssignedTo == null && additionalUsersForNotification.Count < 1)//the additional users check prevents notifications to admin when a ticket is assigned for the first time.
            {
                string[] admins = SecurityManager.GetAdministrativeUsers().Select(a => a.Name).ToArray();
                foreach (string admin in admins)
                {
                    if (!usersToAdd.ContainsKey(admin))
                    {
                        usersToAdd.Add(admin, "HelpDesk");
                    }
                }
            }
            else if (!usersToAdd.ContainsKey(AssignedTo))
            {
                usersToAdd.Add(AssignedTo, "Assigned");
            }

            foreach (var u in usersToAdd)
            {
                var note = CreateTicketEventNotificationForUser(commentId, commentBy, u.Key, u.Value);
                if(note != null)
                {
                    eventNotes.Add(note);
                }
            }

            return eventNotes;
        }



        private TicketEventNotification CreateTicketEventNotificationForUser(int commentId, string commentBy, string user, string userType)
        {
            TicketEventNotification note = null;
            if (!string.IsNullOrEmpty(user))
            {
                bool emailValid = false;
                string email = SecurityManager.GetUserEmailAddress(user);
                var rxv = new RegexStringValidator(@"^[a-zA-Z\.\-_]+@([a-zA-Z\.\-_]+\.)+[a-zA-Z]{2,4}$");
                try
                {
                    rxv.Validate(email);
                    emailValid = true;
                }
                catch { }

                note = new TicketEventNotification();
                note.TicketId = TicketId;
                note.CommentId = commentId;
                note.NotifyUser = SecurityManager.GetFormattedUserName(user);

                note.NotifyUserDisplayName = SecurityManager.GetUserDisplayName(user);
                note.NotifyUserReason = userType;
                note.EventGeneratedByUser = commentBy;
                if (!string.IsNullOrEmpty(email) && emailValid)
                {
                    note.NotifyEmail = email;
                }
                else
                {
                    note.NotifyEmail = "invalid";
                }
            }
            return note;
        }


    }
}
