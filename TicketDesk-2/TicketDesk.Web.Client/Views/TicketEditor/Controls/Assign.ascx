<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<% var Editor = "markitup"; %>
<div class="activityHeadWrapper">
    <div class="activityHead">
        <%: ViewData["activityDisplayName"].ToString() %>:
    </div>
</div>
<div class="activityBody">
    <% var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController; %>
    <% using (Ajax.BeginForm(MVC.TicketEditor.ActionNames.Assign, new { ID = Model.TicketId }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnSuccess = "completeModifyTicketActivityAndDetails", OnFailure = "failModifyTicketActivity" }, new { defaultbutton = "assignButton", @Class = "editForm" }))
       {
    %>
    <div class="commentContainer">
        <%if (Editor == "markitup")
          { %>
        <%= Html.TextArea("comment", new { @Class = "markItUpEditor" })%>
        <%}
          else if (Editor == "wmd")
          { %>
        <div id="wmd-container" class="wmd-container-small">
            <%= Html.TextArea("comment", new { @Class = "wmd-input", Cols = "92", Rows = "15" })%>
        </div>
        <%} %>
        <%: Html.ValidationMessage("comment")%>
    </div>
    <div class="activityFieldsContainer">
        <label for="assignTo" style="float: left; width: auto; text-align: left">
            Assign To:
        </label>
        <%: Html.DropDownListFor(m => m.AssignedTo, Model.GetAssignToSelectList(controller.Security.CurrentUserName))  %>
        <br /><%: Html.ValidationMessageFor(m => m.AssignedTo)%>

    </div>
    <% if (string.IsNullOrEmpty(Model.Priority))
       {
    %>
    <div class="activityFieldsContainer">
        <label for="priority" style="float: left; width: auto; text-align: left">
            Priority:
        </label>
        <%:  Html.DropDownListFor(m => m.Priority, Model.GetPrioritySelectList())%>
    </div>
    <%
        } 
    %>
    <input id="assignButton" type="submit" value="Assign" class="activityButton" style="display: inline;" /><span class="neverMindLink">
    <%= Ajax.ActionLink("Nevermind", MVC.TicketEditor.Display(Model.TicketId, string.Empty), new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnSuccess = "completeActivity" })%></span>
    <%} %>
</div>
