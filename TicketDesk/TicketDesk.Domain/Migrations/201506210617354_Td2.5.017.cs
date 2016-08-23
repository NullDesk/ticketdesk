namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25017 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(nullable: false, maxLength: 100),
                        ProjectDescription = c.String(maxLength: 500),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                    })
                .PrimaryKey(t => t.ProjectId);
            Sql("INSERT INTO [dbo].[Projects] (ProjectName, ProjectDescription) VALUES ('Default','Default project')");

            AddColumn("dbo.TicketEventNotifications", "Version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"));
            AddColumn("dbo.Tickets", "ProjectId", c => c.Int(nullable: false, defaultValue: 1));
            CreateIndex("dbo.Tickets", "ProjectId");
            AddForeignKey("dbo.Tickets", "ProjectId", "dbo.Projects", "ProjectId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Tickets", new[] { "ProjectId" });
            DropColumn("dbo.Tickets", "ProjectId");
            DropColumn("dbo.TicketEventNotifications", "Version");
            DropTable("dbo.Projects");
        }
    }
}
