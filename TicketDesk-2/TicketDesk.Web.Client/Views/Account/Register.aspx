<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TicketDesk.Web.Client.Models.RegisterModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Register
</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="CustomHeadContent" runat="server">
    <script type="text/javascript">

        $(document).ready(function () { corners(); });

        function corners() {
            $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
        }

    </script>
</asp:Content>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contentContainer">
        <div style="max-width: 600px; margin: auto;">
            <div class="displayContainerOuter">
                <div class="displayContainerInner" style="width: 100%;">
                    <div>
                        <div class="activityHeadWrapper">
                            <div class="activityHead">
                                Register User:
                            </div>
                        </div>
                        <div class="activityBody" style="margin: auto; padding: 25px;">
                            <% using (Html.BeginForm())
                               { %>
                            <%: Html.ValidationSummary(true, "Account creation was unsuccessful. Please correct the errors and try again.") %>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.UserName) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.TextBoxFor(model => model.UserName) %>
                                <%: Html.ValidationMessageFor(model => model.UserName) %>
                            </div>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.DisplayName) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.TextBoxFor(model => model.DisplayName) %>
                                <%: Html.ValidationMessageFor(model => model.DisplayName) %>
                            </div>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.Email) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.TextBoxFor(model => model.Email)%>
                                <%: Html.ValidationMessageFor(model => model.Email)%>
                            </div>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.Password) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.PasswordFor(model => model.Password)%>
                                <%: Html.ValidationMessageFor(model => model.Password)%>
                            </div>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.ConfirmPassword) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.PasswordFor(model => model.ConfirmPassword)%>
                                <%: Html.ValidationMessageFor(model => model.ConfirmPassword)%>
                            </div>
                            <p>
                                <input type="submit" value="Register" class="activityButton" class="activityButton"
                                    style="display: inline;" /><span class="neverMindLink">
                                        <%: Html.ActionLink("Nevermind", MVC.Home.Index())%>
                                    </span>
                            </p>
                            <% } %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
