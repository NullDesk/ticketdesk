<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<TicketDesk.Web.Client.Areas.Admin.Models.SecurityManagementUserViewModel>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="AdminTitleContent" runat="server">
    Edit User
</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="AdminCustomHead" runat="server">
    <script type="text/javascript">

        $(document).ready(function () { corners(); });

        function corners() {
            $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
        }
    </script>
    <script runat="server">
    protected object getHtmlEnabledAttributes(bool locked)
    {
        if(!locked)
        {
            return new {Disabled = true};
        }
        else
        {
            return null;
        }
    }    
</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">


    <div class="contentContainer">
        <div style="max-width: 600px; margin: auto;">
            <div class="displayContainerOuter">
                <div class="displayContainerInner" style="width: 100%;">
                    <div>
                        <div class="activityHeadWrapper">
                            <div class="activityHead">
                                Edit User:
                            </div>
                        </div>
                        <div class="activityBody" style="margin: auto; padding: 25px;">
                            <% using (Html.BeginForm())
                               {%>
                            <%: Html.ValidationSummary(true) %>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.UserName) %>
                                <%: Model.UserName %>
                            </div>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.DisplayName) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.TextBoxFor(model => model.DisplayName) %>
                                <%: Html.ValidationMessageFor(model => model.DisplayName) %>
                            </div>
                              <div>
                                <div class="editor-label">
                                    <%: Html.LabelFor(model => model.Email) %>
                                </div>
                                <div class="editor-field">
                                    <%: Html.TextBoxFor(model => model.Email)%>
                                    <%: Html.ValidationMessageFor(model => model.Email)%>
                                </div>
                            </div>
                            <div>
                                <div class="editor-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsAdmin) %>
                                </div>
                                <div class="editor-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsAdmin) %>
                                </div>
                            </div>
                            <div>
                                <div class="editor-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsStaff)%>
                                </div>
                                <div class="editor-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsStaff) %>
                                </div>
                            </div>
                            <div>
                                <div class="editor-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsSubmitter)%>
                                </div>
                                <div class="editor-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsSubmitter) %>
                                </div>
                            </div>
                             <div>
                                <div class="editor-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsApproved)%>
                                </div>
                                <div class="editor-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsApproved) %>
                                </div>
                            </div>
                            <div>
                                <div class="editor-field" style="display: inline;">
                                    <%: Html.CheckBoxFor(model => model.IsLockedOut, getHtmlEnabledAttributes(Model.IsLockedOut))%>
                                </div>
                                <div class="editor-label" style="display: inline;">
                                    <%: Html.LabelFor(model => model.IsLockedOut) %>
                                </div>
                            </div>
                          
                            <p>
                                <input type="submit" value="Save" class="activityButton" class="activityButton" style="display: inline;" /><span
                                    class="neverMindLink">
                                    <%: Html.ActionLink("Nevermind", MVC.Admin.SecurityManagement.ActionNames.UsersList)%>
                                </span>
                            </p>
                            <% } %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
