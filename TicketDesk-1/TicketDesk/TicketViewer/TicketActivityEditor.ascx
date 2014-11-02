<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketActivityEditor.ascx.cs"
    Inherits="TicketDesk.TicketViewer.TicketActivityEditor" %>
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
<asp:ScriptManagerProxy ID="AjaxScriptManagerProxy" runat="server">
    <Scripts>
        <asp:ScriptReference Name="TicketDesk.MultiFile.js" Assembly="TicketDesk" ScriptMode="Release" />
    </Scripts>
</asp:ScriptManagerProxy>
<asp:Panel ID="CommentPanel" runat="server" Visible="true">
    <div class="BlockHeader">
        <asp:Label ID="ActivityLabel" runat="server" />
    </div>
    <div class="BlockSubHeader ActivityDescriptionContainer">
        <asp:Label ID="ActivityDescription" runat="server" />
    </div>
    <div class="BlockBody" style="padding: 10px;">
        <br />
        <div style="padding: 5px;">
            <asp:Label ID="CommentFieldLabel" runat="server" Text="Comments:" CssClass="FieldLabel" />
            <asp:Label ID="RequiredCommentLabel" runat="server" Text="Required" CssClass="WarningText" />
        </div>
          <textarea id="comments" class="markItUpEditor" name="comments"></textarea>
                  
    </div>
