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
using TicketDesk.Web.Identity;

namespace TicketDesk.Web.Client.Models
{
    public class TicketCenterListViewModel
    {
        private FilterBarViewModel _filterBar;

        public static async Task<TicketCenterListViewModel> GetViewModelAsync(int currentPage, string listName, TdDomainContext context, string userId)
        {
            var vm = new TicketCenterListViewModel()
            {
                UserListSettings =
                    (await context.UserSettingsManager.GetUserListSettings(userId)).OrderBy(
                        lp => lp.ListMenuDisplayOrder),
                CurrentPage = currentPage,
                CurrentListSetting = await context.UserSettingsManager.GetUserListSettingByName(listName, userId)
            };

            vm.Tickets = await vm.ListTicketsAsync(currentPage, context);

            return vm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketCenterListViewModel" /> class.
        /// </summary>
        private TicketCenterListViewModel() { }//private ctor

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
        public FilterBarViewModel FilterBar
        {
            get { return _filterBar ?? (_filterBar = new FilterBarViewModel(CurrentListSetting)); }
        }


        /// <summary>
        /// Gets or (private) sets the list of tickets for the view.
        /// </summary>
        /// <value>The tickets.</value>
        public IPagedList<Ticket> Tickets { get; private set; }

        public UserTicketListSetting CurrentListSetting { get; private set; }

        public IOrderedEnumerable<UserTicketListSetting> UserListSettings { get; private set; }

    }
}