namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25013 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ApplicationSettings", "PushNotificationSettingsJson");
            DropColumn("dbo.UserSettings", "PushNotificationSettingsJson");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserSettings", "PushNotificationSettingsJson", c => c.String());
            AddColumn("dbo.ApplicationSettings", "PushNotificationSettingsJson", c => c.String());
        }
    }
}
