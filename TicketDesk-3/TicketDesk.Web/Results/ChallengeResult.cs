using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace TicketDesk.Web.Results
{
    /// <summary>
    /// Redirect to oAuth provider login system
    /// </summary>
    public class ChallengeResult : IHttpActionResult
    {
        public ChallengeResult(string loginProvider, ApiController controller)
        {
            LoginProvider = loginProvider;
            Request = controller.Request;
        }

        public string LoginProvider { get; set; }
        public HttpRequestMessage Request { get; set; }

		/// <summary>
		/// Send a challenge to the Provider authentication middleware to modify response to 302 and redirect to provider
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>302 redirect to provider</returns>
		/// </summary>		
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpContext.Current.GetOwinContext().Authentication.Challenge(LoginProvider);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
    }
}