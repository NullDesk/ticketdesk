<%@ Page Language="C#" Theme="TicketDeskTheme" MasterPageFile="~/TicketDeskMain.Master"
    AutoEventWireup="true" CodeBehind="TicketCenter2.aspx.cs" Inherits="TicketDesk.TicketCenter2"
    Title="TicketCenter"  %>

<%@ Register Src="Controls/ListViewSettingsEditor.ascx" TagName="ListViewSettingsEditor"
    TagPrefix="ticketDesk" %>
<%@ Register Src="Controls/ListViewListManager.ascx" TagName="ListViewListManager"
    TagPrefix="ticketDesk" %>
<%@ Register Src="Controls/ListView.ascx" TagName="ListView" TagPrefix="ticketDesk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="AjaxScriptManagerProxy" runat="server" />
    <ticketDesk:ListViewListManager ID="ListViewListManagerControl" runat="server" />
    <ticketDesk:ListViewSettingsEditor ID="ListViewSettingsEditorControl" OnSettingsChanged="ListViewSettingsEditorControl_SettingsChanged"
        EnableFilters="false" runat="server" />
    <div style="clear: both;" />
    <div>
        <ticketDesk:ListView ID="ListViewControl" runat="server" OnSettingsChanged="ListViewControl_SettingsChanged" />
    </div>
    <div style="clear: both;" />
</asp:Content>
