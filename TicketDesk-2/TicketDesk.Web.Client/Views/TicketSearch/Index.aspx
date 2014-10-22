<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TicketDesk.Domain.Models.Ticket>>" %>


<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Display
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CustomHeadContent" runat="server">
    <script src="<%= Links.Scripts.jquery_clickable_0_1_9_js %>" type="text/javascript"></script>
    <% 
        if (false)
        {
    %>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.clickable-0.1.9.js" type="text/javascript"></script>
    <%
        }
    %>
    <script type="text/javascript">

        $("document").ready(
        function () {
            $(".displayContainerInner").corner("bevel left 6px").parent().css('padding', '3px').corner("round left keep 12px").corner("round right keep 4px");
            $(".searchDisplayContainerInner").corner("bevel top 6px").parent().css('padding', '3px').corner("round top keep 12px");




        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contentContainer">
        <div class="searchDisplayContainerOuter">
            <div class="searchDisplayContainerInner">
                <div class="activityHeadWrapper">
                    <div class="activityHead">
                        Search Results:
                    </div>
                </div>
                <div class="activityBody fieldSubText" style="padding: 3px 3px 3px 25px;">
                    <%: Html.Label("Searched For:") %>
                    "<%: ViewData["searchPhrase"] %>"
               </div>
               <div class="activityBody">
        <% if (Model != null && Model.Count() > 0)
           {
        %>
        <%       
           
           
            foreach (var item in Model)
            {
           
        %>
        <div class="displayContainerOuter" style="width:75%;">
            <div class="displayContainerInner">
                <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.Details, item, ViewData); %>
            </div>
        </div>
        <%   
            }
        %>
        <br /><br /><br />
        <%
           }
           else
           {
        %>
        
            <div style="width:90%;text-align:center; padding:25px;">
                There are no tickets to display.
            </div>
        <%
            }
        %>
    </div> </div>
            </div>
        </div>
</asp:Content>
