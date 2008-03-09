<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditDetails.ascx.cs"
    Inherits="TicketDesk.Controls.EditDetails" %>

Details:<asp:RequiredFieldValidator runat="server" ErrorMessage="Please include some details." Text="*" ControlToValidate="DetailsTextBox" /><br />
<asp:TextBox ID="DetailsTextBox" runat="server" TextMode="MultiLine" Rows="15" style="width:100%" />
