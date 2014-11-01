<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<ul style="list-style-type: none; margin: 10px 0px 10px 0px; padding: 5px;">
    <li>
        <% 
            var baseStyleString = "text-decoration:none;font-weight:{0};";
            var viewName = ViewData["targetViewName"] as string;
        %>
        <%= Html.ActionLink("Admin Home", MVC.Admin.AdminHome.Index(), new { style = "text-decoration:none" })%>
    </li>
    <li>
        <% 
            var fontStyle2 = (viewName.StartsWith("ASP.areas_admin_views_applicationsettings")) ? "bold" : "normal";
            var style2 = new { style = string.Format(baseStyleString, fontStyle2) };
        %>
        <%= Html.ActionLink("Application Settings",MVC.Admin.ApplicationSettings.List(), style2)%>
    </li>
    <li>
        <% 
            var fontStyle3 = (viewName.StartsWith("ASP.areas_admin_views_securitymanagement")) ? "bold" : "normal";
            var style3 = new { style = string.Format(baseStyleString, fontStyle3) };
        %>
        <%= Html.ActionLink("Security Management",MVC.Admin.SecurityManagement.Index(), style3)%>
    </li>
    <li>
        <% 
            var fontStyle4 = (viewName.StartsWith("ASP.areas_admin_views_emailtemplate")) ? "bold" : "normal";
            var style4 = new { style = string.Format(baseStyleString, fontStyle4) };
        %>
        <%= Html.ActionLink("Email Diagnostics",MVC.Admin.EmailTemplate.Index(), style4)%>
    </li>
    <li><a href="<%= Url.Content("~/Admin/Elmah.axd") %>" style="text-decoration: none">
        View Error Logs</a> </li>
</ul>
