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
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Mvc.Ajax;
using System.Text;
using System.Collections.Generic;
using System.Web.Routing;

namespace TicketDesk.Web.Client.Helpers
{

    internal class PagerBuilder
    {
        private AjaxHelper _helper;
        private string _actionName;
        private string _controllerName;
        private int _totalPageCount;
        private int _pageIndex;
        private PagerOptions _options;
        private object _values;
        private AjaxOptions _ajaxOptions;


        private int _startPageIndex;
        private int _endPageIndex;


        internal PagerBuilder(AjaxHelper helper, string actionName, string controllerName, int totalPageCount, int pageIndex, PagerOptions options, AjaxOptions ajaxOptions, object values)
        {
            // Set defaults
            if (String.IsNullOrEmpty(actionName))
                actionName = (string)helper.ViewContext.RouteData.Values["action"];
            if (String.IsNullOrEmpty(controllerName))
                controllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            if (options == null)
                options = new PagerOptions();

            // Assign to fields
            _helper = helper;
            _actionName = actionName;
            _controllerName = controllerName;
            _totalPageCount = totalPageCount;
            _pageIndex = pageIndex;
            _options = options;
            _values = values;
            _ajaxOptions = ajaxOptions;

            // Calculate start page index
            _startPageIndex = pageIndex - (options.MaximumPageNumbers / 2);
            if (_startPageIndex + options.MaximumPageNumbers > _totalPageCount)
                _startPageIndex = _totalPageCount - options.MaximumPageNumbers;
            if (_startPageIndex < 0)
                _startPageIndex = 0;

            // Calculate end page index
            _endPageIndex = _startPageIndex + _options.MaximumPageNumbers;
            if (_endPageIndex > _totalPageCount)
                _endPageIndex = _totalPageCount;

        }


        internal IList<PagerItem> ToList()
        {
            var results = new List<PagerItem>();

            // Add previous link
            if (_options.ShowPrevious)
                AddPrevious(results);

            if (_options.ShowNumbers)
            {
                // Add range ellipsis
                AddPreRange(results);

                // Add page numbers
                AddPageNumbers(results);

                // Add range ellipsis
                AddPostRange(results);
            }

            // Add next link
            if (_options.ShowNext)
                AddNext(results);

            return results;
        }


        private void AddPrevious(List<PagerItem> results)
        {
            if (_pageIndex > 0)
            {
                var text = _options.PreviousText;
                var url = GenerateUrl(_pageIndex - 1);
                var item = new PagerItem(text, url, false, _pageIndex - 1);
                results.Add(item);
            }
        }


        private void AddPreRange(List<PagerItem> results)
        {
            if (_startPageIndex > 0)
            {
                var text = "...";
                var index = _startPageIndex - _options.MaximumPageNumbers;
                if (index < 0) index = 0;
                var url = GenerateUrl(index);
                var item = new PagerItem(text, url, false, index);
                results.Add(item);
            }
        }

        private void AddPageNumbers(List<PagerItem> results)
        {
            for (var pageIndex = _startPageIndex; pageIndex < _endPageIndex; pageIndex++)
            {
                var text = (pageIndex + 1).ToString();
                var url = GenerateUrl(pageIndex);
                var isSelected = pageIndex == _pageIndex;
                if (isSelected)
                    text = String.Format(_options.SelectedPageNumberFormatString, text);
                else
                    text = String.Format(_options.PageNumberFormatString, text);
                var item = new PagerItem(text, url, isSelected, pageIndex);
                results.Add(item);
            }
        }

        private void AddPostRange(List<PagerItem> results)
        {
            if (_endPageIndex < _totalPageCount)
            {
                var text = "...";
                var index = _startPageIndex + _options.MaximumPageNumbers;
                if (index > _totalPageCount) index = _totalPageCount;
                var url = GenerateUrl(index);
                var item = new PagerItem(text, url, false, index);
                results.Add(item);
            }
        }

        private void AddNext(List<PagerItem> results)
        {
            if (_pageIndex < (_totalPageCount - 1))
            {
                var text = _options.NextText;
                var url = GenerateUrl(_pageIndex + 1);
                var item = new PagerItem(text, url, false, _pageIndex + 1);
                results.Add(item);
            }
        }

        private string GenerateUrl(int pageIndex)
        {
            var routeValues = MakeRoute(pageIndex);

            // Return link
            var urlHelper = new UrlHelper(_helper.ViewContext.RequestContext);
            return urlHelper.RouteUrl(routeValues);
        }

        private RouteValueDictionary MakeRoute(int pageIndex)
        {
            var routeValues = new RouteValueDictionary(_values);

            // Add page index
            routeValues[_options.IndexParameterName] = pageIndex + 1;

            // Add action
            routeValues["action"] = _actionName;

            // Add controller
            routeValues["controller"] = _controllerName;
            return routeValues;
        }


        internal string RenderList()
        {
            var results = this.ToList();
            var sb = new StringBuilder();

            UrlHelper uh = new UrlHelper(_helper.ViewContext.RequestContext);

            var formHref = uh.Action(_actionName);



            sb.AppendLine("<ul class='pageNumbers'>");
            foreach (PagerItem item in results)
            {
                if (item.IsSelected)
                    sb.AppendFormat("<li class='selectedPageNumber'>{0}</li>", item.Text);
                else
                    sb.AppendFormat("<li class='pageNumber'>{0}</li>", GenerateLink(item));
            }
            sb.AppendLine("</ul>");

            return sb.ToString();
        }

        private string GenerateLink(PagerItem item)
        {

            return _helper.ActionLink(item.Text, _actionName, MakeRoute(item.PageIndex), _ajaxOptions).ToString();

        }
    }
}