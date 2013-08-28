using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketDesk.Domain;
using TicketDesk.Legacy;
using TicketDesk.Legacy.Migrations;

[assembly: WebActivator.PostApplicationStartMethod(typeof(TicketDesk.Web.TicketDeskDatabaseConfig), "PostStart")]
namespace TicketDesk.Web
{
    public static class TicketDeskDatabaseConfig
    {
        public static void PostStart()
        {
            System.Data.Entity.Database.SetInitializer(new LegacyDatabaseInitializer("TicketDesk"));
            System.Data.Entity.Database.SetInitializer<TicketDeskContext>(new TicketDeskDatabaseInitializer("TicketDesk"));

            using (var legacyCtx = new TicketDeskLegacyContext())
            {
                legacyCtx.Database.Initialize(false);
            }
            using (var ctx = new TicketDeskContext())
            {
                ctx.Database.Initialize(false);
            }
        }
    }
}