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

using System.Data.Entity.Migrations;
namespace TicketDesk.Domain.Legacy.Migrations
{


	public partial class InitialCreate : DbMigration
	{
		public override void Up()
		{
			DropTable("ELMAH_Error");
			Sql("DROP PROCEDURE [dbo].[ELMAH_GetErrorsXml]");
			Sql("DROP PROCEDURE [dbo].[ELMAH_GetErrorXml]");
			Sql("DROP PROCEDURE [dbo].[ELMAH_LogError]");
			RenameColumn("dbo.Tickets", "Type", "TicketType");

			Sql(@"
				ALTER TABLE [dbo].[Tickets] DROP CONSTRAINT [DF_Tickets_CreatedDate];

				ALTER TABLE [dbo].[Tickets] ALTER COLUMN [CreatedDate] DATETIMEOFFSET (7) NOT NULL;
				ALTER TABLE [dbo].[Tickets] ALTER COLUMN [CurrentStatusDate] DATETIMEOFFSET (7) NOT NULL;
				ALTER TABLE [dbo].[Tickets] ALTER COLUMN [LastUpdateDate] DATETIMEOFFSET (7) NOT NULL;

				ALTER TABLE [dbo].[Tickets] ADD CONSTRAINT [DF_Tickets_CreatedDate] DEFAULT (sysdatetimeoffset()) FOR [CreatedDate];

				ALTER TABLE [dbo].[TicketComments] DROP CONSTRAINT [DF_TicketComments_CommentDate];

				ALTER TABLE [dbo].[TicketComments] ALTER COLUMN [CommentedDate] DATETIMEOFFSET (7) NOT NULL;

				ALTER TABLE [dbo].[TicketComments] ADD CONSTRAINT [DF_TicketComments_CommentDate] DEFAULT (sysdatetimeoffset()) FOR [CommentedDate];
				ALTER TABLE [dbo].[TicketAttachments] ALTER COLUMN [UploadedDate] DATETIMEOFFSET (7) NOT NULL;

				ALTER TABLE [dbo].[TicketEventNotifications] ALTER COLUMN [CreatedDate] DATETIMEOFFSET (7) NOT NULL;
				ALTER TABLE [dbo].[TicketEventNotifications] ALTER COLUMN [LastDeliveryAttemptDate] DATETIMEOFFSET (7) NULL;
				ALTER TABLE [dbo].[TicketEventNotifications] ALTER COLUMN [NextDeliveryAttemptDate] DATETIMEOFFSET (7) NULL;


			");

			AddColumn("dbo.Tickets", "TicketStatus", c => c.Int(nullable: false));
			Sql("Update dbo.Tickets set TicketStatus = 0 where CurrentStatus = 'Active'");
			Sql("Update dbo.Tickets set TicketStatus = 1 where CurrentStatus = 'More Info'");
			Sql("Update dbo.Tickets set TicketStatus = 2 where CurrentStatus = 'Resolved'");
			Sql("Update dbo.Tickets set TicketStatus = 3 where CurrentStatus = 'Closed'");

			DropColumn("dbo.Tickets", "CurrentStatus");
			Sql(@"UPDATE [dbo].[Settings] SET [SettingValue] = '2.5.0' WHERE [SettingName] = 'Version'");


			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'AdUserPropertiesSqlCacheRefreshMinutes'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'BlindCopyToEmailAddress'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'CleanupPendingAttachmentsAfterHours'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'CreateSqlMembershipRegistrationsAsSubmitters'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'EmailDeliveryTimerIntervalMinutes'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'EmailMaxConsolidationWaitMinutes'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'EmailMaxDeliveryAttempts'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'EmailNotificationInitialDelayMinutes'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'EmailResendDelayMinutes'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'EmailServiceName'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'EnableEmailNotifications'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'EnableOutlookFriendlyHtmlEmail'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'FromEmailAddress'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'FromEmailDisplayName'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'IsDemo'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'KillAllProfilesOnStartup'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'LuceneDirectory'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'RefreshSecurityCacheMinutes'");
			Sql(@"DELETE [dbo].[Settings] WHERE [SettingName] = 'SiteRootUrlForEmail'");


		}

		public override void Down()
		{
			Sql(@"
				/****** Object:  Table [dbo].[ELMAH_Error]    Script Date: 12/05/2010 18:31:00 ******/
				SET ANSI_NULLS ON

				SET QUOTED_IDENTIFIER ON

				IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ELMAH_Error]') AND type in (N'U'))
				BEGIN
				CREATE TABLE [dbo].[ELMAH_Error](
					[ErrorId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ELMAH_Error_ErrorId]  DEFAULT (newid()),
					[Application] [nvarchar](60) NOT NULL,
					[Host] [nvarchar](50) NOT NULL,
					[Type] [nvarchar](100) NOT NULL,
					[Source] [nvarchar](60) NOT NULL,
					[Message] [nvarchar](500) NOT NULL,
					[User] [nvarchar](50) NOT NULL,
					[StatusCode] [int] NOT NULL,
					[TimeUtc] [datetime] NOT NULL,
					[Sequence] [int] IDENTITY(1,1) NOT NULL,
					[AllXml] [ntext] NOT NULL,
				 CONSTRAINT [PK_ELMAH_Error] PRIMARY KEY NONCLUSTERED 
				(
					[ErrorId] ASC
				)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
				) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
				END

				/****** Object:  StoredProcedure [dbo].[ELMAH_LogError]    Script Date: 12/05/2010 18:31:01 ******/
				SET ANSI_NULLS ON

				SET QUOTED_IDENTIFIER ON

				IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ELMAH_LogError]') AND type in (N'P', N'PC'))
				BEGIN
				EXEC dbo.sp_executesql @statement = N'
				CREATE PROCEDURE [dbo].[ELMAH_LogError]
				(
					@ErrorId UNIQUEIDENTIFIER,
					@Application NVARCHAR(60),
					@Host NVARCHAR(30),
					@Type NVARCHAR(100),
					@Source NVARCHAR(60),
					@Message NVARCHAR(500),
					@User NVARCHAR(50),
					@AllXml NTEXT,
					@StatusCode INT,
					@TimeUtc DATETIME
				)
				AS

					SET NOCOUNT ON

					INSERT
					INTO
						[ELMAH_Error]
						(
							[ErrorId],
							[Application],
							[Host],
							[Type],
							[Source],
							[Message],
							[User],
							[AllXml],
							[StatusCode],
							[TimeUtc]
						)
					VALUES
						(
							@ErrorId,
							@Application,
							@Host,
							@Type,
							@Source,
							@Message,
							@User,
							@AllXml,
							@StatusCode,
							@TimeUtc
						)

				' 
				END

				/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorXml]    Script Date: 12/05/2010 18:31:01 ******/
				SET ANSI_NULLS ON

