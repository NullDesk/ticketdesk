// TicketDesk - Attribution notice
//    
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

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using TicketDesk.Localization;
using TicketDesk.Localization.Infrastructure;

namespace TicketDesk.Web.Client
{

    public static class DescriptionForHelper
    {
        public static MvcHtmlString DescriptionFor(
            this HtmlHelper helper,
            PropertyInfo property,
            string cssClassName = "",
            string tagName = "div")
        {
            var attr = (DescriptionAttribute)property.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
            return GetDescriptionFromAttribute(cssClassName, tagName, attr);
        }


        public static MvcHtmlString DescriptionFor(
            this HtmlHelper helper,
            Type modelType,
            string cssClassName = "",
            string tagName = "div"
            )
        {
            var attr = (DescriptionAttribute)modelType.GetCustomAttributes(typeof(LocalizedDescriptionAttribute), false).FirstOrDefault();
            if (attr == null)
                attr = (DescriptionAttribute)modelType.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
            return GetDescriptionFromAttribute(cssClassName, tagName, attr);

        }

        public static MvcHtmlString DescriptionFor<TModel, TValue>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression,
            string cssClassName = "",
            string tagName = "div")
        {
            //TODO: consider changing this to use Display(description="") instead? Same with enum extension
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException(Strings.MemberExpression);
            }
            var prop = memberExpression.Member;
            var attr = (DescriptionAttribute)prop.GetCustomAttributes(typeof(LocalizedDescriptionAttribute), false).FirstOrDefault();
            if (attr == null)
                attr = (DescriptionAttribute)prop.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
            return GetDescriptionFromAttribute(cssClassName, tagName, attr);
        }

        private static MvcHtmlString GetDescriptionFromAttribute(string cssClassName, string tagName, DescriptionAttribute attr)
        {
            if (attr != null)
            {
                var description = attr.Description;
                if (!string.IsNullOrEmpty(description))
                {
                    var tag = new TagBuilder(tagName) { InnerHtml = description };
                    if (!string.IsNullOrEmpty(cssClassName))
                    {
                        tag.AddCssClass(cssClassName);
                    }
                    return new MvcHtmlString(tag.ToString());
                }
            }

            return MvcHtmlString.Empty;
        }
    }
}