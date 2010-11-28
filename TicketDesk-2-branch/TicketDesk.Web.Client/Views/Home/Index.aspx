<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="CustomHeadContent" runat="server">
    <script type="text/javascript">

        $("document").ready(function () { Corners(); });
        function Corners() {
            $(".displayContainerInner").corner("bevel tl 30px").corner("bevel tr 6px").corner("bevel bottom 6px").parent().css('padding', '6px').corner("round keep tl 20px").corner("round keep tr 12px").corner("round keep bottom 12px");
        }
    </script>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contentContainer">
        <div class="displayContainerOuter">
            <div class="displayContainerInner">
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tbody>
                        <tr>
                            <td style="vertical-align: top; background-color: #fff;">
                                <div style="padding: 10px;">
                                    <h2>
                                        Welcome to TicketDesk 2.0 (beta)!</h2>
                                    <p>
                                        This is a beta version of <b>TicketDesk 2.0</b>. Most of it should work fine, but
                                        it is still a beta release.
                                    </p>
                                     <p>
                                    There are two pre-defined example users:
                                    </p>
                                     <ul> 
                                        <li>Admin User: login as "admin" with "admin" as the password.
                                            <br /> 
                                            This user has full access to the system and is a member of the help desk staff role.<br /> 
                                            <br /> 
                                        </li> 
                                        <li>Toastman Wasiesname: login as "toastman" with "toastman" as the password.
                                            <br /> 
                                            This user is a ticket submitter only and is an example of how the site would behave
                                            to regualr end-users.<br /> 
                                            <br /> 
                                        </li> 
                                      
                                    </ul> 
                                    <p> 
                                        You can register your own users, but they will only be members of the "ticket submitters
                                        role" for now (until the admin tools for user management are implemented).</p> 
                                    <p>
                                    <p>
                                        Notes:
                                    </p>
                                    <ul>
                                        <li>Admin tools have not been implemented
                                            <br />
                                            <br />
                                        </li>
                                        <li>Some of the jquery display effects don't render correctly in IE7/8 in some cases.
                                            <br />
                                            <br />
                                        </li>
                                        <li>Notifications are queued by this application, but will not be sent by it.
                                            <br />
                                            <br />
                                        </li>
                                        <li>The default lists in TicketCenter will use multi-column sorting (same as 1.x), but
                                            if you change the default sort it will only support single-column sorting from that
                                            point forward.
                                            <br />
                                            <br />
                                        </li>
                                    </ul>
                                </div>
                                <div class="clear">
                                </div>
                            </td>
                            <td style="max-width: 500px; min-width: 300px; vertical-align: top; padding: 0px;
                                border-left: solid 1px #B3CBDF;">
                                <div style="float: right;">
                                    <div style="padding: 8px;">
                                        <h3>
                                            This Release</h3>
                                        <ul>
                                            <li>Back-Link in TicketEditor to return you to the TicketCenter</li>
                                            <li>Implemented new AD caching mechanism; should boost performance</li>
                                            <li>New ticket search feature using full-text indexing from Lucene.net</li>
                                            <li>New "unassigned" image flag</li>
                                            <li>Fixed bullet-list and blockquite bug in WMD Editor</li>
                                        </ul>
                                      
                                       
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
