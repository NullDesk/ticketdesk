namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25005 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TicketEventNotifications", new[] { "TicketId", "CommentId" }, "dbo.TicketEvents");
            DropIndex("dbo.TicketEventNotifications", new[] { "TicketId", "CommentId" });
            AddColumn("dbo.TicketEvents", "NotificationsSent", c => c.Boolean());
            DropTable("dbo.TicketEventNotifications");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TicketEventNotifications",
                c => new
                    {
                        TicketId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                        NotifyUser = c.String(nullable: false, maxLength: 100),
                        NotifyUserDisplayName = c.String(nullable: false, maxLength: 100),
                        NotifyEmail = c.String(nullable: false, maxLength: 255),
                        NotifyUserReason = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        DeliveryAttempts = c.Int(nullable: false),
                        LastDeliveryAttemptDate = c.DateTimeOffset(precision: 7),
                        Status = c.String(nullable: false, maxLength: 20),
                        NextDeliveryAttemptDate = c.DateTimeOffset(precision: 7),
                        EventGeneratedByUser = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => new { t.TicketId, t.CommentId, t.NotifyUser });
            
            DropColumn("dbo.TicketEvents", "NotificationsSent");
            CreateIndex("dbo.TicketEventNotifications", new[] { "TicketId", "CommentId" });
            AddForeignKey("dbo.TicketEventNotifications", new[] { "TicketId", "CommentId" }, "dbo.TicketEvents", new[] { "TicketId", "EventId" });
        }
    }
}
