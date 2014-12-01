namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Td25002 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TicketComments", newName: "TicketEvents");
            RenameColumn("dbo.TicketEvents", "CommentId", "EventId");
            RenameColumn("dbo.TicketEvents", "CommentEvent", "EventDescription");
            RenameColumn("dbo.TicketEvents", "CommentedBy", "EventBy");
            RenameColumn("dbo.TicketEvents", "CommentedDate", "EventDate");
        }

        public override void Down()
        {
            RenameColumn("dbo.TicketEvents", "EventId", "CommentId");
            RenameColumn("dbo.TicketEvents", "EventDescription", "CommentEvent");
            RenameColumn("dbo.TicketEvents", "EventBy", "CommentedBy");
            RenameColumn("dbo.TicketEvents", "EventDate", "CommentedDate");
            RenameTable(name: "dbo.TicketEvents", newName: "TicketComments");
        }
    }
}
