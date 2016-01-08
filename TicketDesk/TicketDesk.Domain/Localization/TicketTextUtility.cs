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

using System;
using TicketDesk.Domain.Model;
using TicketDesk.Localization.Domain;

namespace TicketDesk.Domain.Localization
{
    public static class TicketTextUtility
    {
        /// <summary>
        /// Gets the comment event text.
        /// </summary>
        /// <param name="ticketEvent">The ticket event of which to fetch the comment event.</param>
        /// <param name="newPriority">The new priority, leave null if priority change isn't applicable for the activity.</param>
        /// <param name="userName">Name of the user, leave null if a user name isn't applicable for the actiity</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NullReferenceException"></exception>
        public static string GetTicketEventDescription(TicketActivity ticketEvent, string newPriority, string userName)
        {
            //no real perf advantage to a stringbuilder here
            var n = Enum.GetName(typeof(TicketActivity), ticketEvent);
            var val = Strings.ResourceManager.GetString("TicketActivity" + n);
            var pval = Strings.ResourceManager.GetString("TicketActivityPriority");
            if (string.IsNullOrEmpty(val) || string.IsNullOrEmpty(pval))
            {
                throw new NullReferenceException();
            }
            if (!string.IsNullOrEmpty(userName))
            {
                val = string.Format(val, userName);
            }
            if (!string.IsNullOrEmpty(newPriority))
            {
                val += string.Format(pval, newPriority);
            }
            return val;
        }
    }
}
