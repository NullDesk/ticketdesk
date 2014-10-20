using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Domain.Models;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client.Models
{
    public class TicketCenterListViewModel
    {
        public static async Task<TicketCenterListViewModel> GetViewModelAsync(int currentPage, string listName, TicketDeskContext context, string userId)
        {
            var vm = new TicketCenterListViewModel(currentPage, listName, context, userId);
            vm.Tickets = await vm.ListTicketsAsync(currentPage, context);
            return vm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketCenterListViewModel" /> class.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="ticketsList">The tickets list.</param>
        /// <param name="context">The context.</param>
        /// <param name="userId">The user identifier.</param>
        private TicketCenterListViewModel(int currentPage, string listName, TicketDeskContext context, string userId)
        {
            UserListSettings = context.UserSettings.GetUserListSettings(userId).OrderBy(lp => lp.ListMenuDisplayOrder);
            CurrentPage = currentPage;
            if (string.IsNullOrEmpty(listName))
            {
                listName = UserListSettings.First().ListName;
            }
            CurrentListSetting = context.UserSettings.GetUserListSettingByName(listName, userId);

            FilterBar = new FilterBarViewModel(CurrentListSetting);

        }

        public int CurrentPage { get; set; }

        public TicketDeskUserManager UserManager
        {
            get { return DependencyResolver.Current.GetService<TicketDeskUserManager>(); }
        }

        public Task<IPagedList<Ticket>> ListTicketsAsync(int pageIndex, TicketDeskContext context)
        {
            var filterColumns = CurrentListSetting.FilterColumns.ToList();
            var sortColumns = CurrentListSetting.SortColumns.ToList();

            var pageSize = CurrentListSetting.ItemsPerPage;



            var query = context.GetObjectQueryFor(context.Tickets);


            query = filterColumns.ApplyToQuery(query);
            query = sortColumns.ApplyToQuery(query);


            return query.ToPagedListAsync(pageIndex, pageSize);
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