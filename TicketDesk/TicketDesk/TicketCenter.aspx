<%@ Page Language="C#" Theme="TicketDeskTheme" MasterPageFile="~/TicketCenter.Master"
    AutoEventWireup="true" CodeBehind="TicketCenter.aspx.cs" Inherits="TicketDesk.TicketCenter"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Controls/TicketList.ascx" TagName="TicketList" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="SubContentPlaceHolder">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
    <br />
    <table style="width: 100%;" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td id="TagListCell" visible="false" runat="server" style="vertical-align: top;">
                    <asp:Panel ID="TagsListPanel" runat="server" CssClass="TagsListPanel">
                        <div class="Block">
                            <div class="BlockHeader">
                                Categories:</div>
                            <div class="BlockBody" style="white-space: nowrap;">
                                <asp:Repeater ID="CategoryRepeater" runat="server" DataSourceID="CategoryListLinqDataSource">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="TagLink" runat="server" Text='<%# string.Format("{0} ({1})", Eval("Category"), Eval("CategoryCount")) %>'
                                            NavigateUrl='<%# GetUrlForCategory((string)Eval("Category"))  %>' Font-Bold='<%# IsSelectedCategory((string)Eval("Category"))%>' />
                                    </ItemTemplate>
                                    <SeparatorTemplate>
                                        <br />
                                    </SeparatorTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="Block">
                            <div class="BlockHeader">
                                Tags:</div>
                            <div class="BlockBody">
                                <asp:Repeater ID="TagsRepeater" runat="server" DataSourceID="TagListLinqDataSource">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="TagLink" runat="server" Text='<%# string.Format("{0} ({1})", Eval("TagName"), Eval("TagCount")) %>'
                                            NavigateUrl='<%# GetUrlForTag((string)Eval("TagName")) %>' Font-Bold='<%# IsSelectedTag((string)Eval("TagName")) %>' />
                                    </ItemTemplate>
                                    <SeparatorTemplate>
                                        <br />
                                    </SeparatorTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </asp:Panel>
                </td>
                <td style="width: 100%; vertical-align: top;">
                    <asp:Panel ID="StatusPanel" Visible="false" Style="float: right;margin-left:5px;" runat="server">
                        Status:<asp:DropDownList TabIndex="3" ID="StatusList" AutoPostBack="true" runat="server" OnSelectedIndexChanged="StatusDropDownList_SelectedIndexChanged">
                            <asp:ListItem Text="-- any --" Value="all" />
                            <asp:ListItem Text="Active" Value="active" />
                            <asp:ListItem Text="More Info" Value="moreinfo" />
                            <asp:ListItem Text="Resolved" Value="resolved" />
                            <asp:ListItem Text="Closed" Value="closed" />
                        </asp:DropDownList>
                    </asp:Panel>
                    <asp:Panel ID="StaffUserPanel" TabIndex="2" Visible="false" Style="float: right;" runat="server">
                        Assigned to:<asp:DropDownList ID="StaffUserList" AutoPostBack="true"
                            AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="StaffDropDownList_SelectedIndexChanged">
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="StaffUserListExtender" runat="server" PromptCssClass="ListSearchGrid"
                            TargetControlID="StaffUserList">
                        </ajaxToolkit:ListSearchExtender>
                    </asp:Panel>
                    <asp:Panel ID="ListPicketPanel" runat="server" Style="">
                        <uc1:TicketList ID="TicketListControl" runat="server" />
                    </asp:Panel>
                </td>
            </tr>
        </tbody>
    </table>
    <asp:LinqDataSource ID="UnassignedTicketsLinqDataSource" runat="server" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
        Select="new (TicketId, Type, Category, Title, Owner, AssignedTo, CurrentStatus, AffectsCustomer, Priority, CreatedDate, LastUpdateDate)"
        TableName="Tickets" Where="(AssignedTo == null)">
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="AssignedTicketsLinqDataSource" runat="server" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
        Select="new (TicketId, Type, Category, Title, Owner, AssignedTo, CurrentStatus, AffectsCustomer, Priority, CreatedDate, LastUpdateDate)"
        TableName="Tickets" Where="(AssignedTo != null) && (CurrentStatus != @CurrentStatus)">
        <WhereParameters>
            <asp:Parameter DefaultValue="Closed" Name="CurrentStatus" Type="String" />
        </WhereParameters>
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="StatusTicketsLinqDataSource" runat="server" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
        Select="new (TicketId, Type, Category, Title, Owner, AssignedTo, CurrentStatus, AffectsCustomer, Priority, CreatedDate, LastUpdateDate)"
        TableName="Tickets" Where="(CurrentStatus == @CurrentStatus)">
        <WhereParameters>
            <asp:Parameter DefaultValue="More Info" Name="CurrentStatus" Type="String" />
        </WhereParameters>
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="TagListTicketsLinqDataSource" runat="server" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
        Select="new (Ticket.TicketId, Ticket.Type, Ticket.Category, Ticket.Title, Ticket.Owner, Ticket.AssignedTo, Ticket.CurrentStatus, Ticket.AffectsCustomer, Ticket.Priority, Ticket.CreatedDate, Ticket.LastUpdateDate)"
        TableName="TicketTags" Where="(TagName == @TagName)">
        <WhereParameters>
            <asp:QueryStringParameter DefaultValue="" Name="TagName" QueryStringField="TagName"
                Type="String" />
        </WhereParameters>
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="CategoryTicketsLinqDataSource" runat="server" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
        Select="new (TicketId, Type, Category, Title, Owner, AssignedTo, CurrentStatus, AffectsCustomer, Priority, CreatedDate, LastUpdateDate)"
        TableName="Tickets" Where="(Category == @Category)">
        <WhereParameters>
            <asp:QueryStringParameter DefaultValue="" Name="Category" QueryStringField="Category"
                Type="String" />
        </WhereParameters>
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="TagListLinqDataSource" runat="server" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
        GroupBy="TagName" OrderGroupsBy="Count() desc, key asc" Select="new (key as TagName, Count() as TagCount)"
        TableName="TicketTags">
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="CategoryListLinqDataSource" runat="server" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
        GroupBy="Category" OrderGroupsBy="Count() desc, key asc" Select="new (key as Category, Count() as CategoryCount)"
        TableName="Tickets">
    </asp:LinqDataSource>
</asp:Content>
