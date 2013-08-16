<%@ Page Title="TicketDesk Home" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

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
                                    <h2>Welcome to TicketDesk 2.1 <sup>(.Net 4.5 Refresh)</sup></h2>
                                    <p>
                                        <i>You can hide this page by changing the "Hide Home Page" setting in the administration section's applicaiton settings tool.</i>
                                    </p>
                                    <p>
                                        There are three pre-defined example users in a default TicketDesk 2.0 distribution:
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
                                </div>
                                <div class="clear">
                                </div>
                            </td>
                            <td style="max-width: 500px; min-width: 300px; vertical-align: top; padding: 0px; border-left: solid 1px #B3CBDF;">
                                <div style="float: right;">
                                    <div style="padding: 8px;">
                                        <h3>This Release</h3>
                                        <strong>Roll-up of minor fixes</strong>
                                        <ul>
                                            <li>tag and priority fields not displaying</li>
                                            <li>details not indexed for search</li>
                                            <li>TicketCenter default view for submitters</li>
                                            <li>object locking</li>
                                            <li>inconsistent capitalization for usernames; notification failures</li>
                                            <li>account preferences, edit email address</li>
                                            <li>unlock user accounts from security management</li>
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
