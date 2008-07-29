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
    public static class TicketDeskServiceUtilities
    {

        private static TicketDataDataContext ctx = new TicketDataDataContext();

        public static int BeginNotificationCycle(int ticketid, string url)
        {
            IQueryable<DateTime> dt = from t in ctx.Tickets
                                      where t.TicketId == ticketid
                                      select t.LastUpdateDate;

            DateTime currentUpdateTime = new DateTime();

            //Get amount of time to delay
            string delay = ConfigurationManager.AppSettings["EmailNotificationDelay"];
            if (delay != null && delay != "none")
            {
                Int32 minutesToDelay = Convert.ToInt32(delay);
                Int32 millisecondsToDelay = minutesToDelay * 60 * 1000 / 2;

                //Pause for 2.5 min
                Thread.Sleep(millisecondsToDelay);

                while (currentUpdateTime > DateTime.Now.AddMinutes(-(minutesToDelay)))
                {
                    //Pause for 2.5 min
                    Thread.Sleep(millisecondsToDelay);
                    currentUpdateTime = dt.Single();
                };
            }
            //Once the updates have slowed, send Notification
            SendNotification(ticketid, url);
            return ticketid;
        }

        private static void SendNotification(int ticketid, string url)
        {

            Ticket ticket = (from n in ctx.Tickets
                             where n.TicketId == ticketid
                             select n).Single();

            bool enableEmail = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailNotifications"]);

            if (enableEmail)
            {
                SmtpClient client = new SmtpClient();

                string body = GetHTMLBody(ticket, url);

                TicketComment comment = ticket.TicketComments.Single(tc => tc.CommentedDate == (ticket.TicketComments.Max(tcm => tcm.CommentedDate)));

                string displayFrom = ConfigurationManager.AppSettings["FromEmailDisplayName"];
                string addressFrom = ConfigurationManager.AppSettings["FromEmailAddress"];
                MailAddress fromAddr = new MailAddress(addressFrom, displayFrom);

                string subject = string.Format("Ticket {0} changed - {1} {2}", ticket.TicketId.ToString(), comment.CommentedBy, comment.CommentEvent);

                foreach (MailAddress toAddr in ticket.GetNotificationEmailAddressesForUsers())
                {
                    MailMessage msg = new MailMessage(fromAddr, toAddr);
                    msg.Subject = subject;
                    msg.Body = body;  //body;
                    msg.IsBodyHtml = true;
                    //msg.BodyEncoding = Encoding.UTF8;
                    //msg.SubjectEncoding = Encoding.UTF8;
                    try
                    {
                        client.Send(msg);
                    }
                    catch
                    {
                        //do nothing, continue
                    }
                }
            }
        }

        private static string GetHTMLBody(Ticket ticket, string url)
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
                        <A id=TicketType href=""{0}"">Problem</A>: <A id=TicketTitle href=""{0}"">Test</A> 
                    </DIV>
                    <DIV class=BlockBody style=""HEIGHT: 180px"">
                        <SPAN id=Details>{1}</SPAN> 
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

            stringBuilder.Append(GetHTMLComments(ticket));

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
                                    ticket.CreatedDate.ToString(), // {11} 
                                    SecurityManager.GetUserDisplayName(ticket.CurrentStatusSetBy), // {12}
                                    ticket.CurrentStatusDate.ToString(), // {13} 
                                    SecurityManager.GetUserDisplayName(ticket.LastUpdateBy), // {14}
                                    ticket.LastUpdateDate.ToString() // {15}
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
    .CommentBox
    {{
    }}
    .CommentHead
    {{
        padding: 3px;
        color: #416523;
    }}
    .CommentSeperator
    {{
        color: #CDCDCD;
        height: 1px;
    }}
    .CommentText
    {{
        display: block;
        padding-left: 15px;
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

        private static string GetHTMLComments(Ticket ticket)
        {
            StringBuilder commentBuilder = new StringBuilder();
            commentBuilder.Append(@"
        <TR>
            <TD colSpan=2>
                <DIV class=Block>
                    <DIV class=BlockHeader>Activity Log: </DIV>
                    <DIV class=BlockBody>
");

            int repeater = 100;

            foreach (TicketComment tc in ticket.TicketComments)
            {
                string comment = "";
                if (repeater > 100)
                {
                    comment += "\n<HR class=CommentSeperator>\n";
                }

                comment += @"
                        <DIV class=CommentBox>
                            <DIV class=CommentHead>
                                <SPAN id=CommentLogRepeater_ct{0}_CommentDate>{1}</SPAN>
                                <BR>
                                <SPAN id=CommentLogRepeater_ct{0}_CommentBy>{2}</SPAN>
                                <SPAN id=CommentLogRepeater_ct{0}_CommentEvent>{3}</SPAN>
                            </DIV>
                        <SPAN class=CommentText id=CommentLogRepeater_ct{0}_CommentText>{4}</SPAN> 
                        </DIV>
";

                comment = System.String.Format(comment,
                                                repeater.ToString(),
                                                tc.CommentedDate.ToString(),
                                                SecurityManager.GetUserDisplayName(tc.CommentedBy),
                                                tc.CommentEvent,
                                                tc.Comment
                                                );

                commentBuilder.Append(comment);

                repeater++;
            }

            commentBuilder.Append(@"
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
