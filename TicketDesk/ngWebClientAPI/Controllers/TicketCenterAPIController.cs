using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using ngWebClientAPI.Models;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

using X.PagedList;

namespace ngWebClientAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/tickets")]
    public class TicketCenterAPIController : ApiController
    {
        private TicketCenterController ticketCenterController;

        public TicketCenterAPIController()
        {
            TicketDeskContextSecurityProvider secur = new TicketDeskContextSecurityProvider();
            ticketCenterController = new TicketCenterController(new TdDomainContext(secur));
        }

        [Route("reset-user-lists")]
        public async Task<List<TicketCenterDTO>> ResetUserLists()
        {
            List<Ticket> ticketList = await ticketCenterController.ResetUserLists();
            List<TicketCenterDTO> tkDTO = TicketCenterDTO.ticketsToTicketCenterDTO(ticketList);
            return tkDTO;
            
        }

        [HttpPost]
        [Route("index")]
        public async Task<List<TicketCenterDTO>> Index(JObject data)
        {
            int? page = data["page"].ToObject<int?>();
            string listName = null;

            if (data["priority"] != null && !String.IsNullOrEmpty(data["priority"].ToString()))
            {
                listName = data["listName"].ToObject<string>();
            } 
            List<Ticket> ticketList = await ticketCenterController.Index(page, listName);
            List<TicketCenterDTO> tkDTO = TicketCenterDTO.ticketsToTicketCenterDTO(ticketList);
            return tkDTO;
        }

        [HttpGet]
        [Route("pageList")]
        public async Task<List<TicketCenterDTO>> PageList(JObject data)
        {
            int? page = data["page"].ToObject<int?>();
            string listName = data["listName"].ToObject<string>();
            List<Ticket> ticketList = await ticketCenterController.PageList(page, listName);
            List<TicketCenterDTO> tkDTO = TicketCenterDTO.ticketsToTicketCenterDTO(ticketList);
            return tkDTO;
        }

        [Route("filterList")]
        public async Task<List<TicketCenterDTO>> filterlist(JObject data)
        {
            string listName = data["listName"].ToObject<string>();
            int pageSize = data["pageSize"].ToObject<int>();
            string ticketStatus = data["ticketStatus"].ToObject<string>();
            string owner = data["owner"].ToObject<string>();
            string assignedTo = data["assignedTo"].ToObject<string>();

            List<Ticket> ticketList = await ticketCenterController.FilterList(listName, pageSize, ticketStatus, owner, assignedTo);
            List<TicketCenterDTO> tkDTO = TicketCenterDTO.ticketsToTicketCenterDTO(ticketList);
            return tkDTO;
        }

        [Route("sortList")]
        public async Task<List<TicketCenterDTO>> SortList(JObject data)
        {
            int? page = data["page"].ToObject<int?>();
            string listName = data["listName"].ToObject<string>();
            string columnName = data["columnName"].ToObject<string>();
            bool isMultiSort = data["isMultiSort"].ToObject<bool>();

            List<Ticket> ticketList = await ticketCenterController.SortList(page, listName, columnName, isMultiSort);
            List< TicketCenterDTO> tkDTO =  TicketCenterDTO.ticketsToTicketCenterDTO(ticketList);

            return tkDTO;
        }
    }
}
