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

        private static bool _homeEnabled;

        static ApplicationConfig()
        {
            var registerEnabled = ConfigurationManager.AppSettings["ticketdesk:RegisterEnabled"];
            _registerEnabled = (registerEnabled ?? string.Empty).Equals("true", StringComparison.InvariantCultureIgnoreCase);
            var homeEnabled = ConfigurationManager.AppSettings["ticketdesk:HomeEnabled"];
            _homeEnabled = (homeEnabled ?? string.Empty).Equals("true", StringComparison.InvariantCultureIgnoreCase); ;
        }

        public static bool RegisterEnabled { get { return _registerEnabled; } }

        public static bool HomeEnabled { get { return _homeEnabled; } }
    }
}