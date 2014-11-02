<%@ Page Language="C#" MasterPageFile="~/TicketDeskMain.Master" Title="Login" Theme="TicketDeskTheme"
    AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TicketDesk.Login" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MainContentPlaceHolder">
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

    <table cellpadding="10" style="width:100%;">
        <tr>
            <td style="vertical-align: top;">
                <div class="Block">
                    <div class="BlockHeader">
                        Login:</div>
                    <asp:Login ID="LoginForm" runat="server"></asp:Login>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div class="Block">
                    <div class="BlockHeader">
                        Register:</div>
                    <asp:CreateUserWizard runat="server" ID="CreateUserForm" 
                        OnCreatedUser="CreateUserForm_CreatedUser" 
                        oncreatinguser="CreateUserForm_CreatingUser">
                        <WizardSteps>
                            <asp:CreateUserWizardStep runat="server">
                                <ContentTemplate>
                                    <table border="0">
                                        <tr>
                                            <td align="center" colspan="2">
                                                Register a New Account</td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User 
                                                Name:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                                    ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                                    ToolTip="User Name is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="Label1" runat="server" AssociatedControlID="DisplayName">Display 
                                                Name:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="DisplayName" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="DisplayNameRequired" runat="server" 
                                                    ControlToValidate="DisplayName" ErrorMessage="Display Name is required." 
                                                    ToolTip="Display Name is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                                    ControlToValidate="Password" ErrorMessage="Password is required." 
                                                    ToolTip="Password is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="ConfirmPasswordLabel" runat="server" 
                                                    AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" 
                                                    ControlToValidate="ConfirmPassword" 
                                                    ErrorMessage="Confirm Password is required." 
                                                    ToolTip="Confirm Password is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                                    ControlToValidate="Email" ErrorMessage="E-mail is required." 
                                                    ToolTip="E-mail is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="QuestionLabel" runat="server" AssociatedControlID="Question">Security 
                                                Question:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Question" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="QuestionRequired" runat="server" 
                                                    ControlToValidate="Question" ErrorMessage="Security question is required." 
                                                    ToolTip="Security question is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">Security 
                                                Answer:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Answer" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="AnswerRequired" runat="server" 
                                                    ControlToValidate="Answer" ErrorMessage="Security answer is required." 
                                                    ToolTip="Security answer is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:CompareValidator ID="PasswordCompare" runat="server" 
                                                    ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
                                                    Display="Dynamic" 
                                                    ErrorMessage="The Password and Confirmation Password must match." 
                                                    ValidationGroup="CreateUserForm"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="color:Red;">
                                                <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:CreateUserWizardStep>
                            <asp:CompleteWizardStep runat="server">
                            </asp:CompleteWizardStep>
                        </WizardSteps>
                    </asp:CreateUserWizard>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
