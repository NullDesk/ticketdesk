<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Domain.Services" %>
<div class="activityHeadWrapper">
    <div class="activityHead" >
        <%: ViewData["activityDisplayName"].ToString() %>:
    </div>
</div>
<div class="activityBody" style="padding-left:16px; padding-right:16px; padding-top:16px;">
    <% var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.TicketEditorController;%>
    <% 
        if (controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.AddComment))
        {
    %>
    <%= Ajax.ActionLink("Add Comment", "Display", new { ID = Model.TicketId, Activity = "AddComment" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeActivity" }, new { @Class = "activityButton" })%>
    <%  } %>
    <% 
        if (controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.SupplyMoreInfo))
        {
    %>
    <%= Ajax.ActionLink("Provide More Info", "Display", new { ID = Model.TicketId, Activity = "SupplyMoreInfo" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeActivity" }, new { @Class = "activityButton"})%>
    <%  } %>
    <% 
        if (controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.CancelMoreInfo))
        {
    %>
    <%= Ajax.ActionLink("Cancel More Info", "Display", new { ID = Model.TicketId, Activity = "CancelMoreInfo" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeActivity" }, new { @Class = "activityButton" })%>
    <%  } %>
    <% 
        if (controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.RequestMoreInfo))
        {
    %>
    <%= Ajax.ActionLink("Request More Info", "Display", new { ID = Model.TicketId, Activity = "RequestMoreInfo" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeActivity" }, new { @Class = "activityButton" })%>
    <%  } %>
    <% 
        if (controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.TakeOver))
        {
    %>
    <%= Ajax.ActionLink("Take Over", "Display", new { ID = Model.TicketId, Activity = "TakeOver" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeActivity" }, new { @Class = "activityButton" })%>
    <%  } %>
    <% 
        if (controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.Resolve))
        {
    %>
    <%= Ajax.ActionLink("Resolve", "Display", new { ID = Model.TicketId, Activity = "Resolve" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeActivity" }, new { @Class = "activityButton" })%>
    <%  } %>
    <% 
        if
        (
            controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.Assign) ||
            controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.ReAssign) ||
            controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.Pass)
        )
        {
    %>
    <%= Ajax.ActionLink("Assign", "Display", new { ID = Model.TicketId, Activity = "Assign" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeActivity" }, new { @Class = "activityButton" })%>
    <%  } %>
    <% 
        if (controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.Close))
        {
    %>
    <%= Ajax.ActionLink("Close", "Display", new { ID = Model.TicketId, Activity = "Close" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeActivity" }, new { @Class = "activityButton" })%>
    <%  } %>
    <% 
        if (controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.ReOpen))
        {
    %>
    <%= Ajax.ActionLink("Re-Open", "Display", new { ID = Model.TicketId, Activity = "ReOpen" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeActivity" }, new { @Class = "activityButton" })%>
    <%  } %>
    <% 
        if (controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.GiveUp))
        {
    %>
    <%= Ajax.ActionLink("Give Up!", "Display", new { ID = Model.TicketId, Activity = "GiveUp" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeActivity" }, new { @Class = "activityButton" })%>
    <%  } %>
    <% 
        if (controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.ForceClose))
        {
    %>
    <%= Ajax.ActionLink("Force Close", "Display", new { ID = Model.TicketId, Activity = "ForceClose" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeActivity" }, new { @Class = "activityButton" })%>
    <%  } %>
    <% 
        if (controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.ModifyAttachments))
        {
    %>
    <%= Ajax.ActionLink("Attachments", "Display", new { ID = Model.TicketId, Activity = "ModifyAttachments" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeActivity" }, new { id = "ModifyAttachmentsLink", style = "display:none;", @Class = "activityButton" })%>
    <%  } %>
    <% 
        if (controller.Tickets.CheckSecurityForTicketActivity(Model, TicketActivity.EditTicketInfo))
        {
    %>
    <%= Ajax.ActionLink("Edit Details", "Display", new { ID = Model.TicketId, Activity = "EditTicketInfo" }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnComplete = "completeChangeToEditActivity" }, new { @Class = "activityButton" })%>
    <%  } %>
</div>

