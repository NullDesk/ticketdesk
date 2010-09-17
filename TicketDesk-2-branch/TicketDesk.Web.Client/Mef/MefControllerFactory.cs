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
        
    }
}