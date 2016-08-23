namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25009 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TicketEventNotifications",
                c => new
                    {
                        TicketId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                        SubscriberUserName = c.String(nullable: false, maxLength: 256),
                        IsNew = c.Boolean(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        PushNotificationPending = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.TicketId, t.EventId, t.SubscriberUserName })
                .ForeignKey("dbo.TicketEvents", t => new { t.TicketId, t.EventId }, cascadeDelete: true)
                .ForeignKey("dbo.TicketSubscribers", t => new { t.TicketId, t.SubscriberUserName })
                .Index(t => new { t.TicketId, t.EventId })
                .Index(t => new { t.TicketId, t.SubscriberUserName })
                .Index(t => t.EventId)
                .Index(t => t.SubscriberUserName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketEventNotifications", new[] { "TicketId", "SubscriberUserName" }, "dbo.TicketSubscribers");
            DropForeignKey("dbo.TicketEventNotifications", new[] { "TicketId", "EventId" }, "dbo.TicketEvents");
            DropIndex("dbo.TicketEventNotifications", new[] { "SubscriberUserName" });
            DropIndex("dbo.TicketEventNotifications", new[] { "EventId" });
            DropIndex("dbo.TicketEventNotifications", new[] { "TicketId", "SubscriberUserName" });
            DropIndex("dbo.TicketEventNotifications", new[] { "TicketId", "EventId" });
            DropTable("dbo.TicketEventNotifications");
        }
    }
}
