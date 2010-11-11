<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TicketDesk.Web.Client.Models.TicketCenterListViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Pager" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Display
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CustomHeadContent" runat="server">
   
    <script src="<%= Links.Scripts.jquery_clickable_0_1_9_js %>" type="text/javascript"></script>
    <% 
        if (false)
        {
    %>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.clickable-0.1.9.js" type="text/javascript"></script>
    <%
        }
    %>
    <script type="text/javascript">

        $("document").ready(
        function () {
            $(".displayContainerInner").corner("bevel left 6px").parent().css('padding', '3px').corner("round left keep 12px").corner("round right keep 4px");
            $(".clickable").clickable();
        });
       
            
            
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul>
        <% 
            foreach (var list in Model.ListsForMenu)
            {
                var className = ((Model.CurrentListSettings.ListName == list.ListName)) ? "selected" : string.Empty;
        %>
        <li class="<%= className %>">
            <%= Html.ActionLink(list.ListDisplayName, MVC.TicketManager.Index(null, list.ListName))%>
        </li>
        <%
            }
        %>
    </ul>
    <% if (Model.Tickets.Count() < 1)
       {
    %>
    No Tickets
    <% }
       else
       {
    %>
    <div id="ticketList" style="max-width: 700px; margin: auto;">
        <%       
           
           
            foreach (var item in Model.Tickets)
            {
                var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.TicketManagerController;

        %>
        <div>
            <div class="clickable displayContainerOuter">
                <div class="displayContainerInner">
                    <div>
                        <div style="margin-top: 0px;">
                            <div style="color: #0B294F; background-color: #E1EBF2;">
                                <table style="width: 100%; font-size: 9pt;" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td rowspan="3" style="min-height: 80px; width: 20px; border-right: solid 1px #B3CBDF"
                                            class="<%=  Html.Encode(item.CurrentStatus.Replace(" ", "").ToLower())%>Flag">
                                            <img alt="<%: item.CurrentStatus %>" src="<%= Url.Content(string.Format("~/Content/{0}Flag.png", Url.Encode(item.CurrentStatus.ToLower()))) %>" />
                                        </td>
                                        <td style="white-space: nowrap; vertical-align: top; font-weight: bold; padding: 3px;">
                                            <a href='<%= Url.Action("Display", "TicketEditor", new { ID = item.TicketId })%>'>Ticket:
                                                #<%: item.TicketId%>
                                                -
                                                <%: item.Category%>
                                                <%: item.Type%>
                                            </a>
                                        </td>
                                        <td style="padding: 3px;">
                                            <div style="float: right;">
                                                Priority:
                                                <%: item.Priority%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="height: 25px;">
                                        <td colspan="2" style="padding-left: 20px; padding-bottom: 2px; padding-right: 8px;
                                            padding-top: 2px; font-size: 9pt;">
                                            <%: item.Title%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: #EEF3F7; border-top: solid 1px #B3CBDF">
                                            <table style="width: 100%; color: #444; font-size: 8pt;">
                                                <tr>
                                                    <td style="white-space: nowrap; text-align: right;">
                                                        Assigned To:
                                                    </td>
                                                    <td style="white-space: nowrap;">
                                                        <%: item.GetAssignedToDisplayName(controller)%><br />
                                                    </td>
                                                    <td rowspan="2" style="width: 100%;">
                                                        <div style="text-align: right; width: 100%; font-size: 8pt;">
                                                            Tags:
                                                            <%: item.TagList%></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space: nowrap; text-align: right;">
                                                        Owned By:
                                                    </td>
                                                    <td style="white-space: nowrap;">
                                                        <%: item.GetOwnerDisplayName(controller)%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%     }%>
        <%= Html.Pager(Model.Tickets) %>
        <%--
    <br />
    <br />
    <% var ajaxOptions = new AjaxOptions { UpdateTargetId = "ticketList" };//, OnBegin = "beginChangeList", OnSuccess = "completeChangeList", OnFailure = "completeChangeList" };%>

    <%= Ajax.Pager(Model.Tickets, new PagerOptions { IndexParameterName = "page", ShowNumbers = true, PreviousText = "PREV", NextText = "NEXT" }, ajaxOptions)%>
    (page
    <%= Model.Tickets.PageNumber%>
    of
    <%= Model.Tickets.TotalPages%>)--%>
        <% } %>
    </div>
</asp:Content>
