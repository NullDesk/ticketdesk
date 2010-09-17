<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TicketDesk.Web.Client.Models.TicketCenterListViewModel>" %>

<asp:Content ID="head" ContentPlaceHolderID="TitleContent" runat="server">
	 Ticket Center
</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="CustomHeadContent" runat="server">

    <script type="text/javascript">

        $("document").ready(function () { Corners(); });
        function Corners() {
            $(".displayContainerInner").corner("bevel 6px").parent().css('padding', '4px').corner("round keep  12px");

        }
    </script>

</asp:Content>

<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server">
   <div class="contentContainer">
        <div id="submenucontainer" style="margin: 25px 15px 0px 15px;">
            <% 
                Html.RenderPartial(MVC.TicketCenter.Views.Controls.ListViewMenu, Model); %>
        </div>
        <div class="displayContainerOuter" style="margin-top:0;">
            <div class="displayContainerInner" style="margin-top:0; width:100%;">
               
                    
                        <% Html.RenderPartial(MVC.TicketCenter.Views.Controls.TicketList, Model, ViewData); %>
                    
            </div>
        </div>
    </div>

</asp:Content>


