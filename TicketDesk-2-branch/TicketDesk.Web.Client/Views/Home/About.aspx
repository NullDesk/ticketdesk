<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    About Us
</asp:Content>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="CustomHeadContent">
    <script type="text/javascript">

        $(document).ready(function () { Corners(); });

        function Corners() {
            $(".displayContainerInner").corner("bevel 6px").parent().css('padding', '4px').corner("round keep  12px");
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
                                About TicketDesk MVC
                            </div>
                        </div>
                        <div class="activityBody" style="padding: 15px; min-height: 200px;">
                            <p>
                                TicketDesk MVC is a branch of the open source <a href="http://ticketdesk.codeplex.com">
                                    TicketDesk project</a> at <a href="http://codeplex.com">CodePlex</a>. TicketDesk
                                was initially written before the ASP.NET MVC framework was released. Now that ASP.NET MVC
                                is stable, this next version of TicketDesk is a re-write of the original targeting
                                the new framework and technologies.
                            </p>
                            <p>
                                This site is intended to provide an early look at the upcoming TicketDesk MVC application
                                in a pre-release form. Some features will be missing, others may be broken, but
                                it should be enough to provide a glimpse of what the finished application will look
                                like.
                            </p>
                           
                            <p>
                                For more information on the TicketDesk MVC 2.0 project please see the article <a
                                    href="http://reddnet.net/code/ticketdesk-2-0-and-the-asp-net-mvc-framework/">"TicketDesk
                                    2.0 and the ASP.NET MVC Framework"</a> at <a href="http://reddnet.net">Reddnet.net</a>
                            </p>
                            <p>
                                If you are interested in the current shipping version of TicketDesk, please visit
                                the <a href="http://ticketdesk.codeplex.com">official TicketDesk project site</a>.
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
