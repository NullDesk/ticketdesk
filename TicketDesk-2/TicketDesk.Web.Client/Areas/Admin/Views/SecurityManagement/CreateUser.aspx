<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<TicketDesk.Web.Client.Areas.Admin.Models.SecurityManagementUserViewModel>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="AdminTitleContent" runat="server">
    Create User
</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="AdminCustomHead" runat="server">
    <script type="text/javascript">

        $(document).ready(function () { corners(); });

        function corners() {
            $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
        }

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="contentContainer">
        <div style="max-width: 600px; margin: auto;">
            <div class="displayContainerOuter">
                <div class="displayContainerInner" style="width: 100%;">
                    <div>
                        <div class="activityHeadWrapper">
                            <div class="activityHead">
                                Edit User:
                            </div>
                        </div>
                        <div class="activityBody" style="margin: auto; padding: 25px;">
                            <% using (Html.BeginForm())
                               {%>
                            <%: Html.ValidationSummary(true) %>
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
                            <div>
                                <div class="editor-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsAdmin) %>
                                </div>
                                <div class="editor-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsAdmin) %>
                                </div>
                            </div>
                            <div>
                                <div class="editor-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsStaff)%>
                                </div>
                                <div class="editor-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsStaff) %>
                                </div>
                            </div>
                            <div>
                                <div class="editor-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsSubmitter)%>
                                </div>
                                <div class="editor-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsSubmitter) %>
                                </div>
                            </div>
                            <div>
                                <div class="editor-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsApproved)%>
                                </div>
                                <div class="editor-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsApproved)%>
                                </div>
                            </div>
                            <p>
                                <input type="submit" value="Create" class="activityButton" class="activityButton"
                                    style="display: inline;" /><span class="neverMindLink">
                                        <%: Html.ActionLink("Nevermind", MVC.Admin.SecurityManagement.ActionNames.UsersList)%>
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
