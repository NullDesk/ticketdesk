<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<ul style="list-style-type: none; margin: 10px 0px 10px 0px; padding:5px;">
    <li>
        <% 
            var baseStyleString = "text-decoration:none;font-weight:{0};";
            var viewName = ViewData["targetViewName"] as string;

           
        %>
        <%= Html.ActionLink("Admin Home",MVC.Admin.Home.Index())%>
    </li>
     <li>
       
      <a href="<%= Url.Content("~/Admin/Elmah.axd") %>">View Error Logs</a>
    </li>
    <li>
        <% 
            var fontStyle2 = (viewName == "ASP.areas_admin_views_applicationsettings_list_aspx") ? "bold" : "normal";
            var style2 = new { style = string.Format(baseStyleString, fontStyle2) };
        %>
        <%= Html.ActionLink("Application Settings",MVC.Admin.ApplicationSettings.List(), style2)%>
    </li>
     <li>
        <% 
            var fontStyle3 = (viewName == "ASP.areas_admin_views_emailtemplate_index_aspx") ? "bold" : "normal";
            var style3 = new { style = string.Format(baseStyleString, fontStyle3) };
        %>
        <%= Html.ActionLink("Email Diagnostics",MVC.Admin.EmailTemplate.Index(), style3)%>
    </li>
   
</ul>
