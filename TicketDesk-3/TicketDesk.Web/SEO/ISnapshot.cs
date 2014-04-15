using System.Threading.Tasks;
using System.Web;

namespace TicketDesk.Web.SEO
{
    /// <summary>
    /// This interface is the contract that you have implement in order to make your page ajax crawlable for google bots
    /// </summary>
    /// <remarks>
    /// The implementation you´ll find in this site uses Blitline with Azure for SEO Optimization but there are
    /// another valid options like using the same Blitline with Amazon S3 (straighforward) or execute 
    /// a headless browser among others
    /// </remarks>
    /// <see cref="https://developers.google.com/webmasters/ajax-crawling/?hl=es"/>
    public interface ISnapshot
    {
        /// <summary>
        /// Check if the snapshot service is configured
        /// </summary>
        /// <returns>bool</returns>
        bool Configured();

        /// <summary>
        /// Check if is a bot
        /// </summary>
        /// <param name="request">The request object</param>
        /// <returns></returns>
        bool IsBot(HttpRequestBase request);

        /// <summary>
        /// Get the snapshot
        /// </summary>
        Task<string> Get(string url, string userAgent);
    }
}
