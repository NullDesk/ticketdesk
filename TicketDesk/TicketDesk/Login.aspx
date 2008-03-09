<%@ Page Language="C#" MasterPageFile="~/TicketDeskMain.Master" Theme="TicketDeskTheme"
    AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TicketDesk.Login" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MainContentPlaceHolder">
    <table cellpadding="10" style="width:100%;">
        <tr>
            <td style="vertical-align: top;">
                <div class="Block">
                    <div class="BlockHeader">
                        Login:</div>
                    <asp:Login runat="server">
                    </asp:Login>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div class="Block">
                    <div class="BlockHeader">
                        Register:</div>
                    <asp:CreateUserWizard runat="server" ID="CreateUserForm" OnCreatedUser="CreateUserForm_CreatedUser">
                        <WizardSteps>
                            <asp:CreateUserWizardStep runat="server">
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
