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
using System.Threading;
using System.Threading.Tasks;
using TicketDesk.PushNotifications.Model;

namespace TicketDesk.PushNotifications
{
    public interface IPushNotificationDeliveryProvider
    {
        string DestinationType { get; }

        IDeliveryProviderConfiguration Configuration { get; set; }

        Task<object> GenerateMessageAsync(PushNotificationItem notificationItem, CancellationToken ct);
        
        Task SendReadyMessageAsync(PushNotificationItem notificationItem, int retryMax, int retryIntv, CancellationToken ct);
    }
}
