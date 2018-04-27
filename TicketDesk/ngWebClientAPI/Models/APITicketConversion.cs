using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketDesk.Domain.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ngWebClientAPI.Models
{
    public class APITicketConversion
    {
        public static Ticket ConvertPOSTTicket(JObject jsonData, string userName)
        {
            FrontEndTicket data = new FrontEndTicket();
            List<TicketTag> tt = new List<TicketTag>();
            try
            {
                
                data.details = jsonData["details"].ToString();
                data.ticketType = jsonData["ticketType"].ToString();
                
                data.category = jsonData["category"].ToString();
                
                data.title = jsonData["title"].ToString();

                if(jsonData["subcategory"] != null && !String.IsNullOrEmpty(jsonData["subcategory"].ToString()))
                {
                    data.subcategory = jsonData["subcategory"].ToString();
                }

                if (jsonData["tagList"] != null && !String.IsNullOrEmpty(jsonData["tagList"].ToString()))
                {
                    
                    List<string> ts = jsonData["tagList"].ToString().Split(',').ToList();

                    foreach (var name in ts)
                    {
                        tt.Add(new TicketTag() { TagName = name });
                    }

                }
                else
                {
                    //No tag list received 
                    tt.Add(new TicketTag() { TagName = "UnTagged" });
                }
                
                if ( jsonData["priority"] != null && !String.IsNullOrEmpty(jsonData["priority"].ToString()))
                {
                    
                    data.priority = jsonData["priority"].ToString();
                }
                if (jsonData["assignedTo"] != null && !String.IsNullOrEmpty(jsonData["assignedTo"].ToString()))
                {
                    
                    data.assignedTo = jsonData["assignedTo"].ToString();
                }

                if (jsonData["ownerId"] != null && !String.IsNullOrEmpty(jsonData["ownerId"].ToString()))
                {
                    data.ownerId = jsonData["ownerId"].ToString();
                } else
                {
                    data.ownerId = (userName != null) ? userName : "NoName";
                }


            }
            catch(Exception ex)
            {
                return null;
            }

            DateTime now = DateTime.Now;

            Ticket ticket = new Ticket
            {
                TicketId = default(int),
                ProjectId = 1,
                Title = (data.title != null) ? data.title : "DefaultTicket",
                AffectsCustomer = false,
                Category = (data.category != null) ? data.category : "DefaultCategory",
                SubCategory = (data.subcategory != null) ? data.subcategory : "",
                CreatedBy = data.ownerId,
                TicketStatus = TicketStatus.Active,
                CreatedDate = now,
                CurrentStatusDate = now,
                CurrentStatusSetBy = data.ownerId,
                Details = data.details,
                IsHtml = false,
                LastUpdateBy = data.ownerId,
                LastUpdateDate = now,
                Owner = data.ownerId,
                Priority = (data.priority != null) ? data.priority : "unassigned",
                AssignedTo = (data.assignedTo != null) ? data.assignedTo : "UnAssigned",
                TagList = (data.tagList != null) ? data.tagList : "UnTagged",
                TicketTags = tt,
                TicketType = (data.ticketType != null) ? data.ticketType : "DefaultType",
                TicketEvents = new[] { TicketEvent.CreateActivityEvent(data.ownerId, TicketActivity.Create, null, null, null) },
                SemanticId = now.ToString("yyMMddHHmm"), /*Formatting for semantic numbering.*/
            };

            return ticket;
        }
        public static FrontEndTicket ConvertGETTicket(Ticket ticket)
        {
            FrontEndTicket FETicket = new FrontEndTicket();
            string ticketID;

            ticketID = ticket.CreatedDate.ToString("yyMMddHHmm") + ticket.TicketId.ToString();

            Int64 y = Int64.Parse(ticketID);
            FETicket.ticketId = y;
            FETicket.projectId = ticket.ProjectId;
            FETicket.details = ticket.Details;
            FETicket.priority = ticket.Priority;
            FETicket.ticketType = ticket.TicketType;
            FETicket.category = ticket.Category;
            FETicket.subcategory = ticket.SubCategory; //no subcategory in TicketDesk db, might want to add?
            FETicket.ownerId = ticket.Owner;
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
        public string ownerId { get; set; }
        public string assignedTo { get; set; }
        public TicketStatus status { get; set; }
        public string tagList { get; set; }
        public string createdDate { get; set; }
        public string title { get; set; }
    }

    public class POSTTicketResult
    {
        public HttpStatusCode httpCode;
        public Int64 ticketID;
        public string errorMessage;
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