<%@ Page Language="C#" MasterPageFile="~/TicketDeskMain.Master" AutoEventWireup="true"
    CodeBehind="MyAccount.aspx.cs" Inherits="TicketDesk.MyAccount" Title="TicketDesk - My Account"
    Theme="TicketDeskTheme" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
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
    <asp:ScriptManagerProxy id="AjaxScriptManagerProxy" runat="server" />
    <asp:UpdatePanel ID="UpdatePanelMyAccount" runat="server">
        <ContentTemplate>
            <ajaxToolkit:Accordion ID="UserAccountAccordion" runat="server" SelectedIndex="0"
                CssClass="Block" HeaderCssClass="BlockHeader" HeaderSelectedCssClass="BlockHeader"
                ContentCssClass="BlockBody" FadeTransitions="false" FramesPerSecond="40" TransitionDuration="250"
                AutoSize="None" SuppressHeaderPostbacks="true" RequireOpenedPane="true">
                <Panes>
                    <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                        <Header>
                            <a href="" style="text-decoration: none;">Change My Account Details</a>
                        </Header>
                        <Content>
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="SaveDetails">
                                <table border="0">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label1" runat="server" AssociatedControlID="DisplayName">Display 
                                                Name:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="DisplayName" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="DisplayNameRequired" runat="server" ControlToValidate="DisplayName"
                                                ErrorMessage="Display Name is required." ToolTip="Display Name is required."
                                                ValidationGroup="UserDetails">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                                                ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="UserDetails">*</asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="UserDetails"
                                                ControlToValidate="Email" ErrorMessage="Must be a valid E-Mail address" ToolTip="Must be a valid E-Mail address"
                                                ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$">*</asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="SaveDetails" runat="server" Text="Save" OnClick="SaveDetails_Click"
                                                ValidationGroup="UserDetails" />
                                        </td>
                                        <td>
                                            <asp:Label CssClass="WarningText" ID="DetailsMessage" runat="server" /><br />
                                            <asp:ValidationSummary ValidationGroup="UserDetails" ID="UserDetailsValidationSummary"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                    <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                        <Header>
                            <a href="" style="text-decoration: none;">Change My Password</a>
                        </Header>
                        <Content>
                            <asp:Panel ID="PasswordPane" runat="server" DefaultButton="SavePassword">
                                <table border="0">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="OldPasswordLabel" runat="server" AssociatedControlID="OldPassword">Old Password:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="OldPassword" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="OldPassword"
                                                ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ChangePassword">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">New Password:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                ErrorMessage="New Password is required." ToolTip="New Password is required."
                                                ValidationGroup="ChangePassword">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm New Password:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                                                ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
                                                ValidationGroup="ChangePassword">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="SavePassword" runat="server" Text="Save" OnClick="SavePassword_Click"
                                                ValidationGroup="ChangePassword" />
                                        </td>
                                        <td>
                                            <asp:Label CssClass="WarningText" ID="PasswordMessage" runat="server" /><br />
                                            <asp:ValidationSummary ValidationGroup="ChangePassword" ID="ValidationSummary1" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                </Panes>
            </ajaxToolkit:Accordion>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
