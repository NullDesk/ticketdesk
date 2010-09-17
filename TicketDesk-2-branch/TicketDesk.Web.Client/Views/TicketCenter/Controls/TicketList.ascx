<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Web.Client.Models.TicketCenterListViewModel>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<%@ Import Namespace="MvcPaging" %>
<%@ Import Namespace="TicketDesk.Domain.Models" %>

<script type="text/javascript" src="<%= Links.Scripts.jquery_qtip_1_0_0_beta3_1020438.jquery_qtip_1_0_0_beta3_1_min_js %>"></script>

<script type="text/javascript" src="<%= Links.Scripts.jquery_hoverIntent_minified_js %>"></script>

<% 
    if (false)
   {%>

<script src="../../../Scripts/MicrosoftAjax.debug.js" type="text/javascript"></script>

<script src="../../../Scripts/MicrosoftMvcAjax.debug.js" type="text/javascript"></script>

<script src="../../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>

<script src="../../../Scripts/jquery-qtip-1.0.0-beta3.1020438/jquery.qtip-1.0.0-beta3.1.min.js"
    type="text/javascript"></script>

<script src="../../../Scripts/jquery.hoverIntent.minified.js" type="text/javascript"></script>

<% } %>

<script type="text/javascript">
    Sys.Application.add_load(setupTips);

    function setupTips() {
        $('.viewQtip').each(function () {
            $(this).qtip(
            {
                content: "View", //$(this).attr('qtitle'),
                position:
                {
                    corner:
                    {
                        target: 'topRight',
                        tooltip: 'leftBottom'
                    }
                },
                style:
                {
                    padding: 8,
                    tip:
                    { // Now an object instead of a string
                        corner: 'leftBottom', // We declare our corner within the object using the corner sub-option
                        color: '#CEDBE8'

                    },
                    border:
                    {
                        width: 3,
                        radius: 8,
                        color: '#CEDBE8'
                    }
                },
                show:
                {
                    delay: 400
                }
            });
        });
    }

    function beginChangeList(args) {
        $('#ticketList').fadeOut('fast');
    }

    function completeChangeList() {
        // Animate
        $('#ticketList').fadeIn('fast');
        setupTips();
        setupFilterForm();
    }   
</script>

<%   
    var ajaxOptions = new AjaxOptions { UpdateTargetId = "ticketList", OnBegin = "beginChangeList", OnSuccess = "completeChangeList", OnFailure = "completeChangeList" };    
%>
<div id="ticketList">
    <div class="ticketFilterBar">
        <%
            var vdd = new ViewDataDictionary();
            vdd.Add("ajaxOptions", ajaxOptions);

            Html.RenderPartial(MVC.TicketCenter.Views.Controls.FilterBar, Model, vdd); %>
    </div>
    <div>
        <table class="ticketListGrid" cellpadding="0" cellspacing="0" style="width: 100%;">
            <% if (Model.Tickets.Count < 1)
               {
            %>
            <tbody>
                <tr>
                    <td style="text-align: center; padding: 30px; font-size:larger;">
                        There are no tickets to display.
                    </td>
                </tr>
            </tbody>
            <%
                }
               else
               {       
            %>
            <thead>
                <tr>
                    <th>
                    </th>
                    <th>
                        <%= Ajax.SortableColumnHeader(Html, Model.CurrentListSettings, "SortList", Model.CurrentListSettings.ListName, "TicketId", "ID", ajaxOptions)%>
                    </th>
                    <th>
                        <%= Ajax.SortableColumnHeader(Html, Model.CurrentListSettings, "SortList", Model.CurrentListSettings.ListName, "Type", "Type", ajaxOptions)%>
                    </th>
                    <th>
                        <%= Ajax.SortableColumnHeader(Html, Model.CurrentListSettings, "SortList", Model.CurrentListSettings.ListName, "Title", "Title", ajaxOptions)%>
                    </th>
                    <th>
                        <%= Ajax.SortableColumnHeader(Html, Model.CurrentListSettings, "SortList", Model.CurrentListSettings.ListName, "Owner", "Owner", ajaxOptions)%>
                    </th>
                    <th>
                        <%= Ajax.SortableColumnHeader(Html, Model.CurrentListSettings, "SortList", Model.CurrentListSettings.ListName, "AssignedTo", "Assigned", ajaxOptions)%>
                    </th>
                    <th>
                        <%= Ajax.SortableColumnHeader(Html, Model.CurrentListSettings, "SortList", Model.CurrentListSettings.ListName, "CurrentStatus", "Status", ajaxOptions)%>
                    </th>
                    <th>
                        <%= Ajax.SortableColumnHeader(Html, Model.CurrentListSettings, "SortList", Model.CurrentListSettings.ListName, "Category", "Category", ajaxOptions)%>
                    </th>
                    <th>
                        <%= Ajax.SortableColumnHeader(Html, Model.CurrentListSettings, "SortList", Model.CurrentListSettings.ListName, "Priority", "Priority", ajaxOptions)%>
                    </th>
                    <th>
                        <%= Ajax.SortableColumnHeader(Html, Model.CurrentListSettings, "SortList", Model.CurrentListSettings.ListName, "CreatedDate", "Created", ajaxOptions)%>
                    </th>
                    <th>
                        <%= Ajax.SortableColumnHeader(Html, Model.CurrentListSettings, "SortList", Model.CurrentListSettings.ListName, "LastUpdateDate", "Updated", ajaxOptions)%>
                    </th>
                </tr>
            </thead>
            <tfoot>
                <tr>
                    <th colspan="11" style="text-align: right;">
                        <%= Ajax.Pager(Model.Tickets as PageOfList<Ticket>, new PagerOptions { IndexParameterName = "page", ShowNumbers = true, PreviousText = "PREV", NextText = "NEXT" }, new AjaxOptions { UpdateTargetId = "ticketList", OnBegin = "beginChangeList", OnSuccess = "completeChangeList", OnFailure="completeChangeList" })%>
                        (page
                        <%= Model.Tickets.PageIndex + 1%>
                        of
                        <%= Model.Tickets.TotalPageCount%>)
                    </th>
                </tr>
            </tfoot>
            <tbody>
                <% 
                    int count = 0;
                    foreach (var item in Model.Tickets)
                    {
                        var partialViewData = new ViewDataDictionary();
                        partialViewData["odd"] = ((count % 2) != 0);
                        Html.RenderPartial("~/Views/TicketCenter/Controls/TicketListItem.ascx", item, partialViewData);
                        count++;
                    } 
                %>
            </tbody>
            <%
                } 
            %>
            </table>
    </div>
</div>
