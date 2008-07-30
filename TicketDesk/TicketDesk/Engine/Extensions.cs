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

using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Collections.Generic;
namespace TicketDesk.Engine
{
    /// <summary>
    /// Contains extension methods that add capabilities to several other classes.
    /// </summary>
    public static class Extensions
    {
        
        public static string ConvertPascalCaseToFriendlyString(this string stringToConvert)
        {
            List<char> cFriendlyName = new List<char>();
            char[] cName = stringToConvert.ToCharArray();
            for(int cIdx = 0; cIdx < cName.Length; cIdx++)
            {
                char c = cName[cIdx];
                if(cIdx > 0 && char.IsUpper(c))
                {
                    cFriendlyName.Add(' ');
                }
                cFriendlyName.Add(c);
            }
            return new string(cFriendlyName.ToArray());
        }

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

        public static List<Control> GetControls(this Control control, bool includeChildren)
        {
            Control[] ctrlArr = new Control[control.Controls.Count];
            control.Controls.CopyTo(ctrlArr, 0);
            List<Control> controls = new List<Control>(ctrlArr);
            if(includeChildren)
            {
                foreach(Control ctrl in ctrlArr)
                {
                    controls.AddRange(ctrl.GetControls(includeChildren));
                }
            }
            return controls;
        }

        public static Control GetControl(this Control control, string controlId, bool searchChildren)
        {
            Control retControl = null;
            List<Control> controls = control.GetControls(searchChildren);
            foreach(Control ctrl in controls)
            {
                if(ctrl.ID == controlId)
                {
                    retControl = ctrl;
                    break;
                }
            }
            return retControl;
        }

    }
}
