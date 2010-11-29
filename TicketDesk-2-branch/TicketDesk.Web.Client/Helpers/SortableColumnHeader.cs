using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain.Models;
using System.Text;
using System.Web.Mvc.Ajax;

namespace TicketDesk.Web.Client.Helpers
{
    public static class SortableColumnHeaderHelper
    {
        public static string SortableColumnHeader(this AjaxHelper helper, HtmlHelper htmlHelper, TicketCenterListSettings currentDisplayPreferences, string action, string listName, string sortColumn, string linkText, AjaxOptions ajaxOptions)
        {
            var imgContent = string.Empty;
            var linkContent = linkText;
            var indexContent = string.Empty;
            var sortColumns = currentDisplayPreferences.SortColumns;
            var cColumn = sortColumns.SingleOrDefault(sc => sc.ColumnName == sortColumn);
            if (cColumn != null)
            {
                var uh = new UrlHelper(helper.ViewContext.RequestContext);
                string imgSrc = uh.Content((cColumn.SortDirection == ColumnSortDirection.Ascending) ? "~/Content/arrow_top.png" : "~/Content/arrow_down.png");
                imgContent = string.Format("<img src='{0}' alt='{1}' />", imgSrc, cColumn.SortDirection.ToString());

                var idx = sortColumns.IndexOf(cColumn) + 1;
                indexContent = string.Format("<sup>[{0}]</sup>",idx.ToString()); //linkContent + idx.ToString();
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<span style='white-space:nowrap;'>");
            var lLine = helper.ActionLink(linkContent, "SortList", new { ListName = listName, ColumnName = sortColumn }, ajaxOptions, new { OnMouseDown = "setShiftStatus(event);", Title = "[shift+click] to add or remove columns from the sort" }).ToString();


            sb.AppendLine(lLine + indexContent);
            sb.AppendLine(imgContent);
            sb.AppendLine("</span>");
            return sb.ToString();
        }
    }
}