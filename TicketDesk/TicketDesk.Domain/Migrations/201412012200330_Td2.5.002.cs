// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

namespace TicketDesk.Domain.Migrations
{
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
