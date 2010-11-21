using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Compilation;
using System.Reflection;
using System.ComponentModel.Composition.Hosting;
using TicketDesk.Web.Client.Controllers;
using System.IO;
using System.Web.Security;
using System.Configuration;
using TicketDesk.Domain.Repositories;
using TicketDesk.Domain.Services;

namespace TicketDesk.Web.Client
{

   
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : MefHttpApplication
    {
        private System.Timers.Timer DerelictAttachmentsTimer;
        private System.Timers.Timer EmaiNotificationsTimer;

        public static CompositionContainer RootContainer;
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Attachments", "Attachment/{action}/{fileId}", new { Controller = "Attachment" });

            routes.MapRoute(
              "MarkdownPreview",
              "MarkdownPreview",
              new { controller = "Markdown", action = "MarkdownPreview" });

            routes.MapRoute(
                "TicketCreate",
                "NewTicket",
                new { controller = "NewTicket", action = "Create" });

            routes.MapRoute(
                "TicketViewer",
                "Ticket/{id}/{action}/{activity}",
                new { controller = "TicketEditor", action = "Display", activity = string.Empty });

            routes.MapRoute(
                "TicketCenterPaging",                                                  // Route name
                "TicketCenter/{action}/{listName}/{page}",                                  // URL with parameters
                new { controller = "TicketCenter", action = "List", listName = "unassigned", page = "1" },  // Parameter defaults
                new { controller = "TicketCenter", page = @"\d+" }
                );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "404-PageNotFound",
                "{*url}",
                new { controller = "StaticContent", action = "PageNotFound" }
            );

        }

        protected override void Application_Start()
        {
            base.Application_Start();
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            DerelictAttachmentsTimer = new System.Timers.Timer();

            int derelictInterval = 300000;

            DerelictAttachmentsTimer.Elapsed += new System.Timers.ElapsedEventHandler(DerelictAttachmentsTimer_Elapsed);
            DerelictAttachmentsTimer.Interval = derelictInterval;
            DerelictAttachmentsTimer.AutoReset = true;
            DerelictAttachmentsTimer.Start();

            if (ConfigurationManager.AppSettings["EnableEmailNotifications"] != null)
            {
                var emailEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailNotifications"]);

                if (emailEnabled)
                {

                    EmaiNotificationsTimer = new System.Timers.Timer();

                    string deliveryMinutes = ConfigurationManager.AppSettings["EmailDeliveryTimerIntervalMinutes"];
                    int emailInterval = 300000;//5 minutes
                    if (!string.IsNullOrEmpty(deliveryMinutes))
                    {
                        emailInterval = Convert.ToInt32(deliveryMinutes) * 60000;
                    }

                    EmaiNotificationsTimer.Elapsed += new System.Timers.ElapsedEventHandler(EmaiNotificationsTimer_Elapsed);
                    EmaiNotificationsTimer.Interval = emailInterval;
                    EmaiNotificationsTimer.AutoReset = true;
                    EmaiNotificationsTimer.Start();
                }
            }
        }

        private void EmaiNotificationsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Object isRunningLock = new Object();
            lock (isRunningLock)
            {
                var noteSender = MefHttpApplication.ApplicationContainer.GetExportedValue<INotificationSendingService>();
                noteSender.ProcessWaitingTicketEventNotifications();
            }
        }

        private void DerelictAttachmentsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Object isRunningLock = new Object();
            lock (isRunningLock)
            {
                //clean up pending attachments that are now derelict
                int hoursOld = Convert.ToInt32(ConfigurationManager.AppSettings["CleanupPendingAttachmentsAfterHours"]);

                var tsvc = MefHttpApplication.ApplicationContainer.GetExportedValue<ITicketService>();
                tsvc.CleanUpDerelictAttachments(hoursOld);
            }
        }

    }
}