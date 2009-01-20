<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeDetailsPopup.ascx.cs"
    Inherits="TicketDesk.Controls.ChangeDetailsPopup" %>
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
<asp:ImageButton ImageUrl="~/Controls/Images/edit.gif" CausesValidation="false" ID="ShowChangeDetailsButton"
    runat="server" Text="..." />
<ajaxToolkit:ModalPopupExtender BehaviorID="changeDetailsBH" ID="ChangeDetailsModalPopupExtender"
    runat="server" TargetControlID="ShowChangeDetailsButton" PopupControlID="ChangeDetailsPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelChangeDetailsButton"
    DropShadow="true" PopupDragHandleControlID="ChangeDetailsPanelHandle" />
<asp:Panel ID="ChangeDetailsPanel" runat="server" CssClass="ModalPopup" Style="width: 650px;
    display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="ChangeDetailsPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Edit the details for this ticket:
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
            <br />
            Details:<asp:Label ID="lblDetailsRequired" runat="server" ForeColor="Red" Text="Details are required."
                Visible="false" />
            <fck:FCKeditor ID="DetailsTextBox" runat="server" ToolbarSet="Basic" />
            <br />
            <br />
            Comments (optional):<br />
            <fck:FCKeditor ID="CommentsTextBox" runat="server" ToolbarSet="Basic" />
            <br />
            <asp:Button ID="ChangeDetailsButton" ValidationGroup="DetailsChangePopup" OnClick="ChangeDetailsButton_Click"
                runat="server" Text="Change Details" />
            <asp:Button ID="CancelChangeDetailsButton" CausesValidation="false" ValidationGroup="DetailsChangePopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
