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

using System.Collections.Generic;
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

        public async Task ResetAllListSettingsForUserAsync(string userId)
        {
            var settings = await GetSettingsForUserAsync(userId);
            settings.ListSettings = new UserTicketListSettingsCollection
            {
                    UserTicketListSetting.GetDefaultListSettings(userId, Context.SecurityProvider.IsTdHelpDeskUser|| Context.SecurityProvider.IsTdAdministrator)
            };
        }

        public async Task<UserSetting> GetSettingsForUserAsync(string userId)
        {
            var settings = await Context.UserSettings.FindAsync(userId);
            if (settings == null)
            {
                //settings for user not found, make default and save on separate context (so we don't commit other changes on this context as a side-effect).
                settings = UserSetting.GetDefaultSettingsForUser(userId, Context.SecurityProvider.IsTdHelpDeskUser || Context.SecurityProvider.IsTdAdministrator);
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

        public async Task<ICollection<UserTicketListSetting>> GetUserListSettingsAsync(string userId)
        {
            var settings = await GetSettingsForUserAsync(userId);
            return settings.ListSettings;
        }

        public async Task<UserTicketListSetting> GetUserListSettingByNameAsync(string listName, string userId)
        {
            var settings = await GetSettingsForUserAsync(userId);
            return settings.GetUserListSettingByName(listName);
        }

    }
}
