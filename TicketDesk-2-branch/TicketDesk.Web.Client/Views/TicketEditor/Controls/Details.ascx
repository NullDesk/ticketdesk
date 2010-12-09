<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<% 
    
    
    var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController;
    var currentFlagStatus = Model.CurrentStatus.Replace(" ", "").ToLower();
    if (string.IsNullOrEmpty(Model.AssignedTo))
    {
        currentFlagStatus = "unassigned";
    }
    var root = ViewData["siteRootUrl"] as string;
    var flagUrl = root + Url.Content(string.Format("~/Content/{0}Flag.png", Url.Encode(currentFlagStatus)));

    var ticketUrl = root + Url.Content(string.Format("~/Ticket/{0}", Model.TicketId.ToString()));

    var detailsHeight = (ViewData["formatForSearch"] == null) ? 180 : 70;
    
%>
<div class="ticketDetailsHeaderOuter">
    <div class="ticketDetailsHeaderInner">
        <table class="ticketDetailsHeaderTable" cellpadding="0" cellspacing="0">
            <tr>
                <td rowspan="3" class="<%=  currentFlagStatus%>Flag statusFlag">
                    <img alt="<%: currentFlagStatus %>" src="<%= flagUrl %>" />
                </td>
                <td class="ticketDetailsHeaderId" style="">
                    <a href="<%= ticketUrl %>">Ticket: #<%: Model.TicketId %>
                        -
                        <%: Model.Category%>
                        <%: Model.Type%>
                    </a>
                </td>
                <td class="ticketDetailsHeaderPriority">
                    <div>
                        Priority:
                        <%: Model.Priority%>
                    </div>
                </td>
            </tr>
            <tr class="ticketDetailsHeaderTitle">
                <td colspan="2">
                    <%: Model.Title%>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="ticketDetailsHeaderInfo">
                    <table class="ticketDetailsHeaderInfoTable" style="border-top: solid 1px #B3CBDF;">
                        <tr>
                            <td class="ticketDetailsHeaderInfoLabel">
                                Assigned To:
                            </td>
                            <td class="ticketDetailsHeaderInfoText">
                                <%: Model.GetAssignedToDisplayName(controller)%><br />
                            </td>
                            <td rowspan="2" class="ticketDetailsHeaderTagsArea">
                                <div>
                                    Tags:
                                    <%: Model.TagList%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="ticketDetailsHeaderInfoLabel">
                                Owned By:
                            </td>
                            <td class="ticketDetailsHeaderInfoText">
                                <%: Model.GetOwnerDisplayName(controller)%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>
<div class="ticketDetailsOuter" style="">
    <div class="ticketDetailsInner">
        <table class="formatTable" cellpadding="0" cellspacing="0">
            <tbody>
                <tr>
                    <td class="ticketDetailsArea" colspan="2">
                        <div id="detailsText" <%if(ViewData["formatForEmail"] == null){ %> style="height: <%= detailsHeight%>px;"
                            <% } %>>
                            <%= Model.HtmlDetails%>
                        </div>
                    </td>
                </tr>
                <%
                    if (ViewData["formatForEmail"] == null && ViewData["formatForSearch"] == null)
                    { 
                %>
                <tr id="detailTextExpander" class="expanderButton" style="height: 20px; display: none;"
                    onclick="expandDetails();">
                    <td colspan="2" style="height: 20px; padding: 0px; border-bottom: solid 1px #D6D6D6;">
                    </td>
                </tr>
                <%
                    } 
                %>
            </tbody>
        </table>
    </div>
</div>
