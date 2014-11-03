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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Assembly Resource Attribute
[assembly: System.Web.UI.WebResource("TicketDesk.Engine.Ajax.Extenders.UpdatePanelPopupExtender.UpdatePanelPopupBehavior.js", "text/javascript")]
#endregion

namespace TicketDesk.Engine.Ajax.Extenders
{
    [TargetControlType(typeof(UpdatePanel))]
    public class UpdatePanelPopupExtender : ExtenderBase, IPostBackEventHandler
    {

        [Description("This event is raised anytime the UpdatePanel is hidden as a result of clicking on the body of the page or when a 'close' button (RegisterCloseControl) is clicked")]
        public event EventHandler Close;


        private int _offsetX;
        private int _offsetY;
        private HorizontalAlign _horizontalAlign;
        private VerticalAlign _verticalAlign;
        private Color _calloutColor;
        private Color _calloutBorderColor;
        private CalloutType _calloutType;
        private bool _autoPostBack;
        private bool _sendDataItem;

        #region Properties

        [DefaultValue(0)]
        [Description("The horizontal offset between the UpdatePanel and the control where the UpdatePanel should be positioned")]
        public int OffsetX
        {
            get { return _offsetX; }
            set { _offsetX = value; }
        }

        [DefaultValue(0)]
        [Description("The vertical offset between the UpdatePanel and the control where the UpdatePanel should be positioned")]
        public int OffsetY
        {
            get { return _offsetY; }
            set { _offsetY = value; }
        }

        [DefaultValue(typeof(Color), ""), TypeConverter(typeof(WebColorConverter))]
        [Description("The color of the callout.")]
        public Color CalloutColor
        {
            get { return _calloutColor; }
            set { _calloutColor = value; }
        }

        [DefaultValue(typeof(Color), ""), TypeConverter(typeof(WebColorConverter))]
        [Description("The callout border color.")]
        public Color CalloutBorderColor
        {
            get { return _calloutBorderColor; }
            set { _calloutBorderColor = value; }
        }

        [Description("The type of callout.")]
        public CalloutType CalloutType
        {
            get { return _calloutType; }
            set { _calloutType = value; }
        }

        [Description("Raises the close event when the UpdatePanel is hidden.")]
        [DefaultValue(false)]
        public bool AutoPostBack
        {
            get { return _autoPostBack; }
            set { _autoPostBack = value; }
        }

        [DefaultValue(VerticalAlign.Middle)]
        [Description("The vertical alignment of the the UpdatePanel with the control where the UpdatePanel should be positioned")]
        public VerticalAlign VerticalAlign
        {
            get { return _verticalAlign; }
            set { _verticalAlign = value; }
        }

        [DefaultValue(HorizontalAlign.Right)]
        [Description("The horizontal alignment of the the UpdatePanel with the control where the UpdatePanel should be positioned")]
        public HorizontalAlign HorizontalAlign
        {
            get { return _horizontalAlign; }
            set { _horizontalAlign = value; }
        }

        [IDReferenceProperty(typeof(WebControl))]
        [Description("The ClientID of control for where the UpdatePanel should be positioned")]
        [Browsable(false)]
        public string PositionControlClientID
        {
            get { return ViewState["positionControlClientID"] as string; }
            set { ViewState["positionControlClientID"] = value; }
        }

        [Description("Sets the client side visibility of the UpdatePanel")]
        private bool UpdatePanelVisible
        {
            get { return (bool)(ViewState["UpdatePanelVisible"] ?? false); }
            set { ViewState["UpdatePanelVisible"] = value; }
        }

        private ScriptManager CurrentScriptManager
        {
            get
            {
                ScriptManager sm = ScriptManager.GetCurrent(Page);
                if (sm != null)
                {
                    return sm;
                }
                throw new HttpException("A ScriptManager control must exist on the current page.");
            }
        }
        #endregion

