<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<% var Editor = "markitup"; %>

<div class="activityHeadWrapper">
    <div class="activityHead">
        <%: ViewData["activityDisplayName"].ToString() %>:
    </div>
</div>
<div class="activityBody">
    <% var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController; %>
    <% using (Ajax.BeginForm(MVC.TicketEditor.ActionNames.ReOpen, new { ID = Model.TicketId }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnSuccess = "completeModifyTicketActivityAndDetails", OnFailure = "failModifyTicketActivity" }, new { @Class = "editForm" }))
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
    <%
        if (controller.Security.IsTdStaff())
        {
    %>
    <div class="activityFieldsContainer">
        <%= Html.CheckBox("assignToMe", new { Style = "float:left;" })%>
        <label for="assignToMe">
            Assign to me?
        </label>
        <div class="fieldSubText">
            Check this box to assign the ticket to yourself now. If unchecked the ticket will
            be re-opened as unassigned.
        </div>
    </div>
    <%      if (Model.Owner != controller.Security.CurrentUserName)
            {
    %>
    <div class="activityFieldsContainer">
        <%= Html.CheckBox("ownedByMe", new { Style = "float:left;" })%>
        <label for="ownedByMe">
            Reopen as owned by me?
        </label>
        <div class="fieldSubText">
            Check this box to set yourself as the new owner of the ticket. If unchecked the
            ticket will be re-opened with the original owner.</div>
    </div>
    <%
        }
        }
    %>
    
    <input type="submit" value="Re-Open" class="activityButton" style="display:inline;" /><span class="neverMindLink">
    <%= Ajax.ActionLink("Nevermind", MVC.TicketEditor.Display(Model.TicketId, string.Empty ), new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnSuccess = "completeActivity" })%></span>
    <%} %>
</div>
