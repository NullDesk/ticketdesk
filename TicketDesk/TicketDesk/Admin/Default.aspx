<%@ Page Language="C#" Theme="TicketDeskTheme" MasterPageFile="~/Admin/Admin.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TicketDesk.Admin.Default"
    Title="Untitled Page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="AdminContentPlaceHolder" runat="server">
    <table style="width:100%">
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
