namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25012 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TicketEventNotifications", "PushNotificationPending");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketEventNotifications", "PushNotificationPending", c => c.Boolean(nullable: false));
        }
    }
}
