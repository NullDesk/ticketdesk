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
using System.ComponentModel;

namespace TicketDesk.Domain.Model
{

    [Flags]
    public enum TicketActivity
    {
        /// <summary>
        /// No activity
        /// </summary>
        None = 0,

        //TODO: add a display-order attribute so we can sort these in the UI when we generate the buttons, or we can just sort these in place by magnitude
        //TODO: might be a good idea to add attributes indicating if comments are required or not
        [Description("Attachments")]
        ModifyAttachments = 1,
        /// <summary>
        /// Edit ticket information
        /// </summary>
        [Description("Edit")]
        EditTicketInfo = 2,
        /// <summary>
        /// Answer request for more information
        /// </summary>
        [Description("Provide Info")]
        SupplyMoreInfo = 4,
        /// <summary>
        /// Request more information
        /// </summary>
        [Description("Request More Info")]
        RequestMoreInfo = 8,
        /// <summary>
        /// Cancel request for more information
        /// </summary>
        [Description("Cancel More Info")]
        CancelMoreInfo = 16,
        /// <summary>
        /// Re-Open a closed or resoled ticket
        /// </summary>
        [Description("Re-open")]
        ReOpen = 32,
        /// <summary>
        /// New Ticket that is to be owned by the submitting user
        /// </summary>
        [Description("Create")]
        Create = 64,
        /// <summary>
        /// Create A New ticket with a different user as the owner.
        /// </summary>
        [Description("Create")]
        CreateOnBehalfOf = 128,
        /// <summary>
        /// Add a comment to the ticket
        /// </summary>
        [Description("Comment")]
        AddComment = 256,
        /// <summary>
        /// Take over a ticket without changing prioirty
        /// </summary>
        [Description("Take Over")]
        TakeOver = 512,
        
        /// <summary>
        /// Assign the ticket to a help-desk staff member
        /// </summary>
        [Description("Assign")]
        Assign = 1024,
        /// <summary>
        /// Reassign the ticket to a help-desk staff member
        /// </summary>
        [Description("Re-assign")]
        ReAssign = 2048,
        /// <summary>
        /// Pass the ticket to a help-desk staff member
        /// </summary>
        [Description("Pass")]
        Pass = 4096,
        /// <summary>
        /// Give up a ticket (mark ticket unassigned)
        /// </summary>
        [Description("Give Up")]
        GiveUp = 8192,
        /// <summary>
        /// Resolved a ticket
        /// </summary>
        [Description("Resolve")]
        Resolve = 16384,
        /// <summary>
        /// Close a resolved ticket normally
        /// </summary>
        [Description("Close")]
        Close = 32768,
        /// <summary>
        /// Close a ticket immediately without going through the normal active,resolve,close process
        /// </summary>
        [Description("Force Close")]
        ForceClose = 65536 
    }
}
