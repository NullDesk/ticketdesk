// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using TicketDesk.PushNotifications.Model;

namespace TicketDesk.PushNotifications.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<TdPushNotificationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TicketDeskPushNotifications";
            AutomaticMigrationDataLossAllowed = true;
        }
        protected override void Seed(TdPushNotificationContext context)
        {
            var demoMode = ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"];
            if (!string.IsNullOrEmpty(demoMode) && demoMode.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                DemoPushNotificationDataManager.SetupDemoPushNotificationData(context);
            }
          
            base.Seed(context);
        }

       
    }
}
