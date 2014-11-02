<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<TicketDesk.Web.Client.Areas.Admin.Models.SecurityManagementUserViewModel>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="AdminTitleContent" runat="server">
    Delete User
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
                            <h3>
                                Are you sure you want to delete this users? This cannot be undone.</h3>
                            <div>
                                <div class="display-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.UserName)%>:</div>
                                <div class="display-field" style="display: inline;">
                                    <%: Model.UserName %></div>
                            </div>
                            <div>
                                <div class="display-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.DisplayName)%>:</div>
                                <div class="display-field" style="display: inline;">
                                    <%: Model.DisplayName %></div>
                            </div>
                            <div>
                                <div class="display-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.Email) %>:</div>
                                <div class="display-field" style="display: inline;">
                                    <%: Model.Email %></div>
                            </div>
                            <div>
                                <div class="display-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsAdmin, new { disabled = "true" })%></div>
                                <div class="display-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsAdmin)%></div>
                            </div>
                            <div>
                                <div class="display-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsStaff, new { disabled = "true" })%></div>
                                <div class="display-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsStaff)%></div>
                            </div>
                            <div>
                                <div class="display-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsSubmitter, new { disabled = "true" })%></div>
                                <div class="display-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsSubmitter)%></div>
                            </div>
                            <div>
                                <div class="display-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsApproved, new { disabled = "true" })%></div>
                                <div class="display-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsApproved)%></div>
                            </div>
                            <%= Html.ValidationSummary() %>
                            <% using (Html.BeginForm())
                               { %>
                            <p>
                                <input type="submit" value="Delete" class="activityButton" style="display: inline;" /><span
                                    class="neverMindLink">
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
