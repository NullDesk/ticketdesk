<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<% var Editor = "markitup"; %>

<div class="activityHeadWrapper">
    <div class="activityHead">
        <%: ViewData["activityDisplayName"].ToString() %>:
    </div>
</div>
<div class="activityBody">
    <% using (Ajax.BeginForm(MVC.TicketEditor.ActionNames.Resolve, new { ID = Model.TicketId }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnSuccess = "completeModifyTicketActivityAndDetails", OnFailure = "failModifyTicketActivity" }, new { defaultbutton = "resolveButton", @Class = "editForm" }))
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
    <input id="resolveButton" type="submit" value="Resolve" class="activityButton" style="display:inline;" /><span class="neverMindLink">
    <%= Ajax.ActionLink("Nevermind", MVC.TicketEditor.Display(Model.TicketId, string.Empty), new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnSuccess = "completeActivity" })%></span>
    <%} %>
</div>
