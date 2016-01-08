// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.Linq;
using System.Web.Mvc;
using TicketDesk.Web.Client;
using TicketDesk.Web.Identity;

namespace TicketDesk.Domain.Model
{
    public static class ApplicationSettingExtensions
    {
        public static SelectList GetPriorityList(this ApplicationSetting settings, bool includeEmpty, string selectedPriority)
        {
            return settings.SelectLists.PriorityList.ToSelectList(p => p, p => p, selectedPriority, includeEmpty);
        }

        public static SelectList GetCategoryList(this ApplicationSetting settings, bool includeEmpty, string selectedCategory)
        {
            return settings.SelectLists.CategoryList.ToSelectList(p => p, p => p, selectedCategory, includeEmpty);
        }

        public static SelectList GetTicketTypeList(this ApplicationSetting settings, bool includeEmpty, string selectedType)
        {
            return settings.SelectLists.TicketTypesList.ToSelectList(p => p, p => p, selectedType, includeEmpty);
        }

        public static MultiSelectList GetDefaultNewUserRolesList(this ApplicationSetting settings)
        {
            var roleManager = DependencyResolver.Current.GetService<TicketDeskRoleManager>();
            return roleManager.Roles.ToMultiSelectList(
                r => r.Name, 
                r => r.DisplayName,
                settings.SecuritySettings.DefaultNewUserRoles.ToArray(),
                false);
        }

        public static string GetPriorities(this ApplicationSetting settings)
        {
            return string.Join(",", settings.SelectLists.PriorityList);
        }
        public static string GetTicketTypes(this ApplicationSetting settings)
        {
            return string.Join(",", settings.SelectLists.TicketTypesList);
        }
        public static string GetCategories(this ApplicationSetting settings)
        {
            return string.Join(",", settings.SelectLists.CategoryList);
        }


    }
}