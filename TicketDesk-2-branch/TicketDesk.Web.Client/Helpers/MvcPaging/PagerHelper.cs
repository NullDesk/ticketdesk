using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web.Mvc.Ajax;

namespace MvcPaging
{
    public static class PagerHelper
    {
        public static IList<PagerItem> PagerList<T>(this AjaxHelper helper, IPageOfList<T> pageOfList, AjaxOptions ajaxOptions)
        {
            return PagerList<T>(helper, pageOfList.TotalPageCount, pageOfList.PageIndex, null, null, null, ajaxOptions, null);
        }

        public static IList<PagerItem> PagerList<T>(this AjaxHelper helper, IPageOfList<T> pageOfList, PagerOptions options, AjaxOptions ajaxOptions)
        {
            return PagerList<T>(helper, pageOfList.TotalPageCount, pageOfList.PageIndex, null, null, options, ajaxOptions, null);
        }


        public static IList<PagerItem> PagerList<T>(this AjaxHelper helper, int totalPageCount, int pageIndex, string actionName, string controllerName, PagerOptions options, AjaxOptions ajaxOptions, object values)
        {
            var builder = new PagerBuilder
                (
                    helper,
                    actionName,
                    controllerName,
                    totalPageCount,
                    pageIndex,
                    options,
                    ajaxOptions,
                    values
                );
            return builder.ToList();
        }

        public static string Pager<T>(this AjaxHelper helper, PageOfList<T> pageOfList, AjaxOptions ajaxOptions)
        {
            return Pager(helper, pageOfList.TotalPageCount, pageOfList.PageIndex, null, null, null, ajaxOptions, null);
        }

        public static string Pager<T>(this AjaxHelper helper, PageOfList<T> pageOfList, PagerOptions options, AjaxOptions ajaxOptions)
        {
            return Pager(helper, pageOfList.TotalPageCount, pageOfList.PageIndex, null, null, options, ajaxOptions, null);
        }

        public static string Pager(this AjaxHelper helper, int totalPageCount, int pageIndex, string actionName, string controllerName, PagerOptions options, AjaxOptions ajaxOptions, object values)
        {
            var builder = new PagerBuilder
                (
                    helper,
                    actionName,
                    controllerName,
                    totalPageCount,
                    pageIndex,
                    options,
                    ajaxOptions,
                    values
                );
            return builder.RenderList();

        }
    }
}