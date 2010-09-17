<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>&nbsp;
<%
    if (Request.IsAuthenticated)
    {
%>
        Welcome <b><%= ViewData["UserDisplayName"]%></b>
        <%if (ConfigurationManager.AppSettings["SecurityMode"] != "AD")
          { %>


        [ <%= Html.ActionLink("Log Off", MVC.Account.LogOff())%> ]
        [ <%= Html.ActionLink("My Account", MVC.Account.MyAccount())%> ]
<%
          }
    } else
          {
%> 
<%if (ConfigurationManager.AppSettings["SecurityMode"] != "AD")
  { %>
        [ <%= Html.ActionLink("Log On", MVC.Account.LogOn())%> ]
<%
    }
          }
    
%>

