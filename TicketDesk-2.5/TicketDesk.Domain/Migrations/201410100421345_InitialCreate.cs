namespace TicketDesk.Domain.Migrations
{
    using System;
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
            Sql(@"INSERT Settings( SettingName, SettingValue, DefaultValue, SettingType, SettingDescription) 
                          VALUES(
                                    'HideHomePage',
                                    'false',
                                    'false',
                                    'BoolString',
                                    'Hide the home tab from the main menu and makes TicketCenter default home page'
                                )"
                );
            Sql(@"INSERT Settings( SettingName, SettingValue, DefaultValue, SettingType, SettingDescription) 
                          VALUES(
                                    'HelpDeskBroadcastNotificationsEnabled',
                                    'true',
                                    'true',
                                    'BoolString',
                                    'Send broadcast notifications to helpdesk for all new tickets'
                                )"
                );
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'AdUserPropertiesSqlCacheRefreshMinutes', N'120', N'120', N'IntString', N'Used only in AD environments; sets the amount of time the system will wait before updating the user properties in the SQL cache.\n\nThese values are less critical than other values cached from AD (such as the list of group members), and so these properties can be refreshed less frequently. This value should be the same or higher than the Refresh Security Cache Minutes setting. ')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'AllowSubmitterRoleToEditPriority', N'false', N'false', N'BoolString', N'If true submitters can set the priority field either during ticket creation, or in the ticket editor. Setting this to false reserves the priority field for help desk staff use only, though priorities will still be visible to submitters once set by the staff. \n\nIn all cases, the priority is an optional field.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'AllowSubmitterRoleToEditTags', N'true', N'true', N'BoolString', N'If true submitters can set tags during ticket creation and in the ticket editor. Setting this to false reserves the tags for help desk staff use only, though tags will still be visible to submitters once set by the staff.\n\nTagging is encouraged as it assists in later ticket searches.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'BlindCopyToEmailAddress', N'', N'', N'SimpleString', N'An email address that should be blind copied (BCC) on all email notifiacitons.\n\nUsually this is left blank, but can be useful as a diagnostic tool to test the notificiations system.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'CategoryList', N'Hardware,Software,Network', N'Hardware,Software,Network', N'StringList', N'This is the list of possible selections for the Category dropdown list.\n\nIs is advised that your use generic categories. The recommended rule-of-thumb is that there should be one option that fits any possible ticket a user might create, and there should NOT be a value such as other, N/A, or unknown. Keeping the values general in nature increases the odds that users will pick a meaningful value.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'CleanupPendingAttachmentsAfterHours', N'1', N'1', N'IntString', N'This setting controls how long the system will leave pending attachments in the database before purging them.\n\nWhen users first upload an attachment to a ticket, the attachment is in an uncommitted state. This gives the user time to finish writing comments, adding more files, filling in other fields, etc. If for some reason the user does not finish submitting/updating the ticket, these attachments remain on the server for a while (in case they come back).')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'CreateSqlMembershipRegistrationsAsSubmitters', N'true', N'true', N'BoolString', N'If true new users that register will automatically be added to the submitters role.\n\nIf your system is exposed to the public, and you do NOT intend for the general public to be able to use your system, you should set this to false and require that an admin manually grant new users permissions.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EmailDeliveryTimerIntervalMinutes', N'3', N'3', N'IntString', N'Determines how often TicketDesk will fire off the background process that examines the email notifications queue.\n\nGenerally 1 - 5 minutes is appropriate for most environments, but you can change this if you want.\n\nYou should not set this to a value lower than the Email Notification Initial Delay Minutes setting.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EmailMaxConsolidationWaitMinutes', N'12', N'12', N'DoubleString', N'This setting controls the max amount of time that the system will continue waiting on additional changes to a ticket before going ahead and sending a notifiaciton email.\n\nIf changes continue to occur to a ticket within the wait-period, the system will continue to wait before sending the email until either the changes slow down or the limit set by this setting is reached. \n\nThe value here can contain a decimal. This value should be larger or the same as the Email Notification Initial Delay Minutes setting.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EmailMaxDeliveryAttempts', N'5', N'5', N'IntString', N'How many times the system will attempt to deliver an email notifications before giving up.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EmailNotificationInitialDelayMinutes', N'6', N'6', N'DoubleString', N'This controls the minimum amount of time after a notification for a ticket change has been queued before the system will send the notification.\n\nIf additional changes to a ticket occurs before this time is reached, TicketDesk will consolidate the multiple notifications into a single message rather than spamming the user with multiple messages about each change. Each time a new change occurs, the amount of time to wait is incremented again by this same value.\n\nThe value here can contain a decimal. The value should never be lower than the Email Delivery Timer Interval Minutes setting (they can be the same), and should also not be shorter than the Email Max Consolidation Wait Minutes setting (again, they can be the same).')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EmailResendDelayMinutes', N'5', N'5', N'IntString', N'This setting detemines how long TicketDesk will wait if there is an error when trying to send an email notification. Each time the notification failes, the wiat time will be the number of attempts times the value of this setting: Example, if the setting is 5 and message fails the third attempt, it will wait 15 minutes before trying again.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EmailServiceName', N'DefaultEmailHandler', N'DefaultEmailHandler', N'SimpleString', N'Change this setting only if you have created a custom MEF module to handle email delivery! This should be the name of the MEF Export Contract Name of your custom email module. If you don''t know what that means, then DO NOT TOUCH THIS SETTING!')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EnableEmailNotifications', N'true', N'true', N'BoolString', N'This setting controls if the email notifications system is enabled or not.\n\nIf set to false, the system will still queue up notificaitons, but it will never send them out to the users.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EnableOutlookFriendlyHtmlEmail', N'false', N'false', N'BoolString', N'Determines if TicketDesk will use the email template that is friendly to outlook email clients when generating the HTML body of an email.\n\nTicketDesk normally generates both an HTML and a Plain text body for emails. The HTML is rendered using real CSS that is supported by the vast majority of browsers and email clients. However, Microsoft''s Outlook (up to 2010) still use Microsoft Word to redner emails, instead of Internet Explorer. Word''s HTML capabilities are very limisted and are not W3C standards based.\n\nIf the majority of your users use Outlook, you should enable this setting so they their emails render well in outlook.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'FromEmailAddress', N'ticketdesk@nowhere.com', N'ticketdesk@nowhere.com', N'SimpleString', N'The email address to use in the TO field of email notifiacitons.\n\nUsually this is not a real person''s email address, just a made up address at your organization.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'FromEmailDisplayName', N'TicketDesk', N'TicketDesk', N'SimpleString', N'The friendly name to use in the TO field of email notifiacitons.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'IsDemo', N'false', N'false', N'BoolString', N'This setting indicates that TicketDesk is running in demo mode.\n\nCurrently the only thing this affects are attachements. In Demo mode, the attachments system will not store the file contents that are uploaded (to keep people from using your demo site as an ad-hoc warze server).')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'KillAllProfilesOnStartup', N'false', N'false', N'BoolString', N'Setting this to true will cause the system to delete stored profile settings for ALL users.\n\nThis is sometimes useful after upgrading TicketDesk to a new version to reset user''s TicketCenter List Settings. The system will auto-regenerate a default set of profile values.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'LuceneDirectory', N'~/TdSearchIndex', N'~/TdSearchIndex', N'SimpleString', N'This is the relative path to the directory where TicketDesk should store the full-text indexes used by the search feature.\n\nYou can use the text ram (case sensitive) to tell Lucene.net to use a purely in-memory index, but this is not recommended for your production sites. The asp.net user account will need read/write access to this folder location.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'PriorityList', N'High,Low,Medium', N'High,Low,Medium', N'StringList', N'This is the list of possible selections for the Priority dropdown list.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'RefreshSecurityCacheMinutes', N'30', N'30', N'IntString', N'Used only in AD environments (for right now); sets the amount of time the system will wait before a background process attempts to rebuild the SQL cached values retrieved from AD.\n\nThe system will always rebuild the cache when it first starts up.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'SiteRootUrlForEmail', N'http://localhost:2534', N'http://localhost:2534', N'SimpleString', N'The root URL of the web site; will be used in email notifications to create the fully qualified link URLs. Without a valid setting, users clicking links in their notification email will not be directed to the TicketDesk site.')");
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
