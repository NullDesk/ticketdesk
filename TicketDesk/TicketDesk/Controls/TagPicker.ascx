<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagPicker.ascx.cs" Inherits="TicketDesk.Controls.TagPicker" %>
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
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:UpdatePanel ID="TagsPanel" runat="server">
    <ContentTemplate>
        Tags: (separate tags with a comma)<br />
        <asp:TextBox Width="100%" autocomplete="off" ID="TagsTextBox" runat="server" />
        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteTags" ID="autoComplete1"
            TargetControlID="TagsTextBox" ServicePath="~/Services/TagsAutoComplete.asmx"
            ServiceMethod="GetTagCompletionList" MinimumPrefixLength="2" CompletionInterval="1000"
            EnableCaching="true" CompletionSetCount="10" FirstRowSelected="true" DelimiterCharacters="">
            <Animations>
                    <OnShow>
                        <Sequence>
                            <%-- Make the completion list transparent and then show it --%>
                            <OpacityAction Opacity="0" />
                            <HideAction Visible="true" />
                            
                           
                            
                            <%-- Expand from 0px to the appropriate size while fading in --%>
                            <Parallel Duration=".3">
                                <FadeIn />
                                </Parallel>
                        </Sequence>
                    </OnShow>
                    <OnHide>
                        <%-- Collapse down to 0px and fade out --%>
                        <Parallel Duration=".3">
                            <FadeOut />
                           </Parallel>
                    </OnHide>
            </Animations>
        </ajaxToolkit:AutoCompleteExtender>
    </ContentTemplate>
</asp:UpdatePanel>
