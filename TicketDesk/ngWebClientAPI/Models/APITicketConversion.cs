using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketDesk.Domain.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace ngWebClientAPI.Models
{
    public class APITicketConversion
    {
        public static Ticket ConvertPOSTTicket(JObject jsonData)
        {
            FrontEndTicket data = new FrontEndTicket();
            try
            {

                //data = jsonData.ToObject<FrontEndTicket>();//this conversion results in null for everything
                
                data.details = jsonData["details"].ToString();
                data.ticketType = jsonData["ticketType"].ToString();
                
                data.category = jsonData["category"].ToString();
                data.subcategory = jsonData["subcategory"].ToString();
                data.tagList = jsonData["tagList"].ToString();
                data.title = jsonData["title"].ToString();

                if(jsonData["priority"] != null )
                {
                    data.priority = jsonData["priority"].ToString();
                }
                if(jsonData["ownerId"] != null)
                {
                    data.owner = jsonData["ownerId"].ToString();
                }
               
            }
            catch(Exception ex)
            {
                return null;
            }
            Ticket ticket = new Ticket(); //made new ticket object
            ticket.TicketId = default(int); //inserting to DB will assign backend ticketID
            ticket.ProjectId = 1; //assuming front end will pass back project ID as int
            ticket.Details = data.details; //assuming coming from comment field
            ticket.Priority = "None"; //we don't know priority yet
            ticket.TicketType = data.ticketType;
            ticket.Category = data.category;
            ticket.SubCategory = data.subcategory; //no subcategory thing in TD currently, might add?
            ticket.Owner = data.owner; //might have to use auth data to get owner/created by info
            ticket.AssignedTo = "Open"; //probably will be null since we don't want users to assign their own tickets
            ticket.TicketStatus = TicketStatus.Active; //assuming ticket is open
            ticket.TagList = data.tagList;
            ticket.CreatedDate = DateTime.Now; //we get the datetime ourselves when new ticket
            ticket.Title = data.title;
            ticket.CreatedBy = data.owner; //might have to use the auth stuff
            ticket.IsHtml = false;
            ticket.CurrentStatusDate = ticket.CreatedDate;
            ticket.CurrentStatusSetBy = ticket.CreatedBy;
            ticket.LastUpdateBy = ticket.CreatedBy;
            ticket.LastUpdateDate = ticket.CreatedDate;
            ticket.AffectsCustomer = true;
            ticket.SemanticId = ticket.CreatedDate.ToString("yyMMddHHmm"); //formatting for semantic numbering
            return ticket;
        }
        public static FrontEndTicket ConvertGETTicket(Ticket ticket)
        {
            FrontEndTicket FETicket = new FrontEndTicket();
            string ticketID;
            /*if (ticket.SemanticId != null)
            {
                ticketID = ticket.SemanticId + ticket.TicketId.ToString();
            }
            else
            {
                ticketID = ticket.CreatedDate.ToString("yyMMddHHmm") + ticket.TicketId.ToString();
            }*/
            ticketID = ticket.CreatedDate.ToString("yyMMddHHmm") + ticket.TicketId.ToString();
            /*uint x; Int64 y;
            if (uint.TryParse(ticketID, out x))
            {
                FETicket.ticketId = (int)x; //gross conversion to string back to int to get around bit shifting
            }
            else
            {
                Int64.TryParse(ticketID, out y);
                FETicket.ticketId = (int)y;
            }*/
            Int64 y = Int64.Parse(ticketID);
            FETicket.ticketId = y;
            FETicket.projectId = ticket.ProjectId;
            FETicket.details = ticket.Details;
            FETicket.priority = ticket.Priority;
            FETicket.ticketType = ticket.TicketType;
            FETicket.category = ticket.Category;
            FETicket.subcategory = ticket.SubCategory; //no subcategory in TicketDesk db, might want to add?
            FETicket.owner = ticket.Owner;
            FETicket.assignedTo = ticket.AssignedTo;
            FETicket.status = ticket.TicketStatus;
            FETicket.tagList = ticket.TagList;
            FETicket.createdDate = ticket.CreatedDate.ToString();
            FETicket.title = ticket.Title;
            FETicket.subcategory = ticket.SubCategory;

            return FETicket;
        }

        public static int ConvertTicketId(Int64 id)
        {
            string sId = id.ToString();
            if(sId.Length < 10)
            {
                //we might be doing some testing with short ints here
                return (int)id;
            }
            //yymmddhhmm
            return int.Parse(sId.Substring(10, sId.Length-10));
        }

        public static FrontEndEvent ConvertEvent(TicketEvent item)
        {
            FrontEndEvent singleEvent = new FrontEndEvent();

            singleEvent.eventId = item.EventId;
            singleEvent.userId = item.EventBy;
            singleEvent.actionText = item.EventDescription;
            singleEvent.date = item.EventDate.ToString();
            singleEvent.comment = item.Comment;
            singleEvent.actionEnum = item.ForActivity.ToString();

            return singleEvent;
        }
    }
    public class FrontEndEvent
    {
        public int eventId { get; set; }
        public string userId { get; set; }
        public string actionText { get; set; }
        public string date { get; set; }
        public string comment { get; set; }
        public string actionEnum { get; set; }

    }
    public class FrontEndTicket
    {
        public Int64 ticketId { get; set; }
        public int projectId { get; set; }
        public string details { get; set; }
        public string priority { get; set; }
        public string ticketType { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string owner { get; set; }
        public string assignedTo { get; set; }
        public TicketStatus status { get; set; }
        public string tagList { get; set; }
        public string title { get; set; }
    }

    public class JList
    {
        public List<FrontEndTicket> list { get; set; }
    }

    public class EventList
    {
        public List<FrontEndEvent> list { get; set; }
    }
}