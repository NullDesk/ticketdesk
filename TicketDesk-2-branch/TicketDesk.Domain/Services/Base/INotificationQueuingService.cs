using TicketDesk.Domain.Models;
namespace TicketDesk.Domain.Services
{
    public interface INotificationQueuingService
    {
        void AddTicketEventNotifications(TicketComment comment, bool isNewOrGiveUpTicket, string[] subscribers);
    }
}
