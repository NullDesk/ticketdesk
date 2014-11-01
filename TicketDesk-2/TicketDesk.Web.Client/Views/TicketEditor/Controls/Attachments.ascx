<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<%
     
    
    if (Model.TicketAttachments.Where(ta => !ta.IsPending).Count() > 0)
    {
%>
<div id="attachmentsWrapper" style="border-bottom: 2px solid #B3CBDF;">
    <div class="activityHeadWrapper">
        <div class="activityHead">
            Attachments:
        </div>
    </div>
    <div class="" id="attachmentsList" <%if (ViewData["formatForEmail"] == null){ %>style="overflow:auto; max-height:110px;"
        <% } %>>
        <%foreach (var a in Model.TicketAttachments.Where(ta => !ta.IsPending))
          {
        %>
        <div class="activityFieldsContainer">
            <%
                if (ViewData["formatForEmail"] == null)
                {
            %>
            <%:  Html.ActionLink(Html.Encode(Html.DisplayLimitedValue(a.FileName, 38)), MVC.Attachment.Download(a.FileId), new { title= a.FileName })%>
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
            <div class="fieldSubText">
                <%: a.FileDescription%>
            </div>
            <% 
                }
            %>
        </div>
        <%
            } 
        %>
    </div>
</div>
<%
    }
%>