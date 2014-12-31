// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Data.Entity.Migrations;
using TicketDesk.Domain.Model;

namespace TicketDesk.Domain.Migrations
{
    public static class DemoDataManager
    {
        public static void RemoveAllData(TicketDeskContext context)
        {
            context.TicketEventNotifications.RemoveRange(context.TicketEventNotifications);
            context.UserSettings.RemoveRange(context.UserSettings);
            context.TicketAttachments.RemoveRange(context.TicketAttachments);
            context.TicketTags.RemoveRange(context.TicketTags);
            context.TicketEvents.RemoveRange(context.TicketEvents);
            context.Tickets.RemoveRange(context.Tickets);
            
            context.TicketDeskSettings = new ApplicationSetting();
          
            context.SaveChanges();
        }

        public static void SetupDemoData(TicketDeskContext context)
        {
            RemoveAllData(context);
            context.SaveChanges();

          
          


            context.Tickets.AddOrUpdate(t => t.Title,
                   new Ticket
                   {
                       Title = "Test Unassigned Ticket",
                       AffectsCustomer = false,
                       Category = "Hardware",
                       CreatedBy = "17f78f38-fa68-445f-90de-38896140db28",
                       TicketStatus = TicketStatus.Active,
                       CurrentStatusDate = DateTimeOffset.Now,
                       CurrentStatusSetBy = "72bdddfb-805a-4883-94b9-aa494f5f52dc",
                       Details =
                           "Lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus.",
                       IsHtml = false,
                       LastUpdateBy = "72bdddfb-805a-4883-94b9-aa494f5f52dc",
                       LastUpdateDate = DateTimeOffset.Now,
                       Owner = "17f78f38-fa68-445f-90de-38896140db28",
                       Priority = "Low",
                       TagList = "test,moretest",
                       TicketType = "Problem",
                       TicketEvents = new[] {TicketEvent.CreateActivityEvent("17f78f38-fa68-445f-90de-38896140db28",TicketActivity.Create, null,null,null)}

                   });

            var titles = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1", "I1", "J1", "K1", "L1", "M1", "N1", "O1", "P1", "Q1", "R1" };
            var n = 0;
            foreach (var p in titles)
            {
                n--;
                string cc;
                string tt;
                string oo;
                if (Math.Abs(n) % 2 == 0)
                {
                    tt = "Question";
                    cc = "Hardware";
                    oo = "64165817-9cb5-472f-8bfb-6a35ca54be6a";
                }
                else
                {
                    tt = "Problem";
                    cc = "Software";
                    oo = "17f78f38-fa68-445f-90de-38896140db28";
                }


                var now = DateTimeOffset.Now.AddDays(n);
                context.Tickets.AddOrUpdate(t => t.Title,
                    new Ticket
                    {
                        Title = "Test Ticket " + p,
                        AffectsCustomer = false,
                        AssignedTo = "64165817-9cb5-472f-8bfb-6a35ca54be6a",
                        Category = cc,
                        CreatedBy = oo,
                        TicketStatus = (p == "L") ? TicketStatus.Closed : TicketStatus.Active,
                        CurrentStatusDate = now,
                        CurrentStatusSetBy = "72bdddfb-805a-4883-94b9-aa494f5f52dc",
                        Details =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus.",
                        IsHtml = false,
                        LastUpdateBy = "72bdddfb-805a-4883-94b9-aa494f5f52dc",
                        LastUpdateDate = now.AddHours(2),
                        Owner = oo,
                        Priority = "Low",
                        TagList = "test,moretest",
                        TicketType = tt,
                        TicketEvents = new[] { TicketEvent.CreateActivityEvent(oo, TicketActivity.Create, null, null, null) }
                    });
            }
            context.SaveChanges();
        }

    }
}
