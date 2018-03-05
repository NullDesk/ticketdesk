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
    }
}