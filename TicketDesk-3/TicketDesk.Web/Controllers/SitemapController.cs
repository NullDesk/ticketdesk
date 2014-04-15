using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Web.Providers;
using TicketDesk.Web.Results;
using TicketDesk.Web.SEO;


namespace TicketDesk.Web.Controllers
{
    public class SitemapController : Controller
    {
        private TicketDeskBreezeContext breezeContext;

        public SitemapController(TicketDeskBreezeContext ctx)
        {
            this.breezeContext = ctx;
        }

        //
        // GET: /Sitemap/
        public ActionResult Sitemap()
        {
            var url = System.Web.HttpContext.Current.Request.Url;
            var leftpart = url.Scheme + "://" + url.Authority;

            // Static Urls
            var sitemapItems = new List<SitemapItem>
            {                
                new SitemapItem(leftpart + "/home/index", changeFrequency: SitemapChangeFrequency.Weekly, priority: 1.0),
                 new SitemapItem(leftpart + "/home/help", changeFrequency: SitemapChangeFrequency.Weekly, priority: 1.0),
                new SitemapItem(leftpart + "/home/about", changeFrequency: SitemapChangeFrequency.Monthly, priority: 1.0),
                new SitemapItem(leftpart + "/account/login", changeFrequency: SitemapChangeFrequency.Monthly, priority: 0.5),
                new SitemapItem(leftpart + "/account/register", changeFrequency: SitemapChangeFrequency.Monthly, priority: 0.5)
            };

            //TODO: sitemap from durandalauth, implement for tickets?
            //var articles = breezeContext.Context.Articles.Where(a => a.IsPublished);

            //foreach (var article in articles)
            //{
            //    sitemapItems.Add(new SitemapItem(leftpart + "/" + article.CreatedBy + "/" + article.Category.Name + "/" + article.UrlCodeReference, changeFrequency: SitemapChangeFrequency.Weekly, priority: 0.8));
            //}

            return new SitemapResult(sitemapItems);
        }
	}
}