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
using System.Web.Mvc;
using TicketDesk.Domain.Services;
using System.ComponentModel.Composition;
using System.Collections.Specialized;
using TicketDesk.Domain.Models;

namespace TicketDesk.Web.Client.Helpers
{
    [Export]
    public class SelectListUtility
    {

        private SettingsService Settings { get; set; }
        private ISecurityService Security { get; set; }

        [ImportingConstructor]
        public SelectListUtility(SettingsService settings, ISecurityService security)
        {
            Settings = settings;
            Security = security;
        }

        /// <summary>
        /// Gets the priority list.
        /// </summary>
        /// <param name="includeEmpty">if set to <c>true</c> to include an empty "-- select --" option.</param>
        /// <param name="selectedPriority">The selected priority for the list, use empty if the "-- select --" option should be selected.</param>
        /// <returns></returns>
        public SelectList GetPriorityList(bool includeEmpty, string selectedPriority)
        {
            return GetSelectList(Settings.ApplicationSettings.AvailablePriorities, includeEmpty, string.Empty, selectedPriority);
        }

        public SelectList GetCategoryList(bool includeEmpty, string selectedCategory)
        {
            return GetSelectList(Settings.ApplicationSettings.AvailableCategories, includeEmpty, "-- select --", selectedCategory);
        }

        public SelectList GetTicketTypeList(bool includeEmpty, string selectedType)
        {
            return GetSelectList(Settings.ApplicationSettings.AvailableTicketTypes, includeEmpty, "-- select --", selectedType);
        }

        public SelectList GetSubmittersList(bool includeEmpty, string selectedUser, params string[] omitUsers)
        {
            NameValueCollection col = new NameValueCollection();

            List<UserInfo> lusers = new List<UserInfo>(Security.GetTdSubmitterUsers().OrderBy(u => u.DisplayName));

            foreach (var luser in lusers)
            {
                if (!omitUsers.Contains(luser.Name))
                {
                    col.Add(luser.DisplayName, luser.Name);
                }
            }
            return GetSelectList(col, includeEmpty, string.Empty, selectedUser);
        }

        public SelectList GetStaffList(bool includeEmpty, string selectedUser, params string[] omitUsers)
        {
            NameValueCollection col = new NameValueCollection();
            List<UserInfo> lusers = new List<UserInfo>(Security.GetTdStaffUsers().OrderBy(u => u.DisplayName));
            foreach (var luser in lusers)
            {
                if (!omitUsers.Contains(luser.Name))
                {
                    col.Add(luser.DisplayName, luser.Name);
                }
            }
            return GetSelectList(col, includeEmpty, string.Empty, selectedUser);

        }

        public SelectList GetSelectList(string[] itemChoices, bool includeEmpty, string emptyItemText, string selectedValue)
        {
            NameValueCollection col = new NameValueCollection();
            foreach (var item in itemChoices)
            {
                col.Add(item, item);
            }
            return GetSelectList(col, includeEmpty, emptyItemText, selectedValue);
        }

        public SelectList GetSelectList(NameValueCollection itemChoices, bool includeEmpty, string emptyItemText, string selectedValue)
        {
            var selectItems = new List<SelectListItem>();
            if (includeEmpty)
            {
                var emptyItem = new SelectListItem { Text = emptyItemText, Value = string.Empty, Selected = (selectedValue == string.Empty) };
                selectItems.Add(emptyItem);
            }
            foreach (var pri in itemChoices.AllKeys)
            {
                var item = new SelectListItem();
                item.Text = pri;
                item.Value = itemChoices[pri];
                selectItems.Add(item);
            }

            return new SelectList(selectItems, "Value", "Text", selectedValue);
        }

    }
}