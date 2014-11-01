<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="IndexHead" ContentPlaceHolderID="TitleContent" runat="server">
    Admin Home
</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="CustomHeadContent" runat="server">
    <script type="text/javascript">

        $(document).ready(function () { corners(); });

        function corners() {
            $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
        }

    </script>
</asp:Content>
<asp:Content ID="IndexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contentContainer">
        <div style="max-width: 600px; margin: auto;">
            <div class="displayContainerOuter">
                <div class="displayContainerInner" style="width: 100%;">
                    <div>
                        <div class="activityHeadWrapper">
                            <div class="activityHead">
                                Admin Home
                            </div>
                        </div>
                        <div class="activityBody" style="padding: 15px; min-height: 200px;">
                            <div class="adminItemContainer applicationSettings">
                                <%= Html.ActionLink("Change Application Settings", MVC.Admin.ApplicationSettings.List()) %>
                            </div>
                            <%if (string.Equals(ViewData["SecurityMode"], "SQL"))
                              { %>
                            <div class="adminItemContainer securitySettings">
                                <%= Html.ActionLink("Security Management", MVC.Admin.SecurityManagement.Index())%>
                            </div>
                            <%} %>
                            <div class="adminItemContainer emailSettings">
                                <%= Html.ActionLink("Email Diagnostics", MVC.Admin.EmailTemplate.Index())%>
                            </div>
                            <div class="adminItemContainer elmahLogs">
                                <a href="<%= Url.Content("~/Admin/Elmah.axd") %>">View Error Logs</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
