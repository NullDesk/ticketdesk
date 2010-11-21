using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketDesk.Domain.Services
{
    public interface INotificationSendingService
    {
        void ProcessWaitingTicketEventNotifications();
    }
}
