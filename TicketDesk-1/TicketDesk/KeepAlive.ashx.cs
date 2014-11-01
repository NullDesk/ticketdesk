// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketDesk
{
    public class KeepAlive : IHttpHandler
    {

        private static byte[] emptyGif = {
            0x47,0x49,0x46,0x38,0x39,0x61,0x01,0x00,0x01,0x00,0x91,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x21,0xf9,0x04,0x09,0x00,
            0x00,0x00,0x00,0x2c,0x00,0x00,0x00,0x00,0x01,0x00,0x01,0x00,0x00,0x08,0x04,
            0x00,0x01,0x04,0x04,0x00,0x3b,0x00};

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = "image/gif";
            context.Response.BinaryWrite(emptyGif);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
