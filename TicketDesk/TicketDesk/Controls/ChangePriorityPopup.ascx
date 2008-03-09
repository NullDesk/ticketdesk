<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePriorityPopup.ascx.cs"
    Inherits="TicketDesk.Controls.ChangePriorityPopup" %>


<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:ImageButton ImageUrl="~/Controls/Images/edit.gif" CausesValidation="false" ID="ShowChangePriorityButton"
    runat="server" Text="..." />
<ajaxToolkit:ModalPopupExtender BehaviorID="changePriorityBH" ID="ChangePriorityModalPopupExtender"
    runat="server" TargetControlID="ShowChangePriorityButton" PopupControlID="ChangePriorityPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelChangePriorityButton"
    DropShadow="true" PopupDragHandleControlID="ChangePriorityPanelHandle" />
<asp:Panel ID="ChangePriorityPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="ChangePriorityPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Choose the new priority for this ticket:
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
            <br />
            <asp:RequiredFieldValidator ValidationGroup="PriorityChangePopup" ID="RequiredFieldValidator1"
                runat="server" ErrorMessage="You must choose a priority." ControlToValidate="PriorityList"
                Display="Dynamic" Text="*" /><asp:RadioButtonList ValidationGroup="PriorityChangePopup"
                    ID="PriorityList" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                </asp:RadioButtonList>
            
            <br />
            <br />
            Comments (optional):<br />
            <asp:TextBox ID="CommentsTextBox" ValidationGroup="PriorityChangePopup" TextMode="MultiLine" Rows="5" runat="server" Width="100%" />
            <br />
             <asp:Button ID="ChangePriorityButton" ValidationGroup="PriorityChangePopup" OnClick="ChangePriorityButton_Click"
                runat="server" Text="Change Priority" />
            <asp:Button ID="CancelChangePriorityButton" CausesValidation="false" ValidationGroup="PriorityChangePopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
