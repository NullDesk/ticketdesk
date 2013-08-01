using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Model
{
    public static class SettingExtensions
    {
        /// <summary>
        /// Gets the setting value from the application settings store.
        /// </summary>
        /// <param name="settings">The settings dbset.</param>
        /// <param name="settingName">Name of the setting to retrieve.</param>
        /// <returns>The setting value converted to the appropriate underlying .NET type.</returns>
        /// <remarks>This method will attempt to convert the setting value to the appropriate .NET type, or throw an exception if the value cannot convert.</remarks>
        public static object GetSettingValue(this DbSet<Setting> settings, string settingName)
        {
            object settingValue = null;
            var setting = settings.SingleOrDefault(s => s.SettingName == settingName);
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


        public static IEnumerable<SimpleSetting> GetAvailablePriorities(this DbSet<Setting> settings)
        {
            return GetAvailablePriorities(settings, null);
        }

        public static IEnumerable<SimpleSetting> GetAvailablePriorities(this DbSet<Setting> settings, string language)
        {
            return GetLocalizedSimpleSettingList(settings, language, "PriorityList");
        }

        public static IEnumerable<SimpleSetting> GetAvailableTicketTypes(this DbSet<Setting> settings)
        {
            return GetAvailableTicketTypes(settings, null);
        }

        public static IEnumerable<SimpleSetting> GetAvailableTicketTypes(this DbSet<Setting> settings, string language)
        {
            return GetLocalizedSimpleSettingList(settings, language, "TicketTypesList");
        }

        public static IEnumerable<SimpleSetting> GetAvailableCategories(this DbSet<Setting> settings)
        {
            return GetAvailableCategories(settings, null);
        }

        public static IEnumerable<SimpleSetting> GetAvailableCategories(this DbSet<Setting> settings, string language)
        {
            return GetLocalizedSimpleSettingList(settings, language, "CategoryList");
        }

        public static IEnumerable<SimpleSetting> GetAvailableStatuses(this DbSet<Setting> settings)
        {
            //TODO: localize
            var statuses = new [] { "Active", "More Info", "Closed", "Resolved" };
            return statuses.Select(s => new SimpleSetting(s));
        }

        private static IEnumerable<SimpleSetting> GetLocalizedSimpleSettingList(DbSet<Setting> settings, string language, string settingBaseName)
        {
            IEnumerable<SimpleSetting> data = null;
            var settingName = settingBaseName;
            if (!string.IsNullOrEmpty(language) && language.Length > 1)
            {
                var langCode = language.Substring(0, 2);
                settingName = string.Format("{0}-{1}", settingBaseName, langCode);
                if (!settings.Any(s => s.SettingName.Equals(settingName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    settingName = settingBaseName;
                }
            }

            data = ((string[])GetSettingValue(settings, settingName)).Select(s => new SimpleSetting(s));
            return data;
        }

        private static void ThrowSettingTypeException(string settingName, string settingType)
        {
            throw new ApplicationException(string.Format("ApplicationSettings Conversion Error: unable to convert SettingName [{0}] for specified SettingType [{1}]", settingName, settingType));

        }

    }
}
