<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Web.Client.Models.TicketCenterListViewModel>" %>
<%@ Import Namespace="TicketDesk.Domain.Models" %>
<% if (false)
   {%>
<script src="../../../Scripts/MicrosoftAjax.debug.js" type="text/javascript"></script>
<script src="../../../Scripts/MicrosoftMvcAjax.debug.js" type="text/javascript"></script>
<script src="../../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script src="../../../Scripts/jquery-qtip-1.0.0-beta3.1020438/jquery.qtip-1.0.0-beta3.1.min.js"
    type="text/javascript"></script>
<script src="../../../Scripts/jquery.hoverIntent.minified.js" type="text/javascript"></script>
<% } %>
<% 
    var isOwnerFilterDisabled = Model.CurrentListSettings.DisabledFilterColumNames.Contains("Owner");
    Dictionary<string, object> ownerHtmlAttributes = new Dictionary<string, object>();
    ownerHtmlAttributes.Add("Class", String.Format("postback{0}", (isOwnerFilterDisabled) ? " disabled" : string.Empty));
    if (isOwnerFilterDisabled)
    {
        ownerHtmlAttributes.Add("Disabled", true);
    }


    var isAssignedToFilterDisabled = Model.CurrentListSettings.DisabledFilterColumNames.Contains("AssignedTo");
    Dictionary<string, object> assignedHtmlAttributes = new Dictionary<string, object>();
    assignedHtmlAttributes.Add("Class", String.Format("postback{0}", (isAssignedToFilterDisabled) ? " disabled" : string.Empty));
    if (isAssignedToFilterDisabled)
    {
        assignedHtmlAttributes.Add("Disabled", true);
    }

    var isCurrentStatusFilterDisabled = Model.CurrentListSettings.DisabledFilterColumNames.Contains("CurrentStatus");
    Dictionary<string, object> statusHtmlAttributes = new Dictionary<string, object>();
    statusHtmlAttributes.Add("Class", String.Format("postback{0}", (isCurrentStatusFilterDisabled) ? " disabled" : string.Empty));
    if (isCurrentStatusFilterDisabled)
    {
        statusHtmlAttributes.Add("Disabled", true);
    }
%>
<script type="text/javascript">

    Sys.Application.add_load(setupFilterForm);


    function beginChangeFilter(args) {

        $('#ticketList').fadeOut('fast');
    }

    function setupFilterForm() {
        $("#filterSubmitButton").hide();
        $('select.postback').change(function () {
            var btn = $('#filterSubmitButton').click();
        });
    }
</script>
<%
    
     
    var ajaxOptions = new AjaxOptions { UpdateTargetId = "ticketList",  OnBegin = "beginChangeFilter", OnSuccess = "completeChangeList", OnFailure = "completeChangeList" };    

    using (Ajax.BeginForm("FilterList", "TicketCenter", new { ListName = Model.CurrentListSettings.ListName }, ajaxOptions, new { id = "filterForm" }))
    {
%>
<div>
    Items/Page:<%= Html.DropDownList("PageSize", Model.FilterBar.ItemsPerPageSelectList, new { @class = "postback" })%>
    Status:<%= Html.DropDownList("CurrentStatus", Model.FilterBar.CurrentStatusSelectList, statusHtmlAttributes)%>
    Owner:<%= Html.DropDownList("Owner", Model.FilterBar.SubmittersSelectList, ownerHtmlAttributes)%>
    Assigned To:<%= Html.DropDownList("AssignedTo", Model.FilterBar.AssignedToSelectList, assignedHtmlAttributes)%>
    <input type="submit" value="filter" id="filterSubmitButton" />
  
</div>
<%
}
%>
