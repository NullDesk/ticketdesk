using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketDesk.Domain.Model;

namespace ngWebClientAPI.Models
{
    public class TicketCenterDTO
    {
        /*This is strictly the data we want to transer to the ticket center.*/
        public Int64 ticketId { get; set; }
        public string title { get; set; }
        public TicketStatus status { get; set; }
        public string owner { get; set; }
        public string assignedTo { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string priority { get; set; }
        public string createdDate { get; set; }  
        public string LastUpdateDate { get; set; }


        public static List<TicketCenterDTO> ticketsToTicketCenterDTO(List<Ticket> tickets)
        {
            List<TicketCenterDTO> ticketCenterDTOs = new List<TicketCenterDTO>();
            foreach(var tk in tickets)
            {
                TicketCenterDTO tkDataTransfer = new TicketCenterDTO();
                tkDataTransfer.ticketId = tk.TicketId;
                tkDataTransfer.title = tk.Title;
                tkDataTransfer.status = tk.TicketStatus;
                tkDataTransfer.owner = tk.Owner;
                tkDataTransfer.assignedTo = tk.AssignedTo;
                tkDataTransfer.category = tk.Category;
                tkDataTransfer.subcategory = tk.SubCategory;
                tkDataTransfer.priority = tk.Priority;
                tkDataTransfer.createdDate = tk.CreatedDate.ToString("yyMMddHHmm");
                tkDataTransfer.LastUpdateDate = tk.LastUpdateDate.ToString("yyMMddHHmm");
                ticketCenterDTOs.Add(tkDataTransfer);
            }
            return ticketCenterDTOs;
        }
    }
}