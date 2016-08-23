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
            var isHelpDeskUser = Context.SecurityProvider.IsTdHelpDeskUser ||
                                    Context.SecurityProvider.IsTdAdministrator;
            var settings = await Context.UserSettings.FindAsync(userId);

            //ensure settings exist
            if (settings == null )
            {
                settings = UserSetting.GetDefaultSettingsForUser(userId, isHelpDeskUser);
                using (var tempCtx = new TdDomainContext())
                {
                    await tempCtx.UserSettingsManager.AddOrUpdateSettingsForUser(settings);
                    await tempCtx.SaveChangesAsync();
                }
            }
            //ensure that the user has all required lists for their role, if not blow away list settings and recreate
            if (!settings.ListSettings.HasRequiredDefaultListSettings(isHelpDeskUser))
            {
                settings.ListSettings = new UserTicketListSettingsCollection
                {
                    UserTicketListSetting.GetDefaultListSettings(userId, isHelpDeskUser)
                };
                using (var tempCtx = new TdDomainContext())
                {
                    await tempCtx.UserSettingsManager.AddOrUpdateSettingsForUser(settings);
                    await tempCtx.SaveChangesAsync();
                }
            }
            return settings;
        }

        public async Task AddOrUpdateSettingsForUser(UserSetting settings)
        {
            var existing = await Context.UserSettings.FindAsync(settings.UserId);
            if (existing != null)
            {
                //replace existing with new settings
                // ReSharper disable once RedundantAssignment
                existing = settings;
            }
            else
            {
                Context.UserSettings.Add(settings);
            }
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
