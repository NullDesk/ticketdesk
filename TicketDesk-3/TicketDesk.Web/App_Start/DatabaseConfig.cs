using System;
using System.Collections.Generic;
using System.Configuration;
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

            var con = ConfigurationManager.ConnectionStrings["TicketDesk"];
            TicketDeskLegacyDatabaseInitializer.InitDatabase(con.ConnectionString, con.ProviderName);
            TicketDeskDatabaseInitializer.InitDatabase(con.ConnectionString, con.ProviderName);
        }
    }
}