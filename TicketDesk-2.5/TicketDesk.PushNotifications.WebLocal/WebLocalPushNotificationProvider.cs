// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.Collections.Generic;
using System.Threading.Tasks;
using TicketDesk.PushNotifications.Common;
using TicketDesk.PushNotifications.Common.Model;

namespace TicketDesk.PushNotifications.WebLocal
{
    public class WebLocalPushNotificationProvider : IPushNotificationProvider
    {

        public Task<bool> SendNotification(IEnumerable<PushNotificationItem> items)
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
