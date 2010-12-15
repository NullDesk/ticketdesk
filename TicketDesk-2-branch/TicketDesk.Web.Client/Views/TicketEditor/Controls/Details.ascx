<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<% 
    
    
    var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController;
    var currentFlagStatus = Model.CurrentStatus.Replace(" ", "").ToLower();
    if (string.IsNullOrEmpty(Model.AssignedTo) && Model.CurrentStatus != "Closed")
    {
        currentFlagStatus = "unassigned";
    }
    var root = ViewData["siteRootUrl"] as string;

    var flagUrl = root + Url.Content(string.Format("~/Content/{0}Flag.png", Url.Encode(currentFlagStatus)));

    var ticketUrl = root + Url.Content(string.Format("~/Ticket/{0}", Model.TicketId.ToString()));

    var detailsHeight = (ViewData["formatForSearch"] == null) ? 200 : 70;
    
%>
<div class="ticketDetailsHeaderOuter">
    <div class="<%=  currentFlagStatus%>Flag ticketDetailsHeaderInner">
        <div class="statusFlag">
            <img alt="<%: currentFlagStatus %>" src="<%= flagUrl %>" />
        </div>
        <div class="ticketDetailsTopper">
            <div class="ticketDetailsHeader">
                <%
                    if (!string.IsNullOrEmpty(Model.Priority))
                    {     
                %>
                <div class="ticketDetailsHeaderPriority">
                    <div>
                        Priority:
                        <%: Model.Priority%>
                    </div>
                </div>
                <%
                    }
                %>
                <div class="ticketDetailsHeaderId">
                    <a href="<%= ticketUrl %>">Ticket: #<%: Model.TicketId %>
                        -
                        <%: Model.Category%>
                        <%: Model.Type%></a>
                </div>
                <div class="ticketDetailsHeaderTitle">
                    <%: Model.Title%>
                </div>
                <div class="ticketDetailsHeaderInfo">
                    <%
                        if (!string.IsNullOrEmpty(Model.TagList))
                        { 
                    %>
                    <div class="ticketDetailsHeaderTagsArea">
                        <span>Tags:
                            <%: Model.TagList%>
                        </span>
                    </div>
                    <%
                        }
                    %>
                    <div style="padding: 2px;">
                    
                        <div style="white-space: nowrap;">
                           <span class="ticketDetailsHeaderInfoLabel" style="display:inline-block;">Assigned To:</span>&nbsp;<span class="ticketDetailsHeaderAssignedTo ticketDetailsHeaderInfoText">
                                <%: Model.GetAssignedToDisplayName(controller)%>
                            </span>
                        </div>
                    </div>
                    <div style="padding: 2px;">
                        <div style="white-space: nowrap;">
                           <span class="ticketDetailsHeaderInfoLabel" style="display:inline-block;"> Owned By:</span>&nbsp;<span class="ticketDetailsHeaderInfoText">
                                <%: Model.GetOwnerDisplayName(controller)%>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="ticketDetailsOuter">
    <div class="ticketDetailsInner">
        <div class="detailsTextWrapper">
            <div id="detailsText" <%if(ViewData["formatForEmail"] == null){ %> style="height: <%= detailsHeight%>px;"
                <% } %>>
                <%= Model.HtmlDetails%>
            </div>
        </div>
        <% if (ViewData["formatForEmail"] == null && ViewData["formatForSearch"] == null)
           { %>
        <div id="detailTextExpander" class="expanderButton" style="height: 20px; display: none;"
            onclick="expandDetails();">
            <div style="height: 20px; padding: 0px; border-bottom: solid 1px #D6D6D6;">
            </div>
        </div>
        <% } %>
    </div>
</div>
