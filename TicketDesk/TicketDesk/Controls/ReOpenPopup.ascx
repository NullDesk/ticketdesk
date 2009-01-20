<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReOpenPopup.ascx.cs"
    Inherits="TicketDesk.Controls.ReOpenPopup" %>
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
<asp:Button ID="ShowReOpenButton" CausesValidation="false" runat="server" Text="Re-Open" />
<ajaxToolkit:ModalPopupExtender BehaviorID="reOpenBH" ID="ReOpenModalPopupExtender"
    runat="server" TargetControlID="ShowReOpenButton" PopupControlID="ReOpenPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelReOpenButton" DropShadow="true"
    PopupDragHandleControlID="ReOpenPanelHandle" />
<asp:Panel ID="ReOpenPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="ReOpenPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Do you want to re-open this ticket?<br />
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
            <br />
            Comments (required):
            <asp:Label ID="lblCommentRequired" runat="server" ForeColor="Red" Text="A comment is required"
                Visible="false" />
            <fck:FCKeditor ID="CommentsTextBox" runat="server" ToolbarSet="Basic" />
            <br />
            <br />
            <asp:Button ID="ReOpenButton" ValidationGroup="ReOpenPopup" OnClick="ReOpenButton_Click"
                runat="server" Text="Re-Open Ticket" />
            <asp:Button ID="CancelReOpenButton" CausesValidation="false" ValidationGroup="ReOpenPopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
