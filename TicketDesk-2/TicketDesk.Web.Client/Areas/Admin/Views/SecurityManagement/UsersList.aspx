<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<TicketDesk.Domain.Utilities.Pagination.CustomPagination<TicketDesk.Web.Client.Areas.Admin.Models.SecurityManagementUserViewModel>>" %>

<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<%@ Import Namespace="TicketDesk.Web.Client.Areas.Admin.Models" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="AdminTitleContent" runat="server">
</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="AdminCustomHead" runat="server">
    <script type="text/javascript">

        $(document).ready(function () { corners(); });

        function corners() {
            $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
        }

    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="contentContainer">
        <div style="margin: auto;">
            <div class="displayContainerOuter">
                <div class="displayContainerInner" style="width: 100%;">
                    <div>
                        <div class="activityHeadWrapper">
                            <div class="activityHead">
                                Users
                            </div>
                        </div>
                        <div class="activityBody" style="padding: 15px; min-height: 200px;">
                            <table class="ticketListGrid" cellpadding="0" cellspacing="0" style="width: 100%;">
                                <thead>
                                    <tr>
                                        <th>
                                        </th>
                                        <th>
                                            Name
                                        </th>
                                        <th>
                                            DisplayName
                                        </th>
                                        <th>TD Admin</th>
                                        <th>TD Help Desk</th>
                                        <th>TD Submitter</th>
                                        <th>Approved</th>
                                        <th>Locked</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <% foreach (SecurityManagementUserViewModel item in Model)
                                       { %>
                                       
                                    <tr>
                                        <td>
                                            <%: Html.ActionLink("Edit", MVC.Admin.SecurityManagement.EditUser(item.UserName))%> |  <%: Html.ActionLink("Delete", MVC.Admin.SecurityManagement.DeleteUser(item.UserName))%>
                                        </td>
                                        <td>
                                            <%: item.UserName %>
                                        </td>
                                        <td>
                                            <%: item.DisplayName%>
                                        </td>
                                        <td>
                                        <%: Html.CheckBoxFor(i => item.IsAdmin, new { disabled = "true" })%>
                                        
                                        </td>
                                        <td>
                                        <%: Html.CheckBoxFor(i => item.IsStaff, new { disabled = "true" })%>
                                        </td>
                                        <td>
                                        <%: Html.CheckBoxFor(i => item.IsSubmitter, new { disabled = "true" })%>
                                        </td>
                                        <td>
                                        <%: Html.CheckBoxFor(i => item.IsApproved, new { disabled = "true" })%>
                                        </td>
                                        <td>
                                             <%: Html.CheckBoxFor(i => item.IsLockedOut, new {disabled = "true" })%>
                                        </td>
                                    </tr>
                                   <% } %>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <th colspan="7" style="text-align: right;">
                                            <%=  Html.Pager(Model).Link(pageNumber => Url.Action("UsersList", new { Page = pageNumber }))%>
                                        </th>
                                    </tr>
                                </tfoot>
                            </table>
                            <p>
                                <%: Html.ActionLink("New User", MVC.Admin.SecurityManagement.CreateUser()) %>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
