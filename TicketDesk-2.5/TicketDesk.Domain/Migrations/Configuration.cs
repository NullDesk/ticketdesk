// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using TicketDesk.Domain.Model;
using TicketDesk.Domain.Models;
using System;

namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<TicketDeskContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TicketDeskCore";


        }

        protected override void Seed(TicketDeskContext context)
        {

            var titles = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1", "I1", "J1", "K1", "L1", "M1", "N1", "O1", "P1", "Q1", "R1" };
            var n = 0;
            foreach (var p in titles)
            {
                n--;
                string cc;
                string tt;
                if (Math.Abs(n) % 2 == 0)
                {
                    tt = "Question";
                    cc = "Hardware";
                }
                else
                {
                    tt = "Problem";
                    cc = "Software";
                }
                var now = DateTimeOffset.Now.AddDays(n);
                context.Tickets.AddOrUpdate(t => t.Title,
                    new Ticket
                    {
                        Title = "Test Ticket " + p,
                        AffectsCustomer = false,
                        AssignedTo = "64165817-9cb5-472f-8bfb-6a35ca54be6a",
                        Category = cc,
                        CreatedBy = "17f78f38-fa68-445f-90de-38896140db28",
                        TicketStatus = (p == "L") ? TicketStatus.Closed : TicketStatus.Active,
                        CurrentStatusDate = now,
                        CurrentStatusSetBy = "otherstaffer@nowhere.com",
                        Details =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus.",
                        IsHtml = false,
                        LastUpdateBy = "72bdddfb-805a-4883-94b9-aa494f5f52dc",
                        LastUpdateDate = now.AddHours(2),
                        Owner = "17f78f38-fa68-445f-90de-38896140db28",
                        Priority = "Low",
                        TagList = "test,moretest",
                        TicketType = tt

                    });
            }
            context.Settings.AddOrUpdate(
                s => s.SettingName,
                new Setting
                {
                    SettingName = "CategoryList",
                    SettingValue = "Hardware,Software,Network",
                    DefaultValue = "Hardware,Software,Network",
                    SettingType = "StringList",
                    SettingDescription = "This is the list of possible selections for the Category dropdown list.\n\nIs is advised that your use generic categories. The recommended rule-of-thumb is that there should be one option that fits any possible ticket a user might create, and there should NOT be a value such as 'other', 'N/A', or 'unknown'. Keeping the values general in nature increases the odds that users will pick a meaningful value."
                },
                new Setting
                {
                    SettingName = "PriorityList",
                    SettingValue = "High,Low,Medium",
                    DefaultValue = "High,Low,Medium",
                    SettingType = "StringList",
                    SettingDescription = "This is the list of possible selections for the Priority dropdown list."
                },
                new Setting
                {
                    SettingName = "TicketTypesList",
                    SettingValue = "Question,Problem,Request",
                    DefaultValue = "Question,Problem,Request",
                    SettingType = "StringList",
                    SettingDescription = "This is the list of possible selections for the Ticket Type dropdown list. The type of ticket is usually the 'kind' of issue the user is submitting.\n\nIs is advised that your use generic types. The recommended rule-of-thumb is that there should be one option that fits any possible ticket a user might create, and there should NOT be a value such as 'other', 'N/A', or 'unknown'. Keeping the values general in nature increases the odds that users will pick a meaningful value."

                });

            //open tickets
            const string id = "64165817-9cb5-472f-8bfb-6a35ca54be6a"; //the stock admin's user id
            var collection = new UserTicketListSettingsCollection
            {
                UserTicketListSetting.GetDefaultListSettings()
            };

            context.UserSettings.AddOrUpdate(s => s.UserId, new UserSetting { UserId = id, ListSettings = collection });

            base.Seed(context);
        }
    }
}
