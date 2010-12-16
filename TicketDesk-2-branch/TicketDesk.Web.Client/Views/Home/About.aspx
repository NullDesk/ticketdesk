<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    About Us
</asp:Content>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="CustomHeadContent">
    <script type="text/javascript">

        $(document).ready(function () { corners(); });

        function corners() {
            $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
        }

    </script>
</asp:Content>
<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <% var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.HomeController; %>
    <div class="contentContainer">
        <div style="max-width: 600px; margin: auto;">
            <div class="displayContainerOuter">
                <div class="displayContainerInner" style="width: 100%;">
                    <div>
                        <div class="activityHeadWrapper">
                            <div class="activityHead">
                                About TicketDesk 2.0
                            </div>
                        </div>
                        <div class="activityBody" style="padding: 15px; min-height: 200px;">
                            <p>
                                <a href="http://ticketdesk.codeplex.com">TicketDesk 2.0</a> is an open source project
                                hosted on <a href="http://codeplex.com">CodePlex</a>.
                            </p>
                            <p>
                                TicketDesk is designed to be a fully functional, but simplistic issue tracking system
                                for IT help desks. The focus of TicketDesk is enabling quick, and easy communication
                                between help desk staff and users within a single organization.
                            </p>
                            <p>
                                For programmers, TicketDesk can be a great learning tool for the newer breed of
                                Microsoft development technologies. The general design also makes a good foundation
                                for a large number of different projects.
                            </p>
                            <p>
                                TicketDesk uses the following technologies:
                            </p>
                            <ul>
                                <li>Asp.net MVC Framework (2.0) on .NET 4.0</li>
                                <li>MVCContrib</li>
                                <li>Entity Framework 4.0</li>
                                <li>Managed Extensibility Framework (MEF)</li>
                                <li>SQL 2005 or 2008 (compatible with Express and Full SQL editions).</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
