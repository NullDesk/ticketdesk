<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.TicketEventNotification>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<%= Html.DisplayNameFor(m => m.TicketId)%>: <%= Model.TicketComment.Ticket.TicketId%>

<%= Html.DisplayNameFor(m => m.TicketComment.Ticket.Title)%>: <%= Model.TicketComment.Ticket.Title%>

<%= Html.DisplayNameFor(m => m.TicketComment.Ticket.CurrentStatus)%>: <%= Model.TicketComment.Ticket.CurrentStatus%>




