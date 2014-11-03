<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TicketDesk.Domain.Models.Setting>>" %>

<%@ Import Namespace="TicketDesk.Domain.Utilities" %>
<asp:Content ID="IndexHead" ContentPlaceHolderID="AdminTitleContent" runat="server">
    Application Settings
</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="AdminCustomHead" runat="server">
 <script type="text/javascript">

     $(document).ready(function () { corners(); });

     function corners() {
         $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
     }

    </script>

</asp:Content>
<asp:Content ID="IndexContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="contentContainer">
        <div style="margin: auto;">
            <div class="displayContainerOuter">
                <div class="displayContainerInner" style="width: 100%;">
                    <div>
                        <div class="activityHeadWrapper">
                            <div class="activityHead">
                                Application Settings
                            </div>
                        </div>
                        <div class="activityBody" style="padding: 15px; min-height: 200px;">
                            <table class="ticketListGrid" cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <th>
                                    </th>
                                    <th>
                                        Name
                                    </th>
                                    <th>
                                        Value
                                    </th>
                                   
                                    <th>
                                        Type
                                    </th>
                                </tr>
                                <% foreach (var item in Model)
                                   { %>
                                <tr>
                                    <td>
                                        <%: Html.ActionLink("Edit", "Edit", new { settingName = item.SettingName })%>
                                    </td>
                                    <td style="white-space:normal;">
                                        <%: item.SettingName.ConvertPascalCaseToFriendlyString()%>
                                    </td>
                                    <td style="white-space:normal;">
                                        <%: item.SettingValue %>
                                    </td>
                                   
                                    <td>
                                        <%: item.SettingType%>
                                    </td>
                                </tr>
                                <% } %>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

