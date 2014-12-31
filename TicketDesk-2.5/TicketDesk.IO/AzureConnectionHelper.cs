// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.Configuration;
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
