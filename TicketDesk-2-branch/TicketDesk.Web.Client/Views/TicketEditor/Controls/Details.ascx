<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<% var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController; %>
<div style="border-bottom: solid 2px #134A8A; margin-top: 0px;">
    <div style="color: #0B294F; background-color: #E1EBF2;">
        <table style="width: 100%;  font-size: 9pt;" cellpadding="0" cellspacing="0">
            <tr >
                <td rowspan="3" style="min-height:90px;width: 20px; border-right: solid 1px #B3CBDF" class="<%=  Html.Encode(Model.CurrentStatus.Replace(" ", "").ToLower())%>Flag">
                    <img alt="<%: Model.CurrentStatus %>" src="<%= Url.Content(string.Format("~/Content/{0}Flag.png", Url.Encode(Model.CurrentStatus.ToLower()))) %>" />
                </td>
                <td style=";white-space: nowrap; vertical-align: top; font-weight: bold; padding: 5px;">
                    Ticket: #<%: Model.TicketId %>
                    -
                    <%: Model.Category%>
                    <%: Model.Type%>
                </td>
                <td style="padding: 5px;">
                    <div style="float: right;">
                        Priority:
                        <%: Model.Priority%>
                    </div>
                </td>
            </tr>
            <tr style="height:45px;">
                <td colspan="2" style="padding-left: 20px; padding-bottom: 8px; padding-right: 8px;
                    padding-top: 3px; font-size: 9pt;">
                    <%: Model.Title%>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="background-color: #EEF3F7; border-top: solid 1px #B3CBDF">
                    <table style="width: 100%; color: #444; font-size:8pt;" >
                        <tr>
                            <td style="white-space: nowrap; text-align: right;">
                                Assigned To:
                            </td>
                            <td style="white-space: nowrap;">
                                <%: Model.GetAssignedToDisplayName(controller)%><br />
                            </td>
                            <td rowspan="2" style="width: 100%;">
                                <div style="text-align: right; width: 100%; font-size: 8pt;">
                                    Tags:
                                    <%: Model.TagList%></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="white-space: nowrap; text-align: right;">
                                Owned By:
                            </td>
                            <td style="white-space: nowrap;">
                                <%: Model.GetOwnerDisplayName(controller)%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    
</div>
<div style="background-color: #fff; padding-bottom: 0px;">
    <div>
        <table style="width: 100%;" class="formatTable" cellpadding="0" cellspacing="0">
            <tbody>
                <tr>
                    <td colspan="2" style="padding: 0px; border-bottom: solid 1px #D6D6D6;">
                        <div id="detailsText" style="height: 180px; overflow: auto; padding: 3px;">
                            <%= Model.HtmlDetails%>
                        </div>
                    </td>
                </tr>
                <tr id="detailTextExpander" class="expanderButton" style="height: 20px; display: none;"
                    onclick="expandDetails();">
                    <td colspan="2" style="height: 20px; padding: 0px; border-bottom: solid 1px #D6D6D6;">
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
