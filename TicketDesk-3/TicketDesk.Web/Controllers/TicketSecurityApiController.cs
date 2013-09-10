using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Breeze.WebApi;
using TicketDesk.Domain.Identity;

namespace TicketDesk.Web.Controllers
{
    [BreezeController]
    [Authorize]
    public class TicketSecurityApiController : ApiController
    {
        private readonly EFContextProvider<TicketDeskIdentityContext> _contextProvider =
            new EFContextProvider<TicketDeskIdentityContext>();

        [HttpGet]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        [HttpGet]
        public async Task<IEnumerable<TdUser>> SubmitterList()
        {
            return await _contextProvider.Context.GetTdSubmitters();
        }

        [HttpGet]
        public async Task<IEnumerable<TdUser>> StaffList()
        {
            return await _contextProvider.Context.GetTdStaff();
        }

    }
}
