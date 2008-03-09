<%@ Page Language="C#" Theme="TicketDeskTheme" MasterPageFile="~/TicketDeskMain.Master"
    AutoEventWireup="true" CodeBehind="ViewTicket.aspx.cs" Inherits="TicketDesk.ViewTicket"
    Title="Untitled Page" %>

<%@ Register Src="Controls/DisplayTicket.ascx" TagName="DisplayTicket" TagPrefix="ticketDesk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <ticketDesk:DisplayTicket ID="DisplayTicketView" EnableEditControls="true" runat="server" />
   
</asp:Content>
