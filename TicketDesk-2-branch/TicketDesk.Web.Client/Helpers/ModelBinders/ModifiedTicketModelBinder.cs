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