</asp:Panel>
<div>
    <asp:Panel ID="AssignPanel" runat="server" Visible="false">
        <div class="ActivityFieldContainer">
            <label class="FieldLabel">
                Priority:&nbsp;
            </label>
            <div class="ActivityControl">
                <asp:DropDownList ID="PriorityEdit" runat="server" />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="PriorityEdit"
                    Display="Dynamic" ErrorMessage="Please select a valid user for this ticket."
                    Operator="NotEqual" ValueToCompare="-" />
            </div>
        </div>
        <div class="ActivityFieldContainer">
            <label class="FieldLabel">
                Assign to:&nbsp;
            </label>
            <div class="ActivityControl">
                <asp:DropDownList ID="AssignDropDownList" runat="server" AppendDataBoundItems="true" />
                <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="AssignDropDownList"
                    Display="Dynamic" ErrorMessage="Please select a valid user for this ticket."
                    Operator="NotEqual" ValueToCompare="-" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="ActivityNotAllowedPanel" runat="server" Visible="false">
        <div class="WarningContainer">
            The requestion action cannot be performed at this time.
        </div>
        <div class="BlockSubHeader ActivityDescriptionContainer">
            <label>
                The Ticket has likely been updated since you last refreshed the page.</label>
        </div>
        <div class="BlockBody">
        </div>
    </asp:Panel>
    <asp:Panel ID="NoChangesPanel" runat="server" Visible="false">
        <div class="WarningContainer">
            No fields changed. Unable to save ticket.
        </div>
        <div class="BlockSubHeader ActivityDescriptionContainer">
            <label>
                You cannot edit a ticket without making a change.</label>
        </div>
        <div class="BlockBody">
        </div>
    </asp:Panel>
    <asp:Panel ID="AddCommentPanel" runat="server" Visible="false">
        <asp:Panel ID="ResolvedCheckBoxContainer" runat="server" Style="padding: 10px;">
            <asp:CheckBox ID="ResolveCheckBox" runat="server" Checked="false" Text="Resolve Ticket?"
                CssClass="FieldLabel" />
            <div class="DiminishedText" style="margin-left: 30px;">
                Check this to resolve the ticket now.</div>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="SupplyMoreInfoPanel" runat="server" Visible="false">
        <div style="padding: 10px;">
            <asp:CheckBox ID="SupplyInfoActivateTicketCheckBox" runat="server" Checked="true"
                CssClass="FieldLabel" Text="Reactivate Ticket?" />
            <div class="DiminishedText" style="margin-left: 30px;">
                Uncheck this to add a comment without changing the ticket's status (will remain
                in the "more info" status).</div>
        </div>
    </asp:Panel>
    <asp:Panel ID="AttachmentsPanel" runat="server" Visible="false">
        <div style="padding: 10px;">
            <label class="FieldLabel">
                Add New Attachments:
            </label>
            <div class="Block">
                <div class="ActivityFieldContainer">
                    <div class="ActivityControl">
                        &nbsp;&nbsp;<input id="my_file_element" type="file" name="file_1" /></div>
                </div>
                <!-- This is where the output will appear -->
                <div id="files_list" class="MultiUploadFileList">
                </div>
            </div>
            <br />
            <label class="FieldLabel">
                Manage Existing Attachments:
            </label>
            <div class="Block">
                <div class="ActivityFieldContainer">
                    <div class="ActivityControl">
                        <asp:Repeater ID="AttachmentsRepeater" runat="server" DataSourceID="TicketAttachmentsDataSource">
                            <HeaderTemplate>
                                <div class="MultiUploadFileList">
                            </HeaderTemplate>
                            <FooterTemplate>
                                </div>
                            </FooterTemplate>
                            <ItemTemplate>
                                <div class="FileAttachmentItemContainer">
                                    <asp:HiddenField  ID="AttachmentUpdateId" runat="server" Value='<%# Eval("FileId") %>' />
                                    <asp:Label ID="AttachmentLink" style="font-size:larger;" runat="server" Text='<%# Eval("FileName") %>' />
                                    <div class="DiminishedText">Uploaded by:
                                        <asp:Label ID="AttachmentUploader" runat="server" Text='<%# TicketDesk.Engine.SecurityManager.GetUserDisplayName((string)Eval("UploadedBy")) %>' />
                                        on
                                        <asp:Label ID="AttachmentUploadDate" runat="server" Text='<%# ((DateTime)Eval("UploadedDate")).ToString("d")%>' />
                                    </div>
                                    <div style="margin-left:25px; margin-top:5px;" >
                                    Description (optional):
                                        <asp:TextBox ID="AttachmentDescription" Width="400px" runat="server" Text='<%# Eval("FileDescription")%>' />
                                        <asp:CheckBox ID="DeleteAttachmentCheckBox" runat="server" Text="remove attachment" />
                                    
                                    </div>
                                    
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:LinqDataSource runat="server" ID="TicketAttachmentsDataSource" ContextTypeName="TicketDesk.Engine.Linq.TicketDataDataContext"
                            Select="new (FileId, FileName, FileSize, FileType, FileDescription, UploadedBy, UploadedDate)"
                            TableName="TicketAttachments" Where="TicketId == @TicketId" OrderBy="UploadedDate DESC">
                            <WhereParameters>
                                <asp:QueryStringParameter Name="TicketId" QueryStringField="id" Type="Int32" />
                            </WhereParameters>
                        </asp:LinqDataSource>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="ReopenStaffPanel" runat="server" Visible="false">
        <div style="padding: 10px;">
            <asp:CheckBox ID="ReopenAssignToMe" runat="server" Checked="true" Text="Assign to me?"
                CssClass="FieldLabel" />
            <div class="DiminishedText" style="margin-left: 30px;">
                Check this box to assign the ticket to yourself now. If unchecked the ticket will
                be re-opened as unassigned.</div>
            <asp:CheckBox ID="ReopenOwnedByMe" runat="server" Checked="false" Text="Reopen as owned by me?" />
            <div class="DiminishedText" style="margin-left: 30px;">
                Check this box to set yourself as the new owner of the ticket. If unchecked the
                ticket will be re-opened with the original owner.</div>
        </div>
    </asp:Panel>
    <div class="ActivityEditorButtonContainer" style="margin-top: 10px;">
        <asp:Button ID="CommitButton" runat="server" Text="Save" OnClick="CommitButton_Click" />
        <asp:Button ID="CancelButton" runat="server" Text="Nevermind" OnClick="CancelButton_Click"
            CausesValidation="false" />
    </div>
</div>
