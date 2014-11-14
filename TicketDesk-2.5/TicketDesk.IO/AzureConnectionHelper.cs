using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;

namespace TicketDesk.IO
{
    public class AzureConnectionHelper
    {
        /// <summary>
        /// Gets the connection string from cloud configuration manager if it can see it.
        /// </summary>
        /// <remarks>
        /// Typically, this can only get connections defined as an appSetting in web.config or app.config files
        /// </remarks>
        /// <value>The cloud configuration connection string.</value>
        public static string CloudConfigConnString
        {
            get { return CloudConfigurationManager.GetSetting("AzureStorage"); }
        }

        /// <summary>
        /// Gets the connection string from regular configuration manager.
        /// </summary>
        /// <remarks>
        /// This get the connection from the connection strings section of web.config or app.config
        /// </remarks>
        /// <value>The configuration manager connection string.</value>
        public static string ConfigManagerConnString
        {
            get
            {
                var conn = ConfigurationManager.ConnectionStrings["AzureStorage"];
                return conn == null ? null : conn.ConnectionString;
            }
        }
    }
}
