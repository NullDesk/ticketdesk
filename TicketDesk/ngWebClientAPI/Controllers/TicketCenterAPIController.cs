using System;
using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using ngWebClientAPI.Models;
using Newtonsoft.Json.Linq;

namespace ngWebClientAPI.Controllers
{
    [System.Web.Http.Authorize]
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

        /* Depricated - Use pageList instead... */
        [HttpPost]
        [Route("index")]
        public async Task<List<TicketCenterDTO>> Index(JObject data)
        {
            string userName = System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace(@"clarkpud\", string.Empty);
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

        [HttpPost]
        [Route("pageList")]
        public async Task<List<TicketCenterDTO>> PageList(JObject data)
        {
            int? page = data["page"].ToObject<int?>();
            string listName = data["listName"].ToObject<string>();
            List<Ticket> ticketList = await ticketCenterController.PageList(page, listName);
            List<TicketCenterDTO> tkDTO = TicketCenterDTO.ticketsToTicketCenterDTO(ticketList);
            return tkDTO;
        }

        [HttpPost]
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

        [HttpPost]
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
