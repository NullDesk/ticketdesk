// original code by: Raj Kaimal 
//     http://weblogs.asp.net/rajbk/

//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so.

//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.

// Modified by: Stephen M. Redd
//      http://www.reddnet.net



using System;
using System.ComponentModel;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;

#region Assembly Resource Attribute
[assembly: System.Web.UI.WebResource("TicketDesk.Engine.Ajax.Extenders.UpdateProgressOverlayExtender.UpdateProgressOverlayBehavior.js", "text/javascript")]
#endregion

namespace TicketDesk.Engine.Ajax.Extenders
{
    [TargetControlType(typeof(UpdateProgress))]
    public class UpdateProgressOverlayExtender : ExtenderBase
    {

        private string _controlToOverlayID;
        private string _cssClass;
        private bool _centerOnContainer = true;
        private string _elementToCenterID;
        private OverlayType _overlayType;

        [Description("CSS class to apply to the UpdateProgress control")]
        public string CssClass
        {
            get { return _cssClass; }
            set { _cssClass = value; }
        }

        [Description("Control that the UpdateProgress control will Overlay. If left blank and OverlayType is not \"Browser\", the AssociatedUpdatePanelID of the TargetControl is used.")]
        public string ControlToOverlayID
        {
            get { return _controlToOverlayID; }
            set { _controlToOverlayID = value; }
        }

        [Description("Value indicating if the control should center contents of the progress template within the container being overlaid")]
        public bool CenterOnContainer
        {
            get { return _centerOnContainer; }
            set { _centerOnContainer = value; }
        }

        [Description("An element witin the ProgressUpdate template that should be centered")]
        public string ElementToCenterID 
        {
            get
            {
                return _elementToCenterID;
            }
            set
            {
                _elementToCenterID = value;
            }
        }


        [Description("Overlay over a specific control or the browser viewing area")]
        [DefaultValue(typeof(OverlayType), "Control")]
        public OverlayType OverlayType
        {
            get { return _overlayType; }
            set { _overlayType = value; }
        }

        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors(Control targetControl)
        {
            ScriptBehaviorDescriptor descriptor =
                new ScriptBehaviorDescriptor("TicketDesk.Engine.Ajax.Extenders.UpdateProgressOverlayBehavior", targetControl.ClientID);

            UpdateProgress up = targetControl as UpdateProgress;
            string asscUpdatePanelClientId = string.IsNullOrEmpty(up.AssociatedUpdatePanelID) ?
                null : GetClientId(up.AssociatedUpdatePanelID, "AssociatedUpdatePanelID");

           

            string controlToOverlayID = null;
            if (_overlayType != OverlayType.Browser)
            {
                controlToOverlayID = string.IsNullOrEmpty(ControlToOverlayID) ?
                    asscUpdatePanelClientId : GetClientId(ControlToOverlayID, "ControlToOverlayID");
            }

            descriptor.AddProperty("controlToOverlayID", controlToOverlayID);
            descriptor.AddProperty("associatedUpdatePanelID", asscUpdatePanelClientId);
            descriptor.AddProperty("displayAfter", up.DisplayAfter);
            descriptor.AddProperty("targetCssClass", this.CssClass);
            descriptor.AddProperty("centerOnContainer", this.CenterOnContainer);
            descriptor.AddProperty("elementToCenterID", this.ElementToCenterID);

            return new ScriptDescriptor[] { descriptor };
        }

        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            ScriptReference reference = new ScriptReference(
                "TicketDesk.Engine.Ajax.Extenders.UpdateProgressOverlayExtender.UpdateProgressOverlayBehavior.js", "TicketDesk.Engine.Ajax.Extenders");
            return new ScriptReference[] { reference };
        }

        private string GetClientId(string controlID, string propertyName)
        {
            Control control = base.FindControlHelper(controlID);
            if (control == null)
            {
                throw new HttpException(
                   String.Format(Control_Not_Found, controlID,
                       propertyName, this.ID));
            }

            return control.ClientID;
        }
    }

    public enum OverlayType
    {
        Control = 0,
        Browser = 1
    }
}