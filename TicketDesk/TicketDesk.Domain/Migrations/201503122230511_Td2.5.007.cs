namespace TicketDesk.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Td25007 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TicketAttachments", "TicketId", "dbo.Tickets");
            DropIndex("dbo.TicketAttachments", new[] { "TicketId" });
            CreateTable(
                "dbo.TicketSubscribers",
                c => new
                    {
                        TicketId = c.Int(nullable: false),
                        Subscriber = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => new { t.TicketId, t.Subscriber })
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .Index(t => t.TicketId);
            
            DropTable("dbo.TicketAttachments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TicketAttachments",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        TicketId = c.Int(),
                        FileName = c.String(nullable: false, maxLength: 255),
                        FileSize = c.Int(nullable: false),
                        FileType = c.String(nullable: false, maxLength: 250),
                        UploadedBy = c.String(nullable: false, maxLength: 256),
                        UploadedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        FileContents = c.Binary(nullable: false),
                        FileDescription = c.String(maxLength: 500),
                        IsPending = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FileId);
            
            DropForeignKey("dbo.TicketSubscribers", "TicketId", "dbo.Tickets");
            DropIndex("dbo.TicketSubscribers", new[] { "TicketId" });
            DropTable("dbo.TicketSubscribers");
            CreateIndex("dbo.TicketAttachments", "TicketId");
            AddForeignKey("dbo.TicketAttachments", "TicketId", "dbo.Tickets", "TicketId");
        }
    }
}
