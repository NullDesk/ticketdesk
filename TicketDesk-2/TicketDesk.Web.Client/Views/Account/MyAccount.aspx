<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="IndexHead" ContentPlaceHolderID="TitleContent" runat="server">
    My Account
</asp:Content>
<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="CustomHeadContent">

    <script type="text/javascript">

        $(document).ready(function() { corners(); });

        function corners() {
            $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
        }

    </script>

</asp:Content>
<asp:Content ID="IndexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contentContainer">
        <div  style="max-width:600px; margin:auto;">
            <div class="displayContainerOuter">
                <div class="displayContainerInner" style="width: 100%;">
                    <div>
                        <div class="activityHeadWrapper">
                            <div class="activityHead">
                                My Account
                            </div>
                        </div>
                        <div class="activityBody" style="padding: 15px; min-height: 200px; ">
                            <p>
                                Manage your account by choosing an action from the list below.</p>
                            <% Html.RenderPartial(MVC.Account.Views.Shared.MyAccountMiniNav);  %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
