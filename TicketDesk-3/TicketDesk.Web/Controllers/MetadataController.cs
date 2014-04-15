using System.Web.Http;

using Breeze.WebApi2;

using TicketDesk.Web.Providers;

namespace TicketDesk.Web.Controllers
{
    /// <summary>
    /// The Breeze controller providing access to the Model Metadata
    /// </summary>    
    [BreezeController]
    [AllowAnonymous]
    public class MetadataController : ApiController
    {

        TicketDeskBreezeContext context;

        /// <summary>
        /// ctor
        /// </summary>
        public MetadataController(TicketDeskBreezeContext ctx)
        {
            context = ctx;
        }

        /// <summary>
        /// Model Metadata
        /// </summary>
        /// <returns>string containing the metadata</returns>       
        [HttpGet]
        public string Metadata()
        {
            return context.Metadata();
        }
    }
}