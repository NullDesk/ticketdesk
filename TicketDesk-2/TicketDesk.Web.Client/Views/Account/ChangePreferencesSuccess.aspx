<%@  Language="C#" MasterPageFile="~/Views/Account/Shared/MyAccount.master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="changeDisplayNameSuccessHead" ContentPlaceHolderID="MyAccountTitleContent"
    runat="server">
    Change Preferences
</asp:Content>
<asp:Content ID="changePreferencesSuccessContent" ContentPlaceHolderID="MyAccountContent"
    runat="server">
    <div>
        <div class="displayContainerOuter">
            <div class="displayContainerInner" style="width: 100%;">
                <div>
                    <div class="activityHeadWrapper">
                        <div class="activityHead">
                            Change Preferences
                        </div>
                    </div>
                    <div class="activityBody" style="padding: 15px; min-height:200px;">
                        <span style="font-size:larger;">Your preferences have been changed successfully.</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
