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

using System.Linq;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Model
{
    public static class UserTicketListSettingExtensions
    {
        /// <summary>
        /// Modifies the filter settings.
        /// </summary>
        /// <param name="setting">The list settings which will be modified.</param>
        /// <param name="pageSize">Number of items to display on a single page.</param>
        /// <param name="currentStatus">The current status filter setting.</param>
        /// <param name="owner">The owner filter setting.</param>
        /// <param name="assignedTo">The assigned to filter setting.</param>
        public static void ModifyFilterSettings(this UserTicketListSetting setting, int pageSize, string currentStatus, string owner, string assignedTo)
        {
            setting.ItemsPerPage = pageSize;

            if (!setting.DisabledFilterColumnNames.Contains("CurrentStatus"))
            {
               setting.FilterColumns.ChangeCurrentStatusFilter(currentStatus);
            }
            if (!setting.DisabledFilterColumnNames.Contains("Owner"))
            {
                setting.FilterColumns.ChangeOwnerFilter(owner);
            }
            if (!setting.DisabledFilterColumnNames.Contains("AssignedTo"))
            {
                setting.FilterColumns.ChangeAssignedFilter(assignedTo);
            }
        }

        
    }
}
