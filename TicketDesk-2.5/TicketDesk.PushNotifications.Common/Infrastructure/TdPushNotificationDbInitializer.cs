using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.PushNotifications.Common.Migrations;

namespace TicketDesk.PushNotifications.Common
{
    public class TdPushNotificationDbInitializer : MigrateDatabaseToLatestVersion<TdPushNotificationContext, Configuration>
    {
        //no implementation, defined here to simplify and unify naming conventions and usage patterns 
    }
}
