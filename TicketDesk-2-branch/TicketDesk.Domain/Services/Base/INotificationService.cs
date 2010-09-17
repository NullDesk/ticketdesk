using TicketDesk.Domain.Models;
namespace TicketDesk.Domain.Services
{
    public interface INotificationService
    {
        void AddTicketEventNotifications(TicketComment comment, bool isNewOrGiveUpTicket, string[] subscribers);
    }
}
