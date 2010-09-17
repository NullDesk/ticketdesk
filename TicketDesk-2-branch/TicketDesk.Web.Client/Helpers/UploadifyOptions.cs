using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketDesk.Web.Client.Helpers
{
    /// <summary>
    /// Defines all options for <see cref="HtmlHelperExtensions.Uploadify"/>.
    /// </summary>
    public class UploadifyOptions
    {
        #region Public Properties

        /// <summary>
        /// The URL to the action that will process uploaded files.
        /// </summary>
        public string UploadUrl { get; set; }

        /// <summary>
        /// The file extensions to accept.
        /// </summary>
        public string FileExtensions { get; set; }

        /// <summary>
        /// Description corresponding to <see cref="FileExtensions"/>.
        /// </summary>
        public string FileDescription { get; set; }

        /// <summary>
        /// The ASP.NET forms authentication token.
        /// </summary>
        /// <example>
        /// You can get this in a view using:
        /// <code>
        /// Request.Cookies[FormsAuthentication.FormsCookieName].Value
        /// </code>
        /// You should check for the existence of the cookie before accessing
        /// its value.
        /// </example>
        public string AuthenticationToken { get; set; }

        /// <summary>
        /// The name of a JavaScript function to call if an error occurs
        /// during the upload.
        /// </summary>
        public string ErrorFunction { get; set; }

        /// <summary>
        /// The name of a JavaScript function to call when an upload
        /// completes successfully. 
        /// </summary>
        public string CompleteFunction { get; set; }

        /// <summary>
        /// Gets or sets the button text.
        /// </summary>
        /// <value>The button text.</value>
        public string ButtonText { get; set; }



        #endregion
    }
}