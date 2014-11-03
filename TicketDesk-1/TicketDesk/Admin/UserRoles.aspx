<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master"
    Theme="TicketDeskTheme" Title="TicketDesk Users & Roles" CodeBehind="UserRoles.aspx.cs"
    Inherits="TicketDesk.UserRoles" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="AdminContentPlaceHolder">
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
    <div class="Block">
        <div class="BlockHeader">
            Manage User Roles
        </div>
        <div class="BlockBody">
            <table style="width: 100%;">
                <tbody>
                    <tr>
                        <td style="vertical-align: top; width: 50%;">
                            <div class="Block">
                                <div class="BlockHeader">
                                    Choose User to Edit:
                                </div>
                                <div class="BlockBody">
                                    <br />
                                    <asp:ListBox Style="width: 100%; font-size: larger;" ID="UserListBox" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="UserListBox_SelectedIndexChanged" />
                                    <br />
                                    <br />
                                </div>
                            </div>
                        </td>
                        <td style="width: 25px;">
                            &nbsp;
                        </td>
                        <td style="vertical-align: top; width: 50%;">
                            <asp:Panel ID="UserRolesPanel" runat="server" Visible="false">
                                <div class="Block">
                                    <div class="BlockHeader">
                                        Choose Roles for User:
                                    </div>
                                    <div class="BlockBody">
                                        <br />
                                        <asp:CheckBoxList ID="UserRolesList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="UserRolesList_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                        <br />
                                    </div>
                                    <div class="BlockBody">
                                        <asp:Button ID="DeleteUserButton" runat="server" Text="Delete User" OnClick="DeleteUserButton_Click" />
                                        <ajaxToolkit:ConfirmButtonExtender ID="DeleteUserConfirm" ConfirmText="Are you sure you would like to delete this user (this action cannot be undone)?"
                                            runat="server" TargetControlID="DeleteUserButton" />
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Label ID="MessageLabel" runat="server" Style="color: #ff0000;" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
