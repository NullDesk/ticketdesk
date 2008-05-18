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
        public static ListViewSettingsCollection CreateNewSettings()
        {
            return new ListViewSettingsCollection();
        }

        

        #endregion

        #region instance members

        private List<ListViewSettings> _settings;

        /// <summary>
        /// Gets or sets the collection of list view settings.
        /// </summary>
        /// <value>The settings.</value>
        public List<ListViewSettings> Settings
        {
            get 
            {
                if(_settings == null)
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
            foreach(ListViewSettings s in Settings)
            {
                if(s.ListViewName == listName)
                {
                    setting = s;
                    break;
                }
            }

            if(setting == null)
            {
                setting = new ListViewSettings(listName, false);
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
