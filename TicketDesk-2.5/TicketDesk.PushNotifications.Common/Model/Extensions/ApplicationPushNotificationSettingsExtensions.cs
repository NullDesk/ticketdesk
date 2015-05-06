using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.PushNotifications.Common.Model
{
    public static class ApplicationPushNotificationSettingsExtensions
    {
        public static ApplicationPushNotificationSetting GetTicketDeskSettings(this DbSet<ApplicationPushNotificationSetting> settings)
        {
            return settings.FirstOrDefault(s => s.ApplicationName == "TicketDesk");
        }
    }
}
