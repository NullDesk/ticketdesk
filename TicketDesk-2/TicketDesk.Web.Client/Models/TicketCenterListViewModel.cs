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
using System.Web;
using TicketDesk.Domain.Utilities.Pagination;
using TicketDesk.Domain.Models;
using TicketDesk.Domain.Services;

namespace TicketDesk.Web.Client.Models
{
    public class TicketCenterListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketCenterListViewModel"/> class.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="ticketsList">The tickets list.</param>
        /// <param name="serviceModel">The service model.</param>
        public TicketCenterListViewModel(string listName, IPagination<Ticket> ticketsList, SettingsService settings, ISecurityService security)
        {
            Tickets = ticketsList;
            CurrentListSettings = settings.UserSettings.GetDisplayPreferences().GetPreferencesForList(listName);
            FilterBar = new FilterBarViewModel(CurrentListSettings, security.GetTdStaffUsers(), security.GetTdSubmitterUsers());
            ListsForMenu = settings.UserSettings.GetDisplayPreferences().TicketCenterListPreferences.OrderBy(lp => lp.ListMenuDisplayOrder).ToArray();

        }

        /// <summary>
        /// Gets or (private) sets the filter bar model.
        /// </summary>
        /// <value>The filter bar.</value>
        public FilterBarViewModel FilterBar { get; private set; }

        /// <summary>
        /// Gets or (private) sets the list of tickets for the view.
        /// </summary>
        /// <value>The tickets.</value>
        public IPagination<Ticket> Tickets { get; private set; }

        public TicketCenterListSettings CurrentListSettings { get; private set; }

        public TicketCenterListSettings[] ListsForMenu { get; private set; }

    }
}