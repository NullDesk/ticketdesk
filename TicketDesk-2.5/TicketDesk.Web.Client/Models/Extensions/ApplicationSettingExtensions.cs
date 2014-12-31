using System.Data.Entity;
using System.Web.Mvc;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Models
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
    }
}