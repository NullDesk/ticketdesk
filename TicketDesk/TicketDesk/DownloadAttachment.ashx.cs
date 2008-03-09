using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using TicketDesk.Engine;
using TicketDesk.Engine.Linq;
using System.IO;
using System.Data.Common;

namespace TicketDesk
{

    public class DownloadAttachment : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if(context.Request.QueryString["fileId"] != null && CheckSecurity())
            {
                int fileId = Convert.ToInt32(context.Request.QueryString["fileId"]);
                SendFileToClient(context, fileId);
            }
            else
            {
                context.Response.Redirect("~/Default.aspx", true);
            }
        }

        private bool CheckSecurity()
        {
            return (SecurityManager.IsStaffOrAdmin || SecurityManager.IsTicketSubmitter);
        }

        private void SendFileToClient(HttpContext context, int fileId)
        {
            HttpResponse httpResponse = context.Response;
            int bufferSize = 100;
            byte[] outByte = new byte[bufferSize];

            long startIndex = 0;

            TicketDataDataContext ctx = new TicketDataDataContext();

            var query = from ta in ctx.TicketAttachments
                        where ta.FileId == fileId
                        select new
                        {
                            ta.FileName,
                            ta.FileSize,
                            ta.FileType,
                            ta.FileContents
                        };

            DbCommand cmd = ctx.GetCommand(query);


            DbConnection conn = cmd.Connection;
            conn.Open();
            DbDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

            while(reader.Read())
            {

                string filename = reader.GetString(0);
                int filelength = reader.GetInt32(1);//for some reason GetString doesn't convert int32 to string, throws an error. Using alternate access via indexer.
                string contenttype = reader.GetString(2);


                // set the mime type: must occur before getting the file content.
                httpResponse.ContentType = contenttype;

                httpResponse.Clear();
                httpResponse.BufferOutput = false;

                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\";");
                httpResponse.AppendHeader("Content-Length", filelength.ToString());

                BinaryWriter writer = new BinaryWriter(httpResponse.OutputStream);

                bool fileComplete = false;
                while(!fileComplete)
                {
                    if(startIndex + bufferSize >= filelength)
                    {
                        fileComplete = true;
                    }
                    startIndex += reader.GetBytes(3, startIndex, outByte, 0, bufferSize);
                    writer.Write(outByte);
                    writer.Flush();
                }
                writer.Close();
                httpResponse.OutputStream.Close();
            }
            reader.Close();
            conn.Close();

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
