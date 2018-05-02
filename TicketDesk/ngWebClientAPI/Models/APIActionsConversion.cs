using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketDesk.Domain.Model;

namespace ngWebClientAPI.Models
{
    [System.Web.Http.Authorize]
    public class APIActionsConversion
    {
        public static AssignObject ConvertAssign(JObject data)
        {
            AssignObject action;
            try
            {
                action = data.ToObject<AssignObject>();
            }
            catch (Exception ex)
            {
                return null;
            }
            action.ticketId = APITicketConversion.ConvertTicketId(action.ticketId);
            return action;
        }

        public static InfoObject ConvertInfo(JObject data)
        {
            InfoObject info = new InfoObject();
            info.ticketId = APITicketConversion.ConvertTicketId(data["ticketId"].ToObject<Int64>());
            info.comment = data["comment"].ToString();
            return info;
        }
    }

    public class AssignObject
    {
        public int ticketId { get; set; }
        public string comment { get; set; }
        public string priority { get; set; }
        public string assignedTo { get; set; }
    }

    public class InfoObject
    {
        public int ticketId { get; set; }
        public string comment { get; set; }
    }
}