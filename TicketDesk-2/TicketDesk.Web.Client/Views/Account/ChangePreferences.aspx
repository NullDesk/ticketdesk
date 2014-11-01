<%@ Page Language="C#" MasterPageFile="~/Views/Account/Shared/MyAccount.master" Inherits="System.Web.Mvc.ViewPage<TicketDesk.Web.Client.Models.AccountPreferencesModel>" %>

<asp:Content ID="ChangeDisplayNameHead" ContentPlaceHolderID="MyAccountTitleContent"
    runat="server">
    Change Preferences
</asp:Content>
<asp:Content ID="ChangePreferencesContent" ContentPlaceHolderID="MyAccountContent"
    runat="server">
    <div class="displayContainerOuter">
        <div class="displayContainerInner" style="width: 100%;">
            <div class="activityHeadWrapper">
                <div class="activityHead">
                    Change Preferences
                </div>
            </div>
            <div class="activityBody" style="padding: 15px; min-height: 200px;">
                <% using (Html.BeginForm())
                   { %>
                <%= Html.ValidationSummary() %>
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.DisplayName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.DisplayName) %>
                    <%: Html.ValidationMessageFor(m => m.DisplayName) %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.EmailAddress) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.EmailAddress) %>
                    <%: Html.ValidationMessageFor(m => m.EmailAddress) %>
                </div>
                <div class="editor-field">
                    <%: Html.CheckBoxFor(m => m.OpenEditorWithPreview) %>
                    <%: Html.LabelFor(m => m.OpenEditorWithPreview) %>
                </div>

                <p>
                    <input type="submit" value="Save" /></p>
                <% } %>
            </div>
        </div>
    </div>
</asp:Content>
