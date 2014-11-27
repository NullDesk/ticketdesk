using System;
using System.Data.Entity;
using System.Linq;
using TicketDesk.Domain.Localization;

namespace TicketDesk.Domain.Model
{
    public static class SettingExtensions
    {
        public static T GetSettingValue<T>(this DbSet<Setting> settings, string settingName, T defaultValue) where T : struct
        {
            var setting = GetSettingValue(settings, settingName);
            if (setting == null)
            {
                return defaultValue;
            }
            else
            {
                return (T)setting;
            }
        }

        public static string[] GetSettingValue(this DbSet<Setting> settings, string settingName, string[] defaultValue)
        {
            return GetSettingValue(settings, settingName) as string[] ?? defaultValue;
        }

        public static string GetSettingValue(this DbSet<Setting> settings, string settingName, string defaultValue)
        {
            return GetSettingValue(settings, settingName) as string ?? defaultValue;
        }

        public static object GetSettingValue(DbSet<Setting> settings, string settingName)
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
                        catch
                        {
                            ThrowSettingTypeException(setting.SettingName, setting.SettingType);
                        }
                        break;
                    case "IntString":
                        try
                        {
                            settingValue = new int?(Convert.ToInt32(setting.SettingValue));
                        }
                        catch
                        {
                            ThrowSettingTypeException(setting.SettingName, setting.SettingType);
                        }
                        break;
                    case "DoubleString":
                        try
                        {
                            settingValue = Convert.ToDouble(setting.SettingValue);
                        }
                        catch
                        {
                            ThrowSettingTypeException(setting.SettingName, setting.SettingType);
                        }
                        break;
                }
            }

            return settingValue;
        }
       

        private static void ThrowSettingTypeException(string settingName, string settingType)
        {
            throw new SettingTypeException(string.Format(TicketDeskDomainText.ExceptionSettingsTypeConversionError, settingName, settingType));
        }
    }


    public class SettingTypeException : ApplicationException
    {
        public SettingTypeException(string message)
            : base(message) { }
    }
}
