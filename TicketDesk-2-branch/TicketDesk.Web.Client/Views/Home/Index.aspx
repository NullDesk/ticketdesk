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
                                        Welcome to TicketDesk 2.0 <sup>(RC1)</sup></h2>
                                    <p>
                                        This is a pre-release version of <b>TicketDesk 2.0</b>. Most of it should work fine,
                                        but it isn&#39;t quite done cooking yet. This preview is intended to allow users
                                        to experiment with the new system and offer feedback and suggestions before the
                                        final release. This is your opportunity to help us help you!
                                    </p>
                                    <p>
                                        There are three pre-defined example users:
                                    </p>
                                    <ul>
                                        <li>Admin User: login as "admin" with "admin" as the password.
                                            <br />
                                            This user has full access to the system and is a member of all three roles.<br />
                                            <br />
                                        </li>
                                        <li>Other Staffer: login as "otherstaffer" with "otherstaffer" as the password.
                                            <br />
                                            This user is a help desk staff member and submitter; this is an example of how the
                                            site would behave for most of the technical staff.<br />
                                            <br />
                                        </li>
                                        <li>Toastman Wazisname: login as "toastman" with "toastman" as the password.
                                            <br />
                                            This user is a ticket submitter only and is an example of how the site would behave
                                            to regualr end-users.<br />
                                            <br />
                                        </li>
                                    </ul>
                                    <p>
                                        Notes:
                                    </p>
                                    <ul>
                                        <li>This version should work in Firefox 4+, Chrome 7+, and IE8. It has not been tested
                                            with other browsers yet.&nbsp;&nbsp;
                                            <br />
                                            &nbsp; </li>
                                        <li>This system is designed for all major browsers, but not all of the UI effects work
                                            exactly the same in all of them; Google&#39;s Chrome browser has the best overall
                                            user experience, but all of the features should be usable in all browsers.
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
                                            <li>Refactord UI for IE8 compatibility.</li>
                                            <li>New side-by-side preview window for the text editors.</li>
                                            <li>New Email content templates specifically for outlook environments. </li>
                                            <li>Attachments now fully supported in AD environments. </li>
                                            <li>Fixed unassigned flag appearing on closed tickets that lacked an assigned user.</li>
                                            <li>Numerous minor tweaks, fixes, and enhancements. </li>
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
