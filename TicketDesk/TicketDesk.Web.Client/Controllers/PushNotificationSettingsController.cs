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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.PushNotifications;
using TicketDesk.PushNotifications.Delivery;
using TicketDesk.PushNotifications.Model;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("admin/push-notification-settings")]
    [Route("{action=index}")]
    [TdAuthorize(Roles = "TdAdministrators")]
    public class PushNotificationSettingsController : Controller
    {
        private TdPushNotificationContext NoteContext { get; set; }
        private TdDomainContext DomainContext { get; set; }

        public PushNotificationSettingsController(TdPushNotificationContext noteContext, TdDomainContext domainContext)
        {
            ViewBag.Saved = false;

            NoteContext = noteContext;
            DomainContext = domainContext;
        }

        public ActionResult Index()
        {
            ViewBag.CurrentRootUrl = GetCurrentRootUrl();
            ViewBag.SiteRootUrl = GetRootUrlSetting();

            var dbSetting = NoteContext.TicketDeskPushNotificationSettings;

            return View(dbSetting);
        }


        [HttpPost]
        public ActionResult Index(ApplicationPushNotificationSetting settings, string siteRootUrl)
        {
            var dbSetting = NoteContext.TicketDeskPushNotificationSettings;
            if (ModelState.IsValid && TryUpdateModel(dbSetting))
            {
                NoteContext.SaveChanges();
                DomainContext.TicketDeskSettings.ClientSettings.Settings["DefaultSiteRootUrl"] = siteRootUrl.TrimEnd('/');
                DomainContext.SaveChanges();
                ViewBag.Saved = true;
            }
            ViewBag.CurrentRootUrl = GetCurrentRootUrl();
            ViewBag.SiteRootUrl = GetRootUrlSetting();

            Task.Delay(500).ContinueWith(t => System.Web.HttpRuntime.UnloadAppDomain()).ConfigureAwait(false);

            return View(dbSetting);
        }

        [Route("list-providers")]
        public ActionResult ListDeliveryProviderSettings()
        {

            var interfaceType = typeof(IPushNotificationDeliveryProvider);
            var classes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => !t.IsAbstract && interfaceType.IsAssignableFrom(t)).ToList();


            var model =
                new Dictionary<Type, ApplicationPushNotificationSetting.PushNotificationDeliveryProviderSetting>();

            foreach (var c in classes)
            {
                var settings = NoteContext.TicketDeskPushNotificationSettings
                    .DeliveryProviderSettings
                    .FirstOrDefault(p => p.ProviderAssemblyQualifiedName == c.AssemblyQualifiedName);
                model.Add(c,
                    settings ?? new ApplicationPushNotificationSetting.PushNotificationDeliveryProviderSetting());
            }


            return PartialView(model);
        }

        [Route("configure-provider")]
        public ActionResult ConfigureDeliveryProvider(string providerAssemblyQualifiedName)
        {
            var provider = GetProvider(providerAssemblyQualifiedName);
            var settings = GetOrCreatePushNotificationDeliveryProviderSettings(provider);
            if (settings == null)
            {
                return RedirectToAction("Index");
            }
            //ViewBag.Provider = provider;
            return View(settings);
        }

        [HttpPost]
        [Route("configure-provider")]
        public async Task<ActionResult> ConfigureDeliveryProvider(string providerAssemblyQualifiedName, bool isEnabled,
            FormCollection form)
        {
            var provider = GetProvider(providerAssemblyQualifiedName);
            var settings = GetOrCreatePushNotificationDeliveryProviderSettings(provider);

            if (ModelState.IsValid)
            {
                if (settings == null)
                {
                    return RedirectToAction("Index");
                }
                // reflection to get the TryUpdateMethod 
                var method =
                    GetType()
                        .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                        .First(m => m.Name == "TryUpdateModel" && m.IsGenericMethod && m.GetParameters().Count() == 1)
                        .MakeGenericMethod(provider.Configuration.GetType());

                if ((bool)method.Invoke(this, new[] { provider.Configuration }))//TryUpdateModel call
                {
                    settings.ProviderConfigurationData = JObject.FromObject(provider.Configuration);
                    settings.IsEnabled = isEnabled;
                    await NoteContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(settings);
        }

        private ApplicationPushNotificationSetting.PushNotificationDeliveryProviderSetting
            GetOrCreatePushNotificationDeliveryProviderSettings(IPushNotificationDeliveryProvider provider)
        {
            var settings = NoteContext.TicketDeskPushNotificationSettings.DeliveryProviderSettings.FirstOrDefault(
                s => s.ProviderAssemblyQualifiedName == provider.GetType().AssemblyQualifiedName);
            if (settings == null)
            {
                settings = ApplicationPushNotificationSetting.PushNotificationDeliveryProviderSetting.FromProvider(provider);
                //created new settings, add to context (will not be saved here, but may be committed by caller
                NoteContext.TicketDeskPushNotificationSettings.DeliveryProviderSettings.Add(settings);
            }
            return settings;
        }

        private IPushNotificationDeliveryProvider GetProvider(string providerTypeName)
        {
            return PushNotificationDeliveryManager.DeliveryProviders.FirstOrDefault(p => p.GetType().AssemblyQualifiedName == providerTypeName) ??
                   PushNotificationDeliveryManager.CreateDefaultDeliveryProviderInstance(Type.GetType(providerTypeName));
        }


        private string GetRootUrlSetting()
        {
            var root = DomainContext.TicketDeskSettings.ClientSettings.GetDefaultSiteRootUrl();
            if (string.IsNullOrEmpty(root))
            {
                root = GetCurrentRootUrl();
            }
            return root;
        }

        private string GetCurrentRootUrl()
        {
            return Request.Url == null ? string.Empty : Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~/").TrimEnd('/');
        }
    }
}
