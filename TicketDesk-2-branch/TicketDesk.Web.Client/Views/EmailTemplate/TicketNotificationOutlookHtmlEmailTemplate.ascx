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
            margin-bottom: 10px;
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
            margin: 5px;
            font-size: 10pt;
        }
        
        .formatTable
        {
            font-size: 8pt;
        }
        
        .formatTable th
        {
            text-align: right;
            padding: 1px 3px 1px 3px;
            color: #134A8A;
            vertical-align: top;
            white-space: nowrap;
            font-weight: normal;
        }
        
        .formatTable td
        {
            padding: 2px;
            vertical-align: top;
        }
        
        .formatTable td.textField
        {
            padding: 1px 3px 1px 3px;
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
        
        
        .ticketDetailsHeaderTable
        {
            width: 100%;
            background-color: #E1EBF2;
        }
        
        td.ticketDetailsHeaderId
        {
            white-space: nowrap;
            vertical-align: top;
            font-weight: bold;
            padding: 0px 5px 0px 5px;
        }
        
        td.ticketDetailsHeaderPriority
        {
            padding: 0px 5px 0px 5px;
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
        }
        
        .ticketDetailsHeaderInfoTable
        {
            width: 100%;
            color: #444;
            font-size: 9pt;
            background-color: #EEF3F7;
        }
        
        td.ticketDetailsHeaderInfoLabel
        {
            white-space: nowrap;
            text-align: right;
        }
        
        td.ticketDetailsHeaderInfoText
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
            padding: 5px;
            font-size: 11pt;
            color: #444;
        }
        
        td.ticketDetailsArea div
        {
           
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
            margin: 10px;
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
            margin-left: 15px;
        }
        .commentHeaderTable td
        {
            border-style: none;
        }
    </style>
</head>
<body>
    <table cellspacing="10" cellpadding="0">
        <tr>
            <td class="emailDetailsWrapper">
                <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.Details, Model, ViewData); %>
            </td>
            <td class="emailDetailsWrapper" style="vertical-align: top; width: 200px;">
                <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.TicketStats, Model, ViewData); %>
            </td>
        </tr>
        <%
            if (Model.TicketAttachments.Count > 0)
            { 
        %>
        <tr>
            <td colspan="2" class="emailDetailsWrapper">
                <div>
                    <div>
                        <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.Attachments, Model, ViewData); %>
                    </div>
                </div>
            </td>
        </tr>
        <%
            }
        %>
        <tr>
            <td colspan="2" class="emailDetailsWrapper">
                <div>
                    <div>
                        <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.ActivityHistory, Model, ViewData); %>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</body>
</html>
