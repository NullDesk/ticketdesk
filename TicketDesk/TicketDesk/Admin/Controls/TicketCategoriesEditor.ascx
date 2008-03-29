<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketCategoriesEditor.ascx.cs"
    Inherits="TicketDesk.Admin.Controls.TicketCategoriesEditor" %>
<asp:ScriptManagerProxy ID="TicketCategoriesEditorScriptManagerProxy" runat="server">
</asp:ScriptManagerProxy>
<asp:UpdatePanel ID="TicketCategoriesEditorUpdatePanel" runat="server">
    <ContentTemplate>
        <table>
            <tbody>
                <tr>
                    <td>
                        <div class="reorderList" style="width: 100%;">
                            <ajaxToolkit:ReorderList DragHandleAlignment="Left" CallbackCssStyle="reorderListCallbackStyle"
                                ID="TicketCategoriesOrderList" runat="server" AllowReorder="true" LayoutType="Table"
                                OnItemReorder="TicketCategoriesOrderList_ItemReorder" ItemInsertLocation="End" ShowInsertItem="true"
                                PostBackOnReorder="true" OnInsertCommand="TicketCategoriesOrderList_InsertCommand"
                                OnUpdateCommand="TicketCategoriesOrderList_UpdateCommand">
                                <DragHandleTemplate>
                                    <div class="DragHandle">
                                        &nbsp;</div>
                                </DragHandleTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="CategoryName" runat="server" Text='<%# Container.DataItem %>' />
                                    <asp:ImageButton ID="RenameCategory" ToolTip="Edit Category" runat="server" ImageUrl="~/Controls/Images/edit.gif"
                                        CommandName="edit" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="CategoryNameTextBox" runat="server" Text='<%# Container.DataItem %>' />
                                    <asp:ImageButton ID="SaveCategory" ToolTip="Save Category" runat="server" ImageUrl="~/Controls/Images/save.gif"
                                        CommandName="update" />
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="AddCategoryNameTextBox" runat="server" />
                                    <asp:Button ID="AddCategory" runat="server" Text="Add" CommandName="insert" />
                                </InsertItemTemplate>
                            </ajaxToolkit:ReorderList>
                            <asp:Label ID="Message" runat="server" CssClass="WarningText" />
                        </div>
                    </td>
                    <td>
                        <ul>
                            <li>Drag-and-drop to change the display order of categories.
                                <br />
                                &nbsp;</li>
                            <li>To merge two categories, rename one to the same name as the other. The system will manage
                                the merge automatically and update tickets using the old category with the merged category.
                                <br />
                                &nbsp;</li>
                            <li>You cannot delete a category  as that would leave tickets without an associates category.
                                Instead, merge the category with an existing category.
                                <br />
                                &nbsp;</li>
                            <li>Renaming or merging a category will update all existing tickets. Comments will be added
                                to modified tickets, but notifications will <b>not</b> be sent.</li>
                        </ul>
                    </td>
                </tr>
            </tbody>
        </table>
        
    </ContentTemplate>
</asp:UpdatePanel>
