<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%
    var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController;

    if (Request.IsAuthenticated && controller != null && controller.Security.IsInValidTdUserRole())
    {
        using (Html.BeginForm(MVC.TicketSearch.Index(), FormMethod.Post, new { @class = "searchArea" }))
        {
%>
Search:
<%= Html.TextBox("find",null, new { style = "min-width:30px;width:150px;" })%><input
    type="submit" value="go" class="searchButton" />
<%
        }
    }
%>