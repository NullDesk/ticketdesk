<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<% var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController; %>

<div class="activityHeadWrapper">
    <div class="activityHead">
        Ticket Stats:
    </div>
</div>

<div class="activityBody ticketStatsBody">
    <table class="statsTable">
        <tr>
            <th>
                <label for="LastUpdateBy">
                    Status:
                </label>
            </th>
            <td style="" class="textField">
                <%: Model.CurrentStatus%>
            </td>
        </tr>
        <tr>
            <th>
                <label for="LastUpdateBy">
                    Updated By:
                </label>
            </th>
            <td style="" class="textField">
                <%: Model.GetLastUpdateByDisplayName(controller)%>
            </td>
        </tr>
        <tr>
            <th>
                <label for="LastUpdateDate">
                    Updated Date:
                </label>
            </th>
            <td style="" class="textField">
                <%: Model.LastUpdateDate.ToShortDateString()%>
                <%: Model.LastUpdateDate.ToShortTimeString()%>
            </td>
        </tr>
        <tr>
            <th>
                <label for="CurrentStatusSetBy">
                    Status By:
                </label>
            </th>
            <td style="" class="textField">
                <%: Model.GetCurrentStatusByDisplayName(controller)%>
            </td>
        </tr>
        <tr>
            <th>
                <label for="CurrentStatusDate">
                    Status Date:
                </label>
            </th>
            <td style="" class="textField">
                <%: Model.CurrentStatusDate.ToShortDateString()%>
                <%: Model.CurrentStatusDate.ToShortTimeString()%>
            </td>
        </tr>
        <tr>
            <th>
                <label for="CreatedBy">
                    Created by:
                </label>
            </th>
            <td style="" class="textField">
                <%: Model.GetCreatedByDisplayName(controller)%>
            </td>
        </tr>
        <tr>
            <th>
                <label for="CreatedDate">
                    Created Date:
                </label>
            </th>
            <td style="" class="textField">
                <%: Model.CreatedDate.ToShortDateString()%>
                <%: Model.CreatedDate.ToShortTimeString()%>
            </td>
        </tr>
    </table>
</div>
