namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TicketDesk.Domain.Model;

    public sealed class Configuration : DbMigrationsConfiguration<TicketDeskContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TicketDeskDb";
        }

        protected override void Seed(TicketDeskContext context)
        {

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var titles = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P" };
            foreach (var p in titles)
            {
                context.Tickets.AddOrUpdate(t => t.Title,
                    new Ticket
                    {
                        Title = "Test Ticket " + p,
                        AffectsCustomer = false,
                        AssignedTo = "admin",
                        Category = "Hardware",
                        CreatedBy = "otherstaffer",
                        CurrentStatus = "Active",
                        CurrentStatusDate = DateTimeOffset.Now,
                        CurrentStatusSetBy = "otherstaffer",
                        Details = "Lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus.",
                        IsHtml = false,
                        LastUpdateBy = "otherstaffer",
                        LastUpdateDate = DateTimeOffset.Now,
                        Owner = "otherstaffer",
                        Priority = "Low",
                        PublishedToKb = false,
                        TagList = "test,moretest",
                        TicketType = "Problem"

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
                    SettingName = "CategoryList-es",
                    SettingValue = "¿Hardware?,¿Software?,¿Network?",
                    DefaultValue = "¿Hardware?,¿Software?,¿Network?",
                    SettingType = "StringList",
                    SettingDescription = "spanish translation."
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

                    SettingName = "PriorityList-es",
                    SettingValue = "¿High?,¿Low?,¿Medium?",
                    DefaultValue = "¿High?,¿Low?,¿Medium?",
                    SettingType = "StringList",
                    SettingDescription = "spanish translation."
                },
                new Setting
                {

                    SettingName = "TicketTypesList",
                    SettingValue = "Question,Problem,Request",
                    DefaultValue = "Question,Problem,Request",
                    SettingType = "StringList",
                    SettingDescription = "This is the list of possible selections for the Ticket Type dropdown list. The type of ticket is usually the 'kind' of issue the user is submitting.\n\nIs is advised that your use generic types. The recommended rule-of-thumb is that there should be one option that fits any possible ticket a user might create, and there should NOT be a value such as 'other', 'N/A', or 'unknown'. Keeping the values general in nature increases the odds that users will pick a meaningful value."

                },
                new Setting
                {

                    SettingName = "TicketTypesList-es",
                    SettingValue = "¿Question?,¿Problem?,¿Request?",
                    DefaultValue = "¿Question?,¿Problem?,¿Request?",
                    SettingType = "StringList",
                    SettingDescription = "spanish translation."

                });




        }
    }


}
