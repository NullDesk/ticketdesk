using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using TicketDesk.Engine;

namespace TicketDesk.Controls
{
    public partial class TagPicker : System.Web.UI.UserControl
    {
        public string TagList
        {
            get
            {
                return TagsTextBox.Text;
            }
            set
            {
                TagsTextBox.Text = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            TagsPanel.Visible = (SecurityManager.IsStaffOrAdmin || SecurityManager.SubmitterCanEditTags);
        }
    }
}