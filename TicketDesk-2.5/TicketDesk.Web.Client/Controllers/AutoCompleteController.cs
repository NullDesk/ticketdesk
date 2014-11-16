using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain;

namespace TicketDesk.Web.Client.Controllers
{
    public class AutoCompleteController : Controller
    {

        private TicketDeskContext Context { get; set; }
        public AutoCompleteController(TicketDeskContext context)
        {
            Context = context;
        }


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