using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Services;
using TicketDesk.Domain.Models;
using TicketDesk.Web.Client.Helpers;
using TicketDesk.Web.Client.Controllers;

namespace TicketDesk.Web.Client.Areas.Admin.Controllers
{
    [Export("ApplicationSettings", typeof(IController))]
    public partial class ApplicationSettingsController : ApplicationController
    {
        private SettingsService Settings { get; set; }

        [ImportingConstructor]
        public ApplicationSettingsController(ISecurityService security, SettingsService settings)
            : base(security)
        {
            Settings = settings;
        }


        //
        // GET: /ApplicationSettings/

        public virtual ActionResult List()
        {
            
            var model = Settings.ApplicationSettings.CurrentSettings;
            return View(model);
        }

        //
        // GET: /ApplicationSettings/Details/5

        //public virtual ActionResult Details(int id)
        //{
        //    return View();
        //}

        //
        // GET: /ApplicationSettings/Create

        //public virtual ActionResult Create()
        //{
        //    return View();
        //}

        //
        // POST: /ApplicationSettings/Create

        //[HttpPost]
        //public virtual ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /ApplicationSettings/Edit/5

        public virtual ActionResult Edit(string settingName)
        {
            var setting = Settings.ApplicationSettings.CurrentSettings.SingleOrDefault(s => s.SettingName == settingName);
            return View(setting);
        }

        [HttpPost]
        [ValidateOnlyIncomingValues]
        public virtual ActionResult Edit(Setting setting)
        {
            try
            {
                var currentSetting = Settings.ApplicationSettings.CurrentSettings.SingleOrDefault(s => s.SettingName == setting.SettingName);
                TryUpdateModel(currentSetting, new[] { "SettingValue"});
                
                if (ModelState.IsValid)
                {
                    Settings.ApplicationSettings.SaveSettings(new[] { currentSetting });
                    // TODO: Add update logic here

                    return RedirectToAction(MVC.Admin.ApplicationSettings.List());
                }
                else
                {
                    return View(currentSetting);
                }
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ApplicationSettings/Delete/5

        public virtual ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ApplicationSettings/Delete/5

        [HttpPost]
        public virtual ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
