using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketDesk.Domain;
using TicketDesk.Domain.Identity;
using TicketDesk.Domain.Legacy;
using TicketDesk.Domain.Legacy.Migrations;

[assembly: WebActivator.PostApplicationStartMethod(typeof(TicketDesk.Web.TicketDeskDatabaseConfig), "PostStart")]
namespace TicketDesk.Web
{
    public static class TicketDeskDatabaseConfig
    {
        public static void PostStart()
        {
            TicketDeskLegacyDatabaseInitializer.InitDatabase("TicketDesk");
            TicketDeskDatabaseInitializer.InitDatabase("TicketDesk");
            TicketDeskIdentityDatabaseInitializer.InitDatabase("TicketDesk");
          
           
        }
    }
}