namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25011 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSettings", "PushNotificationSettingsJson", c => c.String());
            AddColumn("dbo.UserSettings", "PushNotificationSettingsJson", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSettings", "PushNotificationSettingsJson");
            DropColumn("dbo.ApplicationSettings", "PushNotificationSettingsJson");
        }
    }
}
