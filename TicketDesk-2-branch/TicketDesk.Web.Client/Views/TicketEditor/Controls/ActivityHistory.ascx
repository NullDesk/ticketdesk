<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<div class="activityHeadWrapper">
    <div class="activityHead">
        Activity & History:
    </div>
</div>
<div class="activityBody">
    <div class="historyArea">
        <% var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController;
           var root = ViewData["siteRootUrl"] as string;
           var newEmailAlertUrl = root + Url.Content("~/Content/newEmailAlert.png"); 
    
                 
        %>
        <%
                
            foreach (var c in Model.TicketComments.OrderByDescending(tc => tc.CommentedDate))
            {
                    
        %>
        <% 
            var newFlag = false;
            try
            {
                if (ViewData["firstUnsentCommentId"] != null)//only used in email templates
                {
                    var firstUnsentCommentId = Convert.ToInt32(ViewData["firstUnsentCommentId"]);
                    newFlag = c.CommentId >= firstUnsentCommentId;
                }
            }
            catch { }//just eat any exception here.

            var theHeader = "userComment";
            if (c.CommentedBy == controller.Security.CurrentUserName)
            {
                theHeader = "myComment";
            }
            else if (controller.Security.IsTdStaff(c.CommentedBy))
            {
                theHeader = "staffComment";
            }
        %>
        <div class="activityHistoryItemWrapper">
            <%--<div class="<%= theHeader%>" style="width: 150px; float: left;">
                <%: c.GetCommentByDisplayName(controller) %>
            </div>--%>
            <div>
                <div class="commentHeader <%= theHeader%>">
                    <div class="commentEvent">
                        <%
                            if (newFlag)
                            { 
                        %>
                        <img alt="New Comment" align="left" style="float: left; margin-right: 5px;" src="<%= newEmailAlertUrl %>" />
                        <%
                            } 
                        %>
                        <span class="commentDisplayName">
                            <%: c.GetCommentByDisplayName(controller)%></span>
                        <%: c.CommentEvent %>
                    </div>
                    <div class="commentDate">
                        <%: c.CommentedDate.ToLongDateString()%>
                        <%: c.CommentedDate.ToShortTimeString() %>
                    </div>
                </div>
            <%      
                if (!string.IsNullOrEmpty(c.Comment))
                {
            %>
            <div class="commentBody">
                <%= c.HtmlComment%>
            </div>
            <%
                }
            %>
        </div>
    </div>
    <div style="height: 10px;">
    </div>
    <%
        } 
    %>
</div>
</div>