namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSettings", "ClientSettingsJson", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSettings", "ClientSettingsJson");
        }
    }
}
