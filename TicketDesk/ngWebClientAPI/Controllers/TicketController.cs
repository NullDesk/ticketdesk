using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.IO;
using TicketDesk.Localization.Controllers;
using System.Data.Entity.Migrations;
using Newtonsoft.Json;


namespace ngWebClientAPI.Controllers
{
    public class TicketController : Controller
    {
        private TdDomainContext Context { get; set; }
        public TicketController(TdDomainContext context)
        {
            Context = context;
        }

        public RedirectToRouteResult Index()
        {
            return RedirectToAction("Index", "TicketCenter");
        }
        public static void RemoveAllData(TdDomainContext context)
        {
           // context.UserSettings.RemoveRange(context.UserSettings);
            context.TicketTags.RemoveRange(context.TicketTags);
            context.TicketEventNotifications.RemoveRange(context.TicketEventNotifications);
            context.TicketSubscribers.RemoveRange(context.TicketSubscribers);
            context.TicketEvents.RemoveRange(context.TicketEvents);
            context.Tickets.RemoveRange(context.Tickets);
            context.Projects.RemoveRange(context.Projects.Where(p => p.ProjectName != "Default"));
            context.TicketDeskSettings = new ApplicationSetting() { };

            context.SaveChanges();
        }

        public void init()
        {
            RemoveAllData(Context);
            Context.SaveChanges();
            Context.Projects.AddOrUpdate(p => p.ProjectName, new Project() { ProjectName = "NullSoft", ProjectDescription = "NullSoft Rocks" });
            var dProj = Context.Projects.First();
            Context.Tickets.AddOrUpdate(t => t.Title,
                   new Ticket
                   {
                       ProjectId = dProj.ProjectId,
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
                       TicketTags = new List<TicketTag> { new TicketTag() { TagName = "test" }, { new TicketTag() { TagName = "moretest" } } },
                       TicketType = "Problem",
                       TicketEvents = new[] { TicketEvent.CreateActivityEvent("17f78f38-fa68-445f-90de-38896140db28", TicketActivity.Create, null, null, null) }

                   });
        }
        public async Task<string> getTicket(int id)
        {
            var model = await Context.Tickets.Include(t => t.TicketSubscribers).FirstOrDefaultAsync(t => t.TicketId == id);
            if (model == null)
            {
                Context.Tickets.AddOrUpdate(t => t.Title,
                  new Ticket
                  {
                      ProjectId = 1,
                      Title = "Test Unassigned Ticket",
                      AffectsCustomer = false,
                      Category = "Hardware",
                      CreatedBy = "17f78f38-fa68-445f-90de-38896140db28",
                      TicketStatus = TicketStatus.Active,
                      CurrentStatusDate = DateTimeOffset.Now,
                      CurrentStatusSetBy = "72bdddfb-805a-4883-94b9-aa494f5f52dc",
                      Details = "Lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus.",
                      IsHtml = false,
                      LastUpdateBy = "72bdddfb-805a-4883-94b9-aa494f5f52dc",
                      LastUpdateDate = DateTimeOffset.Now,
                      Owner = "17f78f38-fa68-445f-90de-38896140db28",
                      Priority = "Low",
                      TagList = "test,moretest",
                      TicketTags = new List<TicketTag> { new TicketTag() { TagName = "test" }, { new TicketTag() { TagName = "moretest" } } },
                      TicketType = "Problem",
                      TicketEvents = new[] { TicketEvent.CreateActivityEvent("17f78f38-fa68-445f-90de-38896140db28", TicketActivity.Create, null, null, null) }
                  });
            }

            string output = JsonConvert.SerializeObject(model, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });
            return output;
        }

        private async Task<bool> CreateTicketAsync(Ticket ticket)
        {

            Context.Tickets.Add(ticket);
            await Context.SaveChangesAsync();
            //ticket.CommitPendingAttachments(tempId);

            return ticket.TicketId != default(int);
        }

    }
}