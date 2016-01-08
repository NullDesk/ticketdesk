// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.Configuration;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;

namespace TicketDesk.IO
{
    public static class AzureConnectionHelper
    {
        private static CloudStorageAccount _cloudStorageAccount;

        internal static CloudStorageAccount CloudStorageAccount
        {
            get
            {
                if (_cloudStorageAccount == null)
                {
                     var connectionString = CloudConfigConnString ??
                                           ConfigManagerConnString;
                    CloudStorageAccount cloudStorageAccount;
                    if (CloudStorageAccount.TryParse(connectionString, out cloudStorageAccount))
                    {
                        _cloudStorageAccount = cloudStorageAccount;
                    }
                }
                return _cloudStorageAccount;
            }
        }

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
