<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Web.Client.Models.TicketCenterListViewModel>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
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


    var shiftstatus = false;
    function setShiftStatus(e) {
        //work around the fact that in firefox, there is no clean way to get at the event object from the beginChangeList function
        if (e) {
            shiftstatus = e.shiftKey;
        }
    }

    function beginChangeSort(args) {
        if (shiftstatus) {
            args.get_request()._url = args.get_request()._url + "&isMultiSort=true";
        }
      
        
        $('#ticketList').fadeOut('fast');
    }

    function completeChangeList() {
        // Animate
        $('#ticketList').fadeIn('fast');
        clicks();
        setupFilterForm();
    }   
</script>

<%   
    var ajaxOptions = new AjaxOptions { UpdateTargetId = "ticketList", OnBegin = "beginChangeSort", OnSuccess = "completeChangeList", OnFailure = "completeChangeList" };    
%>
<div id="ticketList">
    <div class="ticketFilterBar">
        <%
           

            Html.RenderPartial(MVC.TicketCenter.Views.Controls.FilterBar, Model); %>
    </div>
    <div>
        <table class="ticketListGrid" cellpadding="0" cellspacing="0" style="width: 100%;">
            <% if (Model.Tickets.Count() < 1)
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
                    
                    <th >
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
                    <th colspan="10" style="text-align: right;">
                        <%= Ajax.Pager(Model.Tickets, new PagerOptions { IndexParameterName = "page", ShowNumbers = true, PreviousText = "PREV", NextText = "NEXT" }, new AjaxOptions { UpdateTargetId = "ticketList", OnBegin = "beginChangeList", OnSuccess = "completeChangeList", OnFailure="completeChangeList" })%>
                        (page
                        <%= Model.Tickets.PageNumber%>
                        of
                        <%= Model.Tickets.TotalPages%> : <%= Model.Tickets.TotalItems%> records)

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
