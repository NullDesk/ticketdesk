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

        public async Task ResetAllListSettingsForUser(string userId)
        {
            var settings = await GetSettingsForUser(userId);
            settings.ListSettings = new UserTicketListSettingsCollection
            {
                    UserTicketListSetting.GetDefaultListSettings(userId, Context.SecurityProvider.IsTdHelpDeskUser)
            };
        }

        public async Task<UserSetting> GetSettingsForUser(string userId)
        {

            var settings = await Context.UserSettings.FindAsync(userId);
            if (settings == null)
            {
                //settings for user not found, make default and save on separate context (so we don't commit other changes on this context as a side-effect).
                settings = UserSetting.GetDefaultSettingsForUser(userId, Context.SecurityProvider.IsTdHelpDeskUser);
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
