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

namespace TicketDesk.Domain.Model
{


    [Flags]
    public enum TicketActivity
    {


        /// <summary>
        /// No change made
        /// </summary>
        NoChange = 0,
        /// <summary>
        /// Get ticket information
        /// </summary>
        GetTicketInfo = 1,
        /// <summary>
        /// Add one or more attachments
        /// </summary>
        ModifyAttachments = 2,
        /// <summary>
        /// Edit ticket information
        /// </summary>
        EditTicketInfo = 4,
        /// <summary>
        /// Answer request for more information
        /// </summary>
        SupplyMoreInfo = 8,
        /// <summary>
        /// Request more information
        /// </summary>
        RequestMoreInfo = 16,
        /// <summary>
        /// Cancel request for more information
        /// </summary>
        CancelMoreInfo = 32,
        /// <summary>
        /// Re-Open a closed or resoled ticket
        /// </summary>
        ReOpen = 64,
        /// <summary>
        /// New Ticket that is to be owned by the submitting user
        /// </summary>
        Create = 128,
        /// <summary>
        /// Create A New ticket with a different user as the owner.
        /// </summary>
        CreateOnBehalfOf = 256,
        /// <summary>
        /// Add a comment to the ticket
        /// </summary>
        AddComment = 512,
        /// <summary>
        /// Take over a ticket without changing prioirty
        /// </summary>
        TakeOver = 1024,
        /// <summary>
        /// Take over ticket with a change or assignment of a priority
        /// </summary>
        TakeOverWithPriority = 2048,
        /// <summary>
        /// Assign the ticket to a help-desk staff member
        /// </summary>
        Assign = 4096,
        /// <summary>
        /// Assign the ticket to a help-desk staff member with a change or assignment of a priority
        /// </summary>
        AssignWithPriority = 8192,
        /// <summary>
        /// Reassign the ticket to a help-desk staff member
        /// </summary>
        ReAssign = 16384,
        /// <summary>
        /// Reassign the ticket to a help-desk staff member with a change or assignment of a priority
        /// </summary>
        ReAssignWithPriority = 32768,
        /// <summary>
        /// Pass the ticket to a help-desk staff member
        /// </summary>
        Pass = 65536,
        /// <summary>
        /// Pass the ticket to a help-desk staff member with a change or assignment of a priority
        /// </summary>
        PassWithPriority = 131072,
        /// <summary>
        /// Give up a ticket (mark ticket unassigned)
        /// </summary>
        GiveUp = 262144,
        /// <summary>
        /// Resolved a ticket
        /// </summary>
        Resolve = 524288,
        /// <summary>
        /// Close a resolved ticket normally
        /// </summary>
        Close = 1048576,
        /// <summary>
        /// Close a ticket immediately without going through the normal active,resolve,close process
        /// </summary>
        ForceClose = 2097152
    }

}
