namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25006 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.UserSettings");
            AlterColumn("dbo.TicketAttachments", "UploadedBy", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Tickets", "CreatedBy", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Tickets", "Owner", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Tickets", "AssignedTo", c => c.String(maxLength: 256));
            AlterColumn("dbo.Tickets", "CurrentStatusSetBy", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Tickets", "LastUpdateBy", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.TicketEvents", "EventBy", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.UserSettings", "UserId", c => c.String(nullable: false, maxLength: 256));
            AddPrimaryKey("dbo.UserSettings", "UserId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.UserSettings");
            AlterColumn("dbo.UserSettings", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TicketEvents", "EventBy", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Tickets", "LastUpdateBy", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Tickets", "CurrentStatusSetBy", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Tickets", "AssignedTo", c => c.String(maxLength: 100));
            AlterColumn("dbo.Tickets", "Owner", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Tickets", "CreatedBy", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.TicketAttachments", "UploadedBy", c => c.String(nullable: false, maxLength: 100));
            AddPrimaryKey("dbo.UserSettings", "UserId");
        }
    }
}
