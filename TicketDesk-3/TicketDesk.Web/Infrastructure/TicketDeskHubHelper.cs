using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using TicketDesk.Web.Controllers;

namespace TicketDesk.Web.Infrastructure
{
    public class TicketDeskHubHelper
    {
        public static TicketDeskHubHelper Instance
        {
            get { return _instance.Value; }
        }

        // Singleton instance
        private static readonly Lazy<TicketDeskHubHelper> _instance = new Lazy<TicketDeskHubHelper>(
            () => new TicketDeskHubHelper(GlobalHost.ConnectionManager.GetHubContext<TicketDeskHub>()));

        private IHubContext _context;

        private TicketDeskHubHelper(IHubContext context)
        {
            _context = context;
        }

        // This method is invoked by a Timer object.
        internal void NotifyTicketChanged(int ticketId)
        {
            _context.Clients.All.ticketChanged(ticketId);

        }
    }
}