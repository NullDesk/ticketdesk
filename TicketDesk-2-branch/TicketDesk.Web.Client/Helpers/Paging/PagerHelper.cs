using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web.Mvc.Ajax;
using MvcContrib.Pagination;

namespace TicketDesk.Web.Client.Helpers
{
    public static class PagerHelper
    {
        public static IList<PagerItem> PagerList<T>(this AjaxHelper helper, IPagination<T> pageOfList, AjaxOptions ajaxOptions)
        {
            return PagerList<T>(helper, pageOfList.TotalPages, pageOfList.PageNumber, null, null, null, ajaxOptions, null);
        }

        public static IList<PagerItem> PagerList<T>(this AjaxHelper helper, IPagination<T> pageOfList, PagerOptions options, AjaxOptions ajaxOptions)
        {
            return PagerList<T>(helper, pageOfList.TotalPages, pageOfList.PageNumber, null, null, options, ajaxOptions, null);
        }


        public static IList<PagerItem> PagerList<T>(this AjaxHelper helper, int totalPageCount, int pageNumber, string actionName, string controllerName, PagerOptions options, AjaxOptions ajaxOptions, object values)
        {
            var builder = new PagerBuilder
                (
                    helper,
                    actionName,
                    controllerName,
                    totalPageCount,
                    pageNumber - 1,//pageNumber is 1 based, decrement for a zero based index
                    options,
                    ajaxOptions,
                    values
                );
            return builder.ToList();
        }

        public static string Pager<T>(this AjaxHelper helper, IPagination<T> pageOfList, AjaxOptions ajaxOptions)
        {
            return Pager(helper, pageOfList.TotalPages, pageOfList.PageNumber, null, null, null, ajaxOptions, null);
        }

        public static string Pager<T>(this AjaxHelper helper, IPagination<T> pageOfList, PagerOptions options, AjaxOptions ajaxOptions)
        {
            return Pager(helper, pageOfList.TotalPages, pageOfList.PageNumber, null, null, options, ajaxOptions, null);
        }

        public static string Pager(this AjaxHelper helper, int totalPageCount, int pageNumber, string actionName, string controllerName, PagerOptions options, AjaxOptions ajaxOptions, object values)
        {
            var builder = new PagerBuilder
                (
                    helper,
                    actionName,
                    controllerName,
                    totalPageCount,
                    pageNumber - 1,//pageNumber is 1 based, decrement for a zero based index
                    options,
                    ajaxOptions,
                    values
                );
            return builder.RenderList();

        }
    }
}