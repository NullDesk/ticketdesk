namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Td25020 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "DueDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "DueDate");
        }
    }
}
