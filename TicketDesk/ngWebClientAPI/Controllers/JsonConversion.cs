using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.IO;
using TicketDesk.Localization;

namespace ngWebClientAPI
{
    public class JsonConversion
    {
        public static string SerializeTicket(Ticket tckt)
        {
            string json = "{";
            json += "\"TicketID\":\"" + tckt.TicketId.ToString() + "\","; //conversion of ticket id to string
            json += "\"ProjectID\":\"" + tckt.ProjectId.ToString() + "\",";
            json += "\"Title\":\"" + tckt.Title + "\"";
            json += "\"Details\":\"" + tckt.Details.ToString() + "\"";

            return json + "}";
        }

        public string DeserializeTicket(string json)
        {
            return null;
        }
    }
}