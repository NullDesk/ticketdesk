<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailTicketView.ascx.cs"
    Inherits="TicketDesk.Controls.EmailTicketView" %>
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

<style>
    body
    {
        font-family: Verdana, Helvetica, Arial, Sans-Serif;
        font-size: 9pt;
    }
    td
    {
        font-size: 9pt;
    }
    .Block
    {
        border: solid 1px #A0A0A0;
        margin: 3px;
    }
    .BlockHeader
    {
        background-color: #CDF2B3;
        padding: 3px;
        font-size: 10pt;
    }
    .BlockSubHeader
    {
        border-top: solid 1px #A0A0A0;
        background-color: #FFFFDD;
        padding: 3px;
        font-size: 10pt;
    }
    .BlockBody
    {
        padding: 3px;
        border-top: solid 1px #A0A0A0;
    }
    .CommentBox
    {
    }
    .CommentHead
    {
        padding: 3px;
        color: #416523;
    }
    .CommentSeperator
    {
        color: #CDCDCD;
        height: 1px;
    }
    .CommentText
    {
        display: block;
        padding-left: 15px;
    }
</style>
<table style="width: 100%;">
    <tbody>
        <tr>
            <td style="vertical-align: top; width: 75%;">
                <div class="Block">
                    <div class="BlockHeader">
                        <asp:HyperLink ID="TicketType" runat="server" />:
                        <asp:HyperLink ID="TicketTitle" runat="server" />
                    </div>
                    <div class="BlockBody" style="height: 180px;">
                        <asp:Label ID="Details" runat="server" />
                    </div>
                </div>
            </td>
            <td style="vertical-align: top; width: 25%;">
                <div class="Block">
                    <div class="BlockHeader">
                        Ticket ID:
                        <asp:HyperLink ID="TicketId" runat="server" />
                    </div>
                    <div class="BlockBody" style="height: 203px; white-space: nowrap;">
                        <table>
                            <tbody>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Status:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:Label ID="CurrentStatus" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Priority:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:Label ID="Priority" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Category:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:Label ID="Category" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Owned by:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:Label ID="Owner" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Assigned to:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:HyperLink ID="AssignedTo" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Affects Customer(s):
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:Label ID="AffectsCustomer" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Published to KB:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:Label ID="PublishedToKb" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Tags:
                                    </td>
                                    <td style="vertical-align: top;">
                                        <asp:Repeater ID="TagRepeater" runat="server">
                                            <ItemTemplate>
                                                <asp:Label ID="TicketTag" runat="server" Text='<%#Eval("TagName") %>' /></ItemTemplate>
                                            <SeparatorTemplate>
                                                ,
                                            </SeparatorTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Created by:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:Label ID="CreatedBy" runat="server" />
                                        on:
                                        <asp:Label ID="CreatedDate" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Status set by:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:Label ID="CurrentStatusBy" runat="server" />
                                        on:
                                        <asp:Label ID="CurrentStatusDate" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                        Updated by:
                                    </td>
                                    <td style="vertical-align: top; white-space: nowrap;">
                                        <asp:Label ID="LastUpdateBy" runat="server" />
                                        on:
                                        <asp:Label ID="LastUpdateDate" runat="server" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="Block">
                    <div class="BlockHeader">
                        Attachments:
                    </div>
                    <div class="BlockBody" style="">
                        <asp:Repeater ID="AttachmentsRepeater" runat="server" >
                            <ItemTemplate>
                                <asp:Label ID="AttachmentLink" runat="server" Text='<%# Eval("FileName") %>' />
                                -
                                <asp:Label ID="AttachmentUploader" runat="server" Text='<%# TicketDesk.Engine.SecurityManager.GetUserDisplayName((string)Eval("UploadedBy")) %>' />
                                :
                                <asp:Label ID="AttachmentUploadDate" runat="server" Text='<%# ((DateTime)Eval("UploadedDate")).ToString("d")%>' />
                            </ItemTemplate>
                            <SeparatorTemplate>
                                <hr />
                            </SeparatorTemplate>
                        </asp:Repeater>
                        <asp:LinqDataSource runat="server" ID="TicketAttachmentsDataSource" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
                            Select="new (FileId, FileName, FileSize, FileType, UploadedBy, UploadedDate)"
                            TableName="TicketAttachments" Where="TicketId == @TicketId">
                            <WhereParameters>
                                <asp:Parameter Name="TicketId" Type="Int32" />
                            </WhereParameters>
                        </asp:LinqDataSource>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="Block">
                    <div class="BlockHeader">
                        Activity Log:
                    </div>
                    <div class="BlockBody" style="">
                        <asp:Repeater ID="CommentLogRepeater" runat="server">
                            <ItemTemplate>
                                <div class="CommentBox">
                                    <div class="CommentHead">
                                        <asp:Label ID="CommentDate" runat="server" Text='<%# Eval("CommentedDate", "{0:dddd, MM/dd/yyyy HH:mm tt}") %>' /><br />
                                        <asp:Label ID="CommentBy" runat="server" Text='<%# TicketDesk.Engine.SecurityManager.GetUserDisplayName((string)Eval("CommentedBy")) %>' />
                                        <asp:Label ID="CommentEvent" runat="server" Text='<%# Eval("CommentEvent") %>' /></div>
                                    <asp:Label CssClass="CommentText" Visible='<%# !string.IsNullOrEmpty((string)Eval("CommentAsHtml")) %>'
                                        ID="CommentText" runat="server" Text='<%# Eval("CommentAsHtml") %>' />
                                </div>
                            </ItemTemplate>
                            <SeparatorTemplate>
                                <hr class="CommentSeperator" />
                            </SeparatorTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </td>
        </tr>
    </tbody>
</table>
