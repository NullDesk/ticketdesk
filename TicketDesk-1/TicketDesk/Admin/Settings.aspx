<%@ Page Language="C#" Theme="TicketDeskTheme" MasterPageFile="~/Admin/Admin.Master"
    AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="TicketDesk.Admin.Settings"
    Title="TicketDesk Site Settings" %>

<%@ Register Src="Controls/TicketTypesEditor.ascx" TagName="TicketTypesEditor" TagPrefix="ticketDesk" %>
<%@ Register Src="Controls/TicketCategoriesEditor.ascx" TagName="TicketCategoriesEditor"
    TagPrefix="ticketDesk" %>
<%@ Register Src="Controls/TicketPrioritiesEditor.ascx" TagName="TicketPrioritiesEditor"
    TagPrefix="ticketDesk" %>
<asp:Content ID="Content2" ContentPlaceHolderID="AdminContentPlaceHolder" runat="server">
    <%
        // TicketDesk - Attribution notice
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
    &nbsp;<br />
    <asp:ScriptManagerProxy ID="AjaxScriptManagerProxy" runat="server" />
    <ajaxToolkit:Accordion ID="SettingsAccordion" runat="server" SelectedIndex="0" CssClass="Block"
        HeaderCssClass="BlockHeader" HeaderSelectedCssClass="BlockHeader" ContentCssClass="BlockBody"
        FadeTransitions="false" FramesPerSecond="40" TransitionDuration="250" AutoSize="None"
        RequireOpenedPane="true" SuppressHeaderPostbacks="true">
        <Panes>
            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header>
                    <a href="" style="text-decoration: none;">Priorities</a>
                </Header>
                <Content>
                    <ticketDesk:TicketPrioritiesEditor ID="TicketPrioritiesEditor1" runat="server" />
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                <Header>
                    <a href="" style="text-decoration: none;">Categories</a>
                </Header>
                <Content>
                    <ticketDesk:TicketCategoriesEditor ID="TicketCategoriesEditor1" runat="server" />
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane3" runat="server">
                <Header>
                    <a href="" style="text-decoration: none;">Ticket Types</a>
                </Header>
                <Content>
                    <ticketDesk:TicketTypesEditor ID="TicketTypesEditor1" runat="server" />
                </Content>
            </ajaxToolkit:AccordionPane>
        </Panes>
    </ajaxToolkit:Accordion>
</asp:Content>
