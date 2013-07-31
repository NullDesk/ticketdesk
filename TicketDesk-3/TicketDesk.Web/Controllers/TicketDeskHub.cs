using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using TicketDesk.Web.Infrastructure;

namespace TicketDesk.Web.Controllers
{
    public class TicketDeskHub : Hub
    {
        // this test is taken from an article at http://www.dotnetcurry.com/ShowArticle.aspx?ID=903
        public void Hello()
        {
            Clients.All.hello("Ping " + DateTime.Now.ToString());
        }

        private readonly TicketDeskHubHelper _helper;

        public TicketDeskHub() : this(TicketDeskHubHelper.Instance) { }

        public TicketDeskHub(TicketDeskHubHelper stockTicker)
        {
            _helper = stockTicker;
        }

        public void NotifyTicketChanged(int ticketId)
        {
            Clients.All.ticketChanged(ticketId);
        }
    }
}