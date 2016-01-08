// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

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