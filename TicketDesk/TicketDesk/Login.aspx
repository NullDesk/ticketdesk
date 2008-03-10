<%@ Page Language="C#" MasterPageFile="~/TicketDeskMain.Master" Theme="TicketDeskTheme"
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
