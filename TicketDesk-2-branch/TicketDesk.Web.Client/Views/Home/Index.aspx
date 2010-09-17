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
                <table cellpadding="0" cellspacing="0" style="width:100%;">
                    <tbody>
                        <tr>
                            <td style="vertical-align: top;background-color: #fff;">
                                <div style="padding: 10px;">
                                    <h2>
                                        Welcome to TicketDesk MVC!</h2>
                                    <p>
                                        This is an unstable beta version of the <b>TicketDesk 2.0 MVC</b> application.
                                        Much of the functionality of this site will be broken or incomplete. This site is intended
                                        to provide a functional preview of the new platform.
                                    </p>
                                    <p>
                                    There are four pre-defined example users:
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
                                        <li>Monkey Boy: login as "monkeyboy" with "monkeyboy" as the password
                                            <br /> 
                                            This user is a ticket submitter only and is an example of how the site would behave
                                            to regualr end-users.<br /> 
                                            <br /> 
                                        </li> 
                                        <li>Other Staff: login as "otherstaff" with "otherstaff" as the password
                                            <br /> 
                                            This is a help desk staff member, but not an administrator.<br /> 
                                            <br /> 
                                        </li> 
                                    </ul> 
                                    <p> 
                                        You can register your own users, but they will only be members of the "ticket submitters
                                        role" for now (until the admin tools for user management are implemented).</p> 
                                    <p>
                                        Notes:
                                    </p>
                                    <ul>
                                        <li>
                                        Admin tools have not been implemented
                                        <br /> <br />
                                        </li>
                                        <li>
                                        There are still significant performance issues with AD. An advanced AD cache mechanism is in the works.
                                        <br /> <br />
                                        </li>
                                        <li>
                                        Unit tests projects are not checked-in currently (my experimentation with pex and moles broke them). They will return in a future check-in.
                                        <br /> <br />
                                        </li>
                                        <li>
                                        Very long titles that lack a space can break the formatting of TicketCenter's grid display in some 
                                        cases. Maximizing the screen is enough to work around this limitation.<br /><br />
                                    
                                        </li>
                                        <li>
                                        Some of the jquery display effects don't render correctly in IE7/8 in some cases. 
                                        <br /> <br />
                                        </li>
                                        <li>
                                        Notifications are queued by this application, but will not be sent by it. 
                                        <br /> <br />
                                        </li>
                                        <li>
                                        Ticket Search has not been implemented.
                                         <br /><br />
                                        </li>
                                        <li>
                                        The default lists in TicketCenter will use multi-column sorting (same as 1.x), but if you change
                                        the default sort it will only support single-column sorting from that point forward. 
                                        <br /><br />
                                        </li>
                                    </ul>
                                   
                                </div>
                                <div class="clear">
                                </div>
                            </td>
                            <td style="max-width: 500px; vertical-align: top;  padding:0px;border-left: solid 1px #B3CBDF; ">
                                <div style=" float:right;">
                                    <div style="padding: 8px;">
                                        
                                        <h3>
                                            This Release</h3>
                                        <ul>
                                            <li>Replaced MarkItUp Editor with the WMD Editor.</li>
                                            <li>Implemented new AD security providers</li>
                                            <li>Migrated to .net 4.0 stack and MS MVC framework 2.0</li>
                                            <li>Entity Layer migrated to EF 4.0</li>
                                            <li>Implemented model validation with DataAnnotations</li>
                                            <li>Implemented MEF (Managed Extensibility Framework) for dependency injection and IoC</li>
                                            <li>Improved attachment management UIs with a flash-based file uploader</li>
                                            <li>Refactored custom model binding for attachments</li>
                                            <li>Implemented T4MVC Helpers (eliminated "magic strings")</li>
                                        </ul>
                                       
                                        <hr />
                                        <h3>Tip:</h3>
                                        <div>
                                            <p>
                                                Wanna see a trick? Try out TicketDesk MVC in a browser that doesn't have javascript
                                                enabled.
                                            </p>
                                            <p>
                                                Ticket attachments are not supported without javascript, but all other features
                                                should be 100% functional (though this hasn't been fully tested yet).
                                            </p>
                                        </div>
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
