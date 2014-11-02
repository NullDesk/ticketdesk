<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<% 
    
    
    var controller = ViewContext.Controller as TicketDesk.Web.Client.Controllers.ApplicationController;
    var currentFlagStatus = Model.CurrentStatus.Replace(" ", "").ToLower();
    if (string.IsNullOrEmpty(Model.AssignedTo))
    {
        currentFlagStatus = "unassigned";
    }
    var root = ViewData["siteRootUrl"] as string;

    var flagUrl = root + Url.Content(string.Format("~/Content/{0}Flag.png", Url.Encode(currentFlagStatus)));

    var ticketUrl = root + Url.Content(string.Format("~/Ticket/{0}", Model.TicketId.ToString()));

    var detailsHeight = (ViewData["formatForSearch"] == null) ? 150 : 70;
    
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <style type="text/css">
        body
        {
            font-size: 11pt;
            font-family: Calibri, Tahoma, Verdana, Helvetica, Sans-Serif;
            color: #555;
        }
        .unassignedFlag
        {
            background-color: #fff600;
        }
        .activeFlag
        {
            background-color: #134A8A;
        }
        
        .moreinfoFlag
        {
            background-color: #842212;
        }
        
        .resolvedFlag
        {
            background-color: #ED9017;
        }
        
        .closedFlag
        {
            background-color: #8A8989;
        }
        
        .statsTable
        {
            font-size: 9pt;
        }
        
        pre
        {
            font-family: Consolas,Menlo,Monaco,Lucida Console,Liberation Mono,DejaVu Sans Mono,Bitstream Vera Sans Mono,Courier New,monospace,serif;
            margin-bottom: 10px;
            min-height: 35px;
            overflow: auto;
            max-width: 450px;
            padding: 5px;
            background-color: #eee;
        }
        
        code
        {
            font-family: Consolas,Menlo,Monaco,Lucida Console,Liberation Mono,DejaVu Sans Mono,Bitstream Vera Sans Mono,Courier New,monospace,serif;
            background-color: #eee;
            padding: 1px 5px 1px 5px;
            font-size: 9pt;
        }
        
        
        .myComment
        {
            color: #134A8A;
            padding: 2px 2px 2px 5px;
            background-color: #ECFBEA;
        }
        
        .staffComment
        {
            color: #134A8A;
            padding: 2px 2px 2px 5px;
            background-color: #FFFFDD;
        }
        
        .userComment
        {
            color: #134A8A;
            padding: 2px 2px 2px 5px;
            background-color: #E1EBF2;
        }
        
         .fieldSubText
        {
            font-size: 9pt;
            color: #999;
            margin-left: 25px;
        }
    </style>
