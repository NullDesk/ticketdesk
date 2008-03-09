<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CancelMoreInfoPopup.ascx.cs" Inherits="TicketDesk.Controls.CancelMoreInfoPopup" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>

<asp:Button ID="ShowCancelMoreInfoButton" CausesValidation="false" runat="server" Text="Cancel More Info" />
<ajaxToolkit:ModalPopupExtender BehaviorID="cancelMoreInfoBH" ID="CancelMoreInfoModalPopupExtender"
    runat="server" TargetControlID="ShowCancelMoreInfoButton" PopupControlID="CancelMoreInfoPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelCancelMoreInfoButton"
    DropShadow="true" PopupDragHandleControlID="CancelMoreInfoPanelHandle" />
<asp:Panel ID="CancelMoreInfoPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="CancelMoreInfoPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Cancel request for more information?<br />
                
            </div>
        </asp:Panel>
        <div style="padding:5px;">
            <br />
            Comments (optional): <asp:TextBox ValidationGroup="CancelMoreInfoPopup" ID="CommentsTextBox" TextMode="MultiLine" Rows="5" runat="server" Width="100%" />
             <br />
             <asp:Button ID="CancelMoreInfoButton" ValidationGroup="CancelMoreInfoPopup" OnClick="CancelMoreInfoButton_Click"
                runat="server" Text="Cancel More Info" />
            <asp:Button ID="CancelCancelMoreInfoButton" CausesValidation="false" ValidationGroup="CancelMoreInfoPopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>