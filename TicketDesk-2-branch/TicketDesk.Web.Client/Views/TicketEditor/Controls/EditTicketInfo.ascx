<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<% var Editor = "wmd"; %>
<% var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.TicketEditorController; %>
<% using (Ajax.BeginForm(MVC.TicketEditor.ActionNames.EditTicketInfo, new { ID = Model.TicketId }, new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnSuccess = "completeEditTicketDetails", OnFailure = "failModifyTicketActivity" }, new { defaultbutton = "editDetailsButton", @Class = "editForm" }))
   {
%>
<div class="activityHeadWrapper">
    <div class="activityHead">
        Ticket #:
        <%:Model.TicketId %>
    </div>
</div>
<div class="activityBody">
    <%: Html.HiddenFor(m => m.TicketId) %>
    <table class="formatTable" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tbody>
            <tr>
                <th>
                    <%: Html.ValidationMessageFor(m => m.Title,"*") %><%=  Html.LabelFor(m => m.Title) %>
                </th>
                <td colspan="2">
                    <%: Html.TextBoxFor(m => m.Title, new { style = "min-width:300px;width:450px;" })%>
                </td>
            </tr>
            <tr>
                <th>
                    <%: Html.ValidationMessageFor(m => m.Type, "*")%><%= Html.LabelFor(m => m.Type) %>
                </th>
                <td style="padding:0px;">
                    <table class="formatTable" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr>
                               
                                <td>
                                    <%: Html.DropDownListFor(m => m.Type, Model.GetTicketTypeSelectList()) %>
                                </td>
                                <th>
                                    <%: Html.ValidationMessageFor(m => m.Category, "*")%><%= Html.LabelFor(m => m.Category) %>
                                </th>
                                <td>
                                    <%:  Html.DropDownListFor(m => m.Category, Model.GetCategorySelectList())%>
                                </td>
                                <th>
                                    <%: Html.ValidationMessageFor(m => m.Priority, "*")%><%= Html.LabelFor(m => m.Priority) %>
                                </th>
                                <td>
                                    <%:  Html.DropDownListFor(m => m.Priority, Model.GetPrioritySelectList())%>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <th>
                    <%: Html.ValidationMessage("details", "*")%><%= Html.LabelFor(m => m.Details) %>
                </th>
                <td colspan="2">
                    <%if (Editor == "markitup")
                      { %>
                    <%: Html.TextArea("details", new { @Class = "markItUpEditor" })%>
                    <%}
                      else if (Editor == "wmd")
                      { %>
                    <div id="wmd-container">
                        <%: Html.TextArea("details", new { @Class = "wmd-input", Cols = "92", Rows = "15" })%>
                    </div>
                    <%} %>
                </td>
            </tr>
            <tr>
                <th>
                    <%: Html.LabelFor(m => m.Owner) %>
                </th>
                <td colspan="2">
                    <%: Html.DropDownListFor(m => m.Owner, Model.GetOwnerSelectList())%>
                    <%: Html.ValidationMessageFor(m => m.Owner, "*")%>
                </td>
            </tr>
            <tr>
                <th>
                    <%: Html.ValidationMessageFor(m => m.TagList, "*")%><%= Html.LabelFor(m => m.TagList) %>
                </th>
                <td colspan="2">
                    <%: Html.TextBoxFor(m => m.TagList, new { style = "min-width:300px;width:450px;" })%>
                </td>
            </tr>
            <tr>
                <th>
                    comment:
                </th>
                <td>
                    <div class="commentContainer">
                        <%if (Editor == "markitup")
                          { %>
                        <%= Html.TextArea("comment", new { @Class = "markItUpEditor" })%>
                        <%}
                          else if (Editor == "wmd")
                          { %>
                        <div id="Div1" class="wmd-container-small">
                            <%= Html.TextArea("comment", new { @Class = "wmd-input", Cols = "92", Rows = "15" })%>
                        </div>
                        <%} %>
                        <%: Html.ValidationMessage("comment")%>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <input id="editDetailsButton" type="submit" value="Save Changes" class="activityButton"
        style="display: inline;" /><span class="neverMindLink">
            <%= Ajax.ActionLink("Nevermind", MVC.TicketEditor.Display(Model.TicketId, string.Empty), new AjaxOptions { UpdateTargetId = "activityArea", OnBegin = "beginChangeActivity", OnSuccess = "completeActivity" })%></span>
    <br />
    <%= Html.ValidationMessage("ticketInfo") %>
</div>
<%} %>