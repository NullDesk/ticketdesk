<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisplayTicket.ascx.cs"
    Inherits="TicketDesk.Controls.DisplayTicket" %>
<%@ Register Src="ChangePriorityPopup.ascx" TagName="ChangePriorityPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="AssignPopup.ascx" TagName="AssignPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="TakeOverPopup.ascx" TagName="TakeOverPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="RequestMoreInfoPopup.ascx" TagName="RequestMoreInfoPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="CancelMoreInfoPopup.ascx" TagName="CancelMoreInfoPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="CloseTicketPopup.ascx" TagName="CloseTicketPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="ReOpenPopup.ascx" TagName="ReOpenPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="ForceCloseTicketPopup.ascx" TagName="ForceCloseTicketPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="GiveUpPopup.ascx" TagName="GiveUpPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="ChangeOwnedByPopup.ascx" TagName="ChangeOwnedByPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="ChangeCategoryPopup.ascx" TagName="ChangeCategoryPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="ChangeTagsPopup.ascx" TagName="ChangeTagsPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="ChangeDetailsPopup.ascx" TagName="ChangeDetailsPopup" TagPrefix="ticketDesk" %>
<%@ Register Src="AddComment.ascx" TagName="AddComment" TagPrefix="ticketDesk" %>
<%@ Register Src="ResolvePopup.ascx" TagName="ResolvePopup" TagPrefix="ticketDesk" %>
<%@ Register Src="ChangeTitleTypePopup.ascx" TagName="ChangeTitleTypePopup" TagPrefix="ticketDesk" %>
<%@ Register Src="ChangeAffectsCustomerPopup.ascx" TagName="ChangeAffectsCustomerPopup"
    TagPrefix="ticketDesk" %>
<%@ Register Src="Attachments.ascx" TagName="Attachments" TagPrefix="ticketDesk" %>
<asp:ScriptManagerProxy ID="scriptProxy" runat="server">
</asp:ScriptManagerProxy>
<asp:UpdatePanel ID="ActionsUpdatePanel" UpdateMode="Always" ChildrenAsTriggers="true"
    runat="server">
    <ContentTemplate>
        <table style="width: 100%;">
            <tbody>
                <tr>
                    <td>
                        <div class="">
                            <div>
                                <ticketDesk:ResolvePopup ID="ResolvePopupControl" runat="server" />
                                <ticketDesk:CloseTicketPopup ID="CloseTicketPopupControl" runat="server" />
                                <ticketDesk:ReOpenPopup ID="ReOpenPopupControl" runat="server" />
                                <ticketDesk:TakeOverPopup ID="TakeOverPopupControl" runat="server" />
                                <ticketDesk:AssignPopup ID="AssignPopupControl" runat="server" />
                                <ticketDesk:RequestMoreInfoPopup ID="RequestMoreInfoPopupControl" runat="server" />
                                <ticketDesk:CancelMoreInfoPopup ID="CancelMoreInfoPopupControl" runat="server" />
                                <ticketDesk:GiveUpPopup ID="GiveUpPopupControl" runat="server" />
                                <ticketDesk:ForceCloseTicketPopup ID="ForceCloseTicketPopupControl" runat="server" />
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <table style="width: 100%;">
            <tbody>
                <tr>
                    <td style="vertical-align: top; width: 75%;">
                        <div class="Block">
                            <div class="BlockHeader">
                                <ticketDesk:ChangeTitleTypePopup ID="ChangeTitleTypePopupControl" runat="server" />
                                <asp:Label ID="TicketType" runat="server" />:
                                <asp:Label ID="TicketTitle" runat="server" />
                            </div>
                            <div class="BlockSubHeader">
                                <ticketDesk:ChangeDetailsPopup ID="ChangeDetailsPopup" runat="server" />
                                Details:
                            </div>
                            <div class="BlockBody" style="height: 180px; overflow: scroll;">
                                <asp:Label ID="Details" runat="server" />
                            </div>
                        </div>
                    </td>
                    <td style="vertical-align: top; width: 25%;">
                        <div class="Block">
                            <div class="BlockHeader">
                                Ticket ID:
                                <asp:Label ID="TicketId" runat="server" />
                            </div>
                            <div class="BlockBody" style="height: 203px; white-space: nowrap;">
                                <table>
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
                                                <asp:Label ID="Priority" runat="server" />
                                            </td>
                                            <td>
                                                <ticketDesk:ChangePriorityPopup ID="ChangePriorityPopupControl" runat="server" />
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
                                                <ticketDesk:ChangeCategoryPopup ID="ChangeCategoryPopupControl" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                Owned by:
                                            </td>
                                            <td style="vertical-align: top; white-space: nowrap;">
                                                <asp:Label ID="Owner" runat="server" />
                                            </td>
                                            <td>
                                                <ticketDesk:ChangeOwnedByPopup ID="ChangeOwnedByPopupControl" runat="server" />
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
                                                <asp:Label ID="AffectsCustomer" runat="server" />
                                            </td>
                                            <td>
                                                <ticketDesk:ChangeAffectsCustomerPopup ID="ChangeAffectsCustomerPopupControl" runat="server" />
                                            </td>
                                        </tr>
                                       <%-- <tr>
                                            <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                Published to KB:
                                            </td>
                                            <td style="vertical-align: top; white-space: nowrap;">
                                                <asp:Label ID="PublishedToKb" runat="server" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                Tags:
                                            </td>
                                            <td style="vertical-align: top;">
                                                <asp:Repeater ID="TagRepeater" runat="server">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="TicketTag" runat="server" NavigateUrl='<%#Eval("Url") %>' Text='<%#Eval("TagName") %>' /></ItemTemplate>
                                                    <SeparatorTemplate>
                                                        ,
                                                    </SeparatorTemplate>
                                                </asp:Repeater>
                                            </td>
                                            <td>
                                                <ticketDesk:ChangeTagsPopup ID="ChangeTagsPopupControl" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                Created by:
                                            </td>
                                            <td style="vertical-align: top; white-space: nowrap;" colspan="2">
                                                <asp:Label ID="CreatedBy" runat="server" />
                                                on:
                                                <asp:Label ID="CreatedDate" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                Status set by:
                                            </td>
                                            <td style="vertical-align: top; white-space: nowrap;" colspan="2">
                                                <asp:Label ID="CurrentStatusBy" runat="server" />
                                                on:
                                                <asp:Label ID="CurrentStatusDate" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top; white-space: nowrap; text-align: right;">
                                                Updated by:
                                            </td>
                                            <td style="vertical-align: top; white-space: nowrap;" colspan="2">
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
            </tbody>
        </table>
        <table style="width: 100%;">
            <tbody>
                <tr>
                    <td style="vertical-align: top; width: 60%;">
                        <div class="Block">
                            <div class="BlockHeader">
                                Add Comment:
                            </div>
                            <div class="BlockBody">
                                <ticketDesk:AddComment ID="AddCommentControl" runat="server" />
                            </div>
                        </div>
                    </td>
                    <td style="vertical-align: top; width: 40%;">
                        <div class="Block">
                            <div class="BlockHeader">
                                Attachments:
                            </div>
                            <div class="BlockBody">
                                <ticketDesk:Attachments ID="AttachmentsControl" runat="server" />
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
    </ContentTemplate>
</asp:UpdatePanel>
