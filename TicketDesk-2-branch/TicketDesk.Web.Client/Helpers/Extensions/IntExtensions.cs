using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketDesk.Web.Client.Helpers
{
    public static class IntExtensions
    {
        public static string ToFileSizeString(this int bytes)
        {
            string size = "0 Bytes";
            double bb = Convert.ToDouble(bytes);

            if (bb >= 1073741824D)
                size = string.Format("{0:n2}", bb / 1073741824D) + " GB";
            else if (bb >= 1048576)
                size = string.Format("{0:n2}", bb / 1048576D) + " MB";
            else
                size = string.Format("{0:n2}", bb / 1024D) + " KB";

            return size;
        }
    }
}