namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Td25021 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "EstimatedDuration", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Tickets", "ActualDuration", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "ActualDuration");
            DropColumn("dbo.Tickets", "EstimatedDuration");
        }
    }
}
