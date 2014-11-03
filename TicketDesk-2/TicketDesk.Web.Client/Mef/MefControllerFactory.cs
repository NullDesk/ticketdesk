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
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Web.Mvc;
using System.Collections;
using System.Web.Routing;
using TicketDesk.Domain.Services;
using System.Web.SessionState;

namespace TicketDesk.Web.Client
{
    public class MefControllerFactory : IControllerFactory
    {
        #region IControllerFactory Members

        public IController CreateController(RequestContext requestContext, string controllerName)
        {

         
            var requestContainer = GetRequestControllerContainer(requestContext.HttpContext.Items);

            var controller = requestContainer.GetExportedValue<IController>(controllerName);

            if (controller == null)
            {
                throw new HttpException(404, "Not found");
            }

            return controller;
        }

        public void ReleaseController(IController controller)
        {
            // nothing to do
        }

        #endregion

        public static CompositionContainer GetRequestControllerContainer(IDictionary contextItemsCollection)
        {
            var app = (MefHttpApplication)HttpContext.Current.ApplicationInstance;

            if (contextItemsCollection == null) throw new ArgumentNullException("dictionary");

            var container = (CompositionContainer)contextItemsCollection["MefRequestControllerContainer"];

            if (container == null)
            {
                container = new CompositionContainer(MefHttpApplication.ControllerCatalog, false, MefHttpApplication.ApplicationContainer);
                contextItemsCollection["MefRequestControllerContainer"] = container;
            }
            return container;
        }



        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }
    }
}