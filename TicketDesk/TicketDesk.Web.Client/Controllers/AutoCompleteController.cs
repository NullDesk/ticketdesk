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
using System.Web.Mvc;
using TicketDesk.Domain;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("auto-complete")]
    [TdAuthorize(Roles = "TdInternalUsers,TdHelpDeskUsers,TdAdministrators")]
    public class AutoCompleteController : Controller
    {

        private TdDomainContext Context { get; set; }
        public AutoCompleteController(TdDomainContext context)
        {
            Context = context;
        }

        [Route("tag-list")]
        [HttpGet]
        public ActionResult TagList(string term)
        {
            //TODO: cache a complete distinct taglist on app start to improve performance, keep this action synchronous
            var tags = Context.TicketTags
                        .Where(tag => tag.TagName.StartsWith(term))
                        .GroupBy(tag => tag.TagName)
                        .Select(g => g.FirstOrDefault())
                        .Take(10);//limit to 10

            //pull data, then convert to format expected by select2
            return new JsonCamelCaseResult
            {
                Data = tags.ToArray().Select(t => new {Id = t.TagName, Text = t.TagName}),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
    }
}