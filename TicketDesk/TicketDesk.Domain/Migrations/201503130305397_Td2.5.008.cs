namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25008 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TicketSubscribers");
            AddColumn("dbo.TicketSubscribers", "SubscriberUserName", c => c.String(nullable: false, maxLength: 256));
            AddPrimaryKey("dbo.TicketSubscribers", new[] { "TicketId", "SubscriberUserName" });
            DropColumn("dbo.TicketEvents", "NotificationsSent");
            DropColumn("dbo.TicketSubscribers", "Subscriber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketSubscribers", "Subscriber", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.TicketEvents", "NotificationsSent", c => c.Boolean());
            DropPrimaryKey("dbo.TicketSubscribers");
            DropColumn("dbo.TicketSubscribers", "SubscriberUserName");
            AddPrimaryKey("dbo.TicketSubscribers", new[] { "TicketId", "Subscriber" });
        }
    }
}