        #region Overrides

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.DesignMode)
            {
                CurrentScriptManager.RegisterAsyncPostBackControl(this);
            }
        }

        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors(Control targetControl)
        {
            ScriptBehaviorDescriptor descriptor =
                new ScriptBehaviorDescriptor("TicketDesk.Engine.Ajax.Extenders.UpdatePanelPopupBehavior", targetControl.ClientID);
            descriptor.AddProperty("clientID", this.ClientID);
            descriptor.AddProperty("uniqueID", this.UniqueID);
            descriptor.AddProperty("offsetX", this.OffsetX);
            descriptor.AddProperty("offsetY", this.OffsetY);
            descriptor.AddProperty("align", this.HorizontalAlign);
            descriptor.AddProperty("valign", this.VerticalAlign);
            descriptor.AddProperty("loadVisible", this.UpdatePanelVisible);
            descriptor.AddProperty("positionElementID", this.PositionControlClientID);
            descriptor.AddProperty("calloutColor", ColorAsHtml(this.CalloutColor));
            descriptor.AddProperty("calloutBorderColor", ColorAsHtml(this.CalloutBorderColor));
            descriptor.AddProperty("calloutType", this.CalloutType);
            descriptor.AddProperty("autoPostBack", this.AutoPostBack);
            return new ScriptDescriptor[] { descriptor };
        }

        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            ScriptReference reference = new ScriptReference(
                "TicketDesk.Engine.Ajax.Extenders.UpdatePanelPopupExtender.UpdatePanelPopupBehavior.js", "TicketDesk.Engine.Ajax.Extenders");
            return new ScriptReference[] { reference };
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!this.DesignMode)
            {
                if (_sendDataItem)
                {
                    SendAsyncData();
                }
            }
            base.OnPreRender(e);
        }

        #endregion

        #region Private

        private void SendAsyncData()
        {
            if (!CurrentScriptManager.IsInAsyncPostBack)
            {
                return;
            }

            var dataItems = new DataItems { PositionElementID = PositionControlClientID, Visible = UpdatePanelVisible };
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(dataItems.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, dataItems);
            string serializedString = Encoding.Default.GetString(ms.ToArray());
            CurrentScriptManager.RegisterDataItem(this, serializedString);
        }

        private string ColorAsHtml(Color c)
        {
            string s = ColorTranslator.ToHtml(c);
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            return s;
        }

        private void OnClose()
        {
            if (Close != null)
            {
                Close(this, EventArgs.Empty);
            }
        }

        private void RaisePostBackEvent(string eventArgument)
        {
            OnClose();
        }

        #endregion

        #region Public methods

        [Description("Register a control which will hide the UpdatePanel on click.")]
        public static void RegisterCloseControl(IAttributeAccessor control, UpdatePanel updatepanel)
        {
            control.SetAttribute("uppHide", "true");
            control.SetAttribute("uppTarget", updatepanel.ClientID);
        }

        [Description("Position and show the UpdatePanel")]
        public void Show()
        {
            if (string.IsNullOrEmpty(this.PositionControlClientID))
            {
                throw new ArgumentNullException("PositionControlClientID", Target_Is_Null);
            }
            this.UpdatePanelVisible = true;
            _sendDataItem = true;
        }

        [Description("Position and show the UpdatePanel at the control specified")]
        public void ShowAt(Control positionControl)
        {
            this.PositionControlClientID = positionControl.ClientID;
            Show();
        }

        [Description("Hide the UpdatePanel on the client")]
        public void Hide()
        {
            this.UpdatePanelVisible = false;
            _sendDataItem = true;
        }

        #endregion


        #region IPostBackEventHandler Members

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            this.RaisePostBackEvent(eventArgument);
        }

        #endregion

        [DataContract]
        class DataItems
        {
            [DataMember]
            internal string PositionElementID { get; set; }

            [DataMember]
            internal bool Visible { get; set; }
        }
    }

    public enum HorizontalAlign
    {
        Right = 0,
        Center = 1,
        Left = 2
    }

    public enum VerticalAlign
    {
        Middle = 0,
        Top = 1,
        Bottom = 2
    }

    public enum CalloutType
    {
        TransparentGradient = 0,
        Solid = 1
    }
}