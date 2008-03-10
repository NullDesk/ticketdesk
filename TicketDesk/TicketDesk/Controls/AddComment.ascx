<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddComment.ascx.cs"
    Inherits="TicketDesk.Controls.AddComment" %>
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
<asp:ScriptManagerProxy runat="server">
</asp:ScriptManagerProxy>    


<asp:Panel ID="NewCommentPanel" runat="server">
    <asp:TextBox ValidationGroup="AddCommentsGroup" ID="AddCommentsTextBox" TextMode="MultiLine"
        Rows="4" runat="server" Style="width: 100%;" />
    <asp:RequiredFieldValidator ValidationGroup="AddCommentsGroup" ID="AddCommentsRequiredValidator"
        runat="server" Display="Dynamic" Text="*" ErrorMessage="Please enter a comment to add."
        ControlToValidate="AddCommentsTextBox" />
    <asp:Button ID="AddCommentButton" ValidationGroup="AddCommentsGroup" runat="server"
        Text="Add Comment" OnClick="AddCommentButton_Click" />
    <asp:CheckBox ID="ResolveCommentCheckBox" ValidationGroup="AddCommentsGroup" runat="server"
        Text="Resolves ticket" />
    <asp:CheckBox ID="ProvideInfoCommentCheckBox" ValidationGroup="AddCommentsGroup"
        runat="server" Text="Answer request for more information and re-activate ticket" />
    
</asp:Panel>