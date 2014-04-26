using TicketDesk.Domain.Identity;

namespace TicketDesk.Domain.Migrations
{
    using TicketDesk.Domain.Model;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TicketDesk.Domain.TicketDeskContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TicketDeskDomain";
        }

        protected override void Seed(TicketDesk.Domain.TicketDeskContext context)
        {
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
                        TicketStatus = (p == "L") ? TicketStatus.Closed: TicketStatus.Active,
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

            var userManager = new TicketDeskUserManager(context);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Administrator"))
            {
                roleManager.Create(new IdentityRole("Administrator"));

            }

            if (!roleManager.RoleExists("HelpDesk"))
            {
                roleManager.Create(new IdentityRole("HelpDesk"));

            }

            if (!roleManager.RoleExists("TicketSubmitter"))
            {
                roleManager.Create(new IdentityRole("TicketSubmitter"));

            }

            if (userManager.FindByName("admin") == null)
            {
                var user = new UserProfile() { UserName = "admin",  Email = "admin@mydomain.com" };
                var result = userManager.Create(user, "admin");
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Administrator" );
                    userManager.AddToRole(user.Id, "HelpDesk");
                    userManager.AddToRole(user.Id, "TicketSubmitter");
                }                
            }

            if (userManager.FindByName("otherstaffer") == null)
            {
                var user = new UserProfile() { UserName = "otherstaffer", Email = "otherstaffer@mydomain.com" };
                var result = userManager.Create(user, "otherstaffer");
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "TicketSubmitter");

                }
            }

            context = new TicketDeskContext();
           


            context.SaveChanges();
        }
    }
}
