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

namespace TicketDesk.Controls
{
    public partial class EditDetails : System.Web.UI.UserControl
    {
        
        public string ValidationGroup
        {
            get
            {
                return DetailsTextBox.ValidationGroup;
            }
            set
            {
                DetailsTextBox.ValidationGroup = value;
            }
        }
        public string Details
        {
            get
            {
                return DetailsTextBox.Text;

            }
            set
            {
                DetailsTextBox.Text = value;
          
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

       

    }
}