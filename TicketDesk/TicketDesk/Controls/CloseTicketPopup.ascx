<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CloseTicketPopup.ascx.cs" Inherits="TicketDesk.Controls.CloseTicketPopup" %>
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
        <div style="padding:5px;">
           
            <br />
            Comments (optional): <asp:TextBox ValidationGroup="CloseTicketPopup" ID="CommentsTextBox" TextMode="MultiLine" Rows="5" runat="server" Width="100%" />
            <br />
            <asp:Button ID="CloseTicketButton" ValidationGroup="CloseTicketPopup" OnClick="CloseTicketButton_Click"
                runat="server" Text="Close Ticket" />
            <asp:Button ID="CancelCloseTicketButton" CausesValidation="false" ValidationGroup="CloseTicketPopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>