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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Services;

namespace TicketDesk.Web.Client.Controllers
{
    [Export("TagList", typeof(IController))]
    public partial class TagListController : ApplicationController
    {
        public ITicketService Tickets { get; private set; }
        
        [ImportingConstructor]
        public TagListController(ITicketService ticketService, ISecurityService security)
            : base(security)
        {
            Tickets = ticketService;
        }


        /// <summary>
        /// Gets the list of tags for autocomplete fields.
        /// </summary>
        /// <param name="tagList">The user's tag list to attempt to complete.</param>
        /// <returns></returns>
        public virtual JsonResult AutoComplete(string q, int limit, string timestamp)
        {
            var tags = from t in Tickets.GetTagCompletionList(q, limit)
                       select new { TagName = t };

            return Json(tags.ToArray(), JsonRequestBehavior.AllowGet);

            //return  Json.Content(string.Join("\n", tags));
        }

    }
}
