<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<% 
    var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController;
    var root = ViewData["siteRootUrl"] as string;
    
    var ticketUrl = root + Url.Content(string.Format("~/Ticket/{0}", Model.TicketId.ToString()));
   
%>
<%= string.Empty.PadRight(75, '-')%>
<%= Html.DisplayPaddedNameFor(m => m.TicketId, 23)%>: <%= Html.DisplayLimitedValueFor(m => m.TicketId, 50)%>
<%= Html.DisplayPaddedNameFor(m => m.CurrentStatus, 23)%>: <%= Html.DisplayLimitedValueFor(m => m.CurrentStatus, 50)%>
<%= string.Empty.PadLeft(25) + ticketUrl%>
<%= string.Empty.PadRight(75, '-')%>
<%= Html.DisplayPaddedNameFor(m => m.Title, 23)%>: <%= Html.DisplayLimitedValueFor(m => m.Title, 50)%>
<%= Html.DisplayPaddedNameFor(m => m.Priority, 23)%>: <%= Html.DisplayLimitedValueFor(m => m.Priority, 50)%>
<%= Html.DisplayPaddedNameFor(m => m.AssignedTo, 23)%>: <%= Html.DisplayLimitedValue(Model.GetAssignedToDisplayName(controller), 50)%>
<%= Html.DisplayPaddedNameFor(m => m.Owner, 23)%>: <%= Html.DisplayLimitedValue(Model.GetOwnerDisplayName(controller), 50)%>
<%= Html.DisplayPaddedNameFor(m => m.CurrentStatusSetBy, 23)%>: <%= Html.DisplayLimitedValue(Model.GetCurrentStatusByDisplayName(controller), 50)%>
<%= Html.DisplayPaddedNameFor(m => m.CurrentStatusDate, 23)%>: <%= Html.DisplayLimitedValue(Model.CurrentStatusDate.ToShortDateString() + Model.CurrentStatusDate.ToShortTimeString(), 50)%>
<%= Html.DisplayPaddedNameFor(m => m.CreatedBy, 23)%>: <%= Html.DisplayLimitedValue(Model.GetCreatedByDisplayName(controller), 50)%>
<%= Html.DisplayPaddedNameFor(m => m.CreatedDate, 23)%>: <%= Html.DisplayLimitedValue(Model.CreatedDate.ToShortDateString() + Model.CreatedDate.ToShortTimeString(), 50)%>
<%= string.Empty.PadRight(75, '-')%>
<%= Html.DisplayPaddedNameFor(m => m.Details, 23)%>: 
<%= string.Empty.PadRight(75, '-')%>
<% 
    var content = Html.WordWrapText(Html.DisplayLimitedValueFor(m => m.Details, 250).ToString(), 50);
    var contentLines = content.Split(new[]{"\n"}, StringSplitOptions.None);
    foreach (var line in contentLines)
    {
%>
<%= string.Empty.PadLeft(25) + line %>
<%
}
%>
<%= string.Empty.PadRight(75, '-')%> 
<%= Html.DisplayPaddedName("Activity History", 23)%>: 
<%= string.Empty.PadRight(75, '-')%>

<% 
    foreach(var comment in Model.TicketComments.OrderByDescending(tc => tc.CommentedDate))
    {
%>

<%= string.Empty.PadLeft(25).PadRight(75, '-')%>
<%= string.Empty.PadLeft(25) %><%= Html.DisplayLimitedValue(comment.GetCommentByDisplayName(controller) + " " + comment.CommentEvent, 50) %>
<%= string.Empty.PadLeft(25) %><%= Html.DisplayLimitedValue("     " + comment.CommentedDate.ToLongDateString() + " " + comment.CommentedDate.ToShortTimeString(), 50) %>

<% 
    var commentContent = Html.WordWrapText(Html.DisplayLimitedValue(comment.Comment, 250).ToString(), 50);
    var commentLines = commentContent.Split(new[] { "\n" }, StringSplitOptions.None);
    foreach (var line in commentLines)
    {
%>
<%= string.Empty.PadLeft(25) + line %>

<% 
    }        
%>
  
<% 
    }        
%>