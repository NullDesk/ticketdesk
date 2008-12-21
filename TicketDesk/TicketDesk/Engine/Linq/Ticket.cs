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
        /// <summary>
        /// Adds old assigned user to the notification list
        /// </summary>
        /// <param name="value"></param>
        partial void OnAssignedToChanging(string value)
        {
            if (!string.IsNullOrEmpty(AssignedTo))
            {
                additionalUsers.Add(AssignedTo);
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
                additionalUsers.Add(Owner);
            }
        }


        private List<string> additionalUsers = new List<string>();

        public List<TicketEventNotification> CreateTicketEventNotificationsForComment(int commentId)
        {
            List<TicketEventNotification> eventNotes = new List<TicketEventNotification>();
            var ownerNote = CreateTicketEventNotificationForUser(commentId, Owner, "Owner");
            if (ownerNote != null)
            {
                eventNotes.Add(ownerNote);
            }

            foreach (string user in additionalUsers)
            {
                var addNote = CreateTicketEventNotificationForUser(commentId, user, "Subscriber");
                if (addNote != null)
                {
                    eventNotes.Add(addNote);
                }
            }

            if (AssignedTo == null && additionalUsers.Count < 1)//the additional users check prevents notifications to admin when a ticket is assigned for the first time.
            {
                string[] admins = SecurityManager.GetAdministrativeUsers().Select(a => a.Name).ToArray();
                foreach (string admin in admins)
                {
                    var adminNote = CreateTicketEventNotificationForUser(commentId, admin, "HelpDesk");
                    if (adminNote != null)
                    {
                        eventNotes.Add(adminNote);
                    }
                }
            }
            else
            {
                //Wee! 
                var assNote = CreateTicketEventNotificationForUser(commentId, AssignedTo, "Assigned");
                if (assNote != null)
                {
                    eventNotes.Add(assNote);
                }
            }

            return eventNotes;
        }



        private TicketEventNotification CreateTicketEventNotificationForUser(int commentId, string user, string userType)
        {
            TicketEventNotification note = null;
            if (!string.IsNullOrEmpty(user) && user != LastUpdateBy)
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

                if (!string.IsNullOrEmpty(email) && emailValid)
                {
                    note = new TicketEventNotification();
                    note.TicketId = TicketId;
                    note.CommentId = commentId;
                    note.NotifyUser = user;
                    note.NotifyEmail = email;
                    note.NotifyUserDisplayName = SecurityManager.GetUserDisplayName(user);
                    note.NotifyUserReason = userType;
                }
            }
            return note;
        }


    }
}
