<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CloseTicketPopup.ascx.cs"
    Inherits="TicketDesk.Controls.CloseTicketPopup" %>
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
<asp:Button ID="ShowCloseTicketButton" CausesValidation="false" runat="server" Text="Close" />
<ajaxToolkit:ModalPopupExtender BehaviorID="closeTicketBH" ID="CloseTicketModalPopupExtender"
    runat="server" TargetControlID="ShowCloseTicketButton" PopupControlID="CloseTicketPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelCloseTicketButton"
    DropShadow="true" PopupDragHandleControlID="CloseTicketPanelHandle" />
<asp:Panel ID="CloseTicketPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="CloseTicketPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Do you want to close the ticket?<br />
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
            <br />
            Comments (optional):
            <fck:FCKeditor ID="CommentsTextBox" runat="server" ToolbarSet="Basic" />
            <br />
            <asp:Button ID="CloseTicketButton" ValidationGroup="CloseTicketPopup" OnClick="CloseTicketButton_Click"
                runat="server" Text="Close Ticket" />
            <asp:Button ID="CancelCloseTicketButton" CausesValidation="false" ValidationGroup="CloseTicketPopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
