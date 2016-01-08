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

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        SettingName = c.String(nullable: false, maxLength: 50),
                        SettingValue = c.String(),
                        DefaultValue = c.String(),
                        SettingType = c.String(nullable: false, maxLength: 50),
                        SettingDescription = c.String(),
                    })
                .PrimaryKey(t => t.SettingName);

            CreateTable(
                "dbo.TicketAttachments",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        TicketId = c.Int(),
                        FileName = c.String(nullable: false, maxLength: 255),
                        FileSize = c.Int(nullable: false),
                        FileType = c.String(nullable: false, maxLength: 250),
                        UploadedBy = c.String(nullable: false, maxLength: 100),
                        UploadedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        FileContents = c.Binary(nullable: false),
                        FileDescription = c.String(maxLength: 500),
                        IsPending = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.Tickets", t => t.TicketId)
                .Index(t => t.TicketId);

            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketId = c.Int(nullable: false, identity: true),
                        TicketType = c.String(nullable: false, maxLength: 50),
                        Category = c.String(nullable: false, maxLength: 50),
                        Title = c.String(nullable: false, maxLength: 500),
                        Details = c.String(nullable: false, storeType: "ntext"),
                        IsHtml = c.Boolean(nullable: false),
                        TagList = c.String(maxLength: 100),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "SYSDATETIMEOFFSET()"),
                        Owner = c.String(nullable: false, maxLength: 100),
                        AssignedTo = c.String(maxLength: 100),
                        TicketStatus = c.Int(nullable: false),
                        CurrentStatusDate = c.DateTimeOffset(nullable: false, precision: 7),
                        CurrentStatusSetBy = c.String(nullable: false, maxLength: 100),
                        LastUpdateBy = c.String(nullable: false, maxLength: 100),
                        LastUpdateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Priority = c.String(maxLength: 25),
                        AffectsCustomer = c.Boolean(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                    })
                .PrimaryKey(t => t.TicketId);

            CreateTable(
                "dbo.TicketComments",
                c => new
                    {
                        TicketId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false, identity: true),
                        CommentEvent = c.String(maxLength: 500),
                        Comment = c.String(storeType: "ntext"),
                        IsHtml = c.Boolean(nullable: false),
                        CommentedBy = c.String(nullable: false, maxLength: 100),
                        CommentedDate = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "SYSDATETIMEOFFSET()"),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                    })
                .PrimaryKey(t => new { t.TicketId, t.CommentId })
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .Index(t => t.TicketId);

            CreateTable(
                "dbo.TicketEventNotifications",
                c => new
                    {
                        TicketId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                        NotifyUser = c.String(nullable: false, maxLength: 100),
                        NotifyUserDisplayName = c.String(nullable: false, maxLength: 100),
                        NotifyEmail = c.String(nullable: false, maxLength: 255),
                        NotifyUserReason = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        DeliveryAttempts = c.Int(nullable: false),
                        LastDeliveryAttemptDate = c.DateTimeOffset(precision: 7),
                        Status = c.String(nullable: false, maxLength: 20),
                        NextDeliveryAttemptDate = c.DateTimeOffset(precision: 7),
                        EventGeneratedByUser = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => new { t.TicketId, t.CommentId, t.NotifyUser })
                .ForeignKey("dbo.TicketComments", t => new { t.TicketId, t.CommentId })
                .Index(t => new { t.TicketId, t.CommentId });

            CreateTable(
                "dbo.TicketTags",
                c => new
                    {
                        TicketTagId = c.Int(nullable: false, identity: true),
                        TagName = c.String(nullable: false, maxLength: 100),
                        TicketId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TicketTagId)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .Index(t => t.TicketId);
            Sql(@"INSERT [dbo].[Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'Version', N'2.5.0',N'2.5.0', N'SimpleString',N'The version of the TicketDesk database. CHANGE AT YOUR OWN RISK!')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'AllowInternalUsersToEditPriority', N'false', N'false', N'BoolString', N'If true submitters can set the priority field either during ticket creation, or in the ticket editor. Setting this to false reserves the priority field for help desk staff use only, though priorities will still be visible to submitters once set by the staff. \n\nIn all cases, the priority is an optional field.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'AllowInternalUsersToEditTags', N'true', N'true', N'BoolString', N'If true submitters can set tags during ticket creation and in the ticket editor. Setting this to false reserves the tags for help desk staff use only, though tags will still be visible to submitters once set by the staff.\n\nTagging is encouraged as it assists in later ticket searches.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'CategoryList', N'Hardware,Software,Network', N'Hardware,Software,Network', N'StringList', N'This is the list of possible selections for the Category dropdown list.\n\nIs is advised that your use generic categories. The recommended rule-of-thumb is that there should be one option that fits any possible ticket a user might create, and there should NOT be a value such as other, N/A, or unknown. Keeping the values general in nature increases the odds that users will pick a meaningful value.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'PriorityList', N'High,Low,Medium', N'High,Low,Medium', N'StringList', N'This is the list of possible selections for the Priority dropdown list.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'TicketTypesList', N'Question,Problem,Request', N'Question,Problem,Request', N'StringList', N'This is the list of possible selections for the Ticket Type dropdown list. The type of ticket is usually the kind of issue the user is submitting.\n\nIs is advised that your use generic types. The recommended rule-of-thumb is that there should be one option that fits any possible ticket a user might create, and there should NOT be a value such as other, N/A, or unknown. Keeping the values general in nature increases the odds that users will pick a meaningful value.')");

        }

        public override void Down()
        {
            DropForeignKey("dbo.TicketTags", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.TicketEventNotifications", new[] { "TicketId", "CommentId" }, "dbo.TicketComments");
            DropForeignKey("dbo.TicketComments", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.TicketAttachments", "TicketId", "dbo.Tickets");
            DropIndex("dbo.TicketTags", new[] { "TicketId" });
            DropIndex("dbo.TicketEventNotifications", new[] { "TicketId", "CommentId" });
            DropIndex("dbo.TicketComments", new[] { "TicketId" });
            DropIndex("dbo.TicketAttachments", new[] { "TicketId" });
            DropTable("dbo.TicketTags");
            DropTable("dbo.TicketEventNotifications");
            DropTable("dbo.TicketComments");
            DropTable("dbo.Tickets");
            DropTable("dbo.TicketAttachments");
            DropTable("dbo.Settings");
        }
    }
}
