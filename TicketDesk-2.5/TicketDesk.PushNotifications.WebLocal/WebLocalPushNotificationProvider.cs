using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.PushNotifications.Common;

namespace TicketDesk.PushNotifications.WebLocal
{
    public class WebLocalPushNotificationProvider : IPushNotificationProvider
    {

        public Task<bool> SendNotification(IEnumerable<Common.Model.PushNotificationItem> items)
        {
            return Task.FromResult(true);
        }

        public bool IsConfigured
        {
            //TODO: find a better way to decide if this provider should be used.
            //always enabled, since this is the last-ditch provider to use when no better option is available
            get { return true; }
        }
    }
}
