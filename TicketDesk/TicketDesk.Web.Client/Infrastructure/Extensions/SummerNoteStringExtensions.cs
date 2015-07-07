using System.Text.RegularExpressions;

namespace TicketDesk.Web.Client
{
    public static class SummerNoteStringExtensions
    {
        public static string StripHtmlWhenEmpty(this string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var ctext = Regex.Replace(content, "<[^>]*>", "").ToLowerInvariant();
                var hasText = ctext.Replace("\n", " ").Replace("&nbsp;", " ").Trim().Length > 0;
                if (!hasText)
                {
                    content = null;
                }
            }
            return content;
        }
    }
}