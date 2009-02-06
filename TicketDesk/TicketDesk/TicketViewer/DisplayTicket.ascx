<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisplayTicket.ascx.cs"
    Inherits="TicketDesk.TicketViewer.DisplayTicket" %>
<%@ Register Src="TicketEditor.ascx" TagName="TicketEditor" TagPrefix="ticketDesk" %>
<%@ Register Src="TicketActivityEditor.ascx" TagName="TicketActivityEditor" TagPrefix="ticketDesk" %>
<%@ Register Src="~/Controls/Attachments.ascx" TagName="Attachments" TagPrefix="ticketDesk" %>
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
<div style="padding:5px;">
<asp:HyperLink ID="BackLink" runat="server" Text="Back" />
</div>
<asp:UpdatePanel ID="TicketUpdatePanel" runat="server">
    <ContentTemplate>
        <asp:Panel ID="PageContainer" runat="server">
            <asp:Panel ID="TicketEditPanel" runat="server" Style="height: 0px;">
                <ticketDesk:TicketEditor ID="TicketEditorControl" runat="server" />
            </asp:Panel>
            <ajaxToolkit:CollapsiblePanelExtender ID="EditTicket_CollapsiblePanelExtender" runat="server"
                Enabled="True" TargetControlID="TicketEditPanel" Collapsed="true" />
            <asp:Panel ID="TicketViewPanel" runat="server">
                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                    <tbody>
                        <tr>
                            <td style="vertical-align: top; width: 60%;">
                                <div class="Block">
                                    <div class="BlockHeader">
                                        <asp:Label ID="TicketType" runat="server" />:
                                        <asp:Label ID="TicketTitle" runat="server" />
                                    </div>
                                    <div class="BlockBody" style="height: 270px; overflow: scroll;">
                                        <asp:Label ID="Details" runat="server" />
                                    </div>
                                </div>
                            </td>
                            <td style="vertical-align: top; width: 40%;">
                                <div class="Block">
                                    <div class="BlockHeader">
                                        Ticket ID:
                                        <asp:Label ID="TicketId" runat="server"></asp:Label>
                                    </div>
                                    <div class="BlockBody" style="height: 200px; white-space: nowrap;">
                                        <table cellpadding="3" cellspacing="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                        Status:
                                                    </td>
                                                    <td style="vertical-align: top; white-space: nowrap;">
                                                        <asp:HyperLink ID="CurrentStatus" runat="server" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                        Priority:
                                                    </td>
                                                    <td style="vertical-align: top; white-space: nowrap;">
                                                        <asp:HyperLink ID="Priority" runat="server" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                        Category:
                                                    </td>
                                                    <td style="vertical-align: top; white-space: nowrap;">
                                                        <asp:HyperLink ID="Category" runat="server" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                        Owned by:
                                                    </td>
                                                    <td style="vertical-align: top; white-space: nowrap;">
                                                        <asp:HyperLink ID="Owner" runat="server" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                        Assigned to:
                                                    </td>
                                                    <td style="vertical-align: top; white-space: nowrap;">
                                                        <asp:HyperLink ID="AssignedTo" runat="server" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                        Affects Customer(s):
                                                    </td>
                                                    <td style="vertical-align: top; white-space: nowrap;">
                                                        <asp:Label ID="AffectsCustomer" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                        Tags:
                                                    </td>
                                                    <td style="vertical-align: top;">
                                                        <asp:Repeater ID="TagRepeater" runat="server">
                                                            <ItemTemplate>
                                                                <%-- NavigateUrl='<%#Eval("Url") %>'--%>
                                                                <asp:HyperLink ID="TicketTag" runat="server" Text='<%#Eval("TagName") %>' NavigateUrl='<%#Eval("Url") %>' />
                                                            </ItemTemplate>
                                                            <SeparatorTemplate>
                                                                ,
                                                            </SeparatorTemplate>
                                                        </asp:Repeater>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                        Created by:
                                                    </td>
                                                    <td colspan="2" style="vertical-align: top; white-space: nowrap;">
                                                        <asp:Label ID="CreatedBy" runat="server"></asp:Label>
                                                        on:
                                                        <asp:Label ID="CreatedDate" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                        Status set by:
                                                    </td>
                                                    <td colspan="2" style="vertical-align: top; white-space: nowrap;">
                                                        <asp:Label ID="CurrentStatusBy" runat="server"></asp:Label>
                                                        on:
                                                        <asp:Label ID="CurrentStatusDate" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                        Updated by:
                                                    </td>
                                                    <td colspan="2" style="vertical-align: top; white-space: nowrap;">
                                                        <asp:Label ID="LastUpdateBy" runat="server"></asp:Label>
                                                        on:
                                                        <asp:Label ID="LastUpdateDate" runat="server"></asp:Label>
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
                                <ticketDesk:Attachments ID="TicketAttachmentsControl" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <ajaxToolkit:CollapsiblePanelExtender ID="ViewTicket_CollapsiblePanelExtender" runat="server"
                Enabled="True" TargetControlID="TicketViewPanel" Collapsed="false" />
            <div class="Form">
                <asp:Panel ID="ActivityButtonPanel" runat="server">
                    <div class="BlockHeader ">
                        Select Activity
                    </div>
                    <div class="ActivityEditorButtonContainer" style="padding: 10px;">
                        <asp:Button ID="AddCommentButton" CommandArgument="AddComment" runat="server" Text="Add Comment"
                            OnClick="ActivityButton_Click" />
                        <asp:Button ID="SupplyMoreInfoButton" CommandArgument="SupplyInfo" runat="server"
                            Text="Provide Info" OnClick="ActivityButton_Click" />
                        <asp:Button ID="ResolveButton" CommandArgument="Resolve" runat="server" Text="Resolve"
                            OnClick="ActivityButton_Click" />
                        <asp:Button ID="RequestMoreInfoButton" CommandArgument="RequestMoreInfo" runat="server"
                            Text="Request Info" OnClick="ActivityButton_Click" />
                        <asp:Button ID="CancelMoreInfoButton" CommandArgument="CancelMoreInfo" runat="server"
                            Text="Cancel Info" OnClick="ActivityButton_Click" />
                        <asp:Button ID="CloseTicketButton" CommandArgument="CloseTicket" runat="server" Text="Close"
                            OnClick="ActivityButton_Click" />
                        <asp:Button ID="ReopenTicketButton" CommandArgument="ReopenTicket" runat="server"
                            Text="Re-Open" OnClick="ActivityButton_Click" />
                        <asp:Button ID="TakeOverButton" CommandArgument="TakeOver" runat="server" Text="Take Over"
                            OnClick="ActivityButton_Click" />
                        <asp:Button ID="AssignButton" CommandArgument="Assign" runat="server" Text="Assign"
                            OnClick="ActivityButton_Click" />
                        <asp:Button ID="GiveUpButton" CommandArgument="GiveUp" runat="server" Text="Give Up!"
                            OnClick="ActivityButton_Click" />
                        <asp:Button ID="ForceCloseButton" CommandArgument="ForceClose" runat="server" Text="Force Close"
                            OnClick="ActivityButton_Click" />
                        <asp:Button ID="AddAttachementsButton" CommandArgument="AddAttachments" runat="server"
                            Text="Attachments" OnClick="ActivityButton_Click" />
                        <asp:Button ID="EditTicketButton" CommandArgument="EditTicket" runat="server" Text="Edit Ticket"
                            OnClick="ActivityButton_Click" />
                    </div>
                </asp:Panel>
                <ajaxToolkit:CollapsiblePanelExtender ID="ActivityButtonPanel_CollapsiblePanelExtender"
                    runat="server" Enabled="True" TargetControlID="ActivityButtonPanel" Collapsed="false" />
                <asp:Panel ID="ActivityPanel" runat="server">
                    <ticketDesk:TicketActivityEditor ID="TicketActivityEditorControl" runat="server" />
                    <div style="height: 5px;">
                    </div>
                </asp:Panel>
                <ajaxToolkit:CollapsiblePanelExtender ID="ActivityPanel_CollapsiblePanelExtender"
                    runat="server" Enabled="True" TargetControlID="ActivityPanel" Collapsed="true" />
            </div>
        </asp:Panel>
        <div style="margin-top: 5px;">
            <table style="width: 100%;" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="Block">
                            <div class="BlockHeader">
                                Activity Log:
                            </div>
                            <div class="BlockBody" style="">
                                <asp:Repeater ID="CommentLogRepeater" runat="server">
                                    <HeaderTemplate>
                                        <table class="CommentBoxTable">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tbody class="CommentBox">
                                            <tr>
                                                <td rowspan="2" runat="server" class='<%# GetCommentHeadClass((string)Eval("CommentedBy")) %>'>
                                                    <asp:Label ID="HeaderDate" runat="server" Text='<%# Eval("CommentedDate", "{0:dddd, MM/dd/yyyy hh:mm tt}") %>' /><br />
                                                    <br />
                                                    <asp:Label ID="HeaderUser" runat="server" Text='<%# TicketDesk.Engine.SecurityManager.GetUserDisplayName((string)Eval("CommentedBy")) %>' />
                                                </td>
                                                <td class="CommentTitleArea">
                                                    <asp:Label ID="CommentDate" runat="server" Text='<%# Eval("CommentedDate", "{0:dddd, MM/dd/yyyy hh:mm tt}") %>' /><br />
                                                    <asp:Label ID="CommentBy" runat="server" Text='<%# TicketDesk.Engine.SecurityManager.GetUserDisplayName((string)Eval("CommentedBy")) %>' />
                                                    <asp:Label ID="CommentEvent" runat="server" Text='<%# Eval("CommentEvent") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="CommentText">
                                                    <asp:Label Visible='<%# !string.IsNullOrEmpty((string)Eval("CommentAsHtml")) %>'
                                                        ID="CommentText" runat="server" Text='<%# Eval("CommentAsHtml") %>' />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>
                                    <SeparatorTemplate>
                                        <tbody>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                        <%-- <hr class="CommentSeperator" />--%>
                                    </SeparatorTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </td>
                </tr>
                </tbody>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="TicketUpdatePanelUpdateProgress" runat="server" AssociatedUpdatePanelID="TicketUpdatePanel">
    <ProgressTemplate>
        <img id="progressImage1" src='<%= Page.ResolveClientUrl("~/App_Themes/TicketDeskTheme/indicator_waitanim_sm.gif") %>' />
    </ProgressTemplate>
</asp:UpdateProgress>
<ticketDesk:UpdateProgressOverlayExtender ID="TicketUpdatePanelUpdateProgressOverlayExtender"
    runat="server" ControlToOverlayID="PageContainer" CenterOnContainer="true" ElementToCenterID="progressImage1"
    CssClass="UpdateProgress" TargetControlID="TicketUpdatePanelUpdateProgress" />
