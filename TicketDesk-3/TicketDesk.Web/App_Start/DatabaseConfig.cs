using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketDesk.Domain;
using TicketDesk.Domain.Legacy;

namespace TicketDesk.Web.App_Start
{
   
    public static class DatabaseConfig
    {
        public static void InitDatabase()
        {
            TicketDeskLegacyDatabaseInitializer.InitDatabase("TicketDesk");
            TicketDeskDatabaseInitializer.InitDatabase("TicketDesk");
        }
    }
}