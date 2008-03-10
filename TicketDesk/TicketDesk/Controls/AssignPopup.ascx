<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssignPopup.ascx.cs"
    Inherits="TicketDesk.Controls.AssignPopup" %>
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
<asp:Button ID="ShowAssignButton" CausesValidation="false" runat="server" Text="Assign"
    OnClick="ShowAssignButton_Click" />
<asp:Button ID="HiddenShowAssignButton" CausesValidation="false" runat="server" Text="Assign"
    Style="display: none" />
<ajaxToolkit:ModalPopupExtender BehaviorID="assignBH" ID="AssignModalPopupExtender"
    runat="server" TargetControlID="HiddenShowAssignButton" PopupControlID="AssignPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelAssignButton" DropShadow="true"
    PopupDragHandleControlID="AssignPanelHandle" />
<asp:Panel ID="AssignPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="AssignPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Assign ticket to new user:
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
        <br />
            <table>
                <tbody>
                    <tr id="SetPriorityPanel" runat="server">
                        <td >
                            Set Priority:
                        </td>
                        <td >
                            <asp:RequiredFieldValidator ValidationGroup="AssignPopup" ID="RequiredFieldValidator1"
                                runat="server" ErrorMessage="You must choose a priority." ControlToValidate="PriorityList"
                                Display="Dynamic" Text="*" />
                            <asp:RadioButtonList ValidationGroup="AssignPopup" ID="PriorityList" runat="server"
                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Text="Low" Value="Low" Selected="True" />
                                <asp:ListItem Text="Medium" Value="Medium" />
                                <asp:ListItem Text="High" Value="High" />
                            </asp:RadioButtonList>
                            
                        </td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td style="padding-top: 10px;">
                            Assign to:
                        </td>
                        <td style="padding-top: 10px;">
                            <asp:DropDownList Width="200px" ValidationGroup="AssignPopup" ID="AssignDropDownList"
                                runat="server" AppendDataBoundItems="true">
                            </asp:DropDownList>
                            <asp:CompareValidator ValidationGroup="AssignPopup" ID="CompareValidator3" runat="server"
                                ControlToValidate="AssignDropDownList" Display="Dynamic" ErrorMessage="Please select a valid user for this ticket."
                                Operator="NotEqual" ValueToCompare="-">*</asp:CompareValidator>
                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" PromptCssClass="ListSearch"
                                TargetControlID="AssignDropDownList">
                            </ajaxToolkit:ListSearchExtender>
                        </td>
                    </tr>
                </tbody>
            </table>
            <br />
            Comments (optional):<br />
            <asp:TextBox ID="CommentsTextBox" ValidationGroup="AssignPopup" TextMode="MultiLine"
                Rows="5" runat="server" Width="100%" />
            <br />
            <asp:Button ID="AssignButton" ValidationGroup="AssignPopup" OnClick="AssignButton_Click"
                runat="server" Text="Assign" />
            <asp:Button ID="CancelAssignButton" CausesValidation="false" ValidationGroup="AssignPopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
