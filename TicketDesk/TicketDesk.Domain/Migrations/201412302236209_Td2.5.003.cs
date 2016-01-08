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
