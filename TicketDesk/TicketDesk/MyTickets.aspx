<%@ Page Language="C#" Theme="TicketDeskTheme" MasterPageFile="~/TicketDeskMain.Master"
    AutoEventWireup="true" CodeBehind="MyTickets.aspx.cs" Inherits="TicketDesk.MyTickets" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Controls/TicketList.ascx" TagName="TicketList" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MainContentPlaceHolder">
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

    <div class="SubMenuContainer">
            <table style="width: 100%">
                <tr>
                    <td style="text-align: center; width:25%; border-right: solid 1px #666666;">
                        <asp:HyperLink ID="OpenTicketsMenuLink" runat="server" NavigateUrl="MyTickets.aspx?View=open"
                            Text="My open tickets" />
                    </td>
                    <td runat="server" id="OwnedSubMenuCell" style="text-align: center; width:25%; border-right: solid 1px #666666;">
                        <asp:HyperLink ID="OwnedTicketsMenuLink" runat="server" NavigateUrl="MyTickets.aspx?View=owned"
                            Text="Owned by me" />
                    </td>
                    <td runat="server" id="AssignedSubMenuCell" style="text-align: center; width:25%; border-right: solid 1px #666666;">
                        <asp:HyperLink ID="AssignedTicketsMenuLink" runat="server" NavigateUrl="MyTickets.aspx?View=assigned"
                            Text=" Assigned to me" />
                    </td>
                    <td style="text-align: center;width:25%;">
                        <asp:HyperLink ID="AllTicketsMenuLink" runat="server" NavigateUrl="MyTickets.aspx?View=all"
                            Text="All my tickets (including closed)" />
                    </td>
                </tr>
            </table>
        </div><div class="SubContentContainer">
        <br />
        <uc1:TicketList ID="TicketListControl" runat="server" />
        <asp:LinqDataSource ID="MyOpenTicketsLinqDataSource" runat="server" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
            Select="new (TicketId, Type, Category, Title, Owner, AssignedTo, CurrentStatus, AffectsCustomer, Priority, CreatedDate, LastUpdateDate)"
            TableName="Tickets" Where="(CurrentStatus != @CurrentStatus) && (AssignedTo == @UserName || Owner == @UserName)" >
            <WhereParameters>
                <asp:Parameter DefaultValue="Closed" Name="CurrentStatus" Type="String" />
                <asp:Parameter Name="UserName" Type="String" />
            </WhereParameters>
        </asp:LinqDataSource>
        <asp:LinqDataSource ID="MySubmittedTicketsLinqDataSource" runat="server" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
            Select="new (TicketId, Type, Category, Title, Owner, AssignedTo, CurrentStatus, AffectsCustomer, Priority, CreatedDate, LastUpdateDate)"
            TableName="Tickets" Where="CurrentStatus != @CurrentStatus && Owner == @UserName">
            <WhereParameters>
                <asp:Parameter DefaultValue="Closed" Name="CurrentStatus" Type="String" />
                <asp:Parameter Name="UserName" Type="String" />
            </WhereParameters>
        </asp:LinqDataSource>
        <asp:LinqDataSource ID="MyAssignedTicketsLinqDataSource" runat="server" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
            Select="new (TicketId, Type, Category, Title, Owner, AssignedTo, CurrentStatus, AffectsCustomer, Priority, CreatedDate, LastUpdateDate)"
            TableName="Tickets" Where="CurrentStatus != @CurrentStatus && AssignedTo == @UserName">
            <WhereParameters>
                <asp:Parameter DefaultValue="Closed" Name="CurrentStatus" Type="String" />
                <asp:Parameter Name="UserName" Type="String" />
            </WhereParameters>
        </asp:LinqDataSource>
        <asp:LinqDataSource ID="MyAllTicketsLinqDataSource" runat="server" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
            Select="new (TicketId, Type, Category, Title, Owner, AssignedTo, CurrentStatus, AffectsCustomer, Priority, CreatedDate, LastUpdateDate)"
            TableName="Tickets" Where="AssignedTo == @UserName || Owner == @UserName">
            <WhereParameters>
                <asp:Parameter Name="UserName" Type="String" />
            </WhereParameters>
        </asp:LinqDataSource>
    </div>
</asp:Content>
