// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//      Steven Murawski 
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Threading;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web;
using TicketDesk;
using TicketDesk.Controls;
using TicketDesk.Engine.Linq;


namespace TicketDesk.Engine
{
    public static class NotificationUtilities
    {

        public static string GetHTMLBody(Ticket ticket, string url, string userToNotify, int minCommentId)
        {

            string viewTicket = string.Format("ViewTicket.aspx?id={0}", ticket.TicketId.ToString());

            url = url.Replace("NewTicket", viewTicket);

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(GetStyle());

            #region Start body
            stringBuilder.Append(@"
<TABLE style=""WIDTH: 100%"">
    <TBODY>
        <TR>
            <TD style=""VERTICAL-ALIGN: top; WIDTH: 75%"">
                <DIV class=Block>");
            #endregion

            //add url {0} and title {1} placeholders
            #region one
            stringBuilder.Append(@"
                    <DIV class=BlockHeader>
                        <A id=TicketType href=""{0}"">{16}</A> : <A id=TicketTitle href=""{0}"">{1}</A> 
                    </DIV>
                    <DIV class=BlockBody style=""HEIGHT: 180px"">
                        <SPAN id=Details>{17}</SPAN> 
                    </DIV>
                </DIV>
            </TD>
");
            #endregion

            //add Ticket number (ticketid) {2}
            #region two
            stringBuilder.Append(@"    
            <TD style=""VERTICAL-ALIGN: top; WIDTH: 25%"">
                <DIV class=Block>
                    <DIV class=BlockHeader>Ticket ID: <A id=TicketId href=""{0}"">{2}</A> </DIV>");
            #endregion

            //add status {3} and priority {4} and category {5}, owner {6), assigned to {7}
            //affects customer {8} and published to kb {9}
            #region three
            stringBuilder.Append(@"
                    <DIV class=BlockBody style=""WHITE-SPACE: nowrap; HEIGHT: 203px"">
                        <TABLE>
                            <TBODY>
                                <TR>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap; TEXT-ALIGN: right"">Status: </TD>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap"">
                                        <SPAN id=CurrentStatus>{3}</SPAN> 
                                    </TD>
                                </TR>
                                <TR>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap; TEXT-ALIGN: right"">Priority: </TD>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap"">
                                        <SPAN id=Priority>{4}</SPAN> 
                                    </TD>
                                </TR>
                                <TR>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap; TEXT-ALIGN: right"">Category: </TD>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap"">
                                        <SPAN id=Category>{5}</SPAN> 
                                    </TD>
                                </TR>
                                <TR>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap; TEXT-ALIGN: right"">Owned by: </TD>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap"">
                                        <SPAN id=Owner>{6}</SPAN> 
                                    </TD>
                                </TR>
                                <TR>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap; TEXT-ALIGN: right"">Assigned to: </TD>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap"">
                                        <A id=AssignedTo>{7}</A> 
                                    </TD>
                                </TR>
                                <TR>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap; TEXT-ALIGN: right"">Affects Customer(s): </TD>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap"">
                                        <SPAN id=AffectsCustomer>{8}</SPAN> 
                                    </TD>
                                </TR>
                                <TR>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap; TEXT-ALIGN: right"">Published to KB: </TD>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap"">
                                        <SPAN id=PublishedToKb>{9}</SPAN> 
                                    </TD>
                                </TR>
");
            #endregion

            //add tags
            #region tags
            stringBuilder.Append(@"		
                                <TR>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap; TEXT-ALIGN: right"">Tags: </TD>
");

            string tagHtml = String.Format(@"          
                                    <TD style=""VERTICAL-ALIGN: top"">
                                        <SPAN id=TagRepeater_ct100_TicketTag>{0}</SPAN> 
                                    </TD>
                                </TR>
",
                                                ticket.TagList);

            stringBuilder.Append(tagHtml);
            #endregion

            //add Created By {10}, created on {11}, current status by {12},
            //current status on {13}, last update by {14}, and last update on {15}
            #region four
            stringBuilder.Append(@"
                                <TR>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap; TEXT-ALIGN: right"">Created by: </TD>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap"">
                                        <SPAN id=CreatedBy>{10}</SPAN> on: <SPAN id=CreatedDate>{11}</SPAN> 
                                    </TD>
                                </TR>
                                <TR>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap; TEXT-ALIGN: right"">Status set by: </TD>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap"">
                                        <SPAN id=CurrentStatusBy>{12}</SPAN> on: <SPAN id=CurrentStatusDate>{13}</SPAN> 
                                    </TD>
                                </TR>
                                <TR>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap; TEXT-ALIGN: right"">Updated by: </TD>
                                    <TD style=""VERTICAL-ALIGN: top; WHITE-SPACE: nowrap"">
                                        <SPAN id=LastUpdateBy>{14}</SPAN> on: <SPAN id=LastUpdateDate>{15}</SPAN> 
                                    </TD>
                                </TR>
                            </TBODY>
                        </TABLE>
                    </DIV>
                </DIV>
            </TD>
        </TR>
");
            #endregion

            stringBuilder.Append(GetHTMLAttachments(ticket));

            stringBuilder.Append(GetHTMLComments(ticket, userToNotify, minCommentId));

            string body = stringBuilder.ToString();

            body = String.Format(body, //base string
                                    url, // {0}
                                    ticket.Title, // {1} 
                                    ticket.TicketId.ToString(), // {2} 
                                    ticket.CurrentStatus, // {3}
                                    ticket.Priority, // {4} 
                                    ticket.Category, // {5} 
                                    SecurityManager.GetUserDisplayName(ticket.Owner), // {6}
                                    SecurityManager.GetUserDisplayName(ticket.AssignedTo), // {7}
                                    BoolToYesNo(ticket.AffectsCustomer),  // {8}
                                    BoolToYesNo(ticket.PublishedToKb), // {9} 
                                    SecurityManager.GetUserDisplayName(ticket.CreatedBy), // {10}
                                    ticket.CreatedDate.ToString("g"), // {11} 
                                    SecurityManager.GetUserDisplayName(ticket.CurrentStatusSetBy), // {12}
                                    ticket.CurrentStatusDate.ToString("g"), // {13} 
                                    SecurityManager.GetUserDisplayName(ticket.LastUpdateBy), // {14}
                                    ticket.LastUpdateDate.ToString("g"), // {15}
                                    ticket.Type, // {16}
                                    ((!ticket.IsHtml) ? ticket.Details.FormatAsHtml() : ticket.Details) // {17}
                                    );

            body = string.Format("{0}{1}{2}",
                                    "<html><head></head><body>",
                                    body,
                                    "</body></html>"
                                    );
            return body;
        }

        private static string BoolToYesNo(bool value)
        {
            if (value)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }

        }

        private static string GetStyle()
        {
            return @"<STYLE>
    body
    {{
        font-family: Verdana, Helvetica, Arial, Sans-Serif;
        font-size: 9pt;
    }}
    td
    {{
        font-size: 9pt;
    }}
    .Block
    {{
        border: solid 1px #A0A0A0;
        margin: 3px;
    }}
    .BlockHeader
    {{
        background-color: #CDF2B3;
        padding: 3px;
        font-size: 10pt;
    }}
    .BlockSubHeader
    {{
        border-top: solid 1px #A0A0A0;
        background-color: #FFFFDD;
        padding: 3px;
        font-size: 10pt;
    }}
    .BlockBody
    {{
        padding: 3px;
        border-top: solid 1px #A0A0A0;
    }}
   .CommentBoxTable
{{
    border-spacing: 0px;
    border-collapse: collapse;
    empty-cells: show;
    margin: 10px;
}}

.CommentBox
{{
}}

.NewCommentArea
{{
    font-size:9pt;
    font-style:italic;
    color: #CC3300;
    text-align:center;
}}

.UserCommentHead
{{
    font-size: 8pt;
    padding: 5px;
    color: #416523;
    border: 1px solid #808080;
    background-color: #DAF5C5;
    vertical-align: top;
}}

.CommentHead
{{ 
    font-size: 8pt;
    padding: 5px;
    color: #416523;
    border: 1px solid #808080;
    background-color: #FFFFDD;
    vertical-align: top;
}}


.CommentTitleArea
{{
    padding: 5px;
    font-size: 9pt;
    border-top: 1px solid #808080;
    border-right: 1px solid #808080;
    border-bottom: solid 1px #C0C0C0;
    vertical-align: top;
    background-color: #ECF2ED;
}}

.CommentSeperator
{{
    color: #CDCDCD;
    height: 1px;
}}
.CommentText
{{
    min-height: 55px;
    border-right: 1px solid #808080;
    border-bottom: 1px solid #808080;
    padding: 10px 10px 10px 20px;
}}


.MultiFieldEditContainer
{{
    color: #555555;
}}


.MultiFieldEditFactsContainer
{{
    margin-left: 15px;
}}
.MultiFieldEditOldValue
{{
}}
.MultiFieldEditOldValue label
{{
    width: 50px;
    float: left;
    text-align: right;
}}
.MultiFieldEditNewValue
{{
}}

.MultiFieldEditNewValue label
{{
    width: 50px;
    float: left;
    text-align: right;
}}
.MultiFieldEditNewValue hr
{{
    height: 1px;
    margin-top: 3px;
    margin-bottom: 3px;
}}




</STYLE>";
        }

        private static string GetHTMLAttachments(Ticket ticket)
        {
            StringBuilder attachBuilder = new StringBuilder();
            attachBuilder.Append(@"
        <TR>
            <TD colSpan=2>
                <DIV class=Block>
                    <DIV class=BlockHeader>Attachments: </DIV>
                    <DIV class=BlockBody style="""">
");

            int repeater = 100;

            foreach (TicketAttachment ta in ticket.TicketAttachments)
            {
                string attachment = "";
                if (repeater > 100)
                {
                    attachment += "\n<hr />\n";
                }

                attachment += @"
                                <SPAN id=ttachmentsRepeater_ct{0}_AttachmentLink>{1}</SPAN> - 
                                <SPAN id=AttachmentsRepeater_ct{0}_AttachmentUploader>{2}</SPAN> : 
                                <SPAN id=AttachmentsRepeater_ct{0}_AttachmentUploadDate>{3}</SPAN>                          
";

                attachment = System.String.Format(attachment,
                                                repeater.ToString(),
                                                ta.FileName,
                                                SecurityManager.GetUserDisplayName(ta.UploadedBy),
                                                ta.UploadedDate.ToShortDateString()
                                                );

                attachBuilder.Append(attachment);
                repeater++;
            }

            attachBuilder.Append(@"
                    </DIV>
                </DIV>
            </TD>
        </TR>
");
            return attachBuilder.ToString();
        }

        private static string GetHTMLComments(Ticket ticket, string userToNotify, int minCommentId)
        {
            StringBuilder commentBuilder = new StringBuilder();
            commentBuilder.Append(@"
        <TR>
            <TD colSpan=2>
                <DIV class=Block>
                    <DIV class=BlockHeader>Activity Log: </DIV>
                    <DIV class=BlockBody>
                    <table class=CommentBoxTable>
");

            int repeater = 100;

            foreach (TicketComment tc in ticket.TicketComments.OrderByDescending(t => t.CommentedDate))
            {
                string comment = string.Empty;
                if (repeater > 100)
                {
                    comment += "<tbody><tr><td colspan='2' style='height:10px;'></td></tr></tbody>";
                }

                comment += @"
                         <tbody class='CommentBox'>
                            <tr>
                                <td rowspan='2' runat='server' class='{0}'>
                                    {5}
                                    {1}<br />
                                    <br />
                                    {2}
                                </td>
                                <td class='CommentTitleArea'>
                                    {1}<br />
                                    {2} {3}
                                </td>
                            </tr>
                            <tr>
                                <td class='CommentText'>
                                    {4}
                                </td>
                            </tr>
                         </tbody>";

                string userbackgroundClass = "CommentHead";
                string newCommentHead = string.Empty;
               
                if (tc.CommentedBy == userToNotify)
                {
                    userbackgroundClass = "UserCommentHead";
                }
                
                if (tc.CommentId >= minCommentId)
                {
                    newCommentHead = "<div class='NewCommentArea'>New</div>";
                    
                }
                comment = string.Format(comment,
                                        userbackgroundClass,
                                        tc.CommentedDate.ToString("dddd, MM/dd/yyyy hh:mm tt"),
                                        SecurityManager.GetUserDisplayName(tc.CommentedBy),
                                        tc.CommentEvent,
                                        tc.CommentAsHtml,
                                        newCommentHead
                                        );

                //comment = System.String.Format(comment,
                //                                repeater.ToString(),
                //                                tc.CommentedDate.ToString(),
                //                                SecurityManager.GetUserDisplayName(tc.CommentedBy),
                //                                tc.CommentEvent,
                //                                tc.CommentAsHtml,
                //                                backgroundClass,
                //                                userbackgroundClass
                //                                );

                commentBuilder.Append(comment);

                repeater++;
            }

            commentBuilder.Append(@"</table>
                    </DIV>
                </DIV>
            </TD>
        </TR>
    </TBODY>
</TABLE>
");
            return commentBuilder.ToString();
        }
    }
}
