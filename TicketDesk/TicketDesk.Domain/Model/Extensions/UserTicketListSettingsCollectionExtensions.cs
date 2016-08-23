using System.Linq;

namespace TicketDesk.Domain.Model
{
    public static class UserTicketListSettingsCollectionExtensions
    {

        internal static bool HasRequiredDefaultListSettings(this UserTicketListSettingsCollection listSettings, bool isHelpDeskOrAdmin)
        {
            var hasLists = true;
            if (isHelpDeskOrAdmin)
            {
                hasLists = 
                    listSettings.Any(s => s.ListName == "unassigned") && 
                    listSettings.Any(s => s.ListName == "assignedToMe");
            }
            return
                hasLists &&
                listSettings.Any(s => s.ListName == "mytickets") &&
                listSettings.Any(s => s.ListName == "opentickets") &&
                listSettings.Any(s => s.ListName == "historytickets");
        }
    }
}
