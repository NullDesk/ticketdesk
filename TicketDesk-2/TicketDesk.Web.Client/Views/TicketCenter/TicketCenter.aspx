<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TicketDesk.Web.Client.Models.TicketCenterListViewModel>" %>

<asp:Content ID="head" ContentPlaceHolderID="TitleContent" runat="server">
	 Ticket Center
</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="CustomHeadContent" runat="server">
    <script src="<%= Links.Scripts.jquery_clickable_0_1_9_js %>" type="text/javascript"></script>

    <script type="text/javascript">

        $("document").ready(function () { corners(); clicks() });
        function corners() {
            $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
        }

        function clicks() {
            $(".clickable").clickable();
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


