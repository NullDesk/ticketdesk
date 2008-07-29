using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using TicketDesk.Engine.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web;
using TicketDesk.Controls;
using System.IO;

namespace TicketDesk.Services
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FeedService : IFeedService
    {
       public SyndicationFeedFormatter CreateFeed()
        {
            TicketDataDataContext ctx = new TicketDataDataContext();
            
            // Create a new Syndication Feed.

            IQueryable<Ticket> tickets = from t in ctx.Tickets
                                         select t;

            
            // Create a new Syndication Item.
            string idArray = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["id"];
            if (idArray != null)
            {
                tickets = from t in tickets
                          where idArray.Split(',').Contains( t.TicketId.ToString())
                          select t;
            }

           

            string categoryArray = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["category"];
            if (categoryArray != null)
            {
                tickets = from t in tickets
                          where categoryArray.Split(',').Contains(t.Category)
                          select t;
            }

            string tagArray = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["tag"];
            if (tagArray != null)
            {
                tickets = from t in tickets
                          join tags in ctx.TicketTags
                          on t.TicketId equals tags.TicketId
                          where tagArray.Split(',').Contains(tags.TagName)
                          select t;
            }

            string priorityArray = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["priority"];
            if (priorityArray != null)
            {
                tickets = from t in tickets
                          where priorityArray.Split(',').Contains(t.Priority)
                          select t;
            }

            string assignedArray = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["assigned"];
            if (assignedArray != null)
            {
                tickets = from t in tickets
                          where assignedArray.Split(',').Contains(t.AssignedTo)
                          select t;
            }

            string ownerArray = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["owner"];
            if (ownerArray != null)
            {
                tickets = from t in tickets
                          where ownerArray.Split(',').Contains(t.Owner)
                          select t;
            }

            string statusArray = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["status"];
            if (statusArray != null)
            {
                tickets = from t in tickets
                          where statusArray.Split(',').Contains(t.CurrentStatus)
                          select t;
            }

            string closed = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["closed"];
            if ((statusArray == null) && ((closed == null) || ((closed != "yes") && (closed != "1"))))
            {
                tickets = from t in tickets
                          where t.CurrentStatus != "Closed"
                          select t;
            }

            List<SyndicationItem> items = new List<SyndicationItem>();
            foreach (Ticket ticket in tickets.Distinct().OrderByDescending(t => t.LastUpdateDate))
            {
                SyndicationItem item = GetRSSEntryForTicket(ticket);
                items.Add(item);
            }

            SyndicationFeed feed = new SyndicationFeed("TicketDesk RSS", "TicketDeskRSS", null, "TicketDeskRSS", DateTimeOffset.Now, items);

            string query = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["format"];
            SyndicationFeedFormatter formatter = null;
            if (query == "atom")
            {
                formatter = new Atom10FeedFormatter(feed);
            }
            else
            {
                formatter = new Rss20FeedFormatter(feed);
            }

            return formatter;
        }

       private SyndicationItem GetRSSEntryForTicket(Ticket ticket)
       {
           string viewTicket = "/ViewTicket.aspx?id=" + ticket.TicketId.ToString();
           string link = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
           HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + viewTicket;
           Uri uri = new Uri(link);

           TextSyndicationContent content = new TextSyndicationContent(GetItemBody(ticket, link), TextSyndicationContentKind.Html);

           SyndicationItem item = new SyndicationItem(ticket.Title,"",uri, ticket.TicketId.ToString(), (DateTimeOffset)ticket.LastUpdateDate);
           item.Content = content;

           return item;
       }

       private string GetItemBody(Ticket ticket, string link)
       {
           Page page = new Page();
        
           StringBuilder stringBuilder = new StringBuilder();
           StringWriter stringWriter = new StringWriter(stringBuilder);
           HtmlTextWriter htmWriter = new HtmlTextWriter(stringWriter);


           EmailTicketView view = (EmailTicketView)page.LoadControl("~/Controls/EmailTicketView.ascx");
           view.TicketToDisplay = ticket;


           view.Populate(link);
           view.RenderControl(htmWriter);

           string body = string.Format("{0}{1}{2}", "<html><head></head><body>", stringBuilder.ToString(), "</body></html>");
           return body;
       }
    }
}
