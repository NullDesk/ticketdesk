using TicketDesk.Domain.Model;

namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<TicketDeskContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TicketDeskCore";
            
            
        }

        protected override void Seed(TicketDeskContext context)
        {

            var titles = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R" };
            foreach (var p in titles)
            {
                context.Tickets.AddOrUpdate(t => t.Title,
                    new Ticket
                    {
                        Title = "Test Ticket " + p,
                        AffectsCustomer = false,
                        AssignedTo = "admin@nowhere.com",
                        Category = "Hardware",
                        CreatedBy = "otherstaffer@nowhere.com",
                        TicketStatus =  (p == "L") ? TicketStatus.Closed : TicketStatus.Active,
                        CurrentStatusDate = DateTimeOffset.Now,
                        CurrentStatusSetBy = "otherstaffer@nowhere.com",
                        Details = "Lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus.",
                        IsHtml = false,
                        LastUpdateBy = "otherstaffer@nowhere.com",
                        LastUpdateDate = DateTimeOffset.Now,
                        Owner = "otherstaffer@nowhere.com",
                        Priority = "Low",
                        TagList = "test,moretest",
                        TicketType = "Problem"

                    });
            }
            base.Seed(context);
        }
    }
}
