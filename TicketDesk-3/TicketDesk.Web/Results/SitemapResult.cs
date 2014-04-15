using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

using TicketDesk.Web.SEO;

namespace TicketDesk.Web.Results
{
    /// <summary>
    /// Generates an XML sitemap from a collection of <see cref="ISitemapItem"/>
    /// </summary>
    public class SitemapResult : ActionResult
    {
        private readonly IEnumerable<ISitemapItem> items;
        private readonly ISitemapGenerator generator;

        public SitemapResult(IEnumerable<ISitemapItem> items)
            : this(items, new SitemapGenerator())
        {
        }

        public SitemapResult(IEnumerable<ISitemapItem> items, ISitemapGenerator generator)
        {
            if (items == null || generator == null)
            {
                throw new NullReferenceException("SitemapResult");
            }

            this.items = items;
            this.generator = generator;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentType = "text/xml";
            response.ContentEncoding = Encoding.UTF8;

            using (var writer = new XmlTextWriter(response.Output))
            {
                writer.Formatting = Formatting.Indented;
                var sitemap = generator.GenerateSiteMap(items);

                sitemap.WriteTo(writer);
            }
        }
    }
}