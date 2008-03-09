<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddComment.ascx.cs"
    Inherits="TicketDesk.Controls.AddComment" %>
<asp:ScriptManagerProxy runat="server">
</asp:ScriptManagerProxy>    
<%--<asp:Panel ID="ExpandNewCommentHandlePanel" runat="server" Style="cursor: pointer;
    vertical-align: middle;">
    Activity Log:&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="ExpandNewCommentButton" Text="Add Comment"
        runat="server" />
    <asp:Image ID="Image1" ImageUrl="~/Controls/Images/expand.jpg" runat="server" />
</asp:Panel>
<asp:Panel ID="NewCommentPanel" runat="server" style="height:0px;">
    <asp:TextBox ValidationGroup="AddCommentsGroup" ID="AddCommentsTextBox" TextMode="MultiLine"
        Rows="5" runat="server" Style="width: 99%; z-index: 0;" />
    <asp:RequiredFieldValidator ValidationGroup="AddCommentsGroup" ID="AddCommentsRequiredValidator"
        runat="server" Display="Dynamic" Text="*" ErrorMessage="Please enter a comment to add."
        ControlToValidate="AddCommentsTextBox" />
    <asp:Button ID="AddCommentButton" ValidationGroup="AddCommentsGroup" runat="server"
        Text="Add Comment" OnClick="AddCommentButton_Click" />
    <asp:CheckBox ID="ResolveCommentCheckBox" ValidationGroup="AddCommentsGroup" runat="server"
        Text="Resolves ticket" />
    <asp:CheckBox ID="ProvideInfoCommentCheckBox" ValidationGroup="AddCommentsGroup"
        runat="server" Text="Answer request for more information and re-activate ticket" />
    <ajaxToolkit:CollapsiblePanelExtender SuppressPostBack="true" TargetControlID="NewCommentPanel"
        ID="AddCommentCollapsiblePanelExtender" ExpandControlID="ExpandNewCommentHandlePanel"
        Collapsed="true" CollapseControlID="ExpandNewCommentHandlePanel" runat="server"
        TextLabelID="Label1" ImageControlID="Image1" ExpandedText="Add Comment" CollapsedText="(Show Details...)"
        ExpandedImage="~/Controls/Images/collapse.jpg" CollapsedImage="~/Controls/Images/expand.jpg">
    </ajaxToolkit:CollapsiblePanelExtender>
</asp:Panel>--%>

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