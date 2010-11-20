<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.TicketEventNotification>" %>
<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
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
        .emailDetailsWrapper
        {
            border: solid 2px #D6D6D6;
            border-collapse: collapse;
        }
        
        a:link
        {
            color: #14498D;
            text-decoration: underline;
        }
        a:visited
        {
            color: #505abc;
        }
        a:hover
        {
            color: #1d60ff;
            text-decoration: none;
        }
        a:active
        {
            color: #12eb87;
        }
        
        .activityHeadWrapper
        {
            background-color: #DFF8DC;
            border-bottom: solid 2px #134A8A;
        }
        
        .activityHead
        {
            color: #0B294F;
            padding: 3px;
        }
        .activityBody
        {
            padding: 3px 3px 8px 3px;
        }
        .activityFieldsContainer
        {
            font-size: 10pt;
        }
        
        .formatTable
        {
        }
        
        .formatTable tbody tr th
        {
            text-align: right;
            padding: 4px 3px 3px 3px;
            color: #134A8A;
            vertical-align: top;
            white-space: nowrap;
            font-weight: normal;
        }
        
        .formatTable tbody tr td
        {
            padding: 2px;
            vertical-align: top;
        }
        
        .formatTable tbody tr td.textField
        {
            padding: 4px 3px 3px 3px;
        }
        
        div.ticketDetailsHeaderOuter
        {
            border-bottom: solid 2px #134A8A;
            margin-top: 0px;
        }
        div.ticketDetailsHeaderInner
        {
            color: #0B294F;
            background-color: #E1EBF2;
        }
        div.ticketDetailsHeaderInner table
        {
            width: 100%;
        }
        
        td.ticketDetailsHeaderId
        {
            white-space: nowrap;
            vertical-align: top;
            font-weight: bold;
            padding: 5px;
        }
        
        td.ticketDetailsHeaderPriority
        {
            padding: 5px;
        }
        
        td.ticketDetailsHeaderPriority div
        {
            float: right;
        }
        
        tr.ticketDetailsHeaderTitle
        {
            height: 45px;
        }
        
        tr.ticketDetailsHeaderTitle td
        {
            padding: 3px 8px 8px 20px;
        }
        
        .ticketDetailsHeaderInfo
        {
            font-size: 8pt;
            background-color: #EEF3F7;
        }
        
        .ticketDetailsHeaderInfoTable
        {
            width: 100%;
            color: #444;
            font-size: 9pt;
        }
        
        .ticketDetailsHeaderInfoLabel
        {
            white-space: nowrap;
            text-align: right;
        }
        
        .ticketDetailsHeaderInfoText
        {
            white-space: nowrap;
        }
        
        td.ticketDetailsHeaderTagsArea
        {
            width: 100%;
        }
        td.ticketDetailsHeaderTagsArea div
        {
            text-align: right;
            width: 100%;
        }
        
        div.ticketDetailsOuter
        {
            background-color: #fff;
            padding-bottom: 0px;
        }
        
        div.ticketDetailsInner
        {
        }
        
        div.ticketDetailsInner table
        {
            width: 100%;
        }
        
        td.ticketDetailsArea
        {
        }
        
        td.ticketDetailsArea div
        {
            height: 180px;
            overflow: auto;
            padding: 3px;
        }
        
        .statusFlag
        {
            min-height: 90px;
            width: 20px;
            border-right: solid 1px #B3CBDF;
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
        
        .historyTable
        {
            border-collapse: collapse;
            margin: 5px;
            width: 98%;
        }
        
        .historyTable th, .historyTable td
        {
            padding: 3px;
            border: solid 1px #B3CBDF;
            vertical-align: top;
        }
        
        .historyTable th
        {
            font-weight: normal;
            font-size: 9pt;
            color: #134A8A;
            white-space: nowrap;
        }
        
        td.historyTableSeperator
        {
            border: none;
            height: 5px;
            padding: 0px;
        }
        
        th.myComment
        {
            background-color: #DFF8DC;
        }
        
        th.staffComment
        {
            background-color: #FFFFDD;
        }
        
        th.userComment
        {
            background-color: #E1EBF2;
        }
        
        td.commentBody
        {
            padding-left: 25px;
        }
        
        .commentContainer
        {
            min-height: 145px;
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
    <div>
        <div class="emailDetailsWrapper" style="float: right; width: 225px;">
            <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.TicketStats, Model.TicketComment.Ticket, ViewData); %>
        </div>
        <div class="emailDetailsWrapper" style="margin-right: 235px;">
            <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.Details, Model.TicketComment.Ticket, ViewData); %>
        </div>
    </div>
    <div style="clear: both;">
    </div>
    <%
        if (Model.TicketComment.Ticket.TicketAttachments.Count > 0)
        { 
    %>
    <div style="margin-top: 5px;">
        <div class="emailDetailsWrapper">
            <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.Attachments, Model.TicketComment.Ticket, ViewData); %>
        </div>
    </div>
    <%
        }
    %>
    <div style="margin-top: 5px;">
        <div class="emailDetailsWrapper">
            <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.ActivityHistory, Model.TicketComment.Ticket, ViewData); %>
        </div>
    </div>
</body>
</html>
