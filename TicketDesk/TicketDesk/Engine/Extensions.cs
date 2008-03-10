// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

namespace TicketDesk.Engine
{
    /// <summary>
    /// Contains extension methods that add capabilities to several other classes.
    /// </summary>
    public static class Extensions
    {

        /// <summary>
        /// Gets a formatted version of the user's name from an identity.
        /// </summary>
        /// <param name="identity">The identity object for the user.</param>
        /// <returns></returns>
        public static string GetFormattedUserName(this System.Security.Principal.IIdentity identity)
        {
            return SecurityManager.GetFormattedUserName(identity.Name);
        }

        /// <summary>
        /// Will get the display name from active directory or the user name from SQL Membership
        /// depending on how the application is configured.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns></returns>
        public static string GetUserDisplayName(this System.Security.Principal.IIdentity identity)
        {
            return SecurityManager.GetUserDisplayName(identity.Name);
        }

        /// <summary>
        /// Gets the user's email address from either SQL membership or active directory depending on how the 
        /// applicaiton is configured.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns></returns>
        public static string GetUserEmailAddress(this System.Security.Principal.IIdentity identity)
        {
            return SecurityManager.GetUserEmailAddress(identity.Name);
        }

        /// <summary>
        /// Formats a regular string as HTML for display on a web page.
        /// </summary>
        /// <remarks>
        /// This is a simple formatter that converts any newlines with HTML line breaks and any double spaces with non-breaking spaces.
        /// 
        /// This is used by ticket details and ticket comment notes to preserve any line-breaks or indentions that were made in original text.
        /// </remarks>
        /// <param name="text">The text to format.</param>
        /// <returns>The text formatted as HTML.</returns>
        public static string FormatAsHtml(this string text)
        {
            //TODO: This is probably a bad idea overall. 
            //      It may be better to simply output these fields using the <pre> HTML tag and simply
            //          use CSS to override the font (so it doesn't come out all mono-space-ugly.
            if(!string.IsNullOrEmpty(text))
            {
                return text.Replace("\n", "<br />").Replace("  ", "&nbsp; ");
            }
            else
            {
                return string.Empty;
            }
        }



    }
}