</head>
<body>
    <table cellpadding="2" cellspacing="0" width="100%">
        <tr>
            <td style="padding: 5px;">
                <table cellpadding="2" cellspacing="0" width="100%" style="background-color: #CCDCEA;
                    border: solid 1px #CCDCEA;">
                    <tr>
                        <td rowspan="3" class="<%=  currentFlagStatus%>Flag" width="20px" style="border-right: solid 1px #CCDCEA;">
                            <img alt="<%: currentFlagStatus %>" src="<%= flagUrl %>" />
                        </td>
                        <td style="padding: 3px;">
                            <a href="<%= ticketUrl %>">Ticket: #<%: Model.TicketId %>
                                -
                                <%: Model.Category%>
                                <%: Model.Type%></a>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-size:11pt;padding: 3px 3px 3px 20px;">
                            <%: Model.Title%>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #FBFCFD">
                            <table cellpadding="2" cellspacing="0" width="100%" style="font-size: 9pt;">
                                <tr>
                                    <td style="white-space: nowrap;" align="right">
                                        Assigned To:
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <%: Model.GetAssignedToDisplayName(controller)%>
                                    </td>
                                    <td rowspan="2" width="100%" align="right" valign="top">
                                     <%
                                         if (!string.IsNullOrEmpty(Model.TagList))
                                         { 
                %>
                                        Tags:
                                        <%: Model.TagList%>
                                    </td>
                                    <%
                                         } 
                                        %>
                                </tr>
                                <tr>
                                    <td style="white-space: nowrap;" align="right">
                                        Owned By:
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <%: Model.GetOwnerDisplayName(controller)%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #FFFFFF; border-top: solid 2px #134A8A; padding: 5px;"
                            colspan="2">
                            <%= Model.HtmlDetails%>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px;" valign="top">
                <table cellpadding="2" cellspacing="0" class="statsTable" style="border: solid 1px #CCDCEA;
                    background-color: #FBFCFD;"  width="100%">
                    <tr>
                        <th colspan="2" style="background-color: #CCDCEA; border-bottom: solid 1px #134A8A;
                            font-weight: bold; color: #0B294F; padding: 8px; font-size:10pt;">
                           
                            Ticket Stats:
                        </th>
                    </tr>
                    <tr>
                        <th style="white-space: nowrap; text-align:right;">
                            <label for="LastUpdateBy">
                                Status:
                            </label>
                        </th>
                        <td style="white-space: nowrap;" class="textField">
                            <%: Model.CurrentStatus%>
                        </td>
                    </tr>
                    <tr>
                        <th style="white-space: nowrap;text-align:right;">
                            <label for="LastUpdateBy">
                                Updated By:
                            </label>
                        </th>
                        <td style="white-space: nowrap;" style="" class="textField">
                            <%: Model.GetLastUpdateByDisplayName(controller)%>
                        </td>
                    </tr>
                    <tr>
                        <th style="white-space: nowrap;text-align:right;">
                            <label for="LastUpdateDate">
                                Updated Date:
                            </label>
                        </th>
                        <td style="white-space: nowrap;" class="textField">
                            <%: Model.LastUpdateDate.ToShortDateString()%>
                            <%: Model.LastUpdateDate.ToShortTimeString()%>
                        </td>
                    </tr>
                    <tr>
                        <th style="white-space: nowrap;text-align:right;">
                            <label for="CurrentStatusSetBy">
                                Status By:
                            </label>
                        </th>
                        <td style="white-space: nowrap;" class="textField">
                            <%: Model.GetCurrentStatusByDisplayName(controller)%>
                        </td>
                    </tr>
                    <tr>
                        <th style="white-space: nowrap;text-align:right;">
                            <label for="CurrentStatusDate">
                                Status Date:
                            </label>
                        </th>
                        <td style="white-space: nowrap;" class="textField">
                            <%: Model.CurrentStatusDate.ToShortDateString()%>
                            <%: Model.CurrentStatusDate.ToShortTimeString()%>
                        </td>
                    </tr>
                    <tr>
                        <th style="white-space: nowrap;text-align:right;">
                            <label for="CreatedBy">
                                Created by:
                            </label>
                        </th>
                        <td style="white-space: nowrap;" class="textField">
                            <%: Model.GetCreatedByDisplayName(controller)%>
                        </td>
                    </tr>
                    <tr>
                        <th style="white-space: nowrap;text-align:right;">
                            <label for="CreatedDate">
                                Created Date:
                            </label>
                        </th>
                        <td style="white-space: nowrap;" class="textField">
                            <%: Model.CreatedDate.ToShortDateString()%>
                            <%: Model.CreatedDate.ToShortTimeString()%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <%
    
            if (Model.TicketAttachments.Where(ta => !ta.IsPending).Count() > 0)
            {
        %>
        <tr>
            <td style="padding: 5px;">
            <table cellpadding="2" cellspacing="0" class="statsTable" style="border: solid 1px #CCDCEA;
                    border-collapse: collapse; background-color: #FBFCFD; padding: 5px;" width="100%">
                    <tr>
                        <th colspan="2" style="background-color: #CCDCEA; border-bottom: solid 1px #134A8A;
                            font-weight: bold; color: #0B294F; padding: 8px;font-size:10pt;">
                            Attachments:
                        </th>
                    </tr>
                </table>
                <%foreach (var a in Model.TicketAttachments.Where(ta => !ta.IsPending))
                  {
                %>
                <div style="border: solid 1px #CCDCEA; padding: 5px; font-size:9pt;">
                    <%: a.FileName%>
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
            </td>
        </tr>
        <%
            }
        %>
        <tr>
            <td style="padding: 5px;">

                <table cellpadding="2" cellspacing="0" class="statsTable" style="border: solid 1px #CCDCEA;
                    border-collapse: collapse; background-color: #FBFCFD; padding: 5px;" width="100%">
                    <tr>
                        <th colspan="2" style="background-color: #CCDCEA; border-bottom: solid 1px #134A8A;
                            font-weight: bold; color: #0B294F; padding: 8px;font-size:10pt;">
                            Activity & History:
                        </th>
                    </tr>
                </table>
               
                <% 
                    var newEmailAlertUrl = root + Url.Content("~/Content/newEmailAlert.png"); 
    
                 
                %>
                <%
                
                    foreach (var c in Model.TicketComments.OrderByDescending(tc => tc.CommentedDate))
                    {
                    
                %>
                <table cellpadding="2" cellspacing="0" class="statsTable" style="border: solid 1px #CCDCEA;
                    border-collapse: collapse; background-color: #FBFCFD; padding: 5px;" width="100%">
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
                        <td rowspan="2" width="35px" class="<%= theHeader%>">
                            <%
                                if (newFlag)
                                { 
                            %>
                            <img alt="New Comment" align="left" style="float: left; margin-right: 5px;" src="<%= newEmailAlertUrl %>" />
                            <%
                                } 
                            %>
                        </td>
                        <td class="<%= theHeader%>" width="100%">
                            <span class="commentDisplayName">
                                <%: c.GetCommentByDisplayName(controller)%></span>
                            <%: c.CommentEvent %>
                        </td>
                    </tr>
                    <tr>
                        <td class="<%= theHeader%>" width="100%">
                            <div class="commentDate">
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
                        <td>
                        </td>
                        <td style="padding: 5px;" width="100%">
                            <div class="commentBody">
                                <%= c.HtmlComment%>
                            </div>
                        </td>
                    </tr>
                    <%
                        }
                    %>
                </table>
                <%
                    } 
                %>
            </td>
        </tr>
    </table>
</body>
</html>
