<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketTypesEditor.ascx.cs"
    Inherits="TicketDesk.Admin.Controls.TicketTypesEditor" %>
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
<asp:ScriptManagerProxy ID="TicketTypesEditorScriptManagerProxy" runat="server">
</asp:ScriptManagerProxy>
<asp:UpdatePanel ID="TicketTypesEditorUpdatePanel" runat="server">
    <ContentTemplate>
        <table>
            <tbody>
                <tr>
                    <td>
                        <div class="reorderList" style="width: 100%;">
                            <ajaxToolkit:ReorderList DragHandleAlignment="Left" CallbackCssStyle="reorderListCallbackStyle"
                                ID="TicketTypesOrderList" runat="server" AllowReorder="true" LayoutType="Table"
                                OnItemReorder="TicketTypesOrderList_ItemReorder" ItemInsertLocation="End" ShowInsertItem="true"
                                PostBackOnReorder="true" OnInsertCommand="TicketTypesOrderList_InsertCommand"
                                OnUpdateCommand="TicketTypesOrderList_UpdateCommand">
                                <DragHandleTemplate>
                                    <div class="DragHandle">
                                        &nbsp;</div>
                                </DragHandleTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="TypeName" runat="server" Text='<%# Container.DataItem %>' />
                                    <asp:ImageButton ID="RenameType" ToolTip="Edit Type" runat="server" ImageUrl="~/Controls/Images/edit.gif"
                                        CommandName="edit" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Panel ID="ButtonGroupPanel" runat="server" DefaultButton="SaveType">
                                        <asp:TextBox ID="TypeNameTextBox" runat="server" Text='<%# Container.DataItem %>' />
                                        <asp:ImageButton ID="SaveType" ToolTip="Save Type" runat="server" ImageUrl="~/Controls/Images/save.gif"
                                            CommandName="update" /></asp:Panel>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:Panel ID="ButtonGroupPanel" runat="server" DefaultButton="AddType">
                                        <asp:TextBox ID="AddTypeNameTextBox" runat="server" />
                                        <asp:Button ID="AddType" runat="server" Text="Add" CommandName="insert" /></asp:Panel>
                                </InsertItemTemplate>
                            </ajaxToolkit:ReorderList>
                            <asp:Label ID="Message" runat="server" CssClass="WarningText" />
                        </div>
                    </td>
                    <td>
                        <ul>
                            <li>Drag-and-drop to change the display order of types.
                                <br />
                                &nbsp;</li>
                            <li>To merge two types, rename one to the same name as the other. The system will manage
                                the merge automatically and update tickets using the old type with the merged type.
                                <br />
                                &nbsp;</li>
                            <li>You cannot delete a type as that would leave tickets without an associates type.
                                Instead, merge the type with an existing type.
                                <br />
                                &nbsp;</li>
                            <li>Renaming or merging a Type will update all existing tickets. Comments will be added
                                to modified tickets, but notifications will <b>not</b> be sent.</li>
                        </ul>
                    </td>
                </tr>
            </tbody>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
