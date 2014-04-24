namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdCachedRoleMembers",
                c => new
                    {
                        GroupName = c.String(nullable: false, maxLength: 150),
                        MemberName = c.String(nullable: false, maxLength: 150),
                        MemberDisplayName = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => new { t.GroupName, t.MemberName });
            
            CreateTable(
                "dbo.AdCachedUserProperties",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 150),
                        PropertyName = c.String(nullable: false, maxLength: 100),
                        PropertyValue = c.String(maxLength: 250),
                        LastRefreshed = c.DateTimeOffset(precision: 7),
                        IsActiveInAd = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserName, t.PropertyName });
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
                        PublishedToKb = c.Boolean(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
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
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
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
                .ForeignKey("dbo.TicketComments", t => new { t.TicketId, t.CommentId }, cascadeDelete: true)
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
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(nullable: false, maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketTags", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.TicketEventNotifications", new[] { "TicketId", "CommentId" }, "dbo.TicketComments");
            DropForeignKey("dbo.TicketComments", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.TicketAttachments", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.TicketTags", new[] { "TicketId" });
            DropIndex("dbo.TicketEventNotifications", new[] { "TicketId", "CommentId" });
            DropIndex("dbo.TicketComments", new[] { "TicketId" });
            DropIndex("dbo.TicketAttachments", new[] { "TicketId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TicketTags");
            DropTable("dbo.TicketEventNotifications");
            DropTable("dbo.TicketComments");
            DropTable("dbo.Tickets");
            DropTable("dbo.TicketAttachments");
            DropTable("dbo.Settings");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AdCachedUserProperties");
            DropTable("dbo.AdCachedRoleMembers");
        }
    }
}
