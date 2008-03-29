<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketForm.ascx.cs"
    Inherits="TicketDesk.Controls.TicketForm" %>
<%@ Register Src="TagPicker.ascx" TagName="TagPicker" TagPrefix="uc1" %>
<%@ Register Src="EditDetails.ascx" TagName="EditDetails" TagPrefix="uc2" %>
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
<asp:ScriptManager runat="server" ID="ajaxScriptManager">
</asp:ScriptManager>
<table cellpadding="2" style="width: 950px;">
    <tbody>
        <tr>
            <td>
                Title:
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                    ErrorMessage="Please fill out a title for the new ticket." ControlToValidate="TitleTextBox">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="TitleTextBox" runat="server" Width="100%" />
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="2">
                    <tbody>
                        <tr>
                            <td>
                                Type:
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="TypeDropDownList"
                                    Display="Dynamic" ErrorMessage="Please choose a valid ticket type." Operator="NotEqual"
                                    ValueToCompare="-">*</asp:CompareValidator>
                            </td>
                            <td>
                                Category:<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="CategoryDropDownList"
                                    Display="Dynamic" ErrorMessage="Please choose a valid category." Operator="NotEqual"
                                    ValueToCompare="-">*</asp:CompareValidator>
                            </td>
                            <td>
                                Priority:
                            </td>
                            <td>
                                Customers:
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="TypeDropDownList" AppendDataBoundItems="true" runat="server">
                                    <asp:ListItem Text="-- select --" Value="-" Selected="True" />
                                   
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="CategoryDropDownList" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Text="-- select --" Value="-" Selected="True" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="PriorityDropDownList" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value="" Selected="True" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="AffectsCustomerCheckBox" Text="Has impact on customer(s)" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <uc2:EditDetails ID="EditDetailsControl" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="OwnerUpdatePanel" runat="server">
                    <ContentTemplate>
                        <table cellpadding="2" style="margin-top: 10px;">
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CreateOnBehalfTextBox" AutoPostBack="true" runat="server" Text="Create on behalf of another user"
                                            OnCheckedChanged="CreateOnBehalfTextBox_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:Panel ID="OwnerPanel" runat="server" Visible="false">
                                            <asp:DropDownList Width="200px" ID="OwnerDropDownList" runat="server">
                                                <asp:ListItem Text="-- select --" Value="-" Selected="True" />
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="OwnerDropDownList"
                                                Display="Dynamic" ErrorMessage="Please select a valid owner for this ticket."
                                                Operator="NotEqual" ValueToCompare="-">*</asp:CompareValidator>
                                            <ajaxToolkit:ListSearchExtender runat="server" TargetControlID="OwnerDropDownList">
                                            </ajaxToolkit:ListSearchExtender>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <uc1:TagPicker ID="TagPickerControl" runat="server" />
            </td>
        </tr>
    </tbody>
</table>
