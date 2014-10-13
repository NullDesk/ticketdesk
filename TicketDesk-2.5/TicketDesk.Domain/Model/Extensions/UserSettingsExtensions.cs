using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Model
{
    public static class UserSettingsExtensions
    {
        public static UserTicketListSetting GetUserListSettingByName(this DbSet<UserSetting> settings, string listName, string userId)
        {
            return GetUserSettings(settings, userId).ListSettings.FirstOrDefault(us => us.ListName == listName);
        }

        public static ICollection<UserTicketListSetting> GetAllUserListSettings(this DbSet<UserSetting> settings,
            string userId)
        {
            return GetUserSettings(settings, userId).ListSettings;

        }

        private static UserSetting GetUserSettings(DbSet<UserSetting> settings, string userId)
        {
            return (settings.FirstOrDefault(us => us.UserId == userId) ??
                    UserSetting.GetDefaultSettingsForUser(userId));
        }
    }
}
