<%@ Page Language="C#" Theme="TicketDeskTheme" MasterPageFile="~/TicketDeskMain.Master"
    AutoEventWireup="true" CodeBehind="TicketSearch.aspx.cs" Inherits="TicketDesk.TicketSearch"
    Title="Ticket Search" %>

<%@ Register Src="Controls/TicketList.ascx" TagName="TicketList" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
    <div class="Block">
        <div class="BlockHeader">
            Select your search options:</div>
        <div class="BlockBody">
            <table >
                <tbody>
                    <tr>
                        <td>
                            <table cellpadding="3">
                                <tbody>
                                    <tr>
                                        <td>
                                            Search For:<asp:RequiredFieldValidator runat="server" ControlToValidate="SearchTerms" Text="*" ErrorMessage="Search words must be provided." /><br />
                                            <asp:TextBox Style="width: 350px;" ID="SearchTerms" runat="server"></asp:TextBox>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="SearchTitleCheckBox" runat="server" Checked="true" Text="Search Title" />
                                            <asp:CheckBox ID="SearchDetailsCheckBox" runat="server" Checked="false" Text="Search Details" />
                                            <asp:CheckBox ID="SearchTagsCheckBox" runat="server" Checked="false" Text="Search Tags" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <asp:Button ID="SearchNow" runat="server" Text="Search" OnClick="SearchNow_Click" />
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
                                            <asp:DropDownList ID="CategoryList" runat="server">
                                                <asp:ListItem Text="Any Category" Value="any" />
                                                <asp:ListItem Text="Hardware" Value="Hardware" />
                                                <asp:ListItem Text="Software" Value="Software" />
                                                <asp:ListItem Text="Network/Services" Value="Network/Services" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 15px;">
                                            &nbsp;
                                        </td>
                                        <td>
                                           &nbsp;
                                        </td>
                                        <td>
                                          &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Type:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="TypeList" runat="server">
                                                <asp:ListItem Text="Any Type" Value="any" />
                                                <asp:ListItem Text="Question" Value="Question" />
                                                <asp:ListItem Text="Problem" Value="Problem" />
                                                <asp:ListItem Text="Request" Value="Request" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 15px;">
                                            &nbsp;
                                        </td>
                                        <td>
                                             Owned By:
                                        </td>
                                        <td>
                                              <asp:DropDownList ID="SubmitterUserList" Style="width: 150px;" AppendDataBoundItems="true"
                                                runat="server">
                                                <asp:ListItem Text="Anyone" Value="any" />
                                            </asp:DropDownList>
                                            <ajaxToolkit:ListSearchExtender ID="SubmitterListSearchExtender" runat="server" PromptCssClass="ListSearch"
                                                TargetControlID="SubmitterUserList">
                                            </ajaxToolkit:ListSearchExtender>
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
    <div runat="server" id="TicketSearchResults" visible="false">
        <uc1:TicketList ID="TicketListControl" runat="server" />
    </div>
    <asp:LinqDataSource ID="SearchTicketsLinqDataSource" runat="server" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
        Select="new (TicketId, Type, Category, Title, Owner, AssignedTo, CurrentStatus, AffectsCustomer, Priority, CreatedDate, LastUpdateDate)"
        TableName="Tickets">
    </asp:LinqDataSource>
</asp:Content>
