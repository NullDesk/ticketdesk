<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<% var Editor = "markitup"; %>
<div class="activityHeadWrapper">
    <div class="activityHead">
        <%: ViewData["activityDisplayName"].ToString() %>:
    </div>
</div>
<div class="activityBody">
    <% var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController; %>
    <% using (Ajax.BeginForm(MVC.TicketEditor.ActionNames.AddComment, new { ID = Model.TicketId }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnSuccess = "completeModifyTicketActivityAndDetails", OnFailure = "failModifyTicketActivity" }, new { defaultbutton = "addCommentButton", @Class = "editForm" }))
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
        <%= Html.ValidationMessage("comment")%>
    </div>
   
    <% if (Model.AssignedTo == controller.Security.CurrentUserName)
       {
    %>
    <div class="activityFieldsContainer">
        <%= Html.CheckBox("resolve", new { Style = "float:left;" })%>
        <label for="resolve">
            Resolve ticket?</label>
        <div class="fieldSubText">
            Check to resolve ticket with this comment.
        </div>
    </div>
    <% 
        } 
    %>
    <input id="addCommentButton" type="submit" value="Add Comment" class="activityButton"
        style="display: inline;" /><span class="neverMindLink">
            <%= Ajax.ActionLink("Nevermind", MVC.TicketEditor.Display(Model.TicketId, string.Empty), new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnSuccess = "completeActivity" })%></span>
    <%} %>
    
</div>
