using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
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
                : attr.Prompt ?? (attr.Name ?? property.Name); 
            return new MvcHtmlString(prompt);
        }
    }
}