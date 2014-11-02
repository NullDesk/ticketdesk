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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace TicketDesk.Engine.Controls
{
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:KeepAlive runat=server />")]
    public class KeepAlive : WebControl
    {

        public KeepAlive()
            : base(HtmlTextWriterTag.Img)
        {
        }

        /// <summary>
        /// Gets or sets the number of milliseconds to wait between refreshes. Default is one minute
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(60000)]
        public int Interval
        {
            get
            {
                return (int)(ViewState["Interval"] ?? 60000);
            }
            set
            {
                ViewState["Interval"] = value;
            }
        }

        /// <summary>
        /// Path to the keep alive HttpHandler
        /// </summary>
        [Category("Behavior")]
        [DefaultValue("~/KeepAlive.ashx")]
        public string HandlerUrl
        {
            get
            {
                return (string)(ViewState["HandlerUrl"] ?? "~/KeepAlive.ashx");
            }
            set
            {
                ViewState["HandlerUrl"] = value;
            }
        }

        

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.Style.Add(HtmlTextWriterStyle.Display, "none");
            

            
            Page.ClientScript.RegisterStartupScript(typeof(KeepAlive), "KeepAliveStartUp",
                string.Format(@"window.setInterval('KeepAlive(\'{0}\', \'{1}\');', {2} );",
                ClientID, ResolveClientUrl(HandlerUrl), Interval), true);

            Page.ClientScript.RegisterClientScriptResource(typeof(KeepAlive), "TicketDesk.KeepAlive.js");

        }


        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Src, ResolveClientUrl(HandlerUrl));
            writer.AddAttribute(HtmlTextWriterAttribute.Alt, "keep alive image");
            base.RenderBeginTag(writer);
            base.RenderEndTag(writer);
        }

    }
}
