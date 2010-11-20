<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<div class="activityHeadWrapper">
    <div class="activityHead">
        Activity & History:
    </div>
</div>
<div class="activityBody">
    <table class="historyTable" cellpadding="0" cellspacing="0">
        <tbody>
            <% var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController; %>
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
            <tr>
                <th class="<%= theHeader%>" rowspan="<%= (string.IsNullOrEmpty(c.Comment)) ? 1 : 2 %>">
                    <%: c.GetCommentByDisplayName(controller) %>
                </th>
                <td style="background-color: #EEF3F7; color: #134A8A;">
                    <%
                        if (newFlag)
                        { %><div style="float: left; margin-right: 5px;">
                            <img alt="New Comment" src="<%= Url.Content("~/Content/newEmailAlert.png") %>" /></div>
                    <%
                        } 
                    %>
                    <%: c.GetCommentByDisplayName(controller)%>
                    <%: c.CommentEvent %>
                    <div class="fieldSubText" style="color: #666;">
                        <%: c.CommentedDate.ToLongDateString()%>
                        <%: c.CommentedDate.ToShortTimeString() %>
                    </div>
                </td>
            </tr>
            <%      
                if (!string.IsNullOrEmpty(c.Comment))
                {
            %>
            <tr>
                <td class="commentBody">
                    <%= c.HtmlComment%>
                </td>
            </tr>
            <%
                }
            %>
            <tr>
                <td class="historyTableSeperator" colspan="2" />
            </tr>
            <%
                } 
            %>
        </tbody>
    </table>
</div>
