<%@ Page Language="C#" MasterPageFile="~/Views/Account/Shared/MyAccount.master" Inherits="System.Web.Mvc.ViewPage<TicketDesk.Web.Client.Models.ChangePasswordModel>" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="MyAccountTitleContent"
    runat="server">
    Change Password
</asp:Content>
<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MyAccountContent" runat="server">
    <div class="displayContainerOuter">
        <div class="displayContainerInner" style="width: 100%;">
            <div>
                <div class="activityHeadWrapper">
                    <div class="activityHead">
                        Change Password
                    </div>
                </div>
                <div class="activityBody" style="padding: 15px; min-height: 200px;">
                    <p>
                        Use the form below to change your password.
                    </p>
                    <p>
                        New passwords are required to be a minimum of
                        <%: ViewData["PasswordLength"] %>
                        characters in length.
                    </p>
                    <% using (Html.BeginForm())
                       { %>
                    <%: Html.ValidationSummary(true, "Password change was unsuccessful. Please correct the errors and try again.") %>
                    <div>
                        <div class="editor-label">
                            <%: Html.LabelFor(m => m.OldPassword) %>
                        </div>
                        <div class="editor-field">
                            <%: Html.PasswordFor(m => m.OldPassword) %>
                            <%: Html.ValidationMessageFor(m => m.OldPassword) %>
                        </div>
                        <div class="editor-label">
                            <%: Html.LabelFor(m => m.NewPassword) %>
                        </div>
                        <div class="editor-field">
                            <%: Html.PasswordFor(m => m.NewPassword) %>
                            <%: Html.ValidationMessageFor(m => m.NewPassword) %>
                        </div>
                        <div class="editor-label">
                            <%: Html.LabelFor(m => m.ConfirmPassword) %>
                        </div>
                        <div class="editor-field">
                            <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                            <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                        </div>
                        <p>
                            <input type="submit" value="Change Password" />
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <% } %>
</asp:Content>
