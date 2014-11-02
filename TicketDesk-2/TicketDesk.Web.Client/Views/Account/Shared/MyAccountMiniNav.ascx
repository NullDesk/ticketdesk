<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>


<ul style="list-style-type: none; margin: 10px 0px 10px 0px; padding:5px;">
    <li>
        <% 
            var baseStyleString = "text-decoration:none;font-weight:{0};";
            var viewName = ViewData["targetViewName"] as string;

            var fontStyle1 = (viewName == "ASP.views_account_changepassword_aspx") ? "bold" : "normal";
            var style1 = new { style = string.Format(baseStyleString, fontStyle1) };
        %>
        <%= Html.ActionLink("Change password",MVC.Account.ChangePassword(), style1)%>
    </li>
    <li>
        <% 
            var fontStyle2 = (viewName == "ASP.views_account_changepreferences_aspx") ? "bold" : "normal";
            var style2 = new { style = string.Format(baseStyleString, fontStyle2) };
        %>
        <%= Html.ActionLink("Change Account Details",MVC.Account.ChangePreferences(), style2)%>
    </li>
</ul>
