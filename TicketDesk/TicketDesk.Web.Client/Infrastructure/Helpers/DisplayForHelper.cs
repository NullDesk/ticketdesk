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

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace TicketDesk.Web.Client
{
    public static class DisplayForHelper
    {
        public static MvcHtmlString DisplayNameFor(this HtmlHelper helper, PropertyInfo property)
        {
            var attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            return new MvcHtmlString(attr == null || attr.Name == null ? property.Name : attr.GetName());
        }
    }
}