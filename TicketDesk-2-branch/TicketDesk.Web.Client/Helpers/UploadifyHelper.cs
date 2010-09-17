using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace TicketDesk.Web.Client.Helpers
{

    public static class UploadifyHelper
    {
        /// <summary>
        /// Renders JavaScript to turn the specified file input control into an 
        /// Uploadify upload control.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string Uploadify(this HtmlHelper helper, string name, UploadifyOptions options)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);


            string scriptPath = urlHelper.Content("~/Scripts/uploadify/");

            StringBuilder sb = new StringBuilder();
            //Include the JS file.
            sb.Append(string.Format("<script type=\"text/javascript\" src=\"{0}\" ></script>", urlHelper.Content("~/Scripts/uploadify/jquery.uploadify.v2.1.0.min.js")));
            sb.Append(string.Format("<script type=\"text/javascript\" src=\"{0}\" ></script>", urlHelper.Content("~/Scripts/uploadify/swfobject.js")));

            sb.Append(string.Format("<script type=\"text/javascript\" src=\"{0}\" ></script>", urlHelper.Content("~/Scripts/uploadify/jquery.uploadify.init.js")));



            //Dump the script to initialze Uploadify
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("function goUploadify() {");
            sb.AppendFormat("initUploadify($('#{0}'),'{1}','{2}','{3}','{4}','{5}',{6},{7}, '{8}');", name, options.UploadUrl,
                            scriptPath, options.FileExtensions, options.FileDescription, options.AuthenticationToken,
                            options.ErrorFunction ?? "null", options.CompleteFunction ?? "null", options.ButtonText);
            sb.AppendLine();
            sb.AppendLine("}");
            sb.AppendLine("</script>");

            return sb.ToString();
        }
    }

}