// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Repositories;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Services
{
    [Export(typeof(IApplicationSettingsService))]
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public IApplicationSettingsRepository Repository { get; set; }

        /// <summary>
        /// Unit test ctor, Initializes a new instance of the <see cref="TdSettingService"/> class.
        /// </summary>
        /// <param name="settingRepository">The setting repository.</param>
        [ImportingConstructor]
        public ApplicationSettingsService(IApplicationSettingsRepository settingsRepository)
        {
            Repository = settingsRepository;
        }

        /// <summary>
        /// Gets a collection of available priorities.
        /// </summary>
        /// <returns></returns>
        public string[] AvailablePriorities
        {
            get
            {
                return (string[])GetSettingValue("PriorityList");
            }
        }

        /// <summary>
        /// Gets a collection of available categories.
        /// </summary>
        /// <returns></returns>
        public string[] AvailableCategories
        {
            get
            {
                return (string[])GetSettingValue("CategoryList");
            }
        }

        /// <summary>
        /// Gets the available ticket types.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public string[] AvailableTicketTypes
        {
            get
            {
                return (string[])GetSettingValue("TicketTypesList");
            }
        }

        public IEnumerable<Setting> CurrentSettings
        {
            get
            {
                return Repository.GetAllSettings();
            }
        }

        public bool SaveSettings(IEnumerable<Setting> settingsToSave)
        {

            return Repository.SaveSettings(settingsToSave);
        }

        /// <summary>
        /// Gets the setting value from the application settings store.
        /// </summary>
        /// <remarks>This method will attempt to convert the setting value to the appropriate .NET type, or throw an exception if the value cannot convert.</remarks>
        /// <param name="settingName">Name of the setting to retrieve.</param>
        /// <returns>The setting value converted to the appropriate underlying .NET type.</returns>
        public object GetSettingValue(string settingName)
        {
            object settingValue = null;
            var setting = Repository.GetAllSettings().SingleOrDefault(s => s.SettingName == settingName);
            if (setting != null && !string.IsNullOrEmpty(setting.SettingValue))
            {
                switch (setting.SettingType)
                {
                    case "StringList":
                        settingValue = setting.SettingValue.Split(',');
                        break;
                    case "SimpleString":
                        settingValue = setting.SettingValue;
                        break;
                    case "BoolString":
                        try
                        {
                            settingValue = Convert.ToBoolean(setting.SettingValue);
                        }
                        catch { ThrowSettingTypeException(setting.SettingName, setting.SettingType); }
                        break;
                    case "IntString":
                        try
                        {
                            settingValue = Convert.ToInt32(setting.SettingValue);
                        }
                        catch { ThrowSettingTypeException(setting.SettingName, setting.SettingType); }
                        break;
                    case "DoubleString":
                        try
                        {
                            settingValue = Convert.ToDouble(setting.SettingValue);
                        }
                        catch { ThrowSettingTypeException(setting.SettingName, setting.SettingType); }
                        break;
                    default:
                        break;
                }

            }
            return settingValue;
        }


        private void ThrowSettingTypeException(string settingName, string settingType)
        {
            throw new ApplicationException(string.Format("ApplicationSettings Conversion Error: unable to convert SettingName [{0}] for specified SettingType [{1}]", settingName, settingType));

        }

        public EditorModes DefaultEditorMode
        {
            get { throw new NotImplementedException(); }
        }

        public EditorModes[] GetAllowedEditorModesForRole(string roleName)
        {
            throw new NotImplementedException();
        }




        public int CleanupPendingAttachmentsAfterHours
        {
            get
            {
                return (int)(GetSettingValue("CleanupPendingAttachmentsAfterHours") ?? 1);
            }
        }

        public bool EnableOutlookFriendlyHtmlEmail
        {
            get
            {
                return (bool)(GetSettingValue("EnableOutlookFriendlyHtmlEmail") ?? false);
            }
        }

        public bool HideHomePage
        {
            get
            {
                return (bool)(GetSettingValue("HideHomePage") ?? false);
            }
        }

        public int EmailDeliveryTimerIntervalMinutes
        {
            get
            {
                return (int)(GetSettingValue("EmailDeliveryTimerIntervalMinutes") ?? 5);
            }
        }

        public string SiteRootUrlForEmail
        {
            get
            {
                return (string)(GetSettingValue("SiteRootUrlForEmail") ?? "http://localhost:2534");
            }
        }

        public string LuceneDirectory//cannot export this one from here, the export will come from the front-end where it can map this value using web hosting namespace
        {
            get
            {
                return (string)(GetSettingValue("LuceneDirectory") ?? "~/TdSearchIndex");
            }
        }

        public bool AllowSubmitterRoleToEditTags
        {
            get
            {
                return (bool)(GetSettingValue("AllowSubmitterRoleToEditTags") ?? true);
            }
        }
        public bool AllowSubmitterRoleToEditPriority
        {
            get
            {
                return (bool)(GetSettingValue("AllowSubmitterRoleToEditPriority") ?? false);
            }
        }

        public bool CreateSqlMembershipRegistrationsAsSubmitters
        {
            get
            {
                return (bool)(GetSettingValue("CreateSqlMembershipRegistrationsAsSubmitters") ?? true);
            }
        }

        #region MEF Setting Exports

        [Export("RefreshSecurityCacheMinutes")]
        public int RefreshSecurityCacheMinutes
        {
            get
            {
                return (int)(GetSettingValue("RefreshSecurityCacheMinutes") ?? 60);
            }
        }

        [Export("AdUserPropertiesSqlCacheRefreshMinutes")]
        public int AdUserPropertiesSqlCacheRefreshMinutes
        {
            get
            {
                return (int)(GetSettingValue("AdUserPropertiesSqlCacheRefreshMinutes") ?? 120);
            }
        }

        //////////////////////////////////////////////////

        [Export("EmailNotificationsEnabled")]
        public bool EmailNotificationsEnabled() { return (bool)(GetSettingValue("EnableEmailNotifications") ?? false); }

        [Export("EmailServiceName")]
        public string EmailServiceName() { return GetSettingValue("EmailServiceName") as string; }

        [Export("EmailNotificationsInitialDelayMinutes")]
        public double EmailNotificationInitialDelayMinutes() { return (double)(GetSettingValue("EmailNotificationInitialDelayMinutes") ?? 2d); }

        [Export("HelpDeskBroadcastNotificationsEnabled")]
        public bool HelpDeskBroadcastNotificationsEnabled() { return (bool)(GetSettingValue("HelpDeskBroadcastNotificationsEnabled") ?? true); }

        [Export("EmailMaxConsolidationWaitMinutes")]
        public double EmailMaxConsolidationWaitMinutes() { return (double)(GetSettingValue("EmailMaxConsolidationWaitMinutes") ?? 12d); }

        [Export("EmailResendDelayMinutes")]
        public int EmailResendDelayMinutes() { return (int)(GetSettingValue("EmailResendDelayMinutes") ?? 5); }

        [Export("EmailMaxDeliveryAttempts")]
        public int EmailMaxDeliveryAttempts() { return (int)(GetSettingValue("EmailMaxDeliveryAttempts") ?? 5); }

        [Export("FromEmailDisplayName")]
        public string FromEmailDisplayName() { return (string)(GetSettingValue("FromEmailDisplayName") ?? "TicketDesk"); }

        [Export("FromEmailAddress")]
        public string FromEmailAddress() { return (string)(GetSettingValue("FromEmailAddress") ?? "ticketdesk@nowhere.com"); }

        [Export("BlindCopyToEmailAddress")]
        public string BlindCopyToEmailAddress() { return (string)(GetSettingValue("BlindCopyToEmailAddress") ?? null); }

        #endregion



    }
}
