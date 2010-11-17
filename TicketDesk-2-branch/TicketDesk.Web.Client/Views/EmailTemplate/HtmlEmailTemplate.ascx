<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.TicketEventNotification>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title></title>
</head>
<body>
<div>
<%= Html.LabelFor(m => m.TicketComment.Ticket.TicketId)%>: <%= Html.DisplayTextFor(m => m.TicketComment.Ticket.TicketId)%><br />
<%= Html.LabelFor(m => m.TicketComment.Ticket.Title)%>: <%= Html.DisplayTextFor(m => m.TicketComment.Ticket.Title)%><br />
<%= Html.LabelFor(m => m.TicketComment.Ticket.CurrentStatus) %>: <%= Html.DisplayTextFor(m => m.TicketComment.Ticket.CurrentStatus)%><br />


</div>
</body>
</html>