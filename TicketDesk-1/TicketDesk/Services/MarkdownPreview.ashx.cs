using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using TicketDesk.Engine;
using System.Text.RegularExpressions;

namespace TicketDesk.Services
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://ticketdesk.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MarkdownPreview1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var md = new Markdown();
            var data = context.Request.Form["data"];
            context.Response.Write(md.Transform(data,true));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        
    }
}
