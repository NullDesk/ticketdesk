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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TicketDesk.Domain.Models.DataAnnotations;
using TicketDesk.Domain.Utilities;

namespace TicketDesk.Domain.Models
{
    [MetadataType(typeof(TicketMeta))]
    public partial class Ticket
    {
        internal string[] GetNotificationSubscribers()
        {
            var subs = new List<string>();
            if (!string.IsNullOrEmpty(PreviousOwner) && !PreviousOwner.Equals(Owner, StringComparison.InvariantCultureIgnoreCase))
            {
                subs.Add(PreviousOwner);
            }
            if (!string.IsNullOrEmpty(PreviousAssignedUser) && !PreviousAssignedUser.Equals(AssignedTo, StringComparison.InvariantCultureIgnoreCase))
            {
                subs.Add(PreviousAssignedUser);
            }
            if (!string.IsNullOrEmpty(Owner))
            {
                subs.Add(Owner);
            }
            if (!string.IsNullOrEmpty(AssignedTo))
            {
                subs.Add(AssignedTo);
            }
            return subs.ToArray();
        }


        internal string PreviousOwner { get; set; }
        internal string PreviousAssignedUser { get; set; }

        protected override void OnPropertyChanging(string property)
        {
            if (property == "AssignedTo")
            {
                PreviousAssignedUser = AssignedTo;
            }
            if (property == "Owner")
            {
                PreviousOwner = Owner;
            }
            base.OnPropertyChanging(property);
        }

        [DisplayName("Details")]
        public string HtmlDetails
        {
            get
            {
                var md = new Markdown();
                return (IsHtml) ? Details : md.Transform(Details, false);
            }
        }

       
    }
}
