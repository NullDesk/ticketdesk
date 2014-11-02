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
using TicketDesk.Domain.Models;
using TicketDesk.Web.Client.Controllers;

namespace TicketDesk.Web.Client.Helpers.ModelBinders
{
    //public class ModifiedTicketModelBinder : IModelBinder
    //{
    //    #region IModelBinder Members

    //    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {

    //        var form = controllerContext.HttpContext.Request.Form;

    //        var ticketId = GetValue<int>(bindingContext, "TicketId");
            
    //        var controller = controllerContext.Controller as TicketEditorController;
    //        var ticket = controller.Tickets.GetTicket(ticketId);
            
    //        var title = GetValue<string>(bindingContext, "Title");
    //        if (ticket.Title != title)
    //        {
    //            ticket.Title = title;
    //        }

    //         "Title", "Details", "TagList", "Priority", "Category", "Type", "Owner"



    //        return ticket;
    //    }

    //    #endregion


    //    private T GetValue<T>(ModelBindingContext bindingContext, string key)
    //    {
            
    //        var valueResult = bindingContext.ValueProvider.GetValue(key);
    //        bindingContext.ModelState.SetModelValue(key, valueResult);
    //        return (T)valueResult.ConvertTo(typeof(T));
    //    }
    //}
}