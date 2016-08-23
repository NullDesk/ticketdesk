namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25010 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TicketEventNotifications", new[] { "TicketId", "SubscriberUserName" }, "dbo.TicketSubscribers");
            RenameColumn(table: "dbo.TicketEventNotifications", name: "SubscriberUserName", newName: "SubscriberId");
            RenameIndex(table: "dbo.TicketEventNotifications", name: "IX_TicketId_SubscriberUserName", newName: "IX_TicketId_SubscriberId");
            DropPrimaryKey("dbo.TicketSubscribers");
            AddColumn("dbo.TicketSubscribers", "SubscriberId", c => c.String(nullable: false, maxLength: 256));
            AddPrimaryKey("dbo.TicketSubscribers", new[] { "TicketId", "SubscriberId" });
            AddForeignKey("dbo.TicketEventNotifications", new[] { "TicketId", "SubscriberId" }, "dbo.TicketSubscribers", new[] { "TicketId", "SubscriberId" });
            DropColumn("dbo.TicketSubscribers", "SubscriberUserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketSubscribers", "SubscriberUserName", c => c.String(nullable: false, maxLength: 256));
            DropForeignKey("dbo.TicketEventNotifications", new[] { "TicketId", "SubscriberId" }, "dbo.TicketSubscribers");
            DropPrimaryKey("dbo.TicketSubscribers");
            DropColumn("dbo.TicketSubscribers", "SubscriberId");
            AddPrimaryKey("dbo.TicketSubscribers", new[] { "TicketId", "SubscriberUserName" });
            RenameIndex(table: "dbo.TicketEventNotifications", name: "IX_TicketId_SubscriberId", newName: "IX_TicketId_SubscriberUserName");
            RenameColumn(table: "dbo.TicketEventNotifications", name: "SubscriberId", newName: "SubscriberUserName");
            AddForeignKey("dbo.TicketEventNotifications", new[] { "TicketId", "SubscriberUserName" }, "dbo.TicketSubscribers", new[] { "TicketId", "SubscriberUserName" });
        }
    }
}
