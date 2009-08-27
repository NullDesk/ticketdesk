<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketForm.ascx.cs"
    Inherits="TicketDesk.Controls.TicketForm" %>
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
<asp:ScriptManagerProxy runat="server" ID="AjaxScriptManagerProxy">
    <Scripts>
        <asp:ScriptReference Name="TicketDesk.MultiFile.js" Assembly="TicketDesk" ScriptMode="Release" />
     </Scripts>
</asp:ScriptManagerProxy>



<table cellpadding="2" cellspacing="0">
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
                <table cellpadding="0" cellspacing="0">
                    <tbody>
                        <tr>
                            <td style="padding-bottom: 2px;">
                                Type:
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="TypeDropDownList"
                                    Display="Dynamic" ErrorMessage="Please choose a valid ticket type." Operator="NotEqual"
                                    ValueToCompare="-">*</asp:CompareValidator>
                            </td>
                            <td style="padding-bottom: 2px;">
                                Category:
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="CategoryDropDownList"
                                    Display="Dynamic" ErrorMessage="Please choose a valid category." Operator="NotEqual"
                                    ValueToCompare="-">*</asp:CompareValidator>
                            </td>
                            <td style="padding-bottom: 2px;">
                                Priority:
                            </td>
                            <td style="padding-bottom: 2px;">
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
                Tags:
            </td>
        </tr>
        <tr>
            <td>
                <div style="width: 500px;">
                    <uc1:TagPicker ID="TagPickerControl" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td>
                Details:
                <asp:Label ID="lblDetailsRequired" runat="server" ForeColor="Red" Text="Details are required."
                    Visible="false" />
                    <textarea id="details" class="markItUpEditor" name="details"></textarea>
                     
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                Attachments:
                <input id="my_file_element" type="file" name="file_1" />
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    <!-- This is where the output will appear -->
                    <div id="files_list" class="MultiUploadFileList">
                    </div>
                </div>

                <script type="text/javascript" language="javascript">
                    var multi_selector = new MultiSelector(document.getElementById('files_list'), 3);
                    multi_selector.addElement(document.getElementById('my_file_element'));

                    
                </script>

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
                                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="OwnerDropDownList">
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
    </tbody>
</table>
