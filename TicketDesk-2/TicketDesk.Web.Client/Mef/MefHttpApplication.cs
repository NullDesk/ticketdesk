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
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using System.Web.Mvc;
using System.IO;
using System.Reflection;

namespace TicketDesk.Web.Client
{
    public class MefHttpApplication : HttpApplication
    {
        public static CompositionContainer ApplicationContainer { get; private set; }
        public static ComposablePartCatalog RootCatalog { get; private set; }
        public static ComposablePartCatalog ControllerCatalog { get; private set; }


        protected virtual void Application_Start()
        {
            if (RootCatalog == null)
            {
                RootCatalog = CreateRootCatalog();
            }
            if (ApplicationContainer == null)
            {
                ApplicationContainer = new CompositionContainer(RootCatalog, false);
            }
            if (ControllerCatalog == null)
            {
                var controllerTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Any(i => i == typeof(IController)));
                ControllerCatalog = new TypeCatalog(controllerTypes);
            }

            ControllerBuilder.Current.SetControllerFactory(new MefControllerFactory());
        }

        protected virtual void Application_End()
        {
            if (ApplicationContainer != null)
            {
                ApplicationContainer.Dispose();
            }
        }

        protected virtual ComposablePartCatalog CreateRootCatalog()
        {
            return new DirectoryCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"));
        }
    }
}