namespace TicketDesk.Domain.Migrations
{
	using System;
	using System.Data.Entity.Migrations;
	
	public partial class Td25003 : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.ApplicationSettings",
				c => new
					{
						ApplicationName = c.String(nullable: false, maxLength: 128),
						PermissionsSettingsJson = c.String(),
						SelectListSettingsJson = c.String(),
					})
				.PrimaryKey(t => t.ApplicationName);


			Sql(@"
					 INSERT INTO [dbo].[ApplicationSettings]
						   ([ApplicationName]
						   ,[PermissionsSettingsJson]
						   ,[SelectListSettingsJson])
					 VALUES
						   ('TicketDesk'
						   ,'
							{
								""AllowInternalUsersToEditPriority"": false,
								""AllowInternalUsersToEditTags"": true
							}
							','
							{
							  ""CategoryList"": [
								""Software"",
								""Hardware"",
								""Network""
							  ],
							  ""PriorityList"": [
								""High"",
								""Medium"",
								""Low""
							  ],
							  ""TicketTypesList"": [
								""Problem"",
								""Question"",
								""Request""
							  ]
							}
						   ')");
		}
		
		public override void Down()
		{
			DropTable("dbo.ApplicationSettings");
		}
	}
}
