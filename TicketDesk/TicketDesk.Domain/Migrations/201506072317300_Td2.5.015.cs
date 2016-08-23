namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25015 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSettings", "SecuritySettingsJson", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSettings", "SecuritySettingsJson");
        }
    }
}
