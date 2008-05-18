<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComplexSortEditor.ascx.cs"
    Inherits="TicketDesk.Controls.ComplexSortEditor" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:Panel ID="ChangeSortPanel" runat="server" Style="margin-bottom: 5px;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="ChangeSortPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Create an advanced sort:
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
            <div class="reorderList" style="width: 100%;">
                <ajaxToolkit:ReorderList DragHandleAlignment="Left" CallbackCssStyle="reorderListCallbackStyle"
                    ID="TicketSortOrderList" runat="server" AllowReorder="true" LayoutType="Table"
                    OnItemReorder="TicketSortOrderList_ItemReorder" ItemInsertLocation="End" ShowInsertItem="true"
                    PostBackOnReorder="true" OnInsertCommand="TicketSortOrderList_InsertCommand"
                    OnDeleteCommand="TicketSortOrderList_DeleteCommand" OnItemCommand="TicketSortOrderList_ItemCommand">
                    <DragHandleTemplate>
                        <div class="DragHandle">
                            &nbsp;</div>
                    </DragHandleTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Sort" runat="server" Text='<%# Container.DataItem %>' />
                        <asp:ImageButton ImageAlign="Top" ID="ChangeSortDirection" ToolTip="Change Sort Direction"
                            runat="server" ImageUrl="~/Controls/Images/swap.gif" CommandName="direction" />
                        <asp:ImageButton ImageAlign="Top" ID="RemoveSort" ToolTip="Remove from Sort" runat="server"
                            ImageUrl="~/Controls/Images/delete.gif" CommandName="delete" Visible='<%#  EnableSortDelete()%>'
                            CommandArgument='<%# Container.DataItemIndex %>' />
                    </ItemTemplate>
                    <InsertItemTemplate>
                        <asp:Panel ID="ButtonGroupPanel" runat="server" DefaultButton="AddSort">
                            <asp:DropDownList ID="ColumnsDropDownList" runat="server" OnPreRender="ColumnsDropDownList_PreRender" />
                            <asp:CheckBox ID="AddSortDescendingCheckBox" runat="server" Text="Sort Descending" />
                            <asp:Button ID="AddSort" runat="server" Text="Add" CommandName="insert" />
                        </asp:Panel>
                    </InsertItemTemplate>
                </ajaxToolkit:ReorderList>
                <asp:Button ID="CancelChangeSortButton" CausesValidation="false" ValidationGroup="SortChangePopup"
                    runat="server" Text="Done" />
            </div>
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="Panel1" Style="float: right; position: relative; vertical-align: bottom;
    margin-left: 5px;" runat="server">
    Sort:
    <asp:Label ID="DisplaySort" runat="server" />
    <asp:ImageButton ImageUrl="~/Controls/Images/editor_small.gif" CausesValidation="false"
        ID="ShowChangeSortButton" ToolTip="Advanced Sorting" runat="server" Text="..." />
    <ajaxToolkit:CollapsiblePanelExtender ID="AdvancedSortCollapsiblePanelExtender" runat="server"
        ExpandControlID="ShowChangeSortButton" CollapseControlID="CancelChangeSortButton"
        TargetControlID="ChangeSortPanel" Collapsed="true" SuppressPostBack="true">
    </ajaxToolkit:CollapsiblePanelExtender>
</asp:Panel>
