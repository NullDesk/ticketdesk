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
    
    public partial class Td25004 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Settings");
        }
        
        public override void Down()
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
            Sql(@"INSERT [dbo].[Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'Version', N'2.5.0',N'2.5.0', N'SimpleString',N'The version of the TicketDesk database. CHANGE AT YOUR OWN RISK!')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'AllowInternalUsersToEditPriority', N'false', N'false', N'BoolString', N'If true submitters can set the priority field either during ticket creation, or in the ticket editor. Setting this to false reserves the priority field for help desk staff use only, though priorities will still be visible to submitters once set by the staff. \n\nIn all cases, the priority is an optional field.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'AllowInternalUsersToEditTags', N'true', N'true', N'BoolString', N'If true submitters can set tags during ticket creation and in the ticket editor. Setting this to false reserves the tags for help desk staff use only, though tags will still be visible to submitters once set by the staff.\n\nTagging is encouraged as it assists in later ticket searches.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'CategoryList', N'Hardware,Software,Network', N'Hardware,Software,Network', N'StringList', N'This is the list of possible selections for the Category dropdown list.\n\nIs is advised that your use generic categories. The recommended rule-of-thumb is that there should be one option that fits any possible ticket a user might create, and there should NOT be a value such as other, N/A, or unknown. Keeping the values general in nature increases the odds that users will pick a meaningful value.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'PriorityList', N'High,Low,Medium', N'High,Low,Medium', N'StringList', N'This is the list of possible selections for the Priority dropdown list.')");
            Sql(@"INSERT [Settings] ([SettingName], [SettingValue], [DefaultValue], [SettingType], [SettingDescription]) VALUES (N'TicketTypesList', N'Question,Problem,Request', N'Question,Problem,Request', N'StringList', N'This is the list of possible selections for the Ticket Type dropdown list. The type of ticket is usually the kind of issue the user is submitting.\n\nIs is advised that your use generic types. The recommended rule-of-thumb is that there should be one option that fits any possible ticket a user might create, and there should NOT be a value such as other, N/A, or unknown. Keeping the values general in nature increases the odds that users will pick a meaningful value.')");

            
        }
    }
}
