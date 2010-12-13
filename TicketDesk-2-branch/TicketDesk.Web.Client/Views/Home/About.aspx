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
                                About TicketDesk MVC
                            </div>
                        </div>
                        <div class="activityBody" style="padding: 15px; min-height: 200px;">
                            <p>
                                TicketDesk 2.0 MVC is a branch of the open source <a href="http://ticketdesk.codeplex.com">
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
