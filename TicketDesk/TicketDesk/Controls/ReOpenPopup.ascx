<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReOpenPopup.ascx.cs" Inherits="TicketDesk.Controls.ReOpenPopup" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>

<asp:Button ID="ShowReOpenButton" CausesValidation="false" runat="server" Text="Re-Open" />
<ajaxToolkit:ModalPopupExtender BehaviorID="reOpenBH" ID="ReOpenModalPopupExtender"
    runat="server" TargetControlID="ShowReOpenButton" PopupControlID="ReOpenPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelReOpenButton"
    DropShadow="true" PopupDragHandleControlID="ReOpenPanelHandle" />
<asp:Panel ID="ReOpenPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="ReOpenPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Do you want to re-open this ticket?<br />
                
            </div>
        </asp:Panel>
        <div style="padding:5px;">
           
           
            <br />
            Comments (required): <asp:RequiredFieldValidator ID="CommentsRequiredValidator" runat="server" 
            Display="Dynamic" ControlToValidate="CommentsTextBox" ValidationGroup="ReOpenPopup" Text="*" ErrorMessage="Comments are required" /><br />
            <asp:TextBox ValidationGroup="ReOpenPopup" ID="CommentsTextBox" TextMode="MultiLine" Rows="5" runat="server" Width="100%" />
            <br />
             <asp:Button ID="ReOpenButton" ValidationGroup="ReOpenPopup" OnClick="ReOpenButton_Click"
                runat="server" Text="Re-Open Ticket" />
            <asp:Button ID="CancelReOpenButton" CausesValidation="false" ValidationGroup="ReOpenPopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>