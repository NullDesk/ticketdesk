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
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.PushNotifications.Common;
using TicketDesk.PushNotifications.Common.Model;
using TicketDesk.Search.Azure;
using TicketDesk.Search.Common;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("admin/push-notification-settings")]
    [Route("{action=index}")]
    [Authorize(Roles = "TdAdministrators")]
    public class PushNotificationSettingsController : Controller
    {
        private TdPushNotificationContext Context { get; set; }
        public PushNotificationSettingsController(TdPushNotificationContext context)
        {
            Context = context;
        }

        public ActionResult Index()
        {
            var dbSetting = Context.TicketDeskPushNotificationSettings;

            return View(dbSetting);
        }

        [HttpPost]
        public ActionResult Index(ApplicationPushNotificationSetting settings)
        {
            var dbSetting = Context.TicketDeskPushNotificationSettings;
            if (ModelState.IsValid && TryUpdateModel(dbSetting))
            {

                Context.SaveChanges();
            }
            return View(dbSetting);
        }

        public ActionResult ListDeliveryProviderSettings()
        {

            var interfaceType = typeof(IPushNotificationDeliveryProvider);
            var classes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => !t.IsAbstract && interfaceType.IsAssignableFrom(t)).ToList();


            var model = new Dictionary<Type, ApplicationPushNotificationSetting.PushNotificationDeliveryProviderSetting>();

            foreach (var c in classes)
            {
                var settings = Context.TicketDeskPushNotificationSettings
                    .DeliveryProviderSettings
                    .FirstOrDefault(p => p.ProviderAssemblyQualifiedName == c.AssemblyQualifiedName);
                model.Add(c, settings ?? new ApplicationPushNotificationSetting.PushNotificationDeliveryProviderSetting());
            }


            return PartialView(model);
        }

        public ActionResult ConfigureDeliveryProvider(string providerTypeName)
        {
            var providerType = Type.GetType(providerTypeName);
            var settings = Context.TicketDeskPushNotificationSettings.DeliveryProviderSettings.FirstOrDefault(
                s => s.ProviderAssemblyQualifiedName == providerType.AssemblyQualifiedName);
            if (settings == null)
            {

                var provider = PushNotificationDeliveryManager.DeliveryProviders.FirstOrDefault(p => p.GetType() == providerType);
                if (provider == null)
                {
                    var ci = providerType.GetConstructor(new[] {typeof (JToken)});
                    if (ci != null)
                    {
                        provider = (IPushNotificationDeliveryProvider)ci.Invoke(new object[]{null});
                    }
                }

                settings = new ApplicationPushNotificationSetting.PushNotificationDeliveryProviderSetting()
                {
                    IsEnabled = false, 
                    ProviderAssemblyQualifiedName = providerType.AssemblyQualifiedName,
                    ProviderConfigurationData = provider.Configuration
                };
            }
            return View(settings);
        }

    }
}