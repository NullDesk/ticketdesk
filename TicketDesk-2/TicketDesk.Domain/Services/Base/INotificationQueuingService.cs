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

using TicketDesk.Domain.Models;
namespace TicketDesk.Domain.Services
{
    public interface INotificationQueuingService
    {
        void AddTicketEventNotifications(TicketComment comment, bool isNewOrGiveUpTicket, string[] subscribers);
    }
}
