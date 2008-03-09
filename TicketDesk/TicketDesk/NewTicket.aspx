<%@ Page Language="C#" Theme="TicketDeskTheme" MasterPageFile="~/TicketDeskMain.Master"
    AutoEventWireup="true"  CodeBehind="NewTicket.aspx.cs" Inherits="TicketDesk.NewTicket" %>

<%@ Register Src="Controls/TicketForm.ascx" TagName="TicketForm" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="Block">
        <div class="BlockHeader" style="font-size: larger; font-weight:bold; text-align:center;">
            Create a new ticket
        </div>
        <uc1:TicketForm ID="NewTicketForm" runat="server" />

        <script type="text/javascript">
                // Work around browser behavior of "auto-submitting" simple forms
                var frm = document.getElementById("aspnetForm");
                if (frm) {
                    frm.onsubmit = function() { return false; };
                }
        </script>

        <input type="submit" style="display: none;" />
        <table>
            <tbody>
                <tr>
                    <td style="vertical-align: top;">
                        <asp:Button ID="CreateTicketButton" runat="server" Text="Create Ticket" UseSubmitBehavior="false"
                            OnClick="CreateTicketButton_Click" /> You may add file attachments after the ticket has been created.
                    </td>
                    </tr><tr>
                    <td style="vertical-align: top;">
                        <asp:ValidationSummary runat="server" />
                        <asp:Label ID="MessageLabel" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
