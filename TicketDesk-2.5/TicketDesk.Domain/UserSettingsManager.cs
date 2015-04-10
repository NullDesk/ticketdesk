using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Model;

namespace TicketDesk.Domain
{
    public class UserSettingsManager
    {
        private TdDomainContext Context { get; set; }
        public UserSettingsManager(TdDomainContext context)
        {
            Context = context;
        }

        public async Task<UserSetting> GetSettingsForUser(string userId)
        {
            var settings = await Context.UserSettings.FindAsync(userId);
            if (settings == null)
            {
                //settings for user not found, make default and save on separate context (so we don't commit other changes on this context as a side-effect).
                settings = UserSetting.GetDefaultSettingsForUser(userId);
                using (var tempCtx = new TdDomainContext()) 
                {
                    tempCtx.UserSettingsManager.AddSettingsForUser(settings);
                    await tempCtx.SaveChangesAsync();
                }
            }
            return settings;
        }

        public void AddSettingsForUser(UserSetting settings)
        {
            Context.UserSettings.Add(settings);
        }

        public async Task<ICollection<UserTicketListSetting>> GetUserListSettings(string userId)
        {
            var settings = await GetSettingsForUser(userId);
            return settings.ListSettings;
        }

        public async Task<UserTicketListSetting> GetUserListSettingByName(string listName, string userId)
        {
            var settings = await GetSettingsForUser(userId);
            return settings.GetUserListSettingByName(listName);
        }

    }
}
