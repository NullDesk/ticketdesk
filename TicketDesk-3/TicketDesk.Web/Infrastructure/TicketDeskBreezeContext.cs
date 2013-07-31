using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Breeze.WebApi;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Infrastructure
{
    public class TicketDeskBreezeContext : EFContextProvider<TicketDeskContext>
    {
        public TicketDeskBreezeContext() : base() { }

        protected override bool BeforeSaveEntity(EntityInfo entityInfo)
        {
            if (entityInfo.Entity.GetType() == typeof(Ticket) && entityInfo.EntityState == EntityState.Modified)
            {
                
                var ticket = ((Ticket) entityInfo.Entity);

                //this hack is necessary so server thinks the client updated this field
                entityInfo.OriginalValuesMap.Add("LastUpdateDate", ticket.LastUpdateDate);
                
                ticket.LastUpdateDate = DateTime.Now;

                TicketDeskHubHelper.Instance.NotifyTicketChanged(ticket.TicketId);
            }
            return true;
        }
    }
}