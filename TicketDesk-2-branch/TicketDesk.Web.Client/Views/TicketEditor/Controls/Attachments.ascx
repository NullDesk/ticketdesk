<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<%
    var activity = ViewData["activity"] as string;

    if (Model.TicketAttachments.Where(ta => !ta.IsPending).Count() > 0 && activity != "ModifyAttachments")
    {
%>
<div class="activityHeadWrapper">
    <div class="activityHead" >
        Attachments:
    </div>
</div>
<div class="activityBody" id="attachmentsList">
    <%foreach (var a in Model.TicketAttachments.Where(ta => !ta.IsPending))
      {
    %>
    <div class="activityFieldsContainer" >
        
        <%= Html.ActionLink(Html.Encode(a.FileName), MVC.Attachment.Download(a.FileId)) %>
        
      
        (<%: a.FileSize.ToFileSizeString()%>)
        <%  if (!string.IsNullOrEmpty(a.FileDescription))
            {
        %>
        -
        <%: a.FileDescription%>
        <% 
            }
        %>
    </div>
    <%
        } 
    %>
</div>
<%}
    else
    {
%>
<div style="background-color: #DFF8DC; padding: 8px;">
    <div style="color: #0B294F; text-align: center;">
        No attachments
    </div>
</div>
<%
        
    }
%>
