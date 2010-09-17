using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketDesk.Domain.Models
{
    public class UserDisplayPreferences
    {
        private List<TicketCenterListSettings> _ticketCenterListPreferences;

        /// <summary>
        /// Gets or sets the ticket center list preferences.
        /// </summary>
        /// <value>The ticket center list preferences.</value>
        public List<TicketCenterListSettings> TicketCenterListPreferences
        {
            get
            {
                if (_ticketCenterListPreferences == null)
                {
                    _ticketCenterListPreferences = new List<TicketCenterListSettings>();
                }
                return _ticketCenterListPreferences;
            }
            set { _ticketCenterListPreferences = value; }
        }

        /// <summary>
        /// Gets the preferences for a specific ticket center list.
        /// </summary>
        /// <param name="listName">Name of the ticket center list.</param>
        /// <returns>The preferences for the specified list.</returns>
        public TicketCenterListSettings GetPreferencesForList(string listName)
        {
            TicketCenterListSettings setting = null;
            foreach (TicketCenterListSettings s in TicketCenterListPreferences)
            {
                if (s.ListName == listName)
                {
                    setting = s;
                    break;
                }
            }

            if (setting == null)
            {
                var m = this.TicketCenterListPreferences.Max(st => st.ListMenuDisplayOrder);
                setting = new TicketCenterListSettings(listName, listName, m + 1, false);
                _ticketCenterListPreferences.Add(setting);//adds this setting to the collection for future use
            }
            return setting;
        }
    }
}
