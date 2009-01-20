<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeTagsPopup.ascx.cs"
    Inherits="TicketDesk.Controls.ChangeTagsPopup" %>
<%@ Register Src="TagPicker.ascx" TagName="TagPicker" TagPrefix="uc1" %>
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
<asp:ImageButton ImageUrl="~/Controls/Images/edit.gif" CausesValidation="false" ID="ShowChangeTagsButton"
    runat="server" Text="..." />
<ajaxToolkit:ModalPopupExtender BehaviorID="changeTagsBH" ID="ChangeTagsModalPopupExtender"
    runat="server" TargetControlID="ShowChangeTagsButton" PopupControlID="ChangeTagsPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelChangeTagsButton"
    DropShadow="true" PopupDragHandleControlID="ChangeTagsPanelHandle" />
<asp:Panel ID="ChangeTagsPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="ChangeTagsPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Choose the new Tags for this ticket:
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
            <br />
            <uc1:TagPicker ID="TagPickerControl" runat="server" />
            <br />
            <br />
            Comments (optional):
            <fck:FCKeditor ID="CommentsTextBox" runat="server" ToolbarSet="Basic" />
            <br />
            <br />
            <asp:Button ID="ChangeTagsButton" ValidationGroup="TagsChangePopup" OnClick="ChangeTagsButton_Click"
                runat="server" Text="Change Tags" />
            <asp:Button ID="CancelChangeTagsButton" CausesValidation="false" ValidationGroup="TagsChangePopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
