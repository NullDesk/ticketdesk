using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TicketDesk.Web.Client
{
    public static class ApplicationConfig
    {
        private static bool _registerEnabled;

        static ApplicationConfig()
        {
            var registerEnabled = ConfigurationManager.AppSettings["ticketdesk:RegisterEnabled"];
            _registerEnabled = (registerEnabled ?? string.Empty).Equals("true", StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool RegisterEnabled { get { return _registerEnabled; } }
    }
}