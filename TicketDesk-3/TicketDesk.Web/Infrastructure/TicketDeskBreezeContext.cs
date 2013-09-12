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
            var allowSave = false;
            if (entityInfo.Entity.GetType() == typeof(Ticket))
            {
                var now = DateTime.Now;
                var user = HttpContext.Current.User.Identity.Name;
                var ticket = ((Ticket)entityInfo.Entity);

                ticket.LastUpdateDate = now;
                ticket.LastUpdateBy = user;

                ProcessTicketTags(entityInfo, ticket);

                if (entityInfo.EntityState == EntityState.Modified)
                {
                    //this hack is necessary so server thinks the client updated this field
                    entityInfo.OriginalValuesMap.Add("LastUpdateDate", ticket.LastUpdateDate);
                    //TODO: move to an event and subscribe to the event from the hubhelper
                    TicketDeskHubHelper.Instance.NotifyTicketChanged(ticket.TicketId);
                    allowSave = true;
                }
                else if (entityInfo.EntityState == EntityState.Added)
                {
                    ticket.CreatedBy = user;
                    ticket.CreatedDate = now;
                    ticket.CurrentStatusDate = now;
                    ticket.CurrentStatusSetBy = user;
                    allowSave = true;
                }
            }

            return allowSave;
        }

        private void ProcessTicketTags(EntityInfo entityInfo, Ticket ticket)
        {
            object oldTagList;
            entityInfo.OriginalValuesMap.TryGetValue("TagList", out oldTagList);
            if (oldTagList == null || (string)oldTagList != ticket.TagList)
            {
                //if not new, kill all old tags, we'll just make new ones
                if (entityInfo.EntityState != EntityState.Added)
                {
                    this.Context.TicketTags.RemoveRange(this.Context.TicketTags.Where(t => t.TicketId == ticket.TicketId));
                }

                var tagsArr = TicketTag.GetTagsFromString(ticket.TagList);
                if (ticket.TicketTags == null && tagsArr.Length > 0)
                {
                    ticket.TicketTags = new List<TicketTag>();
                }
                foreach (var tag in tagsArr)
                {
                    var tTag = new TicketTag {TagName = tag};
                    ticket.TicketTags.Add(tTag);
                }
            }
        }
    }
}