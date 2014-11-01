<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<% 
    var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController;

    bool isOdd = (bool)ViewData["odd"];
    string rowClass = isOdd ? "ticketListGridOddRow" : "ticketListGridEvenRow";
%>
<tr class="<%= rowClass %> clickable">
    
    <td>
        <%: Model.TicketId%>
    </td>
    <td>
        <%: Model.Type%>
    </td>
    <td style="white-space: normal; width: 100%;">
         <a  href='<%= Url.Action("Display", "TicketEditor", new { ID = Model.TicketId })%>'><%: Model.Title%></a>
    </td>
    <td>
        <%: Model.GetOwnerDisplayName(controller)%>
    </td>
    <td>
        <%: Model.GetAssignedToDisplayName(controller)%>
    </td>
    <td>
        <%: Model.CurrentStatus%>
    </td>
    <td>
        <%: Model.Category%>
    </td>
    <td>
        <%: Model.Priority%>
    </td>
    <td>
        <%: Model.CreatedDate.ToString("MM/dd/yy")%><br />
        <%: Model.CreatedDate.ToString("hh:mm tt")%>
    </td>
    <td>
        <%: Model.LastUpdateDate.ToString("MM/dd/yy")%><br />
        <%: Model.LastUpdateDate.ToString("hh:mm tt")%>
    </td>
</tr>


