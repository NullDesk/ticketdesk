// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace TicketDesk.Engine.ListView
{

    /// <summary>
    /// A container class for a user's list view settings
    /// </summary>
    public class ListViewSettingsCollection
    {
        #region static members

        /// <summary>
        /// Gets the settings for the current user.
        /// </summary>
        /// <returns></returns>
        public static ListViewSettingsCollection GetSettingsForUser()
        {
            ProfileCommon pc = new ProfileCommon();
            return pc.TicketListSettings;
            
            
        }

       

        /// <summary>
        /// Creates a new [empty] settings instance.
        /// </summary>
        /// <returns></returns>
        public static ListViewSettingsCollection CreateNewSettings(ListViewSettingsCollection settingsCollection)
        {
            string user = HttpContext.Current.User.Identity.GetFormattedUserName();
           

            List<string> disableStatusColumn = new List<string>();
            disableStatusColumn.Add("CurrentStatus");

            List<string> disableAssignedColumn = new List<string>();
            disableAssignedColumn.Add("AssignedTo");

            List<string> disableOwnerColumn = new List<string>();
            disableOwnerColumn.Add("Owner");


            if (SecurityManager.IsStaff)
            {
                //unassigned
                List<ListViewSortColumn> unassignedSortColumns = new List<ListViewSortColumn>();
                List<ListViewFilterColumn> unassignedFilterColumns = new List<ListViewFilterColumn>();
                unassignedSortColumns.Add(new ListViewSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
                unassignedFilterColumns.Add(new ListViewFilterColumn("CurrentStatus", false, "closed"));
                unassignedFilterColumns.Add(new ListViewFilterColumn("AssignedTo", null, null));
                ListViewSettings unassigned = new ListViewSettings("unassigned", "Unassigned Tickets", 0, 20, unassignedSortColumns, unassignedFilterColumns, disableAssignedColumn);
                settingsCollection.Settings.Add(unassigned);

                //assigned to me
                List<ListViewSortColumn> assignedtomeSortColumns = new List<ListViewSortColumn>();
                List<ListViewFilterColumn> assignedtomeFilterColumns = new List<ListViewFilterColumn>();
                assignedtomeSortColumns.Add(new ListViewSortColumn("CurrentStatus", ColumnSortDirection.Ascending));
                assignedtomeSortColumns.Add(new ListViewSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
                assignedtomeFilterColumns.Add(new ListViewFilterColumn("CurrentStatus", false, "closed"));
                assignedtomeFilterColumns.Add(new ListViewFilterColumn("AssignedTo", true, user));
                ListViewSettings assignedtome = new ListViewSettings("assignedtome", "Tickets Assigned To Me", 1, 20, assignedtomeSortColumns, assignedtomeFilterColumns, disableAssignedColumn);
                settingsCollection.Settings.Add(assignedtome);
            }

            int disOrder = (SecurityManager.IsStaff) ? 2 : 0; //if staff display order starts at 2 for the remaining built-in lists, otherwise starts at 0
            //my tickets
            List<ListViewSortColumn> myticketsSortColumns = new List<ListViewSortColumn>();
            List<ListViewFilterColumn> myticketsFilterColumns = new List<ListViewFilterColumn>();
            myticketsSortColumns.Add(new ListViewSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
            myticketsFilterColumns.Add(new ListViewFilterColumn("CurrentStatus", false, "closed"));
            myticketsFilterColumns.Add(new ListViewFilterColumn("Owner", true, user));
            ListViewSettings mytickets = new ListViewSettings("mytickets", "All My Tickets", disOrder, 20, myticketsSortColumns, myticketsFilterColumns, disableOwnerColumn);
            settingsCollection.Settings.Add(mytickets);


            //open tickets
            List<ListViewSortColumn> openSortColumns = new List<ListViewSortColumn>();
            List<ListViewFilterColumn> openFilterColumns = new List<ListViewFilterColumn>();
            openSortColumns.Add(new ListViewSortColumn("CurrentStatus", ColumnSortDirection.Ascending));
            openSortColumns.Add(new ListViewSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
            openFilterColumns.Add(new ListViewFilterColumn("CurrentStatus", false, "closed"));
            ListViewSettings opentickets = new ListViewSettings("opentickets", "All Open Tickets", disOrder + 1, 20, openSortColumns, openFilterColumns, disableStatusColumn);
            settingsCollection.Settings.Add(opentickets);


            //history
            List<ListViewSortColumn> historyticketsSortColumns = new List<ListViewSortColumn>();
            List<ListViewFilterColumn> historyticketsFilterColumns = new List<ListViewFilterColumn>();
            historyticketsSortColumns.Add(new ListViewSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
            historyticketsFilterColumns.Add(new ListViewFilterColumn("CurrentStatus", true, "closed"));
            historyticketsFilterColumns.Add(new ListViewFilterColumn("AssignedTo", true, user));
            if (SecurityManager.IsStaff)
            {
                historyticketsFilterColumns.Add(new ListViewFilterColumn("Owner", true, user));
            }
            ListViewSettings historytickets = new ListViewSettings("historytickets", "Ticket History", disOrder + 2, 20, historyticketsSortColumns, historyticketsFilterColumns, disableStatusColumn);
            settingsCollection.Settings.Add(historytickets);
            
            return settingsCollection;
        }



        #endregion

        #region instance members

       
        public ListViewSettingsCollection()
        {
            
        }

        /// <summary>
        /// Verifies that the settings contain the correct default lists, regenerates the lists if not.
        /// </summary>
        public void VerifyDefaultLists()
        {
            int numStaffLists = Settings.Count(s => s.ListViewName == "unassigned" || s.ListViewName == "assignedtome");
            int numSubmitterLists = Settings.Count(s => s.ListViewName == "mytickets" || s.ListViewName == "opentickets" || s.ListViewName == "historytickets");

            if ((((SecurityManager.IsStaff) && (numStaffLists < 2)) || ((!SecurityManager.IsStaff) && (numStaffLists > 0)) || (numSubmitterLists < 3)))
            {
                Settings.Clear();
                ListViewSettingsCollection.CreateNewSettings(this);
                Save();
            }
        }

        private List<ListViewSettings> _settings;

        /// <summary>
        /// Gets or sets the collection of list view settings.
        /// </summary>
        /// <value>The settings.</value>
        public List<ListViewSettings> Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new List<ListViewSettings>();
                }
                return _settings;
            }
            set { _settings = value; }
        }

        /// <summary>
        /// Gets the settings for a particular list view.
        /// </summary>
        /// <param name="listName">Name of the list view whose settings you wish to retrieve.</param>
        /// <returns></returns>
        public ListViewSettings GetSettingsForList(string listName)
        {
            ListViewSettings setting = null;
            foreach (ListViewSettings s in Settings)
            {
                if (s.ListViewName == listName)
                {
                    setting = s;
                    break;
                }
            }

            if (setting == null)
            {
                var m = this.Settings.Max(st => st.ListViewDisplayOrder);
                setting = new ListViewSettings(listName, listName, m + 1, false);
                _settings.Add(setting);//adds this setting to the collection for future use
            }
            return setting;
        }

        /// <summary>
        /// Saves the user's list settings.
        /// </summary>
        public void Save()
        {
            ProfileCommon pc = new ProfileCommon();
            pc.TicketListSettings = this;
        }

        #endregion


    }
}
