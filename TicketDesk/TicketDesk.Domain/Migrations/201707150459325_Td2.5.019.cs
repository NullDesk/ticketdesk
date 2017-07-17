namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Td25019 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketEvents", "ForActivity", c => c.Int(nullable: false, defaultValue: 16384));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketEvents", "ForActivity");
        }
    }
}
