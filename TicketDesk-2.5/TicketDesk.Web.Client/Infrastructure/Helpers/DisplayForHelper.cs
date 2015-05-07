using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace TicketDesk.Web.Client
{
    public static class DisplayForHelper
    {
        public static MvcHtmlString DisplayNameFor(this HtmlHelper helper, PropertyInfo property)
        {
            var attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            
            return new MvcHtmlString(attr == null || attr.Name == null?  property.Name: attr.Name);
        }
    }
}