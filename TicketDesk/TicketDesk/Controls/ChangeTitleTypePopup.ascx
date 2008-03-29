<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeTitleTypePopup.ascx.cs"
    Inherits="TicketDesk.Controls.ChangeTitleTypePopup" %>
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
<asp:ImageButton ImageUrl="~/Controls/Images/edit.gif" CausesValidation="false" ID="ShowChangeTitleTypeButton"
    runat="server" Text="..." />
<ajaxToolkit:ModalPopupExtender BehaviorID="changeTitleTypeBH" ID="ChangeTitleTypeModalPopupExtender"
    runat="server" TargetControlID="ShowChangeTitleTypeButton" PopupControlID="ChangeTitleTypePanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelChangeTitleTypeButton"
    DropShadow="true" PopupDragHandleControlID="ChangeTitleTypePanelHandle" />
<asp:Panel ID="ChangeTitleTypePanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="ChangeTitleTypePanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Edit the Type and/or Title for the ticket:
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
            <br />
            Type:<asp:DropDownList ValidationGroup="TitleTypeChangePopup" ID="TypeDropDownList" runat="server" style="width:200px;">
                
            </asp:DropDownList>
            <br /><br />
            Title:<br />
            <asp:TextBox ID="TitleTextBox" runat="server" ValidationGroup="TitleTypeChangePopup" Width="100%" />
            <asp:RequiredFieldValidator ID="TitleRequiredValidator" runat="server" ControlToValidate="TitleTextBox"
             Text="*" ErrorMessage="Title is required" Display="Dynamic" ValidationGroup="TitleTypeChangePopup" />
            
            <br />
            Comments (optional):<br />
            <asp:TextBox ID="CommentsTextBox" ValidationGroup="TitleTypeChangePopup" TextMode="MultiLine"
                Rows="5" runat="server" Width="100%" />
            <br />
            <asp:Button ID="ChangeTitleTypeButton" ValidationGroup="TitleTypeChangePopup" OnClick="ChangeTitleTypeButton_Click"
                runat="server" Text="Change Title/Type" />
            <asp:Button ID="CancelChangeTitleTypeButton" CausesValidation="false" ValidationGroup="TitleTypeChangePopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
