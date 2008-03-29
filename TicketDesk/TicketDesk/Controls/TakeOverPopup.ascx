<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TakeOverPopup.ascx.cs"
    Inherits="TicketDesk.Controls.TakeOverPopup" %>
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
<asp:Button ID="ShowTakeOverButton" CausesValidation="false" runat="server" Text="Take Over" />
<ajaxToolkit:ModalPopupExtender BehaviorID="takeOverBH" ID="TakeOverModalPopupExtender"
    runat="server" TargetControlID="ShowTakeOverButton" PopupControlID="TakeOverPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelTakeOverButton" DropShadow="true"
    PopupDragHandleControlID="TakeOverPanelHandle" />
<asp:Panel ID="TakeOverPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="TakeOverPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Do you want to take over this ticket?
            </div>
        </asp:Panel>
        <br />
        <table>
            <tbody>
                <tr id="SetPriorityPanel" runat="server">
                    <td>
                        Set Priority:
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ValidationGroup="TakeOverPopup" ID="RequiredFieldValidator1"
                            runat="server" ErrorMessage="You must choose a priority." ControlToValidate="PriorityList"
                            Display="Dynamic" Text="*" />
                        <asp:RadioButtonList ValidationGroup="TakeOverPopup" ID="PriorityList" runat="server"
                            RepeatDirection="Vertical" RepeatLayout="Flow" />
                           
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </tbody>
        </table>

        <div style="padding: 5px;">
          
            Comments (optional):<br />
            <asp:TextBox ID="CommentsTextBox" ValidationGroup="TakeOverPopup" TextMode="MultiLine"
                Rows="5" runat="server" Width="100%" />
            <br />
            <asp:Button ID="TakeOverButton" ValidationGroup="TakeOverPopup" OnClick="TakeOverButton_Click"
                runat="server" Text="Take Over" />
            <asp:Button ID="CancelTakeOverButton" CausesValidation="false" ValidationGroup="TakeOverPopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
