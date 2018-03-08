using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketDesk.Domain.Model;
using Newtonsoft.Json;

namespace ngWebClientAPI.Models
{
    public class APITicketConversion
    {
        public static Ticket ConvertPOSTTicket(string jsonData)
        {
            FrontEndTicket data;
            try
            {
                data = JsonConvert.DeserializeObject<FrontEndTicket>(jsonData);
            }
            catch
            {
                // misconfigured ticket sent from front end
                return null;
            }
            Ticket ticket = new Ticket(); //made new ticket object
            ticket.TicketId = 1; //this will have to change, not sure how TD currently numbering tickets
            ticket.ProjectId = data.projectId; //assuming front end will pass back project ID as int
            ticket.Details = data.comment; //assuming coming from comment field
            ticket.Priority = null; //we don't know priority yet
            ticket.TicketType = data.ticketType;
            ticket.Category = data.category;
            //data.subcategory; //no subcategory thing in TD currently, might add?
            ticket.Owner = data.owner; //might have to use auth data to get owner/created by info
            ticket.AssignedTo = null; //probably will be null since we don't want users to assign their own tickets
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
            return ticket;
        }
        public static FrontEndTicket ConvertGETTicket(Ticket ticket)
        {
            FrontEndTicket FETicket = new FrontEndTicket();
            FETicket.ticketId = ticket.TicketId;
            FETicket.projectId = ticket.ProjectId;
            FETicket.comment = ticket.Details;
            FETicket.priority = ticket.Priority;
            FETicket.ticketType = ticket.TicketType;
            FETicket.category = ticket.Category;
            FETicket.subcategory = null; //no subcategory in TicketDesk db, might want to add?
            FETicket.owner = ticket.Owner;
            FETicket.assignedTo = ticket.AssignedTo;
            FETicket.status = ticket.TicketStatus;
            FETicket.tagList = ticket.TagList;
            FETicket.createdDate = ticket.CreatedDate.ToString();
            FETicket.title = ticket.Title;
            return FETicket;
        }
    }

    public class FrontEndTicket
    {
        public int ticketId { get; set; }
        public int projectId { get; set; }
        public string comment { get; set; }
        public string details { get; set; }
        public string priority { get; set; }
        public string ticketType { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string owner { get; set; }
        public string assignedTo { get; set; }
        public TicketStatus status { get; set; }
        public string tagList { get; set; }
        public string createdDate { get; set; }
        public string title { get; set; }
    }
}