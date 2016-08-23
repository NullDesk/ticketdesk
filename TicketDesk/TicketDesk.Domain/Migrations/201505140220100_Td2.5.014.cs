namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25014 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TicketEvents", "Comment", c => c.String());
            AlterColumn("dbo.Tickets", "Details", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "Details", c => c.String(nullable: false, storeType: "ntext"));
            AlterColumn("dbo.TicketEvents", "Comment", c => c.String(storeType: "ntext"));
        }
    }
}
