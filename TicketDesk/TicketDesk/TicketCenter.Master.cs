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

namespace TicketDesk
{
    public partial class TicketCenter1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string viewMode = Request.QueryString["View"] ?? "unassigned";
            switch(viewMode)
            {
                case "unassigned":
                    UnassignedTicketsMenuLink.Font.Bold = true;
                    break;
                case "assigned":
                    AssignedTicketsMenuLink.Font.Bold = true;
                    break;
                case "status":
                    StatusTicketsMenuLink.Font.Bold = true;
                    break;
                //case "resolved":
                //    ResolvedTicketsMenuLink.Font.Bold = true;
                //    break;
                //case "closed":
                //    ClosedTicketsMenuLink.Font.Bold = true;
                //    break;
                case "tagsandcategories":
                    TagsTicketMenuLink.Font.Bold = true;
                    break;
            }
        }
    }
}
