<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="AdminTitleContent" runat="server">
    Email Diagnostics
</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="AdminCustomHead" runat="server">
    <script type="text/javascript">

        $(document).ready(function () { corners(); });

        function corners() {
            $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
        }

    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="contentContainer">
        <div style="margin: auto;">
            <div class="displayContainerOuter">
                <div class="displayContainerInner" style="width: 100%;">
                    <div>
                        <div class="activityHeadWrapper">
                            <div class="activityHead">
                                Email Diagnostics
                            </div>
                        </div>
                        <div class="activityBody" style="padding: 15px; min-height: 200px;">
                            <p >
                                View examples of how email body content will be rendered by entring a ticket ID
                                and a rendering mode. Please remember that the emails may not render the same in 
                                an actual email client.
                            </p>
                            <% using (Html.BeginForm())
                               {%>
                            <div class="editor-field">
                                Ticket ID: <%: Html.TextBox("id", null, new { Style = "width:25px;" })%>
                            <%: Html.RadioButton("mode", "html", true)%>Regular HTML
                                <%: Html.RadioButton("mode", "outlook", false)%>Outlook HTML
                                <%: Html.RadioButton("mode", "text", false)%>Plain Text
                                
                            </div>
                            <p>
                                <input id="submitButton" type="submit" value="Render Ticket Email" class="activityButton"
                                    style="display: inline;" />
                                <span class="neverMindLink">
                                    <%: Html.ActionLink("Nevermind", MVC.Admin.AdminHome.Index())%>
                                </span><%: Html.ValidationMessage("id") %>
                            </p>
                            <%
                                } 
                            %>
                            <hr />
                            <p >
                               You can also force TicketDesk to send all waiting notification immediatly. This is useful for local development
                               when you have the email notifications system disable in configuration. This doesn't do anything differently than
                               what the notifications service does by default. For example: messages that are still waiting on consolidations or 
                               are waiting for a re-try will continue to be held until their scheduled delivery time has passed. This link 
                               doesn't change the behavior of the notifications system; it just invokes it the same as the timer normally would. 
                            </p>
                            <p>
                             <%: Html.ActionLink("Send notifications", MVC.Admin.EmailTemplate.ActionNames.ProcessWaitingNotesNow)%>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
