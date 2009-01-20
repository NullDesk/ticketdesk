<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeOwnedByPopup.ascx.cs"
    Inherits="TicketDesk.Controls.ChangeOwnedByPopup" %>
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
<asp:ImageButton ImageUrl="~/Controls/Images/edit.gif" CausesValidation="false" ID="ShowChangeOwnedByButton"
    runat="server" Text="..." OnClick="ShowChangeOwnedByButton_Click" />
<asp:Button ID="HiddenShowChangeOwnedByButton" CausesValidation="false" runat="server"
    Text="ChangeOwnedBy" Style="display: none" />
<ajaxToolkit:ModalPopupExtender BehaviorID="changeOwnedByBH" ID="ChangeOwnedByModalPopupExtender"
    runat="server" TargetControlID="HiddenShowChangeOwnedByButton" PopupControlID="ChangeOwnedByPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelChangeOwnedByButton"
    DropShadow="true" PopupDragHandleControlID="ChangeOwnedByPanelHandle" />
<asp:Panel ID="ChangeOwnedByPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="ChangeOwnedByPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Change owner of the ticket:
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
            <br />
            <asp:DropDownList Width="200px" ValidationGroup="ChangeOwnedByPopup" ID="ChangeOwnedByDropDownList"
                runat="server" AppendDataBoundItems="true">
            </asp:DropDownList>
            <asp:CompareValidator ValidationGroup="ChangeOwnedByPopup" ID="CompareValidator3"
                runat="server" ControlToValidate="ChangeOwnedByDropDownList" Display="Dynamic"
                ErrorMessage="Please select a valid user for this ticket." Operator="NotEqual"
                ValueToCompare="-">*</asp:CompareValidator>
            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" PromptCssClass="ListSearch"
                TargetControlID="ChangeOwnedByDropDownList">
            </ajaxToolkit:ListSearchExtender>
            <br />
            <br />
            Comments (optional):
            <fck:FCKeditor ID="CommentsTextBox" runat="server" ToolbarSet="Basic" />
            <br />
            <asp:Button ID="ChangeOwnedByButton" ValidationGroup="ChangeOwnedByPopup" OnClick="ChangeOwnedByButton_Click"
                runat="server" Text="Change Owner" />
            <asp:Button ID="CancelChangeOwnedByButton" CausesValidation="false" ValidationGroup="ChangeOwnedByPopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
