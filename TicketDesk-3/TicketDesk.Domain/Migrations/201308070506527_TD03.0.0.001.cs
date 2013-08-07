namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TD0300001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "TicketStatus", c => c.Int(nullable: false));
            Sql("Update dbo.Tickets set TicketStatus = 0 where CurrentStatus = 'Active'");
            Sql("Update dbo.Tickets set TicketStatus = 1 where CurrentStatus = 'More Info'");
            Sql("Update dbo.Tickets set TicketStatus = 2 where CurrentStatus = 'Resolved'");
            Sql("Update dbo.Tickets set TicketStatus = 3 where CurrentStatus = 'Closed'");

            DropColumn("dbo.Tickets", "CurrentStatus");
        }

        public override void Down()
        {
            AddColumn("dbo.Tickets", "CurrentStatus", c => c.String(nullable: false, maxLength: 50));

            Sql("Update dbo.Tickets set CurrentStatus = 'Active' where TicketStatus = 0 ");
            Sql("Update dbo.Tickets set CurrentStatus = 'More Info' where TicketStatus = 1 ");
            Sql("Update dbo.Tickets set CurrentStatus = 'Resolved' where TicketStatus = 2 ");
            Sql("Update dbo.Tickets set CurrentStatus = 'Closed' where TicketStatus = 3 ");

            DropColumn("dbo.Tickets", "TicketStatus");
        }
    }
}
