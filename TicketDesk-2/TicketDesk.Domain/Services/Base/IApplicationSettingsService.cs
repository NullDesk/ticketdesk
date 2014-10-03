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
using TicketDesk.Domain.Repositories;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Services
{
    public interface IApplicationSettingsService
    {
        IApplicationSettingsRepository Repository { get; }

        //TODO: When adding the update side for the editor modes settings, the set side of allowed editor modes must ensure that
        //          it throws a rules exception if the default application editor is not included in the list.
        //      On the flip side, the set for default editor needs to throw an exception if ANY of the roles lack the new value 
        //          in their list of allowed modes OR it needs to automatically add the new default to any roles that are missing
        //          that value.



        /// <summary>
        /// Gets the default editor mode.
        /// </summary>
        /// <value>The default editor mode.</value>
        EditorModes DefaultEditorMode { get; }

        /// <summary>
        /// Gets the allowed custom editor options that a role is allowed to choose from.
        /// </summary>
        /// <param name="roleName">Name of the role whose allowed modes you wish to fetch.</param>
        /// <returns></returns>
        /// <value>The allowed user editor modes for the specified role.</value>
        EditorModes[] GetAllowedEditorModesForRole(string roleName);


        /// <summary>
        /// Gets a collection of available priorities.
        /// </summary>
        /// <returns></returns>
        string[] AvailablePriorities { get; }
        /// <summary>
        /// Gets a collection of available categories.
        /// </summary>
        /// <returns></returns>
        string[] AvailableCategories { get; }

        /// <summary>
        /// Gets the available ticket types.
        /// </summary>
        /// <returns></returns>
        string[] AvailableTicketTypes { get; }

        /// <summary>
        /// Gets all settings.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Setting> CurrentSettings { get; }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="settingsToSave">The settings to save.</param>
        /// <returns></returns>
        bool SaveSettings(IEnumerable<Setting> settingsToSave);

        /// <summary>
        /// Gets the setting value from the application settings store.
        /// </summary>
        /// <remarks>This method will attempt to convert the setting value to the appropriate .NET type, or throw an exception if the value cannot convert.</remarks>
        /// <param name="settingName">Name of the setting to retrieve.</param>
        /// <returns>The setting value converted to the appropriate underlying .NET type.</returns>
        object GetSettingValue(string settingName);

        int CleanupPendingAttachmentsAfterHours { get; }
        bool HideHomePage { get; }
        bool EnableOutlookFriendlyHtmlEmail { get; }
        int EmailDeliveryTimerIntervalMinutes { get; }
        string SiteRootUrlForEmail { get; }
        string LuceneDirectory { get; }
        bool AllowSubmitterRoleToEditTags { get; }
        bool AllowSubmitterRoleToEditPriority { get; }
        bool CreateSqlMembershipRegistrationsAsSubmitters { get; }
        int RefreshSecurityCacheMinutes { get; }
        int AdUserPropertiesSqlCacheRefreshMinutes { get; }
        bool HelpDeskBroadcastNotificationsEnabled();
        bool EmailNotificationsEnabled();
        string EmailServiceName();
        double EmailNotificationInitialDelayMinutes();
        double EmailMaxConsolidationWaitMinutes();
        int EmailResendDelayMinutes();
        int EmailMaxDeliveryAttempts();
        string FromEmailDisplayName();
        string FromEmailAddress();
        string BlindCopyToEmailAddress();
         
    }
}
