<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListViewSettingsEditor.ascx.cs"
    Inherits="TicketDesk.Controls.ListViewSettingsEditor" %>
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
<style type="text/css">

</style>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />
<div>
    <table class="ListControlTable" cellspacing="0" cellpadding="0" style="float: right;">
        <tbody>
            <tr>
                <td>
                    Items/page:<asp:DropDownList TabIndex="1" ID="PageSizeList" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="PageSizeList_SelectedIndexChanged" OnLoad="PageSizeList_OnLoad">
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    Status:<asp:DropDownList TabIndex="3" ID="StatusList" AutoPostBack="true" runat="server"
                        OnSelectedIndexChanged="StatusList_SelectedIndexChanged" OnLoad="StatusList_Load">
                        <asp:ListItem Text="-- any --" Value="any" />
                        <asp:ListItem Text="-- open --" Value="open" />
                        <asp:ListItem Text="Active" Value="active" />
                        <asp:ListItem Text="More Info" Value="moreinfo" />
                        <asp:ListItem Text="Resolved" Value="resolved" />
                        <asp:ListItem Text="Closed" Value="closed" />
                    </asp:DropDownList>
                </td>
                <td>
                    Owned:<asp:DropDownList ID="OwnedStaffUserList" AutoPostBack="true" AppendDataBoundItems="true"
                        runat="server" DataTextField="DisplayName" DataValueField="Name" OnSelectedIndexChanged="OwnedStaffUserList_SelectedIndexChanged"
                        OnLoad="OwnedStaffUserList_Load">
                        <asp:ListItem Text="-- anyone --" Value="anyone" />
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="OwnedStaffUserListListSearchExtender" runat="server"
                        PromptCssClass="ListSearchPrompt" TargetControlID="OwnedStaffUserList">
                    </ajaxToolkit:ListSearchExtender>
                </td>
                <td>
                    Assigned:<asp:DropDownList ID="AssignedStaffUserList" AutoPostBack="true" AppendDataBoundItems="true"
                        runat="server" OnSelectedIndexChanged="AssignedStaffUserList_SelectedIndexChanged"
                        DataTextField="DisplayName" DataValueField="Name" OnLoad="AssignedStaffUserList_Load">
                        <asp:ListItem Text="-- anyone --" Value="anyone" />
                        <asp:ListItem Text="-- unassigned --" Value="unassigned" />
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="AssignedStaffUserListExtender" runat="server"
                        PromptCssClass="ListSearchPrompt" TargetControlID="AssignedStaffUserList">
                    </ajaxToolkit:ListSearchExtender>
                </td>
                <td>
                    <asp:Button ID="AdvancedSettingsButton" runat="server" Text="Advanced" />
                </td>
            </tr>
        </tbody>
    </table>
    <asp:Panel ID="AdvancedSettingsPanel" runat="server" Style="clear: right;overflow: hidden;" Height="0px">
        <div class="FieldBlock">
            <div class="FieldBlockHeader" style="text-align:center">
                Advanced Settings
            </div>
            <div class="FieldBlockBody">
                <div style="padding: 5px;">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tbody>
                            <tr>
                                <td>
                                    <div class="FieldBlock">
                                        <div class="FieldBlockSubHeader" style="text-align:center">
                                            Advanced Sorting</div>
                                        <div class="FieldBlockBody">
                                            <div class="ReorderList">
                                                <ajaxToolkit:ReorderList ID="AdvancedSortOrderList" runat="server" CallbackCssStyle="ReorderListItemCallbackStyle"
                                                    DragHandleAlignment="Left" AllowReorder="true" LayoutType="Table" ItemInsertLocation="End"
                                                    ShowInsertItem="true" PostBackOnReorder="true" OnInsertCommand="AdvancedSortOrderList_InsertCommand"
                                                    OnDeleteCommand="AdvancedSortOrderList_DeleteCommand" OnItemCommand="AdvancedSortOrderList_ItemCommand"
                                                    OnItemReorder="AdvancedSortOrderList_ItemReorder" OnLoad="AdvancedSortOrderList_Load">
                                                    <DragHandleTemplate>
                                                        <div class="ReorderListItemDragHandle">
                                                            &nbsp;</div>
                                                    </DragHandleTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Sort" runat="server" Text='<%# Eval("ColumnFriendlyName") %>' />
                                                        <asp:ImageButton ImageAlign="Top" ID="ChangeSortDirection" ToolTip="Change Sort Direction"
                                                            runat="server" ImageUrl='<%# GetSortItemDirectionImage((TicketDesk.Engine.ListView.ColumnSortDirection)Eval("SortDirection")) %>'
                                                            CommandName="direction" CommandArgument='<%# Eval("ColumnName") %>' />
                                                        <asp:ImageButton ImageAlign="Top" ID="RemoveSort" ToolTip="Remove from Sort" runat="server"
                                                            ImageUrl="~/Controls/Images/delete.gif" CommandName="delete" Visible='<%#  EnableSortItemDelete()%>'
                                                            CommandArgument='<%# Eval("ColumnName") %>' />
                                                    </ItemTemplate>
                                                    <InsertItemTemplate>
                                                        <asp:Panel ID="ButtonGroupPanel" runat="server" DefaultButton="AddColumnToSort" style="padding:3px;margin-top:5px;">
                                                            <asp:Button ID="AddColumnToSort" runat="server" Text="Add" CommandName="insert" />
                                                            <asp:DropDownList ID="AddSortColumnsList" DataTextField="Key" DataValueField="Value"
                                                                runat="server" />
                                                            <asp:CheckBox ID="AddColumnToSortDescendingCheckBox" runat="server" Text="Sort Descending" />
                                                        </asp:Panel>
                                                    </InsertItemTemplate>
                                                </ajaxToolkit:ReorderList>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <asp:Button ID="CloseAdvancedSettingsButton" CausesValidation="false" runat="server"
                        Text="Done" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <ajaxToolkit:CollapsiblePanelExtender ID="AdvancedSettingsPanel_CollapsiblePanelExtender"
        runat="server" Enabled="True" TargetControlID="AdvancedSettingsPanel" CollapseControlID="CloseAdvancedSettingsButton"
        ExpandControlID="AdvancedSettingsButton" Collapsed="true" SuppressPostBack="true" />
</div>
