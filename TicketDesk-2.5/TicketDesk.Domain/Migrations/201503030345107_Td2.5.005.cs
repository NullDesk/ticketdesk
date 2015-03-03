namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Td25005 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSettings", "SearchSettings_Serialized", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSettings", "SearchSettings_Serialized");
        }
    }
}
