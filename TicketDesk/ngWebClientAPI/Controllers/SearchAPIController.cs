using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Search.Common;
using TicketDesk.Localization.Controllers;
using Newtonsoft.Json.Linq;
using ngWebClientAPI.Models;

namespace ngWebClientAPI.Controllers
{
    [System.Web.Http.RoutePrefix("api/search")]
    public class SearchAPIController : ApiController
    {
        private TdDomainContext Context { get; set; }
        public SearchAPIController()
        {
            Context = new TdDomainContext();
        }

        [Route("")]
        [HttpPost]
        public async /*Task<IEnumerable<Ticket>>*/ Task<List<ngWebClientAPI.Models.TicketCenterDTO>> Index(JObject data)
        {
            string term = data["term"].ToObject<string>();
            //List<TicketCenterDTO> tkDTO = TicketCenterDTO.ticketsToTicketCenterDTO(ticketList);

            //var projectId = await Context.UserSettingsManager.GetUserSelectedProjectIdAsync(Context);
            if (!string.IsNullOrEmpty(term))
            {
                var model = await TdSearchContext.Current.SearchAsync(Context.Tickets.Include(t => t.Project), term, 1);

                List<TicketCenterDTO> tkDTO = TicketCenterDTO.ticketsToTicketCenterDTO(model.ToList());
                return tkDTO;
            }
            return null;
        }
    }
}
