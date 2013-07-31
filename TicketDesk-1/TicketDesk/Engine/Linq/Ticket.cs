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


        private bool isGiveUp = false;
        /// <summary>
        /// Adds old assigned user to the notification list
        /// </summary>
        /// <param name="value"></param>
        partial void OnAssignedToChanging(string value)
        {
            if (!string.IsNullOrEmpty(AssignedTo))
            {
                additionalUsersForNotification.Add(AssignedTo);
                if (string.IsNullOrEmpty(value))
                {
                    isGiveUp = true;
                }
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
            if (AssignedTo != null && !usersToAdd.ContainsKey(AssignedTo))
            {
                usersToAdd.Add(AssignedTo, "Assigned");
            }
            foreach (string user in additionalUsersForNotification)
            {
                if (!usersToAdd.ContainsKey(user))
                {
                    usersToAdd.Add(user, "Subscriber");
                }
            }
            //only add help desk for brand new tickets (they should have exactly 1 comment when new)
            //  or tickets that have been given up on (was assigned but is not anymore).
            if (AssignedTo == null && (this.TicketComments.Count < 2 || isGiveUp))
            {
                string[] admins = SecurityManager.GetHelpDeskUsers().Select(a => a.Name).ToArray();
                foreach (string admin in admins)
                {
                    if (!usersToAdd.ContainsKey(admin))
                    {
                        usersToAdd.Add(admin, "HelpDesk");
                    }
                }
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

                // replaced pattern with variation based on the stock MS regex validator control's built-in pattern for email addresses
                //var rxv = new RegexStringValidator(@"^[a-zA-Z\.\-_]+@([a-zA-Z\.\-_]+\.)+[a-zA-Z]{2,4}$");
                var rxv = new RegexStringValidator(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
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
