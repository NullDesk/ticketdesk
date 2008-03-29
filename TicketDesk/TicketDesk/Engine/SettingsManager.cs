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
// provided to the recipient.using System.Linq;
using System;
using System.Linq;
using TicketDesk.Engine;
using TicketDesk.Engine.Linq;
using System.Collections.Generic;
using System.Web;

namespace TicketDesk.Engine
{
    public enum SettingChangeResult
    {
        Success,
        Failure,
        Merge
    }
    public static class SettingsManager
    {
        private static TicketDataDataContext ctx = new TicketDataDataContext();

        #region Enum Properties

        private static string[] priorities;
        public static string[] PrioritiesList
        {
            get
            {
                if(priorities == null)
                {
                    priorities = GetStringEnumFromDb("PriorityList");
                    if(priorities == null)
                    {
                        priorities = CreateDefaultPriorities();
                        SaveStringEnumToDb("PriorityList", priorities);
                    }
                }
                return priorities;
            }
            set
            {
                priorities = value;
                SaveStringEnumToDb("PriorityList", priorities);
            }
        }

        private static string[] categories;
        public static string[] CategoriesList
        {
            get
            {
                if(categories == null)
                {
                    categories = GetStringEnumFromDb("CategoryList");
                    if(categories == null)
                    {
                        categories = CreateDefaultCategories();
                        SaveStringEnumToDb("CategoryList", categories);
                    }
                }
                return categories;
            }
            set
            {
                categories = value;
                SaveStringEnumToDb("CategoryList", categories);
            }
        }

        private static string[] ticketTypes;
        public static string[] TicketTypesList
        {
            get
            {
                if(ticketTypes == null)
                {
                    ticketTypes = GetStringEnumFromDb("TicketTypesList");
                    if(ticketTypes == null)
                    {
                        ticketTypes = CreateDefaultTicketTypes();
                        SaveStringEnumToDb("TicketTypesList", ticketTypes);
                    }
                }
                return ticketTypes;
            }
            set
            {
                ticketTypes = value;
                SaveStringEnumToDb("TicketTypesList", ticketTypes);
            }
        }

        #endregion

        #region Enums Defaults

        private static string[] CreateDefaultPriorities()
        {
            return new string[] { "Low", "Medium", "High" };
        }

        private static string[] CreateDefaultTicketTypes()
        {
            return new string[] { "Question", "Problem", "Request" };
        }

        private static string[] CreateDefaultCategories()
        {
            return new string[] { "Hardware", "Software", "Network/Services", "Non-Technical" };
        }

        #endregion

        #region Enum Rename Methods

        internal static SettingChangeResult RenamePriority(string oldPriorityName, string newPriorityName)
        {
            SettingChangeResult results = SettingChangeResult.Failure;
            string[] newSettings = RenameStringEmunSetting("PriorityList", oldPriorityName, newPriorityName, PrioritiesList, false, out results);
            SaveRenamedPriority(newSettings, oldPriorityName, newPriorityName, false);
            ctx.SubmitChanges();
            priorities = newSettings;
            return results;
        }

        internal static SettingChangeResult RenameCategory(string oldCategoryName, string newCategoryName)
        {
            SettingChangeResult results = SettingChangeResult.Failure;
            string[] newSettings = RenameStringEmunSetting("CategoryList", oldCategoryName, newCategoryName, CategoriesList, false, out results);
            SaveRenamedCategory(newSettings, oldCategoryName, newCategoryName, false);
            ctx.SubmitChanges();
            categories = newSettings;
            return results;
        }


        internal static SettingChangeResult RenameTicketType(string oldTypeName, string newTypeName)
        {
            SettingChangeResult results = SettingChangeResult.Failure;
            string[] newSettings = RenameStringEmunSetting("TicketTypesList", oldTypeName, newTypeName, TicketTypesList, false, out results);
            SaveRenamedTicketTypes(newSettings, oldTypeName, newTypeName, false);
            ctx.SubmitChanges();
            ticketTypes = newSettings;
            return results;
        }

