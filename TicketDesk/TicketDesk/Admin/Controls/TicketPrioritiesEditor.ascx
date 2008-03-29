<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketPrioritiesEditor.ascx.cs"
    Inherits="TicketDesk.Admin.Controls.TicketPrioritiesEditor" %>
<asp:ScriptManagerProxy ID="TicketPrioritiesEditorScriptManagerProxy" runat="server">
</asp:ScriptManagerProxy>
<asp:UpdatePanel ID="TicketPrioritiesEditorUpdatePanel" runat="server">
    <ContentTemplate>
        <table>
            <tbody>
                <tr>
                    <td>
                        <div class="reorderList" style="width: 100%;">
                            <ajaxToolkit:ReorderList DragHandleAlignment="Left" CallbackCssStyle="reorderListCallbackStyle"
                                ID="TicketPrioritiesOrderList" runat="server" AllowReorder="true" LayoutType="Table"
                                OnItemReorder="TicketPrioritiesOrderList_ItemReorder" ItemInsertLocation="End" ShowInsertItem="true"
                                PostBackOnReorder="true" OnInsertCommand="TicketPrioritiesOrderList_InsertCommand"
                                OnUpdateCommand="TicketPrioritiesOrderList_UpdateCommand">
                                <DragHandleTemplate>
                                    <div class="DragHandle">
                                        &nbsp;</div>
                                </DragHandleTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="PriorityName" runat="server" Text='<%# Container.DataItem %>' />
                                    <asp:ImageButton ID="RenamePriority" ToolTip="Edit Priority" runat="server" ImageUrl="~/Controls/Images/edit.gif"
                                        CommandName="edit" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="PriorityNameTextBox" runat="server" Text='<%# Container.DataItem %>' />
                                    <asp:ImageButton ID="SavePriority" ToolTip="Save Priority" runat="server" ImageUrl="~/Controls/Images/save.gif"
                                        CommandName="update" />
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="AddPriorityNameTextBox" runat="server" />
                                    <asp:Button ID="AddPriority" runat="server" Text="Add" CommandName="insert" />
                                </InsertItemTemplate>
                            </ajaxToolkit:ReorderList>
                            <asp:Label ID="Message" runat="server" CssClass="WarningText" />
                        </div>
                    </td>
                    <td>
                        <ul>
                            <li>Drag-and-drop to change the display order of Priorities.
                                <br />
                                &nbsp;</li>
                            <li>To merge two priorities, rename one to the same name as the other. The system will manage
                                the merge automatically and update tickets using the old priority with the merged priority.
                                <br />
                                &nbsp;</li>
                            <li>You cannot delete a priority as that would leave tickets without an associates priority.
                                Instead, merge the priority with an existing priority.
                                <br />
                                &nbsp;</li>
                            <li>Renaming or merging a priority will update all existing tickets. Comments will be added
                                to modified tickets, but notifications will <b>not</b> be sent.</li>
                        </ul>
                    </td>
                </tr>
            </tbody>
        </table>
        
    </ContentTemplate>
</asp:UpdatePanel>
