<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TicketDesk.Domain.Models.Ticket>" %>
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
            margin-top: 0px;
        }
        div.ticketDetailsHeaderInner
        {
            color: #0B294F;
            border-bottom: solid 2px #134A8A;
        }
        
        .ticketDetailsHeader
        {
            min-height: 100px;
            margin-left: 20px;
            background-color: #F0F5F9;
        }
        
        .ticketDetailsHeaderId
        {
            white-space: nowrap;
            vertical-align: top;
            font-weight: bold;
            padding: 3px;
        }
        .ticketDetailsHeaderPriority
        {
            padding: 3px;
            float: right;
            margin-right: 15px;
        }
        
        .ticketDetailsHeaderTitle
        {
            font-size: 10pt;
            padding: 3px 3px 3px 20px;
            color: #444;
        }
        .ticketDetailsHeaderInfo
        {
            background-color: #FBFCFD;
            border-top: solid 1px #B3CBDF;
            font-size: 9pt;
            padding: 3px;
        }
        
        .ticketDetailsHeaderInfoLabel
        {
            display: inline-block;
            color: #444;
            width: 75px;
            text-align: right;
        }
        .ticketDetailsHeaderInfoText
        {
            display: inline-block;
        }
        
        .ticketDetailsHeaderAssignedTo
        {
            font-weight: bold;
        }
        
        .ticketDetailsHeaderTagsArea
        {
            color: #444;
            float: right;
            margin-right: 15px;
        }
        
        div.ticketDetailsOuter
        {
            background-color: #fff;
            padding-bottom: 0px;
        }
        div.ticketDetailsInner
        {
        }
        
        .detailsArea
        {
            overflow: auto;
        }
        
        #detailsText
        {
            padding: 8px;
        }
        
        
        
        .statusFlag
        {
            float: left;
            min-height: 90px;
            width: 20px;
            border-right: solid 1px #B3CBDF;
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
        
        .historyArea
        {
            border-collapse: collapse;
            margin: 10px;
            width: 98%;
        }
        
        .activityHistoryItemWrapper
        {
            border: solid 1px #B3CBDF;
        }
        
        .commentHeader
        {
            color: #134A8A;
        }
        .myComment
        {
            background-color: #ECFBEA;
        }
        
        .staffComment
        {
            background-color: #FFFFDD;
        }
        
        .userComment
        {
            background-color: #E1EBF2;
        }
        
        .commentDisplayName
        {
            font-weight: bold;
        }
        .commentDate
        {
            color: #666;
            font-size: 9pt;
            margin-left: 25px;
        }
        .commentBody
        {
            padding-left: 25px;
            border-top: solid 1px #B3CBDF;
        }
        
        
        .commentContainer
        {
            min-height: 145px;
        }
        #attachmentsList
        {
            font-size: 9pt;
        }
        .statsTable
        {
        }
        
        .ticketStatsBody
        {
            background-color: #FBFCFD;
            font-size: 9pt;
            padding: 3px;
        }
        .statsTable tbody tr th label
        {
        }
        
        .statsTable tbody tr th
        {
            text-align: right;
            padding: 2px;
            color: #134A8A;
            vertical-align: top;
            white-space: nowrap;
            font-weight: normal;
        }
        
        .statsTable tbody tr td
        {
            padding: 2px;
            vertical-align: top;
        }
        
        .statsTable tbody tr td.textField
        {
            padding: 2px;
        }
        
        pre
        {
            font-family: Consolas,Menlo,Monaco,Lucida Console,Liberation Mono,DejaVu Sans Mono,Bitstream Vera Sans Mono,Courier New,monospace,serif;
            margin-bottom: 10px;
            min-height: 35px;
            overflow: auto;
            max-width: 550px;
            padding: 5px;
            background-color: #eee;
            padding-bottom: 20px!ie7;
            max-height: 600px;
        }
        code
        {
            font-family: Consolas,Menlo,Monaco,Lucida Console,Liberation Mono,DejaVu Sans Mono,Bitstream Vera Sans Mono,Courier New,monospace,serif;
            background-color: #eee;
            padding: 1px 5px 1px 5px;
            font-size: 9pt;
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
            <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.TicketStats, Model, ViewData); %>
        </div>
        <div class="emailDetailsWrapper" style="margin-right: 235px;">
            <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.Details, Model, ViewData); %>
        </div>
    </div>
    <div style="clear: both;">
    </div>
    <%
        if (Model.TicketAttachments.Count > 0)
        { 
    %>
    <div style="margin-top: 5px;">
        <div class="emailDetailsWrapper">
            <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.Attachments, Model, ViewData); %>
        </div>
    </div>
    <%
        }
    %>
    <div style="margin-top: 5px;">
        <div class="emailDetailsWrapper">
            <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.ActivityHistory, Model, ViewData); %>
        </div>
    </div>
</body>
</html>
