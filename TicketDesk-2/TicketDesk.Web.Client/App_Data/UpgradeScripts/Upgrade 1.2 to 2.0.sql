alter table TicketAttachments ADD IsPending bit not null CONSTRAINT TicketAttachments_IsPending DEFAULT 0 WITH VALUES 
go
alter table TicketAttachments  DROP CONSTRAINT PK_TicketAttachments 
go
alter table TicketAttachments ALTER COLUMN TicketId int null
go
alter table TicketAttachments ADD CONSTRAINT PK_TicketAttachments PRIMARY KEY CLUSTERED (FileID)
go
alter table TicketAttachments DROP CONSTRAINT [FK_TicketAttachments_Tickets]
go
ALTER TABLE [TicketAttachments]  WITH NOCHECK ADD CONSTRAINT [FK_TicketAttachments_Tickets] FOREIGN KEY([TicketId])
REFERENCES [Tickets] ([TicketId])
go

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Settings
	(
	SettingName nvarchar(50) NOT NULL,
	SettingValue nvarchar(MAX) NULL,
	DefaultValue nvarchar(MAX) NULL,
	SettingType nvarchar(50) NOT NULL,
	SettingDescription nvarchar(MAX) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Settings SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Settings ADD CONSTRAINT
	DF_Settings_SettingType DEFAULT N'SimpleString' FOR SettingType
GO
IF EXISTS(SELECT * FROM dbo.Settings)
	 EXEC('INSERT INTO dbo.Tmp_Settings (SettingName, SettingValue)
		SELECT SettingName, SettingValue FROM dbo.Settings WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.Settings
GO
EXECUTE sp_rename N'dbo.Tmp_Settings', N'Settings', 'OBJECT' 
GO
ALTER TABLE dbo.Settings ADD CONSTRAINT
	PK_Settings PRIMARY KEY CLUSTERED 
	(
	SettingName
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

UPDATE Settings 
SET 
	
	DefaultValue = 'Hardware,Software,Network',
	SettingType = 'StringList',
	SettingDescription = 'This is the list of possible selections for the Category dropdown list.\n\nIs is advised that your use generic categories. The recommended rule-of-thumb is that there should be one option that fits any possible ticket a user might create, and there should NOT be a value such as "other", "N/A", or "unknown". Keeping the values general in nature increases the odds that users will pick a meaningful value.'
WHERE
	SettingName = 'CategoryList'
GO
UPDATE Settings 
SET 
	DefaultValue = 'High,Low,Medium',
	SettingType = 'StringList',
	SettingDescription = 'This is the list of possible selections for the Priority dropdown list.'
WHERE
	SettingName = 'PriorityList'
GO
UPDATE Settings 
SET 
	DefaultValue = 'Question,Problem,Request',
	SettingType = 'StringList',
	SettingDescription = 'This is the list of possible selections for the Ticket Type dropdown list. The type of ticket is usually the "kind" of issue the user is submitting.\n\nIs is advised that your use generic types. The recommended rule-of-thumb is that there should be one option that fits any possible ticket a user might create, and there should NOT be a value such as "other", "N/A", or "unknown". Keeping the values general in nature increases the odds that users will pick a meaningful value.'
WHERE
	SettingName = 'TicketTypesList'
GO

UPDATE Settings 
SET 
	DefaultValue = 'false',
	SettingType = 'BoolString',
	SettingDescription = 'Setting this to "true" will cause the system to delete stored profile settings for ALL users.\n\nThis is sometimes useful after upgrading TicketDesk to a new version to reset user''s TicketCenter List Settings. The system will auto-regenerate a default set of profile values.'
WHERE
	SettingName = 'KillAllProfilesOnStartup'
GO

DECLARE @tdAllowSubmitterRoleToEditTags int
SET @tdAllowSubmitterRoleToEditTags = 0
SELECT @tdAllowSubmitterRoleToEditTags = 1 FROM Settings WHERE SettingName = 'AllowSubmitterRoleToEditTags'
IF @tdAllowSubmitterRoleToEditTags = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('AllowSubmitterRoleToEditTags','true','true','BoolString','If true submitters can set tags during ticket creation and in the ticket editor. Setting this to false reserves the tags for help desk staff use only, though tags will still be visible to submitters once set by the staff.\n\nTagging is encouraged as it assists in later ticket searches.')
END
GO



DECLARE @tdCreateSqlMembershipRegistrationsAsSubmitters int
SET @tdCreateSqlMembershipRegistrationsAsSubmitters = 0
SELECT @tdCreateSqlMembershipRegistrationsAsSubmitters = 1 FROM Settings WHERE SettingName = 'CreateSqlMembershipRegistrationsAsSubmitters'
IF @tdCreateSqlMembershipRegistrationsAsSubmitters = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('CreateSqlMembershipRegistrationsAsSubmitters','true','true','BoolString','If true new users that register will automatically be added to the submitters role.\n\nIf your system is exposed to the public, and you do NOT intend for the general public to be able to use your system, you should set this to false and require that an admin manually grant new users permissions.')
END
GO


DECLARE @tdAllowSubmitterRoleToEditPriority int
SET @tdAllowSubmitterRoleToEditPriority = 0
SELECT @tdAllowSubmitterRoleToEditPriority = 1 FROM Settings WHERE SettingName = 'AllowSubmitterRoleToEditPriority'
IF @tdAllowSubmitterRoleToEditPriority = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('AllowSubmitterRoleToEditPriority','false','false','BoolString','If true submitters can set the priority field either during ticket creation, or in the ticket editor. Setting this to false reserves the priority field for help desk staff use only, though priorities will still be visible to submitters once set by the staff. \n\nIn all cases, the priority is an optional field.')
END
GO

DECLARE @tdRefreshSecurityCacheMinutes int
SET @tdRefreshSecurityCacheMinutes = 0
SELECT @tdRefreshSecurityCacheMinutes = 1 FROM Settings WHERE SettingName = 'RefreshSecurityCacheMinutes'
IF @tdRefreshSecurityCacheMinutes = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('RefreshSecurityCacheMinutes','30','30','IntString','Used only in AD environments (for right now); sets the amount of time the system will wait before a background process attempts to rebuild the SQL cached values retrieved from AD.\n\nThe system will always rebuild the cache when it first starts up.')
END
GO

DECLARE @tdAdUserPropertiesSqlCacheRefreshMinutes int
SET @tdAdUserPropertiesSqlCacheRefreshMinutes = 0
SELECT @tdAdUserPropertiesSqlCacheRefreshMinutes = 1 FROM Settings WHERE SettingName = 'AdUserPropertiesSqlCacheRefreshMinutes'
IF @tdAdUserPropertiesSqlCacheRefreshMinutes = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('AdUserPropertiesSqlCacheRefreshMinutes','120','120','IntString','Used only in AD environments; sets the amount of time the system will wait before updating the user properties in the SQL cache.\n\nThese values are less critical than other values cached from AD (such as the list of group members), and so these properties can be refreshed less frequently. This value should be the same or higher than the "Refresh Security Cache Minutes" setting. ')
END
GO

DECLARE @tdIsDemo int
SET @tdIsDemo = 0
SELECT @tdIsDemo = 1 FROM Settings WHERE SettingName = 'IsDemo'
IF @tdIsDemo = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('IsDemo','false','false','BoolString','This setting indicates that TicketDesk is running in "demo mode".\n\nCurrently the only thing this affects are attachements. In Demo mode, the attachments system will not store the file contents that are uploaded (to keep people from using your demo site as an ad-hoc warze server).')
END
GO

DECLARE @tdCleanupPendingAttachmentsAfterHours int
SET @tdCleanupPendingAttachmentsAfterHours = 0
SELECT @tdCleanupPendingAttachmentsAfterHours = 1 FROM Settings WHERE SettingName = 'CleanupPendingAttachmentsAfterHours'
IF @tdCleanupPendingAttachmentsAfterHours = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('CleanupPendingAttachmentsAfterHours','1','1','IntString','This setting controls how long the system will leave pending attachments in the database before purging them.\n\nWhen users first upload an attachment to a ticket, the attachment is in an uncommitted state. This gives the user time to finish writing comments, adding more files, filling in other fields, etc. If for some reason the user does not finish submitting/updating the ticket, these attachments remain on the server for a while (in case they come back).')
END
GO

DECLARE @tdEnableEmailNotifications int
SET @tdEnableEmailNotifications = 0
SELECT @tdEnableEmailNotifications = 1 FROM Settings WHERE SettingName = 'EnableEmailNotifications'
IF @tdEnableEmailNotifications = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('EnableEmailNotifications','true','true','BoolString','This setting controls if the email notifications system is enabled or not.\n\nIf set to false, the system will still queue up notificaitons, but it will never send them out to the users.')
END
GO

DECLARE @tdEnableOutlookFriendlyHtmlEmail int
SET @tdEnableOutlookFriendlyHtmlEmail = 0
SELECT @tdEnableOutlookFriendlyHtmlEmail = 1 FROM Settings WHERE SettingName = 'EnableOutlookFriendlyHtmlEmail'
IF @tdEnableOutlookFriendlyHtmlEmail = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('EnableOutlookFriendlyHtmlEmail','false','false','BoolString','Determines if TicketDesk will use the email template that is "friendly" to outlook email clients when generating the HTML body of an email.\n\nTicketDesk normally generates both an HTML and a Plain text body for emails. The HTML is rendered using real CSS that is supported by the vast majority of browsers and email clients. However, Microsoft''s Outlook (up to 2010) still use Microsoft Word to redner emails, instead of Internet Explorer. Word''s HTML capabilities are very limisted and are not W3C standards based.\n\nIf the majority of your users use Outlook, you should enable this setting so they their emails render well in outlook.')
END
GO

DECLARE @tdEmailDeliveryTimerIntervalMinutes int
SET @tdEmailDeliveryTimerIntervalMinutes = 0
SELECT @tdEmailDeliveryTimerIntervalMinutes = 1 FROM Settings WHERE SettingName = 'EmailDeliveryTimerIntervalMinutes'
IF @tdEmailDeliveryTimerIntervalMinutes = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('EmailDeliveryTimerIntervalMinutes','3','3','IntString','Determines how often TicketDesk will fire off the background process that examines the email notifications queue.\n\nGenerally 1 - 5 minutes is appropriate for most environments, but you can change this if you want.\n\nYou should not set this to a value lower than the "Email Notification Initial Delay Minutes" setting.')
END
GO

DECLARE @tdEmailServiceName int
SET @tdEmailServiceName = 0
SELECT @tdEmailServiceName = 1 FROM Settings WHERE SettingName = 'EmailServiceName'
IF @tdEmailServiceName = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('EmailServiceName','DefaultEmailHandler','DefaultEmailHandler','SimpleString','Change this setting only if you have created a custom MEF module to handle email delivery! This should be the name of the MEF Export Contract Name of your custom email module. If you don''t know what that means, then DO NOT TOUCH THIS SETTING!')
END
GO

DECLARE @tdEmailNotificationInitialDelayMinutes int
SET @tdEmailNotificationInitialDelayMinutes = 0
SELECT @tdEmailNotificationInitialDelayMinutes = 1 FROM Settings WHERE SettingName = 'EmailNotificationInitialDelayMinutes'
IF @tdEmailNotificationInitialDelayMinutes = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('EmailNotificationInitialDelayMinutes','6','6','DoubleString','This controls the minimum amount of time after a notification for a ticket change has been queued before the system will send the notification.\n\nIf additional changes to a ticket occurs before this time is reached, TicketDesk will consolidate the multiple notifications into a single message rather than spamming the user with multiple messages about each change. Each time a new change occurs, the amount of time to wait is incremented again by this same value.\n\nThe value here can contain a decimal. The value should never be lower than the "Email Delivery Timer Interval Minutes" setting (they can be the same), and should also not be shorter than the "Email Max Consolidation Wait Minutes" setting (again, they can be the same).')
END
GO

DECLARE @tdEmailMaxConsolidationWaitMinutes int
SET @tdEmailMaxConsolidationWaitMinutes = 0
SELECT @tdEmailMaxConsolidationWaitMinutes = 1 FROM Settings WHERE SettingName = 'EmailMaxConsolidationWaitMinutes'
IF @tdEmailMaxConsolidationWaitMinutes = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('EmailMaxConsolidationWaitMinutes','12','12','DoubleString','This setting controls the max amount of time that the system will continue waiting on additional changes to a ticket before going ahead and sending a notifiaciton email.\n\nIf changes continue to occur to a ticket within the wait-period, the system will continue to wait before sending the email until either the changes slow down or the limit set by this setting is reached. \n\nThe value here can contain a decimal. This value should be larger or the same as the "Email Notification Initial Delay Minutes" setting.')
END
GO

DECLARE @tdEmailResendDelayMinutes int
SET @tdEmailResendDelayMinutes = 0
SELECT @tdEmailResendDelayMinutes = 1 FROM Settings WHERE SettingName = 'EmailResendDelayMinutes'
IF @tdEmailResendDelayMinutes = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('EmailResendDelayMinutes','5','5','IntString','This setting detemines how long TicketDesk will wait if there is an error when trying to send an email notification. Each time the notification failes, the wiat time will be the number of attempts times the value of this setting: Example, if the setting is 5 and message fails the third attempt, it will wait 15 minutes before trying again.')
END
GO

DECLARE @tdEmailMaxDeliveryAttempts int
SET @tdEmailMaxDeliveryAttempts = 0
SELECT @tdEmailMaxDeliveryAttempts = 1 FROM Settings WHERE SettingName = 'EmailMaxDeliveryAttempts'
IF @tdEmailMaxDeliveryAttempts = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('EmailMaxDeliveryAttempts','5','5','IntString','How many times the system will attempt to deliver an email notifications before giving up.')
END
GO

DECLARE @tdSiteRootUrlForEmail int
SET @tdSiteRootUrlForEmail = 0
SELECT @tdSiteRootUrlForEmail = 1 FROM Settings WHERE SettingName = 'SiteRootUrlForEmail'
IF @tdSiteRootUrlForEmail = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('SiteRootUrlForEmail','http://localhost:2534','http://localhost:2534','SimpleString','The root URL of the web site; will be used in email notifications to create the fully qualified link URLs. Without a valid setting, users clicking links in their notification email will not be directed to the TicketDesk site.')
END
GO

DECLARE @tdFromEmailDisplayName int
SET @tdFromEmailDisplayName = 0
SELECT @tdFromEmailDisplayName = 1 FROM Settings WHERE SettingName = 'FromEmailDisplayName'
IF @tdFromEmailDisplayName = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('FromEmailDisplayName','TicketDesk','TicketDesk','SimpleString','The "friendly name" to use in the "TO" field of email notifiacitons.')
END
GO

DECLARE @tdFromEmailAddress int
SET @tdFromEmailAddress = 0
SELECT @tdFromEmailAddress = 1 FROM Settings WHERE SettingName = 'FromEmailAddress'
IF @tdFromEmailAddress = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('FromEmailAddress','ticketdesk@nowhere.com','ticketdesk@nowhere.com','SimpleString','The email address to use in the "TO" field of email notifiacitons.\n\nUsually this is not a real person''s email address, just a made up address at your organization.')
END
GO

DECLARE @tdBlindCopyToEmailAddress int
SET @tdBlindCopyToEmailAddress = 0
SELECT @tdBlindCopyToEmailAddress = 1 FROM Settings WHERE SettingName = 'BlindCopyToEmailAddress'
IF @tdBlindCopyToEmailAddress = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('BlindCopyToEmailAddress','','','SimpleString','An email address that should be blind copied (BCC) on all email notifiacitons.\n\nUsually this is left blank, but can be useful as a diagnostic tool to test the notificiations system.')
END
GO

DECLARE @tdLuceneDirectory int
SET @tdLuceneDirectory = 0
SELECT @tdLuceneDirectory = 1 FROM Settings WHERE SettingName = 'LuceneDirectory'
IF @tdLuceneDirectory = 0
BEGIN
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('LuceneDirectory','~/TdSearchIndex','~/TdSearchIndex','SimpleString','This is the relative path to the directory where TicketDesk should store the full-text indexes used by the search feature.\n\nYou can use the text "ram" (case sensitive) to tell Lucene.net to use a purely in-memory index, but this is not recommended for your production sites. The asp.net user account will need read/write access to this folder location.')
END
GO







DECLARE @tdVersion int
SET @tdVersion = 0

SELECT @tdVersion = 1 FROM Settings WHERE SettingName = 'Version'

IF @tdVersion = 0
BEGIN
	
	INSERT Settings(SettingName,SettingValue,DefaultValue,SettingType,SettingDescription)
	VALUES('Version','2.0.0','','SimpleString','The version of the TicketDesk database. CHANGE AT YOUR OWN RISK!')
END
ELSE
BEGIN
	UPDATE Settings 
		SET 
			SettingValue = '2.0.0'
		WHERE 
			SettingName = 'Version'
END
GO


CREATE TABLE [dbo].[AdCachedRoleMembers](
	[GroupName] [nvarchar](150) NOT NULL,
	[MemberName] [nvarchar](150) NOT NULL,
	[MemberDisplayName] [nvarchar](150) NOT NULL
 CONSTRAINT [PK_AdCachedRoleMembers] PRIMARY KEY CLUSTERED 
(
	[GroupName] ASC,
	[MemberName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[AdCachedUserProperties](
	[UserName] [nvarchar](150) NOT NULL,
	[PropertyName] [nvarchar](100) NOT NULL,
	[PropertyValue] [nvarchar](250) NULL,
	[LastRefreshed] [datetime] NULL,
	[IsActiveInAd] [bit] NOT NULL,
 CONSTRAINT [PK_AdCachedUserProperties] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AdCachedUserProperties] ADD  CONSTRAINT [DF_AdCachedUserProperties_IsActiveInAd]  DEFAULT ((1)) FOR [IsActiveInAd]
GO


DROP PROCEDURE [dbo].[ELMAH_GetErrorXml]
go
DROP PROCEDURE [dbo].[ELMAH_GetErrorsXml]
go
DROP PROCEDURE [dbo].[ELMAH_LogError]
go
DROP TABLE [dbo].[ELMAH_Error]
go




/*
  
   ELMAH - Error Logging Modules and Handlers for ASP.NET
   Copyright (c) 2004-9 Atif Aziz. All rights reserved.
  
	Author(s):
  
		Atif Aziz, http://www.raboof.com
		Phil Haacked, http://haacked.com
  
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at
  
	  http://www.apache.org/licenses/LICENSE-2.0
  
   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
  
*/

-- ELMAH DDL script for Microsoft SQL Server 2000 or later.

-- $Id: SQLServer.sql 568 2009-05-11 14:18:34Z azizatif $


/* ------------------------------------------------------------------------ 
		TABLES
   ------------------------------------------------------------------------ */

CREATE TABLE [dbo].[ELMAH_Error]
(
	[ErrorId]     UNIQUEIDENTIFIER NOT NULL,
	[Application] NVARCHAR(60)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Host]        NVARCHAR(50)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Type]        NVARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Source]      NVARCHAR(60)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Message]     NVARCHAR(500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[User]        NVARCHAR(50)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[StatusCode]  INT NOT NULL,
	[TimeUtc]     DATETIME NOT NULL,
	[Sequence]    INT IDENTITY (1, 1) NOT NULL,
	[AllXml]      NTEXT COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) 
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[ELMAH_Error] WITH NOCHECK ADD 
	CONSTRAINT [PK_ELMAH_Error] PRIMARY KEY NONCLUSTERED ([ErrorId]) ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ELMAH_Error] ADD 
	CONSTRAINT [DF_ELMAH_Error_ErrorId] DEFAULT (NEWID()) FOR [ErrorId]
GO

CREATE NONCLUSTERED INDEX [IX_ELMAH_Error_App_Time_Seq] ON [dbo].[ELMAH_Error] 
(
	[Application]   ASC,
	[TimeUtc]       DESC,
	[Sequence]      DESC
) 
ON [PRIMARY]
GO

/* ------------------------------------------------------------------------ 
		STORED PROCEDURES                                                      
   ------------------------------------------------------------------------ */

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



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

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


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
		time        = CONVERT(VARCHAR(50), [TimeUtc], 126) + 'Z'
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

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


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

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

