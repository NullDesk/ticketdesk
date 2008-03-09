<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResolvePopup.ascx.cs" Inherits="TicketDesk.Controls.ResolvePopup" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>

<asp:Button ID="ShowResolveButton" CausesValidation="false" runat="server" Text="Resolve" />
<ajaxToolkit:ModalPopupExtender BehaviorID="resolveBH" ID="ResolveModalPopupExtender"
    runat="server" TargetControlID="ShowResolveButton" PopupControlID="ResolvePanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelResolveButton"
    DropShadow="true" PopupDragHandleControlID="ResolvePanelHandle" />
<asp:Panel ID="ResolvePanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="ResolvePanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Do you want to resolve this ticket?<br />
            </div>
        </asp:Panel>
        <div style="padding:5px;">
            <br />
            Comments (required): <asp:RequiredFieldValidator ID="CommentsRequiredValidator" runat="server" 
            Display="Dynamic" ControlToValidate="CommentsTextBox" ValidationGroup="ResolvePopup" Text="*" ErrorMessage="Comments are required" /><br />
            <asp:TextBox ValidationGroup="ResolvePopup" ID="CommentsTextBox" TextMode="MultiLine" Rows="5" runat="server" Width="100%" />
            <br />
             <asp:Button ID="ResolveButton" ValidationGroup="ResolvePopup" OnClick="ResolveButton_Click"
                runat="server" Text="Resolve Ticket" />
            <asp:Button ID="CancelResolveButton" CausesValidation="false" ValidationGroup="ResolvePopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>