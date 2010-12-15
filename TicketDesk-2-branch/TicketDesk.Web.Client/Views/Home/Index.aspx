<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="CustomHeadContent" runat="server">
    <script type="text/javascript">

        $("document").ready(function () { cornersHome(); });
        function cornersHome() {
            $(".displayContainerInnerHome").corner("bevel tl 30px").corner("bevel tr 6px").corner("bevel bottom 6px").parent().css('padding', '6px').corner("round keep tl 20px").corner("round keep tr 12px").corner("round keep bottom 12px");
        }
    </script>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contentContainer">
        <div class="displayContainerOuterHome">
            <div class="displayContainerInnerHome">
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tbody>
                        <tr>
                            <td style="vertical-align: top; background-color: #fff;">
                                <div style="padding: 10px;">
                                    <h2>
                                        Welcome to TicketDesk 2.0 (beta 2)!</h2>
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
                                            <li>Admin tools have been implemented, except for SQL security user management.
                                                <br />
                                                <br />
                                            </li>
                                            <li>This release includes some database structure changes in the settings table, as
                                                well as the need for seed data for the default set of application settings.
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
                                            <li>Admin area and tools implemented</li>
                                            <li>Refactored application settings & moved most of the settings from web.config to
                                                the DB.</li>
                                            <li>TicketDesk 2 Logo (hey, I'm not a graphics guy... gimmie a break!)</li>
                                            <li>New Structure for the Settings DB table, as well as new "seed" data.</li>
                                            <li>New Email Diagnostics admin section; allows you to view how email templates will be rendered</li>
                                            <li>Fixed lucene bug; adding attachments for new tickets broke index update.</li>
                                            <li>Fixed bug with edit details not correctly identifying "changed" ticket properties when building the comment details.</li>
                                            <li>Changed search behavior to limit results to 20 (was 100)</li>
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
