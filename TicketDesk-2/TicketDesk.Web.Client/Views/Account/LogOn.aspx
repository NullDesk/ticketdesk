<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TicketDesk.Web.Client.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Log On
</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="CustomHeadContent" runat="server">
    <script type="text/javascript">

        $(document).ready(function () { corners(); });

        function corners() {
            $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
        }

    </script>
</asp:Content>
<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="max-width: 600px; margin: auto;">
        <div class="displayContainerOuter">
            <div class="displayContainerInner">
                <div>
                    <div class="activityHeadWrapper">
                        <div class="activityHead">
                            Account Information
                        </div>
                    </div>
                    <div class="activityBody" style="padding: 15px; min-height: 200px;">
                        <p>
                            Please enter your username and password.
                            <%: Html.ActionLink("Register", MVC.Account.Register()) %>
                            if you don't have an account.
                        </p>
                        <% using (Html.BeginForm())
                           { %>
                        <%: Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.") %>
                        <div>
                            <div class="editor-label">
                                <%: Html.LabelFor(m => m.UserName) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.TextBoxFor(m => m.UserName) %>
                                <%: Html.ValidationMessageFor(m => m.UserName) %>
                            </div>
                            <div class="editor-label">
                                <%: Html.LabelFor(m => m.Password) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.PasswordFor(m => m.Password) %>
                                <%: Html.ValidationMessageFor(m => m.Password) %>
                            </div>
                            <div class="editor-label">
                                <%: Html.CheckBoxFor(m => m.RememberMe) %>
                                <%: Html.LabelFor(m => m.RememberMe) %>
                            </div>
                            <p>
                                <input type="submit" value="Log On" />
                            </p>
                        </div>
                        <% } %></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
