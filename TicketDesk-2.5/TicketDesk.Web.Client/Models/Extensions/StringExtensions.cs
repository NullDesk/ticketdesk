using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarkdownSharp;

namespace TicketDesk.Web.Client
{
    public static class StringExtensions
    {
        public static string HtmlFromMarkdown(this string content)
        {
            var options = new MarkdownOptions
            {
                AutoHyperlink = true,
                AutoNewLines = true,
                EncodeProblemUrlCharacters = true,
                LinkEmails = true
            };


            var md = new Markdown(options);
            return md.Transform(content);

        }
    }
}