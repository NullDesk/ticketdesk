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

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace TicketDesk.Web.Client
{
    public static class PromptForHelper
    {
        public static MvcHtmlString PromptFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            var md = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            return new MvcHtmlString(md.Watermark ?? md.DisplayName ?? md.PropertyName);
        }

        public static MvcHtmlString PromptFor(this HtmlHelper helper, PropertyInfo property)
        {
            var attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            var prompt = attr == null
                ? property.Name
                : attr.GetPrompt() ?? (attr.GetName() ?? property.Name); 
            return new MvcHtmlString(prompt);
        }
    }
}