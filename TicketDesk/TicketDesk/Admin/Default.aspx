<%@ Page Language="C#" Theme="TicketDeskTheme" MasterPageFile="~/Admin/Admin.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TicketDesk.Admin.Default"
    Title="TicketDesk Administration" %>

<asp:Content ID="Content2" ContentPlaceHolderID="AdminContentPlaceHolder" runat="server">
    <%
        // TicketDesk - Attribution notice
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
    <table style="width: 100%">
        <tbody>
            <tr>
                <td>
                    <div class="AdminItemContainer_Settings">
                        <a href="Settings.aspx">Manage Site Settings</a>
                    </div>
                </td>
                <td runat="server" id="UsersTool">
                    <div class="AdminItemContainer_Users">
                        <a href="UserRoles.aspx">Manage Users and Roles</a>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
