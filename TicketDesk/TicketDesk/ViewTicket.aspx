<%@ Page Language="C#" Theme="TicketDeskTheme" MasterPageFile="~/TicketDeskMain.Master"
    AutoEventWireup="true" CodeBehind="ViewTicket.aspx.cs" Inherits="TicketDesk.ViewTicket" %>

<%@ Register Src="TicketViewer/DisplayTicket.ascx" TagName="DisplayTicket" TagPrefix="ticketDesk" %>
<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
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


    <link rel="Stylesheet" type="text/css" media="all" href="<%= Page.ResolveUrl(@"~/Scripts/markitup/markitup/skins/markdown/style.css") %>" />
    <link rel="Stylesheet" type="text/css" media="all" href="<%= Page.ResolveUrl(@"~/Scripts/markitup/markitup/sets/markdown/style.css") %>" />
    
    <script type="text/javascript" src="<%= Page.ResolveUrl(@"~/Scripts/jquery-1.3.2.js") %>"></script>

    <script type="text/javascript" src="<%= Page.ResolveUrl(@"~/Scripts/markitup/markitup/jquery.markitup.js") %>"></script>

    <script type="text/javascript" src="<%= Page.ResolveUrl(@"~/Scripts/markitup/markitup/sets/markdown/set.js") %>"></script>

    <script type="text/javascript">

        function pageLoad(sender, args) {
            mySettings.previewParserPath = '<%= Page.ResolveUrl(@"~/Services/MarkdownPreview.ashx")%>';

            $("#details").markItUp(mySettings);
            $("#comments").markItUp(mySettings);
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy id="AjaxScriptManagerProxy" runat="server" />
    <ticketDesk:DisplayTicket ID="DisplayTicketView" EnableEditControls="true" runat="server" />
   
</asp:Content>
