<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Attachments.ascx.cs"
    Inherits="TicketDesk.Controls.Attachments" %>
<asp:Repeater ID="AttachmentsRepeater" runat="server" DataSourceID="TicketAttachmentsDataSource"
    OnItemCommand="AttachmentsRepeater_ItemCommand">
    <ItemTemplate>
        <asp:HyperLink ID="AttachmentLink" runat="server" Text='<%# Eval("FileName") %>'
            NavigateUrl='<%# GetAttachmentLinkUrl((int)Eval("FileId")) %>' />
         -
            <asp:Label ID="AttachmentUploader" runat="server" Text='<%# TicketDesk.Engine.SecurityManager.GetUserDisplayName((string)Eval("UploadedBy")) %>' />
          :
            <asp:Label ID="AttachmentUploadDate" runat="server" Text='<%# ((DateTime)Eval("UploadedDate")).ToString("d")%>' />
        <asp:ImageButton ID="AttachmentRemoveButton" ImageUrl="~/Controls/Images/delete.gif"
            runat="server" CommandArgument='<%# Eval("FileId") %>' CommandName="delete" CausesValidation="false" />
        
    </ItemTemplate>
    <SeparatorTemplate>
        <hr />
    </SeparatorTemplate>
</asp:Repeater>
<asp:LinqDataSource runat="server" ID="TicketAttachmentsDataSource" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
    Select="new (FileId, FileName, FileSize, FileType, UploadedBy, UploadedDate)"
    TableName="TicketAttachments" Where="TicketId == @TicketId">
    <WhereParameters>
        <asp:QueryStringParameter Name="TicketId" QueryStringField="id" Type="Int32" />
    </WhereParameters>
</asp:LinqDataSource>
<asp:UpdatePanel ID="AttachmentsUpdatePanel" runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="UploadFile" />
    </Triggers>
    <ContentTemplate>
        <hr />
        <div style="width:100%;text-align:right;">
        <asp:FileUpload style="width:300px;" runat="server" ID="FileUploader" />&nbsp;<asp:Button ID="UploadFile"
            runat="server" Text="Upload" OnClick="UploadFile_Click" /></div>
    </ContentTemplate>
</asp:UpdatePanel>
