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
                                About TicketDesk 2.1
                            </div>
                        </div>
                        <div class="activityBody" style="padding: 15px; min-height: 200px;">
                            <p>
                                <a href="http://ticketdesk.codeplex.com">TicketDesk 2.1</a> is an open source project
                                hosted on <a href="http://codeplex.com">CodePlex</a>.
                            </p>
                            <p>
                                TicketDesk is designed to be a fully functional yet simplistic web based issue tracking system
                                with a focus on promoting frictionless communication between the help desk and
                                end-users. TicketDesk is designed primarily for internal use within a single organization. 
                            </p>
                            
                            <p>
                                TicketDesk was developed with the following technologies:
                            </p>
                            <ul>
                                <li>Asp.net MVC Framework (2.0) on .NET 4.0</li>
                                <li>MVCContrib</li>
                                <li>Entity Framework 4.0</li>
                                <li>Managed Extensibility Framework (MEF)</li>
                                <li>SQL 2005 or 2008 (compatible with Express and Full SQL editions).</li>
                            </ul>
                            <p>
                                This version has since been updated for compatibility with the following:
                            </p>
                            <ul>
                                <li>Asp.net MVC 4</li>
                                <li>.net Framework 4.5</li>
                                <li>Managed Extensibility Framework 2</li>
                                <li>Tested with SQL 2012 localdb, express, and full editions</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
