<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<% var Editor = "markitup"; %>
<% var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.TicketEditorController; %>
<% using (Ajax.BeginForm(MVC.TicketEditor.ActionNames.ModifyAttachments, new { ID = Model.TicketId }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnSuccess = "completeModifyTicketActivityAttachmentsAndDetails", OnFailure = "failModifyTicketActivity" }, new { defaultbutton = "modifyAttachmentsButton", @Class = "editForm" }))
   {
%>
<div class="activityHeadWrapper">
    <div class="activityHead">
        <%: ViewData["activityDisplayName"].ToString() %>:
    </div>
</div>
<div class="activityBody">
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
        <table>
            <tr>
                <td>
                    <label for="fileUpload" style="margin-bottom: 25px;">
                        Attachments:
                    </label>
                </td>
                <td style="height:35px;">
                    <div id="fileUploader" class="activityButton" style="width: 100px; display:inline-block;">
                        Upload</div><img id="progress" src="<%= Url.Content("~/Content/progress.gif") %>" style="display: none;" />
                </td>
            </tr>
        </table>
        <div id="attachmentsArea" style="padding-left: 15px;">
            <table id="files_list">
                <tr>
                    <td>
                    </td>
                </tr>
                <!-- This is where the output will appear -->
                <% foreach (var att in Model.TicketAttachments.Where(ta => !ta.IsPending || ta.IsPending && ta.UploadedBy == controller.Security.CurrentUserName))
                   {
                %>
                <tr id="fileItem_<%= att.FileId %>">
                    <td>
                        <table class="formatTable" cellpadding="0" cellspacing="0" style="width: 100%; border: solid 1px #B3CBDF;">
                            <tbody>
                                <tr>
                                    <td rowspan="2" class="<%= (att.IsPending)? "PendingFileAttachmentItemContainer" : "FileAttachmentItemContainer" %>">
                                        <img alt="<%= (att.IsPending) ? "Pending File": "Attached File" %>" src="<%= Url.Content(string.Format("~/Content/{0}Flag.png", (att.IsPending)? "pending" : "attached" )) %>" />
                                    </td>
                                    <th>
                                        <label>
                                            File:
                                        </label>
                                    </th>
                                    <td>
                                        <%: Html.Hidden("fileId_" + att.FileId, att.FileId) %>
                                        <%: Html.TextBox("fileName_" + att.FileId, att.FileName, new { Style = "width: 325px;"})%>
                                    </td>
                                    <td rowspan="2" style="text-align: right;">
                                        <a  class="noLink" href="" onclick="removeAttachment(<%= att.FileId.ToString() %>);return false;">
                                            <img src="<%= Url.Content("~/Content/cancel.png") %>" alt="remove" /></a>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <label>
                                            Description:
                                        </label>
                                    </th>
                                    <td>
                                        <%: Html.TextBox("fileDescription_" + att.FileId, att.FileDescription, new { Style = "width: 325px;" })%>
                                        (optional)
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <%
                    }
                %>
            </table>
        </div>
    </div>
    <input id="modifyAttachmentsButton" type="submit" value="Save Changes" class="activityButton"
        style="display: inline;" /><span class="neverMindLink">
            <%= Ajax.ActionLink("Nevermind", MVC.TicketEditor.Display(Model.TicketId, string.Empty), new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnSuccess = "completeActivity" })%></span>
    <br />
    <%: Html.ValidationMessage("attachments") %>
</div>
<%} %>
