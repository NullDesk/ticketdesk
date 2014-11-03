// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Text;

namespace TicketDesk.Web.Client.Helpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString DisplayNameFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression
        )
        {
            var metaData = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string value = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));
            return MvcHtmlString.Create(value);
        }

        public static MvcHtmlString DisplayPaddedName
        (
            this HtmlHelper htmlHelper,
            string value,
            int numberOfCharacters
        )
        {
            return MvcHtmlString.Create(value.PadLeft(numberOfCharacters));
        }

        public static MvcHtmlString DisplayPaddedNameFor<TModel, TProperty>
        (
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            int numberOfCharacters
        )
        {
            var metaData = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string value = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));
            return MvcHtmlString.Create(value.PadLeft(numberOfCharacters));
        }


        public static MvcHtmlString DisplayLimitedValue
        (
             this HtmlHelper htmlHelper,
             string value,
             int numberOfCharacters
        )
        {
            string ret = value;
            if (string.IsNullOrEmpty(value))
            {
                ret = string.Empty;
            }
            if (value != null && value.Length > numberOfCharacters)
            {
                ret = string.Concat(value.Take(numberOfCharacters - 6)) + "[...]";
            }
            return MvcHtmlString.Create(ret);
        }

        public static MvcHtmlString DisplayLimitedValueFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            int numberOfCharacters
        )
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string fieldText = metadata.SimpleDisplayText ?? metadata.NullDisplayText ?? string.Empty;
            return DisplayLimitedValue(htmlHelper, fieldText, numberOfCharacters);
        }



        public static string WordWrapText(this HtmlHelper htmlHelper, string text, int width)
        {
            string _newline = "\n";
            int pos, next;
            StringBuilder sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return text;

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(_newline, pos);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + _newline.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > width)
                            len = BreakLine(text, pos, width);

                        sb.Append(text, pos, len);
                        sb.Append(_newline);
                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    } while (eol > pos);
                }
                else sb.Append(_newline); // Empty line
            }
            return sb.ToString();
        }

        public static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max - 1;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;
            if (i < 0)
                return max; // No whitespace found; break at maximum length

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }


    }
}