				SET QUOTED_IDENTIFIER ON

				IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ELMAH_GetErrorXml]') AND type in (N'P', N'PC'))
				BEGIN
				EXEC dbo.sp_executesql @statement = N'
				CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml]
				(
					@Application NVARCHAR(60),
					@ErrorId UNIQUEIDENTIFIER
				)
				AS

					SET NOCOUNT ON

					SELECT 
						[AllXml]
					FROM 
						[ELMAH_Error]
					WHERE
						[ErrorId] = @ErrorId
					AND
						[Application] = @Application

				' 
				END

				/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorsXml]    Script Date: 12/05/2010 18:31:01 ******/
				SET ANSI_NULLS ON

				SET QUOTED_IDENTIFIER ON

				IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ELMAH_GetErrorsXml]') AND type in (N'P', N'PC'))
				BEGIN
				EXEC dbo.sp_executesql @statement = N'
				CREATE PROCEDURE [dbo].[ELMAH_GetErrorsXml]
				(
					@Application NVARCHAR(60),
					@PageIndex INT = 0,
					@PageSize INT = 15,
					@TotalCount INT OUTPUT
				)
				AS 

					SET NOCOUNT ON

					DECLARE @FirstTimeUTC DATETIME
					DECLARE @FirstSequence INT
					DECLARE @StartRow INT
					DECLARE @StartRowIndex INT

					SELECT 
						@TotalCount = COUNT(1) 
					FROM 
						[ELMAH_Error]
					WHERE 
						[Application] = @Application

					-- Get the ID of the first error for the requested page

					SET @StartRowIndex = @PageIndex * @PageSize + 1

					IF @StartRowIndex <= @TotalCount
					BEGIN

						SET ROWCOUNT @StartRowIndex

						SELECT  
							@FirstTimeUTC = [TimeUtc],
							@FirstSequence = [Sequence]
						FROM 
							[ELMAH_Error]
						WHERE   
							[Application] = @Application
						ORDER BY 
							[TimeUtc] DESC, 
							[Sequence] DESC

					END
					ELSE
					BEGIN

						SET @PageSize = 0

					END

					-- Now set the row count to the requested page size and get
					-- all records below it for the pertaining application.

					SET ROWCOUNT @PageSize

					SELECT 
						errorId     = [ErrorId], 
						application = [Application],
						host        = [Host], 
						type        = [Type],
						source      = [Source],
						message     = [Message],
						[user]      = [User],
						statusCode  = [StatusCode], 
						time        = CONVERT(VARCHAR(50), [TimeUtc], 126) + ''Z''
					FROM 
						[ELMAH_Error] error
					WHERE
						[Application] = @Application
					AND
						[TimeUtc] <= @FirstTimeUTC
					AND 
						[Sequence] <= @FirstSequence
					ORDER BY
						[TimeUtc] DESC, 
						[Sequence] DESC
					FOR
						XML AUTO

				' 
				END

			");
			RenameColumn("dbo.Tickets", "TicketType", "Type");

			Sql(@"
				ALTER TABLE [dbo].[Tickets] DROP CONSTRAINT [DF_Tickets_CreatedDate];

				ALTER TABLE [dbo].[Tickets] ALTER COLUMN [CreatedDate] DATETIME NOT NULL;
				ALTER TABLE [dbo].[Tickets] ALTER COLUMN [CurrentStatusDate] DATETIME NOT NULL;
				ALTER TABLE [dbo].[Tickets] ALTER COLUMN [LastUpdateDate] DATETIME NOT NULL;

				ALTER TABLE [dbo].[Tickets] ADD CONSTRAINT [DF_Tickets_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

				ALTER TABLE [dbo].[TicketComments] DROP CONSTRAINT [DF_TicketComments_CommentDate];

				ALTER TABLE [dbo].[TicketComments] ALTER COLUMN [CommentedDate] DATETIME NOT NULL;

				ALTER TABLE [dbo].[TicketComments] ADD CONSTRAINT [DF_TicketComments_CommentDate] DEFAULT (getdate()) FOR [CommentedDate];
				ALTER TABLE [dbo].[TicketAttachments] ALTER COLUMN [UploadedDate] DATETIME NOT NULL;

				ALTER TABLE [dbo].[TicketEventNotifications] ALTER COLUMN [CreatedDate] DATETIME NOT NULL;
				ALTER TABLE [dbo].[TicketEventNotifications] ALTER COLUMN [LastDeliveryAttemptDate] DATETIME NULL;
				ALTER TABLE [dbo].[TicketEventNotifications] ALTER COLUMN [NextDeliveryAttemptDate] DATETIME NULL;

				
			");

			AddColumn("dbo.Tickets", "TicketStatus", c => c.String(nullable: false, maxLength: 50));

			Sql("Update dbo.Tickets set TicketStatus = 'Active' where TicketStatus = 0 ");
			Sql("Update dbo.Tickets set TicketStatus = 'More Info' where TicketStatus = 1 ");
			Sql("Update dbo.Tickets set TicketStatus = 'Resolved' where TicketStatus = 2 ");
			Sql("Update dbo.Tickets set TicketStatus = 'Closed' where TicketStatus = 3 ");

			DropColumn("dbo.Tickets", "TicketStatus");

			Sql(@"UPDATE [dbo].[Settings] SET [SettingValue] = '2.0.2' WHERE [SettingName] = 'Version'");
			Sql(@"UPDATE [dbo].[Settings] SET [SettingName] = 'AllowInternalUsersToEditPriority' WHERE [SettingName] = 'AllowSubmitterRoleToEditPriority'");
			Sql(@"UPDATE [dbo].[Settings] SET [SettingName] = 'AllowInternalUsersToEditTags' WHERE [SettingName] = 'AllowSubmitterRoleToEditTags'");


			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'AdUserPropertiesSqlCacheRefreshMinutes', N'120', N'120', N'IntString', N'Used only in AD environments; sets the amount of time the system will wait before updating the user properties in the SQL cache.\n\nThese values are less critical than other values cached from AD (such as the list of group members), and so these properties can be refreshed less frequently. This value should be the same or higher than the Refresh Security Cache Minutes setting. ')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'BlindCopyToEmailAddress', N'', N'', N'SimpleString', N'An email address that should be blind copied (BCC) on all email notifiacitons.\n\nUsually this is left blank, but can be useful as a diagnostic tool to test the notificiations system.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'CleanupPendingAttachmentsAfterHours', N'1', N'1', N'IntString', N'This setting controls how long the system will leave pending attachments in the database before purging them.\n\nWhen users first upload an attachment to a ticket, the attachment is in an uncommitted state. This gives the user time to finish writing comments, adding more files, filling in other fields, etc. If for some reason the user does not finish submitting/updating the ticket, these attachments remain on the server for a while (in case they come back).')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'CreateSqlMembershipRegistrationsAsSubmitters', N'true', N'true', N'BoolString', N'If true new users that register will automatically be added to the submitters role.\n\nIf your system is exposed to the public, and you do NOT intend for the general public to be able to use your system, you should set this to false and require that an admin manually grant new users permissions.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EmailDeliveryTimerIntervalMinutes', N'3', N'3', N'IntString', N'Determines how often TicketDesk will fire off the background process that examines the email notifications queue.\n\nGenerally 1 - 5 minutes is appropriate for most environments, but you can change this if you want.\n\nYou should not set this to a value lower than the Email Notification Initial Delay Minutes setting.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EmailMaxConsolidationWaitMinutes', N'12', N'12', N'DoubleString', N'This setting controls the max amount of time that the system will continue waiting on additional changes to a ticket before going ahead and sending a notifiaciton email.\n\nIf changes continue to occur to a ticket within the wait-period, the system will continue to wait before sending the email until either the changes slow down or the limit set by this setting is reached. \n\nThe value here can contain a decimal. This value should be larger or the same as the Email Notification Initial Delay Minutes setting.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EmailMaxDeliveryAttempts', N'5', N'5', N'IntString', N'How many times the system will attempt to deliver an email notifications before giving up.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EmailNotificationInitialDelayMinutes', N'6', N'6', N'DoubleString', N'This controls the minimum amount of time after a notification for a ticket change has been queued before the system will send the notification.\n\nIf additional changes to a ticket occurs before this time is reached, TicketDesk will consolidate the multiple notifications into a single message rather than spamming the user with multiple messages about each change. Each time a new change occurs, the amount of time to wait is incremented again by this same value.\n\nThe value here can contain a decimal. The value should never be lower than the Email Delivery Timer Interval Minutes setting (they can be the same), and should also not be shorter than the Email Max Consolidation Wait Minutes setting (again, they can be the same).')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EmailResendDelayMinutes', N'5', N'5', N'IntString', N'This setting detemines how long TicketDesk will wait if there is an error when trying to send an email notification. Each time the notification failes, the wiat time will be the number of attempts times the value of this setting: Example, if the setting is 5 and message fails the third attempt, it will wait 15 minutes before trying again.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EmailServiceName', N'DefaultEmailHandler', N'DefaultEmailHandler', N'SimpleString', N'Change this setting only if you have created a custom MEF module to handle email delivery! This should be the name of the MEF Export Contract Name of your custom email module. If you don''t know what that means, then DO NOT TOUCH THIS SETTING!')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EnableEmailNotifications', N'true', N'true', N'BoolString', N'This setting controls if the email notifications system is enabled or not.\n\nIf set to false, the system will still queue up notifications, but it will never send them out to the users.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'EnableOutlookFriendlyHtmlEmail', N'false', N'false', N'BoolString', N'Determines if TicketDesk will use the email template that is friendly to outlook email clients when generating the HTML body of an email.\n\nTicketDesk normally generates both an HTML and a Plain text body for emails. The HTML is rendered using real CSS that is supported by the vast majority of browsers and email clients. However, Microsoft''s Outlook (up to 2010) still use Microsoft Word to redner emails, instead of Internet Explorer. Word''s HTML capabilities are very limisted and are not W3C standards based.\n\nIf the majority of your users use Outlook, you should enable this setting so they their emails render well in outlook.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'FromEmailAddress', N'ticketdesk@nowhere.com', N'ticketdesk@nowhere.com', N'SimpleString', N'The email address to use in the TO field of email notifiacitons.\n\nUsually this is not a real person''s email address, just a made up address at your organization.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'FromEmailDisplayName', N'TicketDesk', N'TicketDesk', N'SimpleString', N'The friendly name to use in the TO field of email notifiacitons.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'IsDemo', N'false', N'false', N'BoolString', N'This setting indicates that TicketDesk is running in demo mode.\n\nCurrently the only thing this affects are attachements. In Demo mode, the attachments system will not store the file contents that are uploaded (to keep people from using your demo site as an ad-hoc warze server).')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'KillAllProfilesOnStartup', N'false', N'false', N'BoolString', N'Setting this to true will cause the system to delete stored profile settings for ALL users.\n\nThis is sometimes useful after upgrading TicketDesk to a new version to reset user''s TicketCenter List Settings. The system will auto-regenerate a default set of profile values.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'LuceneDirectory', N'~/TdSearchIndex', N'~/TdSearchIndex', N'SimpleString', N'This is the relative path to the directory where TicketDesk should store the full-text indexes used by the search feature.\n\nYou can use the text ram (case sensitive) to tell Lucene.net to use a purely in-memory index, but this is not recommended for your production sites. The asp.net user account will need read/write access to this folder location.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'RefreshSecurityCacheMinutes', N'30', N'30', N'IntString', N'Used only in AD environments (for right now); sets the amount of time the system will wait before a background process attempts to rebuild the SQL cached values retrieved from AD.\n\nThe system will always rebuild the cache when it first starts up.')");
			Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'SiteRootUrlForEmail', N'http://localhost:2534', N'http://localhost:2534', N'SimpleString', N'The root URL of the web site; will be used in email notifications to create the fully qualified link URLs. Without a valid setting, users clicking links in their notification email will not be directed to the TicketDesk site.')");

		}
	}
}
