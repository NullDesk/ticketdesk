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

namespace TicketDesk.Web.Client.Models
{
    public static class ApplicationSettingsServiceBaseExtensions
    {
        /// <summary>
        /// Gets a SelectList for a collection.
        /// </summary>
        /// <param name="settingCollection">The setting collection.</param>
        /// <returns></returns>
        public static SelectList GetSelectListOfSettingCollection(this IApplicationSettingsService settingsService, string[] settingCollection)
        {
            return GetSelectListOfSettingCollection(settingsService, settingCollection, false, null, null, null);
        }

        /// <summary>
        /// Gets a select list for a collection.
        /// </summary>
        /// <param name="settingCollection">The setting collection.</param>
        /// <param name="selectedOptionValue">The selected option value.</param>
        /// <returns></returns>
        public static SelectList GetSelectListOfSettingCollection(this IApplicationSettingsService settingsService, string[] settingCollection, string selectedOptionValue)
        {
            return GetSelectListOfSettingCollection(settingsService, settingCollection, false, null, null, selectedOptionValue);
        }

        /// <summary>
        /// Gets a select list for a collection.
        /// </summary>
        /// <param name="settingCollection">The setting collection.</param>
        /// <param name="includeEmptyOption">if set to <c>true</c> [include empty option].</param>
        /// <param name="emptyOption">The empty option.</param>
        /// <returns></returns>
        public static SelectList GetSelectListOfSettingCollection(this IApplicationSettingsService settingsService, string[] settingCollection, bool includeEmptyOption, string emptyOption)
        {
            return GetSelectListOfSettingCollection(settingsService, settingCollection, includeEmptyOption, emptyOption, emptyOption, null);
        }

        /// <summary>
        /// Gets a select list for a collection.
        /// </summary>
        /// <param name="settingCollection">The setting collection.</param>
        /// <param name="includeEmptyOption">if set to <c>true</c> [include empty option].</param>
        /// <param name="emptyOption">The empty option.</param>
        /// <param name="selectedOption">The selected option.</param>
        /// <returns></returns>
        public static SelectList GetSelectListOfSettingCollection(this IApplicationSettingsService settingsService, string[] settingCollection, bool includeEmptyOption, string emptyOption, string selectedOption)
        {
            return GetSelectListOfSettingCollection(settingsService, settingCollection, includeEmptyOption, emptyOption, emptyOption, selectedOption);
        }

        /// <summary>
        /// Gets a select list for a collection.
        /// </summary>
        /// <param name="settingCollection">The setting collection.</param>
        /// <param name="includeEmptyOption">if set to <c>true</c> [include empty option].</param>
        /// <param name="emptyOptionText">The empty option text.</param>
        /// <param name="emptyOptionValue">The empty option value.</param>
        /// <param name="selectedOptionValue">The selected option value.</param>
        /// <returns></returns>
        public static SelectList GetSelectListOfSettingCollection(this IApplicationSettingsService settingsService, string[] settingCollection, bool includeEmptyOption, string emptyOptionText, string emptyOptionValue, string selectedOptionValue)
        {
            var selectItems = new List<SelectListItem>();
            if (includeEmptyOption)
            {
                var emptyItem = new SelectListItem { Text = emptyOptionText, Value = emptyOptionValue, Selected = (emptyOptionValue == selectedOptionValue) };
                selectItems.Add(emptyItem);
            }
            foreach (var pri in settingCollection)
            {
                var item = new SelectListItem();
                item.Text = pri;
                item.Value = pri;
                item.Selected = (pri == selectedOptionValue);
                selectItems.Add(item);
            }

            return new SelectList(selectItems, "Value", "Text");
        }

    }
}