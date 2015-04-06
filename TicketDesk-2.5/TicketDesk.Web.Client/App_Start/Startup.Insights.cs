using System.Configuration;
using Microsoft.ApplicationInsights.Extensibility;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {

        public void ConfigureApplicationInsights()
        {
            var ikey = ConfigurationManager.AppSettings["ticketdesk:InsightsInstrumentationKey"];
            if (ikey == null)
            {
                TelemetryConfiguration.Active.DisableTelemetry = true;
            }
            else{
               TelemetryConfiguration.Active.InstrumentationKey = ikey;
            }
        }
    }
}