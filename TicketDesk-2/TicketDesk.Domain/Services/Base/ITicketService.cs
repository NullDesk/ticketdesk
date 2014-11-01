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

using System.Collections.Generic;
using TicketDesk.Domain.Utilities.Pagination;
using TicketDesk.Domain.Models;
using TicketDesk.Domain.Repositories;

namespace TicketDesk.Domain.Services
{
    public interface ITicketService
    {
        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        ITicketRepository Repository { get; }

        /// <summary>
        /// Gets the ticket with the specified ID.
        /// </summary>
        /// <param name="ticketId">The ticket id to fetch.</param>
        /// <returns></returns>
        Ticket GetTicket(int ticketId);

        /// <summary>
        /// Gets a paged list of tickets.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="listSettings">The list settings.</param>
        /// <returns></returns>
        IPagination<Ticket> ListTickets(int pageIndex, TicketCenterListSettings listSettings);


        /// <summary>
        /// Lists a paged list of tickets.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        IPagination<Ticket> ListTickets(int pageIndex, int pageSize, bool includeComments);

        /// <summary>
        /// Gets a list of tickets from an ordered list of ticket IDs.
        /// </summary>
        /// <param name="orderedTicketList">The ordered ticket list.</param>
        /// <returns>Tickets in the same order as the supplied ticket IDs</returns>
        IEnumerable<Ticket> ListTickets(SortedList<int, int> orderedTicketList, bool includeComments);

        /// <summary>
        /// Creates the new ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="creatorUserName">User name of the creator.</param>
        /// <param name="creatorDisplayName">Display name of the creator.</param>
        /// <param name="ownerDisplayName">Display name of the owner.</param>
        /// <returns></returns>
        int? CreateNewTicket(Ticket ticket);

        /// <summary>
        /// Adds the comment to ticket
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="comment">The comment content.</param>
        /// <returns></returns>
        bool AddCommentToTicket(Ticket ticket, string comment);

        /// <summary>
        /// Takes the over ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="comment">optional comment contents.</param>
        /// <param name="priority">a priority or null if no change to priority.</param>
        /// <returns></returns>
        bool TakeOverTicket(Ticket ticket, string comment, string priority);


        /// <summary>
        /// Resolves the ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="comment">The comment.</param>
        /// <returns></returns>
        bool ResolveTicket(Ticket ticket, string comment);



        /// <summary>
        /// Closes the ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="force">if set to <c>true</c> force closes ticket regardless of status.</param>
        /// <returns></returns>
        bool CloseTicket(Ticket ticket, string comment, bool force);


        /// <summary>
        /// Gives up a ticket and marks it unassigned again.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="comment">The comments.</param>
        /// <returns></returns>
        bool GiveUpTicket(Ticket ticket, string comment);


        /// <summary>
        /// Res the open ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="reopenAssignedToUser">if set to <c>true</c> reopen and assign to the reopening user.</param>
        /// <param name="reopenOwnedByUser">if set to <c>true</c> reopen and set owner to the reopening user.</param>
        /// <returns></returns>
        bool ReOpenTicket(Ticket ticket, string comment, bool reopenAssignedToUser, bool reopenOwnedByUser);

        /// <summary>
        /// Checks the current user's security for ticket activity.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        bool CheckSecurityForTicketActivity(Ticket ticket, TicketActivity activity);

        /// <summary>
        /// Checks the specified user's security for ticket activity.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="activity">The activity.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        bool CheckSecurityForTicketActivity(Ticket ticket, TicketActivity activity, string userName);



        /// <summary>
        /// Assigns the ticket to a help desk staff user.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="assignTo">The user to assign the ticket to.</param>
        /// <param name="priority">A priority or null if no change to priority.</param>
        /// <returns></returns>
        bool AssignTicket(Ticket ticket, string comment, string assignTo, string priority);

        /// <summary>
        /// Requests the more information from the owner about the ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="comment">The comment.</param>
        /// <returns></returns>
        bool RequestMoreInfoForTicket(Ticket ticket, string comment);

        /// <summary>
        /// Supplies more information for the ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="markActive">if set to <c>true</c> marks ticket active again.</param>
        /// <returns></returns>
        bool SupplyMoreInfoForTicket(Ticket ticket, string comment, bool markActive);


        /// <summary>
        /// Cancels the request for more information for ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="comment">The comment.</param>
        /// <returns></returns>
        bool CancelMoreInfoForTicket(Ticket ticket, string comment);

        /// <summary>
        /// Modifies the attachments for ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="attachments">The list attachment files with the desired state of the attachments</param>
        /// <returns></returns>
        bool ModifyAttachmentsForTicket(Ticket ticket, string comment, List<TicketAttachment> attachments);

        /// <summary>
        /// Adds the pending attachment.
        /// </summary>
        /// <param name="ticketId">The ticket id.</param>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        int AddPendingAttachment(int? ticketId, TicketAttachment file);

        /// <summary>
        /// Gets the attachment.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        TicketAttachment GetAttachment(int fileId);

        /// <summary>
        /// Cleans up pending attachments that were not committed to a ticket in a timely manner.
        /// </summary>
        /// <param name="hoursOld">The number of hours old a pending attachment should be before being removed.</param>
        /// <returns></returns>
        bool CleanUpDerelictAttachments(int hoursOld);

        /// <summary>
        /// Edits the ticket.
        /// </summary>
        /// <param name="ticket">The ticket being edited with updated values.</param>
        /// <param name="comment">The comment.</param>
        /// <returns></returns>
        bool EditTicketDetails(Ticket ticket, string comment);

        /// <summary>
        /// Gets possible suggestions for the last element of an incomplete taglist.
        /// </summary>
        /// <param name="partialTagList"></param>
        /// <param name="maxNumTagsToReturn">The max num tags to return.</param>
        /// <returns></returns>
        string[] GetTagCompletionList(string partialTagList, int maxNumTagsToReturn);
    }
}
