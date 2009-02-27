<%@ Page Language="C#" Theme="TicketDeskTheme" MasterPageFile="~/TicketDeskMain.Master"
    AutoEventWireup="true" CodeBehind="TicketSearch.aspx.cs" Inherits="TicketDesk.TicketSearch"
    Title="Ticket Search" %>

<%@ Register Src="Controls/ListViewSettingsEditor.ascx" TagName="ListViewSettingsEditor"
    TagPrefix="ticketDesk" %>
<%@ Register Src="Controls/ListView.ascx" TagName="ListView" TagPrefix="ticketDesk" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
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
    <asp:ScriptManagerProxy ID="AjaxScriptManagerProxy" runat="server" />
    <br />
    <asp:UpdatePanel ID="TicketSearchUpdatePanel" runat="server">
        <ContentTemplate>
            <div class="Block">
                <div class="BlockHeader">
                    Select your search options:</div>
                <div class="BlockBody">
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    <table cellpadding="3">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    Search For:
                                                    <asp:TextBox Style="width: 350px;" ID="SearchTerms" runat="server" ValidationGroup="SearchGroup" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="SearchTitleCheckBox" runat="server" Checked="false" Text="Search Title" />
                                                    <asp:CheckBox ID="SearchDetailsCheckBox" runat="server" Checked="false" Text="Search Details" />
                                                    <asp:CheckBox ID="SearchTagsCheckBox" runat="server" Checked="false" Text="Search Tags" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="SearchNow" runat="server" Text="Search" OnClick="SearchNow_Click" ValidationGroup="SearchGroup" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                                <td>
                                    <table cellpadding="3">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    Status:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="StatusList" runat="server">
                                                        <asp:ListItem Text="Any Status" Value="Any" />
                                                        <asp:ListItem Text="Not Closed" Value="Open" />
                                                        <asp:ListItem Text="Only Active" Value="Active" />
                                                        <asp:ListItem Text="Only More Info" Value="More Info" />
                                                        <asp:ListItem Text="Only Resolved" Value="Resolved" />
                                                        <asp:ListItem Text="Only Closed" Value="Closed" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 15px;">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    Assigned To:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="StaffUserList" Style="width: 150px;" AppendDataBoundItems="true"
                                                        runat="server">
                                                        <asp:ListItem Text="Anyone" Value="any" />
                                                    </asp:DropDownList>
                                                    <ajaxToolkit:ListSearchExtender ID="StaffListSearchExtender" runat="server" PromptCssClass="ListSearch"
                                                        TargetControlID="StaffUserList">
                                                    </ajaxToolkit:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Category:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="CategoryList" runat="server" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="Any Category" Value="any" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 15px;">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp; Owned By:
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    <asp:DropDownList ID="SubmitterUserList" runat="server" 
                                                        AppendDataBoundItems="true" Style="width: 150px;">
                                                        <asp:ListItem Text="Anyone" Value="any" />
                                                    </asp:DropDownList>
                                                    <ajaxToolkit:ListSearchExtender ID="SubmitterListSearchExtender" runat="server" 
                                                        PromptCssClass="ListSearch" TargetControlID="SubmitterUserList">
                                                    </ajaxToolkit:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Type:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="TypeList" runat="server" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="Any Type" Value="any" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 15px;">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    Priority:</td>
                                                <td>
                                                    <asp:DropDownList ID="PriorityList" runat="server" AppendDataBoundItems="true" >
                                                     <asp:ListItem Text="Any Priority" Value="any" />
                                                    </asp:DropDownList>
                                                    </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div>
                <ticketDesk:ListView Visible="false" ID="ListViewControl" ListName="search" runat="server"  />
            </div>
            <div style="clear: both;" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
