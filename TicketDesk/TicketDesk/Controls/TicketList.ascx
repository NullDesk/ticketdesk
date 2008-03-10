<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketList.ascx.cs"
    Inherits="TicketDesk.Controls.TicketList" %>
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

<div style="position: relative; float: right; margin-right:5px;">
    Items/page:<asp:DropDownList TabIndex="1" ID="PageSizeDropDownList" runat="server" AutoPostBack="true"
        OnLoad="PageSizeDropDownList_Load" OnSelectedIndexChanged="PageSizeDropDownList_SelectedIndexChanged">
        <asp:ListItem Text="10" Value="10"></asp:ListItem>
        <asp:ListItem Text="20" Value="20"></asp:ListItem>
        <asp:ListItem Text="30" Value="30"></asp:ListItem>
        <asp:ListItem Text="50" Value="50"></asp:ListItem>
        <asp:ListItem Text="100" Value="100"></asp:ListItem>
    </asp:DropDownList>
</div>
<div style="clear: both;">
    <asp:ListView ID="TicketListView" runat="server" OnSorted="TicketListView_Sorted">
        <AlternatingItemTemplate>
            <tr class="TicketListAltRow">
                <td>
                    <asp:Label ID="TicketIdLabel" runat="server" Text='<%# Eval("TicketId") %>' />
                </td>
                <td>
                    <asp:Label ID="TypeLabel" runat="server" Text='<%# Eval("Type") %>' />
                </td>
                <td style="white-space: normal;">
                    <asp:HyperLink ID="TitleLabel" runat="server" NavigateUrl='<%# string.Format("~/ViewTicket.aspx?id={0}", Eval("TicketId")) %>'
                        Text='<%# Eval("Title") %>' />
                </td>
                <td>
                    <asp:Label ID="OwnerLabel" runat="server" Text='<%# TicketDesk.Engine.SecurityManager.GetUserDisplayName((string)Eval("Owner")) %>' />
                </td>
                <td>
                    <asp:Label ID="AssignedToLabel" runat="server" Text='<%# TicketDesk.Engine.SecurityManager.GetUserDisplayName((string)Eval("AssignedTo")) %>' />
                </td>
                <td>
                    <asp:Label ID="CurrentStatusLabel" runat="server" Text='<%# Eval("CurrentStatus") %>' />
                </td>
                <td>
                    <asp:Label ID="CategoryLabel" runat="server" Text='<%# Eval("Category") %>' />
                </td>
                <td>
                    <asp:Label ID="PriorityLabel" runat="server" Text='<%# Eval("Priority") %>' />
                </td>
                <td>
                    <asp:Label ID="CreatedDateLabel" runat="server" Text='<%# ((DateTime)Eval("CreatedDate")).ToString("d") %>' />
                </td>
                <td>
                    <asp:Label ID="LastUpdateDateLabel" runat="server" Text='<%# ((DateTime)Eval("LastUpdateDate")).ToString("d") %>' />
                </td>
            </tr>
        </AlternatingItemTemplate>
        <LayoutTemplate>
            <table runat="server" style="width: 100%; margin-top: 0px;" cellpadding="0" cellspacing="0">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="3" class="TicketListTable">
                            <tr runat="server" class="TicketListHeaderRow">
                                <th runat="server">
                                    <asp:LinkButton runat="server" CssClass="SortLink" ID="TicketIdSortLink" CommandName="Sort"
                                        CommandArgument="TicketId" Text="ID" OnLoad="SortLinkLoading" /><asp:Label ID="SortDirectionTicketId"
                                            runat="server" CssClass="SortDirectionIndicator" />
                                </th>
                                <th runat="server">
                                    <asp:LinkButton runat="server" CssClass="SortLink" ID="TypeSortLink" CommandName="Sort"
                                        CommandArgument="Type" Text="Type" OnLoad="SortLinkLoading" /><asp:Label ID="SortDirectionType"
                                            runat="server" CssClass="SortDirectionIndicator" />
                                </th>
                                <th runat="server" style="width: 100%; white-space: normal;">
                                    <asp:LinkButton runat="server" CssClass="SortLink" ID="TitleSortLink" CommandName="Sort"
                                        CommandArgument="Title" Text="Title" OnLoad="SortLinkLoading" /><asp:Label ID="SortDirectionTitle"
                                            runat="server" CssClass="SortDirectionIndicator" />
                                </th>
                                <th runat="server">
                                    <asp:LinkButton runat="server" CssClass="SortLink" ID="OwnerSortLink" CommandName="Sort"
                                        CommandArgument="Owner" Text="Owner" OnLoad="SortLinkLoading" /><asp:Label ID="SortDirectionOwner"
                                            runat="server" CssClass="SortDirectionIndicator" />
                                </th>
                                <th runat="server">
                                    <asp:LinkButton runat="server" CssClass="SortLink" ID="AssignedToSortLink" CommandName="Sort"
                                        CommandArgument="AssignedTo" Text="Assigned" OnLoad="SortLinkLoading" /><asp:Label
                                            ID="SortDirectionAssignedTo" runat="server" CssClass="SortDirectionIndicator" />
                                </th>
                                <th runat="server">
                                    <asp:LinkButton runat="server" CssClass="SortLink" ID="CurrentStatusSortLink" CommandName="Sort"
                                        CommandArgument="CurrentStatus" OnLoad="SortLinkLoading" Text="Status" /><asp:Label
                                            ID="SortDirectionCurrentStatus" runat="server" CssClass="SortDirectionIndicator" />
                                </th>
                                <th runat="server">
                                    <asp:LinkButton runat="server" CssClass="SortLink" ID="CategorySortLink" CommandName="Sort"
                                        CommandArgument="Category" Text="Category" OnLoad="SortLinkLoading" /><asp:Label
                                            ID="SortDirectionCategory" runat="server" CssClass="SortDirectionIndicator" />
                                </th>
                                <th runat="server">
                                    <asp:LinkButton runat="server" CssClass="SortLink" ID="PrioritySortLink" CommandName="Sort"
                                        CommandArgument="Priority" Text="Priority" OnLoad="SortLinkLoading" /><asp:Label
                                            ID="SortDirectionPriority" runat="server" CssClass="SortDirectionIndicator" />
                                </th>
                                <th runat="server">
                                    <asp:LinkButton runat="server" CssClass="SortLink" ID="CreatedDateSortLink" CommandName="Sort"
                                        CommandArgument="CreatedDate" Text="Created" OnLoad="SortLinkLoading" /><asp:Label
                                            ID="SortDirectionCreatedDate" runat="server" CssClass="SortDirectionIndicator" />
                                </th>
                                <th runat="server">
                                    <asp:LinkButton runat="server" CssClass="SortLink" ID="LastUpdateDateSortLink" CommandName="Sort"
                                        CommandArgument="LastUpdateDate" Text="Updated" OnLoad="SortLinkLoading" /><asp:Label
                                            ID="SortDirectionLastUpdateDate" runat="server" CssClass="SortDirectionIndicator" />
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                            <tr class="TicketListPagerRow">
                                <td colspan="10">
                                    <asp:DataPager ID="TicketListDataPager" runat="server" OnInit="TicketListDataPager_Load">
                                        <Fields>
                                            <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="True" ShowNextPageButton="False"
                                                ShowPreviousPageButton="True" />
                                            <asp:NumericPagerField />
                                            <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="True" ShowNextPageButton="True"
                                                ShowPreviousPageButton="False" />
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="width: 100%; background-color: #FFFFFF; margin-top: 20px;
                margin-bottom: 10px; border-collapse: collapse; border-color: #999999; border-style: solid;
                border-width: 1px;">
                <tr>
                    <td style="width: 100%; height: 75px; padding: 5px;">
                        No data was returned.
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <ItemTemplate>
            <tr class="TicketListRow">
                <td>
                    <asp:Label ID="TicketIdLabel" runat="server" Text='<%# Eval("TicketId") %>' />
                </td>
                <td>
                    <asp:Label ID="TypeLabel" runat="server" Text='<%# Eval("Type") %>' />
                </td>
                <td style="white-space: normal;">
                    <asp:HyperLink ID="TitleLabel" runat="server" NavigateUrl='<%# string.Format("~/ViewTicket.aspx?id={0}", Eval("TicketId")) %>'
                        Text='<%# Eval("Title") %>' />
                </td>
                <td>
                    <asp:Label ID="OwnerLabel" runat="server" Text='<%# Eval("Owner") %>' />
                </td>
                <td>
                    <asp:Label ID="AssignedToLabel" runat="server" Text='<%# Eval("AssignedTo") %>' />
                </td>
                <td>
                    <asp:Label ID="CurrentStatusLabel" runat="server" Text='<%# Eval("CurrentStatus") %>' />
                </td>
                <td>
                    <asp:Label ID="CategoryLabel" runat="server" Text='<%# Eval("Category") %>' />
                </td>
                <td>
                    <asp:Label ID="PriorityLabel" runat="server" Text='<%# Eval("Priority") %>' />
                </td>
                <td>
                    <asp:Label ID="CreatedDateLabel" runat="server" Text='<%# ((DateTime)Eval("CreatedDate")).ToString("d") %>' />
                </td>
                <td>
                    <asp:Label ID="LastUpdateDateLabel" runat="server" Text='<%# ((DateTime)Eval("LastUpdateDate")).ToString("d") %>' />
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
