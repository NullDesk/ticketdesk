<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketEditor.ascx.cs"
    Inherits="TicketDesk.TicketViewer.TicketEditor" %>
<%@ Register Src="~/Controls/TagPicker.ascx" TagName="TagPicker" TagPrefix="ticketDesk" %>
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
<table style="width: 100%;">
    <tbody>
        <tr>
            <td style="vertical-align: top; width: 60%;">
                <div class="Block">
                    <div class="BlockHeader">
                        <asp:DropDownList ID="TicketTypeEdit" runat="server" />
                        <asp:TextBox ID="TicketTitleEdit" runat="server" Width="425px" />
                    </div>
                    <div class="BlockSubHeader">
                        Details:
                    </div>
                    <div class="BlockBody">
                        <fck:FCKeditor ID="DetailsEdit" runat="server" ToolbarSet="Basic" Height="300px" />
                    </div>
                </div>
            </td>
            <td style="vertical-align: top; width: 40%;">
                <div class="Block">
                    <div class="BlockHeader">
                        Ticket ID:
                        <asp:Label ID="TicketIdEditLabel" runat="server" />
                    </div>
                    <div class="BlockBody" style="height: 243px; white-space: nowrap;">
                        <table cellpadding="3" cellspacing="0" border="0">
                            <tbody>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Status:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:Label ID="CurrentStatusEditLabel" runat="server" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Priority:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:DropDownList ID="PriorityEdit" runat="server" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Category:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:DropDownList ID="CategoryEdit" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Owned by:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:DropDownList ID="OwnerEdit" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Assigned to:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:Label ID="AssignedToEditLabel" runat="server" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Affects Customer(s):
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:CheckBox ID="AffectsCustomerEdit" runat="server" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Tags:
                                    </td>
                                    <td style="vertical-align: top;">
                                        <ticketDesk:TagPicker ID="TagPickerEdit" runat="server" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Created by:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;" colspan="2">
                                        <asp:Label ID="CreatedByEditLabel" runat="server" />
                                        on:
                                        <asp:Label ID="CreatedDateEditLabel" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Status set by:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;" colspan="2">
                                        <asp:Label ID="CurrentStatusByEditLabel" runat="server" />
                                        on:
                                        <asp:Label ID="CurrentStatusDateEditLabel" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Updated by:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;" colspan="2">
                                        <asp:Label ID="LastUpdateByEditLabel" runat="server" />
                                        on:
                                        <asp:Label ID="LastUpdateDateEditLabel" runat="server" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
              
            </td>
        </tr>
       
    </tbody>
</table>
