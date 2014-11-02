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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Services
{
    [Export(typeof(IUserSettingsService))]
    public class UserSettingsService : IUserSettingsService
    {

        private ISecurityService Security { get; set; }

        /// <summary>
        /// Unit test ctor, Initializes a new instance of the <see cref="TdUserSettingService"/> class.
        /// </summary>
        /// <param name="isStaffUser">if set to <c>true</c> [is staff user].</param>
        /// <param name="currentUserName">Name of the current user.</param>
        /// <param name="profileRepository">The profile repository.</param>
        [ImportingConstructor]
        public UserSettingsService(ISecurityService securityService, IProfile profileRepository)
        {
            Security = securityService;



            Repository = profileRepository;
        }

        #region IUserSettingService Members

        /// <summary>
        /// Gets or sets a value indicating whether to open editor with preview visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if open editor with preview; otherwise, <c>false</c>.
        /// </value>
        public bool OpenEditorWithPreview
        {
            get
            {
                return Repository.OpenEditorWithPreview;
            }
            set
            {
                Repository.OpenEditorWithPreview = value;
            }
        }

        /// <summary>
        /// Gets or sets the user's preferred editor mode.
        /// </summary>
        /// <value>The editor mode.</value>
        public EditorModes EditorMode
        {
            get { return Repository.EditorMode; }
            set { Repository.EditorMode = value; }
        }



        /// <summary>
        /// Gets or (private) sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public IProfile Repository { get; private set; }

        /// <summary>
        /// Gets the display preferences for user.
        /// </summary>
        /// <returns></returns>
        public UserDisplayPreferences GetDisplayPreferences()
        {
            var dp = Repository.DisplayPreferences;
            VerifyDefaultTicketCenterLists(dp);
            return dp;
        }

        /// <summary>
        /// Saves the display preferences.
        /// </summary>
        /// <param name="prefrences">The preferences.</param>
        public void SaveDisplayPreferences(UserDisplayPreferences prefrences)
        {
            Repository.DisplayPreferences = prefrences;//ProfileCommon implicitly saves on set accessor
        }
        #endregion

        /// <summary>
        /// Verifies the minimum set of default preferences for the default lists.
        /// </summary>
        /// <param name="preferences">The preferences to check for defaults.</param>
        public void VerifyDefaultTicketCenterLists(UserDisplayPreferences preferences)
        {
            if (preferences == null)
            {
                preferences = new UserDisplayPreferences();
            }
            int numStaffLists = preferences.TicketCenterListPreferences.Count(s => s.ListName == "unassigned" || s.ListName == "assignedtome");
            int numSubmitterLists = preferences.TicketCenterListPreferences.Count(s => s.ListName == "mytickets" || s.ListName == "opentickets" || s.ListName == "historytickets");

            if ((((Security.IsTdStaff()) && (numStaffLists < 2)) || ((!Security.IsTdStaff()) && (numStaffLists > 0)) || (numSubmitterLists < 3)))
            {
                preferences.TicketCenterListPreferences.Clear();
                preferences.TicketCenterListPreferences = CreateDefaultTicketCenterListPreferences();
                SaveDisplayPreferences(preferences);//Saves data to profile
            }
        }

        /// <summary>
        /// Creates the default ticket center list preferences.
        /// </summary>
        /// <returns></returns>
        private List<TicketCenterListSettings> CreateDefaultTicketCenterListPreferences()
        {

            List<TicketCenterListSettings> preferences = new List<TicketCenterListSettings>();

            List<string> disableStatusColumn = new List<string>();
            disableStatusColumn.Add("CurrentStatus");

            List<string> disableAssignedColumn = new List<string>();
            disableAssignedColumn.Add("AssignedTo");

            List<string> disableOwnerColumn = new List<string>();
            disableOwnerColumn.Add("Owner");

            if (Security.IsTdStaff())
            {

                //unassigned
                List<TicketListSortColumn> unassignedSortColumns = new List<TicketListSortColumn>();
                List<TicketListFilterColumn> unassignedFilterColumns = new List<TicketListFilterColumn>();
                unassignedSortColumns.Add(new TicketListSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
                unassignedFilterColumns.Add(new TicketListFilterColumn("CurrentStatus", false, "closed"));
                unassignedFilterColumns.Add(new TicketListFilterColumn("AssignedTo", null, null));
                TicketCenterListSettings unassigned = new TicketCenterListSettings("unassigned", "Unassigned Tickets", 0, 20, unassignedSortColumns, unassignedFilterColumns, disableAssignedColumn);
                preferences.Add(unassigned);

                //assigned to me
                List<TicketListSortColumn> assignedtomeSortColumns = new List<TicketListSortColumn>();
                List<TicketListFilterColumn> assignedtomeFilterColumns = new List<TicketListFilterColumn>();
                assignedtomeSortColumns.Add(new TicketListSortColumn("CurrentStatus", ColumnSortDirection.Ascending));
                assignedtomeSortColumns.Add(new TicketListSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
                assignedtomeFilterColumns.Add(new TicketListFilterColumn("CurrentStatus", false, "closed"));
                assignedtomeFilterColumns.Add(new TicketListFilterColumn("AssignedTo", true, Security.CurrentUserName));
                TicketCenterListSettings assignedtome = new TicketCenterListSettings("assignedtome", "Tickets Assigned To Me", 1, 20, assignedtomeSortColumns, assignedtomeFilterColumns, disableAssignedColumn);
                preferences.Add(assignedtome);
            }

            int disOrder = (Security.IsTdStaff()) ? 2 : 0; //if staff display order starts at 2 for the remaining built-in lists, otherwise starts at 0
            //my tickets
            List<TicketListSortColumn> myticketsSortColumns = new List<TicketListSortColumn>();
            List<TicketListFilterColumn> myticketsFilterColumns = new List<TicketListFilterColumn>();
            myticketsSortColumns.Add(new TicketListSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
            myticketsFilterColumns.Add(new TicketListFilterColumn("CurrentStatus", false, "closed"));
            myticketsFilterColumns.Add(new TicketListFilterColumn("Owner", true, Security.CurrentUserName));
            TicketCenterListSettings mytickets = new TicketCenterListSettings("mytickets", "All My Tickets", disOrder, 20, myticketsSortColumns, myticketsFilterColumns, disableOwnerColumn);
            preferences.Add(mytickets);

            //open tickets
            List<TicketListSortColumn> openSortColumns = new List<TicketListSortColumn>();
            List<TicketListFilterColumn> openFilterColumns = new List<TicketListFilterColumn>();
            openSortColumns.Add(new TicketListSortColumn("CurrentStatus", ColumnSortDirection.Ascending));
            openSortColumns.Add(new TicketListSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
            openFilterColumns.Add(new TicketListFilterColumn("CurrentStatus", false, "closed"));
            TicketCenterListSettings opentickets = new TicketCenterListSettings("opentickets", "All Open Tickets", disOrder + 1, 20, openSortColumns, openFilterColumns, disableStatusColumn);
            preferences.Add(opentickets);

            //history
            List<TicketListSortColumn> historyticketsSortColumns = new List<TicketListSortColumn>();
            List<TicketListFilterColumn> historyticketsFilterColumns = new List<TicketListFilterColumn>();
            historyticketsSortColumns.Add(new TicketListSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
            historyticketsFilterColumns.Add(new TicketListFilterColumn("CurrentStatus", true, "closed"));

            if (Security.IsTdStaff())
            {
                historyticketsFilterColumns.Add(new TicketListFilterColumn("AssignedTo", true, Security.CurrentUserName));

            }
            else
            {
                historyticketsFilterColumns.Add(new TicketListFilterColumn("Owner", true, Security.CurrentUserName));

            }
            TicketCenterListSettings historytickets = new TicketCenterListSettings("historytickets", "Ticket History", disOrder + 2, 20, historyticketsSortColumns, historyticketsFilterColumns, disableStatusColumn);
            preferences.Add(historytickets);

            return preferences;
        }

    }
}
