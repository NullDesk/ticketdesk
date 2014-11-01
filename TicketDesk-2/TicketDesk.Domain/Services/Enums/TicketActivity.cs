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
using System.Linq;
using System.Text;

namespace TicketDesk.Domain.Services
{
    public enum TicketActivity
    {
        /// <summary>
        /// No change made
        /// </summary>
        NoChange,
        /// <summary>
        /// Get ticket information
        /// </summary>
        GetTicketInfo,
        /// <summary>
        /// Add one or more attachments
        /// </summary>
        ModifyAttachments,
        /// <summary>
        /// Edit ticket information
        /// </summary>
        EditTicketInfo,
        /// <summary>
        /// Answer request for more information
        /// </summary>
        SupplyMoreInfo,
        /// <summary>
        /// Request more information
        /// </summary>
        RequestMoreInfo,
        /// <summary>
        /// Cancel request for more information
        /// </summary>
        CancelMoreInfo,
        /// <summary>
        /// Re-Open a closed or resoled ticket
        /// </summary>
        ReOpen,
        /// <summary>
        /// New Ticket that is to be owned by the submitting user
        /// </summary>
        Create,
        /// <summary>
        /// Create A New ticket with a different user as the owner.
        /// </summary>
        CreateOnBehalfOf,
        /// <summary>
        /// Add a comment to the ticket
        /// </summary>
        AddComment,
        /// <summary>
        /// Take over a ticket without changing prioirty
        /// </summary>
        TakeOver,
        /// <summary>
        /// Take over ticket with a change or assignment of a priority
        /// </summary>
        TakeOverWithPriority,
        /// <summary>
        /// Assign the ticket to a help-desk staff member
        /// </summary>
        Assign,
        /// <summary>
        /// Assign the ticket to a help-desk staff member with a change or assignment of a priority
        /// </summary>
        AssignWithPriority,
        /// <summary>
        /// Reassign the ticket to a help-desk staff member
        /// </summary>
        ReAssign,
        /// <summary>
        /// Reassign the ticket to a help-desk staff member with a change or assignment of a priority
        /// </summary>
        ReAssignWithPriority,
        /// <summary>
        /// Pass the ticket to a help-desk staff member
        /// </summary>
        Pass,
        /// <summary>
        /// Pass the ticket to a help-desk staff member with a change or assignment of a priority
        /// </summary>
        PassWithPriority,
        /// <summary>
        /// Give up a ticket (mark ticket unassigned)
        /// </summary>
        GiveUp,
        /// <summary>
        /// Resolved a ticket
        /// </summary>
        Resolve,
        /// <summary>
        /// Close a resolved ticket normally
        /// </summary>
        Close,
        /// <summary>
        /// Close a ticket immediatly without going through the normal active,resolve,close process
        /// </summary>
        ForceClose
    }
}
