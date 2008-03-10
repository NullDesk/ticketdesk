<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeAffectsCustomerPopup.ascx.cs"
    Inherits="TicketDesk.Controls.ChangeAffectsCustomerPopup" %>
<%  // TicketDesk - Attribution notice
    // Contributor(s):
    //
    //      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
    //
    // This file is distributed under the terms of the Microsoft Public 
    // License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
    // for the complete terms of use. 
    //
    // For any distribution that contains code from this file, this notice of 
    // attribution must remain intact, and a copy of the license must be 
    // provided to the recipient.
%>

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:ImageButton ImageUrl="~/Controls/Images/edit.gif" CausesValidation="false" ID="ShowChangeAffectsCustomerButton"
    runat="server" Text="..." />
<ajaxToolkit:ModalPopupExtender BehaviorID="changeAffectsCustomerBH" ID="ChangeAffectsCustomerModalPopupExtender"
    runat="server" TargetControlID="ShowChangeAffectsCustomerButton" PopupControlID="ChangeAffectsCustomerPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelChangeAffectsCustomerButton"
    DropShadow="true" PopupDragHandleControlID="ChangeAffectsCustomerPanelHandle" />
<asp:Panel ID="ChangeAffectsCustomerPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="ChangeAffectsCustomerPanelHandle" runat="server" Style="cursor: move;
            border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Does this ticket affect any customers?
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
            <br />
            <asp:RadioButtonList ValidationGroup="AffectsCustomerChangePopup" ID="AffectsCustomerList" runat="server"
                RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Text="Yes" Value="yes" />
                <asp:ListItem Text="No" Value="no" />
            </asp:RadioButtonList>
            <br />
            <br />
            Comments (required):<asp:RequiredFieldValidator ID="CommentsRequiredValidator" runat="server" Display="Dynamic"
                ControlToValidate="CommentsTextBox" ValidationGroup="AffectsCustomerChangePopup" Text="*"
                ErrorMessage="Comments are required" /><br />
            <asp:TextBox ID="CommentsTextBox" ValidationGroup="AffectsCustomerChangePopup" TextMode="MultiLine"
                Rows="5" runat="server" Width="100%" />
            <br />
            <asp:Button ID="ChangeAffectsCustomerButton" ValidationGroup="AffectsCustomerChangePopup"
                OnClick="ChangeAffectsCustomerButton_Click" runat="server" Text="Change Affects Customer" />
            <asp:Button ID="CancelChangeAffectsCustomerButton" CausesValidation="false" ValidationGroup="AffectsCustomerChangePopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
