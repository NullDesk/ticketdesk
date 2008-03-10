<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditDetails.ascx.cs"
    Inherits="TicketDesk.Controls.EditDetails" %>
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

Details:<asp:RequiredFieldValidator runat="server" ErrorMessage="Please include some details." Text="*" ControlToValidate="DetailsTextBox" /><br />
<asp:TextBox ID="DetailsTextBox" runat="server" TextMode="MultiLine" Rows="15" style="width:100%" />
