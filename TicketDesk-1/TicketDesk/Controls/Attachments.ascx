<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Attachments.ascx.cs"
    Inherits="TicketDesk.Controls.Attachments" %>
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
<div id="Div1" runat="server" class="Block" >
    <div class="BlockHeader">
        Attachments:
    </div>
    <div class="BlockBody">
        <asp:Repeater ID="AttachmentsRepeater" runat="server" DataSourceID="TicketAttachmentsDataSource">
            <ItemTemplate>
                <div class="MultiUploadFileList">
                    <div class="FileAttachmentItemContainer">
                        <asp:HyperLink ID="AttachmentLink" style="font-size:larger;"  runat="server" Text='<%# Eval("FileName") %>'
                            NavigateUrl='<%# GetAttachmentLinkUrl((int)Eval("FileId")) %>' />
                        <span class="DiminishedText">Uploaded by:
                            <asp:Label ID="AttachmentUploader" runat="server" Text='<%# TicketDesk.Engine.SecurityManager.GetUserDisplayName((string)Eval("UploadedBy")) %>' />
                            on
                            <asp:Label ID="AttachmentUploadDate" runat="server" Text='<%# ((DateTime)Eval("UploadedDate")).ToString("d")%>' />
                        </span>
                        <div class="DiminishedText" style="margin-left: 15px;">
                            <asp:Label ID="AttachmentDescription" runat="server" Text='<%# Eval("FileDescription")%>' />
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
<asp:LinqDataSource runat="server" ID="TicketAttachmentsDataSource" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
    Select="new (FileId, FileName, FileSize, FileType, FileDescription, UploadedBy, UploadedDate)"
    TableName="TicketAttachments" Where="TicketId == @TicketId" OrderBy="UploadedDate DESC">
    <WhereParameters>
        <asp:QueryStringParameter Name="TicketId" QueryStringField="id" Type="Int32" />
    </WhereParameters>
</asp:LinqDataSource>
