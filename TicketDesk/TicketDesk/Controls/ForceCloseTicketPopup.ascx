<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForceForceCloseTicketPopup.ascx.cs"
    Inherits="TicketDesk.Controls.ForceCloseTicketPopup" %>
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
<asp:Button ID="ShowForceCloseTicketButton" CausesValidation="false" runat="server"
    Text="Close (force)" />
<ajaxToolkit:ModalPopupExtender BehaviorID="forceCloseTicketBH" ID="ForceCloseTicketModalPopupExtender"
    runat="server" TargetControlID="ShowForceCloseTicketButton" PopupControlID="ForceCloseTicketPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelForceCloseTicketButton"
    DropShadow="true" PopupDragHandleControlID="ForceCloseTicketPanelHandle" />
<asp:Panel ID="ForceCloseTicketPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="ForceCloseTicketPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Do you want to close the ticket by force (the ticket is not resolved)?<br />
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
            <br />
            Comments (required):
            <asp:RequiredFieldValidator ID="CommentsRequiredValidator" runat="server" Display="Dynamic"
                ControlToValidate="CommentsTextBox" ValidationGroup="ForceCloseTicketPopup" Text="*"
                ErrorMessage="Comments are required" /><br />
            <asp:TextBox ValidationGroup="ForceCloseTicketPopup" ID="CommentsTextBox" TextMode="MultiLine"
                Rows="5" runat="server" Width="100%" />
            <br />
            <asp:Button ID="ForceCloseTicketButton" ValidationGroup="ForceCloseTicketPopup" OnClick="ForceCloseTicketButton_Click"
                runat="server" Text="Close By Force" />
            <asp:Button ID="CancelForceCloseTicketButton" CausesValidation="false" ValidationGroup="ForceCloseTicketPopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
