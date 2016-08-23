using System.Linq;

namespace TicketDesk.Domain.Model
{
    public static class ClientSettingExtensions
    {
        public static string GetDefaultTextEditorType(this ClientSetting clientSettings)
        {
            return GetClientSetting(clientSettings, "DefaultTextEditorType", "summernote");
        }

        public static string GetDefaultSiteRootUrl(this ClientSetting clientSettings)
        {
            return GetClientSetting(clientSettings, "DefaultSiteRootUrl", string.Empty);
        }

        public static string GetClientSetting(this ClientSetting clientSettings, string settingName, string defaultValue)
        {
            return clientSettings.Settings
                           .Where(s => s.Key == settingName)
                           .Select(s => s.Value)
                           .FirstOrDefault() ?? defaultValue;
        }

    }
}