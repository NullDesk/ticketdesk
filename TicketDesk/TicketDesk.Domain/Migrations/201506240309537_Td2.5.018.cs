namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25018 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSettings", "SelectedProjectId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSettings", "SelectedProjectId");
        }
    }
}
