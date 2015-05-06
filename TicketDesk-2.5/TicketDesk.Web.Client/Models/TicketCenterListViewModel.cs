// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Models
{
    public class TicketCenterListViewModel
    {
        public static async Task<TicketCenterListViewModel> GetViewModelAsync(int currentPage, string listName, TdDomainContext context, string userId)
        {
            var vm = new TicketCenterListViewModel(currentPage, listName, context, userId);
            vm.Tickets = await vm.ListTicketsAsync(currentPage, context);
            return vm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketCenterListViewModel" /> class.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="context">The context.</param>
        /// <param name="userId">The user identifier.</param>
        private TicketCenterListViewModel(int currentPage, string listName, TdDomainContext context, string userId)
        {
            // ReSharper disable once ImplicitlyCapturedClosure
            UserListSettings = AsyncHelper
                                .RunSync(() => context.UserSettingsManager.GetUserListSettings(userId))
                                .OrderBy(lp => lp.ListMenuDisplayOrder);
            CurrentPage = currentPage;
            if (string.IsNullOrEmpty(listName))
            {
                listName = UserListSettings.First().ListName;
            }
            CurrentListSetting = AsyncHelper.RunSync(() => context.UserSettingsManager.GetUserListSettingByName(listName, userId));

            FilterBar = new FilterBarViewModel(CurrentListSetting);

        }

        public int CurrentPage { get; set; }

        public TicketDeskUserManager UserManager
        {
            get { return DependencyResolver.Current.GetService<TicketDeskUserManager>(); }
        }

        public Task<IPagedList<Ticket>> ListTicketsAsync(int pageIndex, TdDomainContext context)
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