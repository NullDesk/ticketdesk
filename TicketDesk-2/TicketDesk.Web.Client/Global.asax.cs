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
        public static void RegisterRoutes(RouteCollection routes, bool hideHome)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*)?favicon.ico" });
            //routes.IgnoreRoute("{*crossdomain}", new { crossdomain = @"(.*)?crossdomain.xml" });
            //routes.IgnoreRoute("{*robots}", new { crossdomain = @"(.*)?robots.txt" });
            //routes.IgnoreRoute("{*css}", new { css = @"(.*)?.css"});
            routes.IgnoreRoute("{file}.txt");
            routes.IgnoreRoute("{file}.htm");
            routes.IgnoreRoute("{file}.html");
            routes.IgnoreRoute("{file}.xml");
            routes.IgnoreRoute("{*script}", new { script = @"Scripts/(.*)" });
            routes.IgnoreRoute("{*content}", new { content = @"Content/(.*)" });
            
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

            if (hideHome)
            {
                routes.MapRoute(
                    "Default", // Route name
                    "{controller}/{action}/{id}", // URL with parameters
                    new { controller = "TicketCenter", action = "List", id = UrlParameter.Optional } // Parameter defaults
                );
            }
            else
            {
                routes.MapRoute(
                       "Default", // Route name
                       "{controller}/{action}/{id}", // URL with parameters
                       new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                   );
            }

            routes.MapRoute(
                "404-PageNotFound",
                "{*url}",
                new { controller = "StaticContent", action = "PageNotFound" }
            );
        }

        private static System.Timers.Timer SecurityRefreshTimer { get; set; }

        private IApplicationSettingsService AppSettings;

        protected override void Application_Start()
        {
            
            base.Application_Start();
            AppSettings = MefHttpApplication.ApplicationContainer.GetExportedValue<IApplicationSettingsService>();

            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes, AppSettings.HideHomePage);

            var databaseSchemaManager = MefHttpApplication.ApplicationContainer.GetExportedValue<IDatabaseSchemaManagerService>();
            var searchService = MefHttpApplication.ApplicationContainer.GetExportedValue<TicketSearchService>();
            var ticketService = MefHttpApplication.ApplicationContainer.GetExportedValue<ITicketService>();
            
            databaseSchemaManager.EnsureSchemaVersion();

            searchService.InitializeSearch(ticketService);

            var securityService = MefHttpApplication.ApplicationContainer.GetExportedValue<ISecurityService>();

            //timer is initialized by the service, but we have to hold a reference to it here or it will be garbage collected
            //  in SQL Security Environments, this will just return null; there is no timer
            SecurityRefreshTimer = securityService.InitializeSecurityCacheRefreshTimer();

            DerelictAttachmentsTimer = new System.Timers.Timer();
            int derelictInterval = 300000;
            DerelictAttachmentsTimer.Elapsed += new System.Timers.ElapsedEventHandler(DerelictAttachmentsTimer_Elapsed);
            DerelictAttachmentsTimer.Interval = derelictInterval;
            DerelictAttachmentsTimer.AutoReset = true;
            DerelictAttachmentsTimer.Start();

            if (AppSettings.EmailNotificationsEnabled())
            {

                EmaiNotificationsTimer = new System.Timers.Timer();

                int emailInterval = AppSettings.EmailDeliveryTimerIntervalMinutes * 60000;

                EmaiNotificationsTimer.Elapsed += new System.Timers.ElapsedEventHandler(EmaiNotificationsTimer_Elapsed);
                EmaiNotificationsTimer.Interval = emailInterval;
                EmaiNotificationsTimer.AutoReset = true;
                EmaiNotificationsTimer.Start();

            }
        }
        private static Object emaiNotificationsTimerIsRunningLock = new Object();
        private void EmaiNotificationsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (emaiNotificationsTimerIsRunningLock)
            {
                var noteSender = MefHttpApplication.ApplicationContainer.GetExportedValue<INotificationSendingService>();
                noteSender.ProcessWaitingTicketEventNotifications();
            }
        }

        private static Object derelictAttachmentsTimerIsRunningLock = new Object();
        private void DerelictAttachmentsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (derelictAttachmentsTimerIsRunningLock)
            {
                //clean up pending attachments that are now derelict
                int hoursOld = AppSettings.CleanupPendingAttachmentsAfterHours;

                var tsvc = MefHttpApplication.ApplicationContainer.GetExportedValue<ITicketService>();
                tsvc.CleanUpDerelictAttachments(hoursOld);
            }
        }

    }
}