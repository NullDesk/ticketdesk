using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain.Utilities;
using System.ComponentModel.Composition;

namespace TicketDesk.Web.Client.Controllers
{
    [Export("Markdown", typeof(IController))]
    public partial class MarkdownController : Controller
    {
        [ValidateInput(false)]
        public virtual ContentResult MarkdownPreview(string data)
        {
            var c = new ContentResult();

            var md = new Markdown();
            c.Content = "<style>body{font-size: 8pt;font-family: Verdana, Helvetica, Sans-Serif;margin: 0;padding: 2px;color: #555;}\n</style>";
            c.Content += md.Transform(data, true);

            return c;
        }


    }
}
