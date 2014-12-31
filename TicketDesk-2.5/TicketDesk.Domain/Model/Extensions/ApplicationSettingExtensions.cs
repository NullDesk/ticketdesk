using System.Data.Entity;
using System.Linq;

namespace TicketDesk.Domain.Model
{
    public static class ApplicationSettingExtensions
    {
        public static ApplicationSetting GetTicketDeskSettings(this DbSet<ApplicationSetting> settings)
        {
            return settings.FirstOrDefault(s => s.ApplicationName == "TicketDesk");
        }
    }
}
