using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Domain.Models;

namespace TicketDesk.Web.Client.Models
{
    public class TicketCenterListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketCenterListViewModel" /> class.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="ticketsList">The tickets list.</param>
        /// <param name="context">The context.</param>
        /// <param name="userId">The user identifier.</param>
        public TicketCenterListViewModel(string listName, IPagedList<Ticket> ticketsList, TicketDeskContext context, string userId)
        {
            UserListSettings = context.UserSettings.GetAllUserListSettings(userId).OrderBy(lp => lp.ListMenuDisplayOrder);
            Tickets = ticketsList;
            if (string.IsNullOrEmpty(listName))
            {
                listName = UserListSettings.First().ListName;
            }
            CurrentListSetting = context.UserSettings.GetUserListSettingByName(listName, userId);

            FilterBar = new FilterBarViewModel(CurrentListSetting);

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
        public IPagedList<Ticket> Tickets { get; private set; }

        public UserTicketListSetting CurrentListSetting { get; private set; }

        public IOrderedEnumerable<UserTicketListSetting> UserListSettings { get; private set; }

    }
}