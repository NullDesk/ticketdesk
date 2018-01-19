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
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TicketDesk.Domain.Model;
using TicketDesk.Localization.Infrastructure;

namespace TicketDesk.Web.Client
{
    public static class SortableColumnHeaderHelper
    {
        public static IHtmlString SortableColumnHeader(this AjaxHelper helper, HtmlHelper htmlHelper,
            UserTicketListSetting currentDisplayPreferences,int currentPage, string action, string listName, string sortColumn,
            string linkText, AjaxOptions ajaxOptions)
        {
            var imgContent = string.Empty;
            var linkContent = linkText ?? " ";
            var indexContent = string.Empty;
            var sortColumns = currentDisplayPreferences.SortColumns;
            var cColumn = sortColumns.SingleOrDefault(sc => sc.ColumnName == sortColumn);
            if (cColumn != null)
            {
                var uh = new UrlHelper(helper.ViewContext.RequestContext);
                var imgSrc =
                    uh.Content((cColumn.SortDirection == ColumnSortDirection.Ascending)
                        ? "~/Content/Images/arrow_top.png"
                        : "~/Content/Images/arrow_down.png");
                imgContent = string.Format("<img src='{0}' alt='{1}' />", imgSrc, cColumn.SortDirection.ToString());

                var idx = sortColumns.IndexOf(cColumn) + 1;
                indexContent = string.Format("<sup>[{0}]</sup>", idx);
            }

            var sb = new StringBuilder();
            sb.AppendLine("<span style='white-space:nowrap;'>");
            var lLine =
                // ReSharper disable once Mvc.ActionNotResolved
                helper.ActionLink(linkContent, "SortList", new {page = currentPage,  ListName = listName, ColumnName = sortColumn },
                    ajaxOptions,
                    new
                    {
                        OnMouseDown = "ticketCenter.sorts.setShiftStatus(event);",
                        Title = Strings.RemoveColumnsFromSort
                    }).ToString();


            sb.AppendLine(lLine + indexContent);
            sb.AppendLine(imgContent);
            sb.AppendLine("</span>");
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}