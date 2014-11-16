using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Models
{
    public static class SettingsExtensions
    {
        public static SelectList GetPriorityList(this DbSet<Setting> settings, bool includeEmpty, string selectedPriority)
        {
            return settings.GetSettingValue("PriorityList", new string[0]).ToSelectList(p => p, p => p, selectedPriority, includeEmpty);
        }

        public static SelectList GetCategoryList(this DbSet<Setting> settings, bool includeEmpty, string selectedCategory)
        {
            return settings.GetSettingValue("CategoryList", new string[0]).ToSelectList(p => p, p => p, selectedCategory, includeEmpty);
        }

        public static SelectList GetTicketTypeList(this DbSet<Setting> settings, bool includeEmpty, string selectedType)
        {
            return settings.GetSettingValue("TicketTypesList", new string[0]).ToSelectList(p => p, p => p, selectedType, includeEmpty);
        }
        
    }
}