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

using System;
using System.Collections.Generic;
using System.Configuration;

namespace TicketDesk.Notifications.Common
{
    public class TicketDeskNotificationContext
    {
        private static TicketDeskNotificationContext _current;

        private static Func<NotificaitonContextConfiguration> GetConfigurationFunc { get; set; }

        public static void Configure(Func<NotificaitonContextConfiguration> getConfigurationFunc)
        {
            GetConfigurationFunc = getConfigurationFunc;
        }

        public static TicketDeskNotificationContext Current
        {
            get
            {
                if (GetConfigurationFunc == null)
                {
                    throw new ConfigurationErrorsException("Attempt to access the notifcation system before TicketDeskNotificationContext has been configured");
                }
                return _current ?? (_current = new TicketDeskNotificationContext(GetConfigurationFunc()));
            }
        }

        internal TicketDeskNotificationContext(NotificaitonContextConfiguration configuration)
        {
            NotifcationProviders = configuration.NotificationProviders;
        }

        public IEnumerable<INotifcationProvider> NotifcationProviders { get; set; }
    }
}
