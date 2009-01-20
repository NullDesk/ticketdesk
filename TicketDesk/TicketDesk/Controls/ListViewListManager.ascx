<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListViewListManager.ascx.cs"
    Inherits="TicketDesk.Controls.ListViewListManager" %>
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
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />
<style>
    .SubMenuContainer td
    {
        text-align: center;
    }
    .SelectedSubMenuItem
    {
        font-weight: bold;
    }
</style>
<div>
    <asp:Repeater ID="ListViewRepeater" runat="server">
        <HeaderTemplate>
            <table class="SubMenuContainer" cellpadding="3" cellspacing="0" border="0" style="width: 100%;">
                <tbody>
                    <tr>
                        <td>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:HyperLink runat="server" CssClass='<%# (IsCurrentList((string)Eval("ListViewName")))? "SelectedSubMenuItem" : "SubMenuItem" %>'
                NavigateUrl='<%# string.Format("~/TicketCenter2.aspx?list={0}", (string)Eval("ListViewName")) %>'
                Text='<%# Eval("ListViewDisplayName") %>' /></div>
        </ItemTemplate>
        <SeparatorTemplate>
            </td><td>
        </SeparatorTemplate>
        <FooterTemplate>
            </td></tr></tbody></table></FooterTemplate>
    </asp:Repeater>
</div>
