<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="AdminTitleContent" runat="server">
    Security Management
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
                                Application Settings
                            </div>
                        </div>
                       <div class="activityBody" style="padding: 15px; min-height: 200px;">
                            
                             <div class="adminItemContainer userSettings">
                               <%= Html.ActionLink("Manage Users", MVC.Admin.SecurityManagement.UsersList()) %>
                            </div>
                          <%--  
                            <div class="adminItemContainer roleSettings">
                                 <%= Html.ActionLink("Manage Roles", MVC.Admin.SecurityManagement.RolesList())%>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
