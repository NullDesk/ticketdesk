<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<%
    
    if (Model.TicketAttachments.Where(ta => !ta.IsPending).Count() > 0)
    {
%>
<div id="attachmentsWrapper">
    <div class="displayContainerOuter">
        <div class="displayContainerInner">
            <div class="activityHeadWrapper">
                <div class="activityHead">
                    Attachments:
                </div>
            </div>
            <div class="activityBody" id="attachmentsList">
                <%foreach (var a in Model.TicketAttachments.Where(ta => !ta.IsPending))
                  {
                %>
                <div class="activityFieldsContainer">
                    <%
                        if (ViewData["formatForEmail"] == null)
                        {
                    %>
                    <%= Html.ActionLink(Html.Encode(a.FileName), MVC.Attachment.Download(a.FileId))%>
                    <%
                        }
            else
            { 
                    %>
                    <%: a.FileName%>
                    <%
                        }
                    %>
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
        </div>
    </div>
</div>
<%
    }
%>