        private static void SaveRenamedPriority(string[] newSettings, string oldPriorityName, string newPriorityName, bool commitChanges)
        {
           
            string user = HttpContext.Current.User.Identity.GetUserDisplayName();
            string evt = string.Format("renamed the ticket priority from {0} to {1} globally.", oldPriorityName, newPriorityName);
            var tickets = ctx.Tickets.Where(t => t.Priority == oldPriorityName);
            foreach(Ticket ticket in tickets)
            {
                ticket.Priority = newPriorityName;
                TicketComment comment = new TicketComment();
                comment.IsHtml = false;
                comment.CommentedBy = user;
                comment.CommentEvent = evt;
                ticket.TicketComments.Add(comment);
            }
            if(commitChanges)
            {
                ctx.SubmitChanges();
            }
        }

        private static void SaveRenamedCategory(string[] newSettings, string oldCategoryName, string newCategoryName, bool commitChanges)
        {
            

            string user = HttpContext.Current.User.Identity.GetUserDisplayName();
            string evt = string.Format("renamed the ticket category from {0} to {1} globally.", oldCategoryName, newCategoryName);
            var tickets = ctx.Tickets.Where(t => t.Category == oldCategoryName);
            foreach(Ticket ticket in tickets)
            {
                ticket.Category = newCategoryName;
                TicketComment comment = new TicketComment();
                comment.IsHtml = false;
                comment.CommentedBy = user;
                comment.CommentEvent = evt;
                ticket.TicketComments.Add(comment);
            }
            if(commitChanges)
            {
                ctx.SubmitChanges();
            }
        }

        private static void SaveRenamedTicketTypes(string[] newSettings, string oldTypeName, string newTypeName, bool commitChanges)
        {
           
            string user = HttpContext.Current.User.Identity.GetUserDisplayName();
            string evt = string.Format("renamed the ticket type from {0} to {1} globally.", oldTypeName, newTypeName);
            var tickets = ctx.Tickets.Where(t => t.Type == oldTypeName);
            foreach(Ticket ticket in tickets)
            {
                ticket.Type = newTypeName;
                TicketComment comment = new TicketComment();
                comment.IsHtml = false;
                comment.CommentedBy = user;
                comment.CommentEvent = evt;
                ticket.TicketComments.Add(comment);
            }
            if(commitChanges)
            {
                ctx.SubmitChanges();
            }
        }
        #endregion

        #region Enum shared methods

        private static string[] GetStringEnumFromDb(string settingName)
        {
            string[] values = null;
            string p = (from settings in ctx.Settings
                        where settings.SettingName == settingName
                        select settings.SettingValue).SingleOrDefault();
            if(!string.IsNullOrEmpty(p))
            {
                values = p.Split(',');
            }
            return values;
        }

        private static void SaveStringEnumToDb(string settingName, string[] items)
        {
            SaveStringEnumToDb(settingName, items, true);
        }

        private static void SaveStringEnumToDb(string settingName, string[] items, bool commitChanges)
        {
            Setting setting = ctx.Settings.SingleOrDefault(s => s.SettingName == settingName);
            if(setting == null)
            {
                setting = new Setting();
                setting.SettingName = settingName;
                ctx.Settings.InsertOnSubmit(setting);
            }
            setting.SettingValue = string.Join(",", items);

            if(commitChanges)
            {
                ctx.SubmitChanges();
            }
        }

        internal static string[] RenameStringEmunSetting(string settingName, string oldSetting, string newSetting, string[] origionalSettings, bool commitChanges, out SettingChangeResult results)
        {
            string[] newSettings = null;
            results = SettingChangeResult.Failure;
            if(oldSetting != newSetting)
            {
                int newLen = origionalSettings.Length;
                bool deleteOldSetting = false;
                //if new setting is same as an existing setting in list, we have to remove the old setting rather than rename it
                if(origionalSettings.Contains(newSetting))
                {
                    deleteOldSetting = true;
                    newLen--;
                }

                List<string> newSettingssList = new List<string>(newLen);
                for(int i = 0; i < origionalSettings.Length; i++)
                {
                    if(origionalSettings[i] == oldSetting)
                    {
                        if(!deleteOldSetting)
                        {
                            newSettingssList.Add(newSetting);
                        }
                    }
                    else
                    {
                        newSettingssList.Add(origionalSettings[i]);
                    }
                }
                newSettings = newSettingssList.ToArray();
                SaveStringEnumToDb(settingName, newSettings, false);

                results = (deleteOldSetting) ? SettingChangeResult.Merge : SettingChangeResult.Success;
                if(commitChanges)
                {
                    ctx.SubmitChanges();
                }
            }
            return newSettings;
        }

        #endregion
    }
}
