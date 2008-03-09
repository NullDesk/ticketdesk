<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeCategoryPopup.ascx.cs"
    Inherits="TicketDesk.Controls.ChangeCategoryPopup" %>


<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:ImageButton ImageUrl="~/Controls/Images/edit.gif" CausesValidation="false" ID="ShowChangeCategoryButton"
    runat="server" Text="..." />
<ajaxToolkit:ModalPopupExtender BehaviorID="changeCategoryBH" ID="ChangeCategoryModalPopupExtender"
    runat="server" TargetControlID="ShowChangeCategoryButton" PopupControlID="ChangeCategoryPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelChangeCategoryButton"
    DropShadow="true" PopupDragHandleControlID="ChangeCategoryPanelHandle" />
<asp:Panel ID="ChangeCategoryPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="ChangeCategoryPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Choose the new category for this ticket:
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
            <br />
            <asp:RequiredFieldValidator ValidationGroup="CategoryChangePopup" ID="RequiredFieldValidator1"
                runat="server" ErrorMessage="You must choose a Category." ControlToValidate="CategoryList"
                Display="Dynamic" Text="*" /><asp:RadioButtonList ValidationGroup="CategoryChangePopup"
                    ID="CategoryList" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                </asp:RadioButtonList>
            
            <br />
            <br />
            Comments (optional):<br />
            <asp:TextBox ID="CommentsTextBox" ValidationGroup="CategoryChangePopup" TextMode="MultiLine" Rows="5" runat="server" Width="100%" />
            <br />
             <asp:Button ID="ChangeCategoryButton" ValidationGroup="CategoryChangePopup" OnClick="ChangeCategoryButton_Click"
                runat="server" Text="Change" />
            <asp:Button ID="CancelChangeCategoryButton" CausesValidation="false" ValidationGroup="CategoryChangePopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
