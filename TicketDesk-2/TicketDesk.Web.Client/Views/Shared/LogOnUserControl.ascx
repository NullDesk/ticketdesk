<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
&nbsp;
<%
    var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController;
    if (Request.IsAuthenticated)
    {
%>
Welcome <b>
    <%= ViewData["UserDisplayName"]%></b>
<%
    if (ConfigurationManager.AppSettings["SecurityMode"] != "AD")
    { %>
[
<%= Html.ActionLink("Log Off", MVC.Account.LogOff())%>
] [
<%= Html.ActionLink("My Account", MVC.Account.MyAccount())%>
]
<%  
             
%>
<%
    }
        if (controller != null && controller.Security.IsTdAdmin())
        {
%>
[
<%= Html.ActionLink("Administration", MVC.Admin.AdminHome.Index()) %>
]
<%
}
    }
    else
    {
%>
<%
    if (ConfigurationManager.AppSettings["SecurityMode"] != "AD")
    { %>
[
<%= Html.ActionLink("Log On", MVC.Account.LogOn())%>
]
<%
}
    }
    
%>
