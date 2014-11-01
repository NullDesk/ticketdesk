<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<TicketDesk.Domain.Models.Setting>" %>

<%@ Import Namespace="TicketDesk.Domain.Utilities" %>
<asp:Content ID="EditTitle" ContentPlaceHolderID="AdminTitleContent" runat="server">
    Edit
</asp:Content>
<asp:Content ID="EditHead" ContentPlaceHolderID="AdminCustomHead" runat="server">
    <script type="text/javascript">

        $(document).ready(function () { corners(); });

        function corners() {
            $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
        }

    </script>
</asp:Content>
<asp:Content ID="EditMain" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="contentContainer">
        <div style="max-width: 600px; margin: auto;">
            <div class="displayContainerOuter">
                <div class="displayContainerInner" style="width: 100%;">
                    <div>
                        <div class="activityHeadWrapper">
                            <div class="activityHead">
                                Change Application Property:
                            </div>
                        </div>
                        <div class="activityBody" style="margin: auto; padding: 25px;">
                            <% using (Html.BeginForm())
                               {%>
                            
                            <div class="editor-label">
                                <%: Html.Label(Model.SettingName.ConvertPascalCaseToFriendlyString())%>:
                            </div>
                            <div class="editor-field">
                                <%: Html.TextBoxFor(model => model.SettingValue, new { Style = "width:350px;" })%>
                                <%: Html.ValidationMessageFor(model => model.SettingValue) %>
                            </div>
                            <div class="fieldSubText">
                                <%= (Model.SettingDescription ?? string.Empty).Replace("\\n","<br/>") %>
                            </div>
                            <p>
                                <input id="saveButton" type="submit" value="Save" class="activityButton" style="display: inline;" /><span
                                    class="neverMindLink">
                                    <%: Html.ActionLink("Nevermind", MVC.Admin.ApplicationSettings.ActionNames.List)%>
                                </span>
                                <%: Html.ValidationSummary(true) %>
                            </p>
                            <% } %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
