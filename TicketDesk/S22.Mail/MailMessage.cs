using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace S22.Imap {
	/// <summary>
	/// Adds extension methods to the MailMessage class of the .Net Framework.
	/// These are only used internally and are not visible outside of the
	/// S22.Imap assembly so as to not interfere with other assemblies.
	/// </summary>
	internal static class MailMessageExtension {
		/// <summary>
		/// Constructs a textual representation of a mail message from the specified
		/// MailMessage instance compliant with the RFC822 and MIME standards.
		/// </summary>
		/// <param name="message">The MailMessage instance to construct the
		/// textual representation from.</param>
		/// <returns>An RFC822/MIME-compliant string describing a mail
		/// message.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the From
		/// property is null or is not properly initialized.</exception>
		internal static string ToMIME822(this MailMessage message) {
			NameValueCollection header = BuildHeader(message);
			StringBuilder builder = new StringBuilder();

			foreach (string h in header)
				builder.AppendLine(h + ": " + header[h]);
			// The mail body is separated by an empty line from the header
			builder.AppendLine();
			builder.Append(BuildBody(message, header));
			builder.AppendLine();

			return builder.ToString();
		}

		/// <summary>
		/// Builds a RFC822/MIME-compliant mail header from the specified
		/// MailMessage instance and returns it as a NameValueCollection.
		/// </summary>
		/// <param name="m">The MailMessage instance to build the header
		/// from.</param>
		/// <returns>A NameValueCollection representing the RFC822/MIME
		/// mail header fields.</returns>
		static NameValueCollection BuildHeader(MailMessage m) {
			string[] ignore = new string[] {
				"MIME-Version", "Date", "Subject", "From", "To", "Cc", "Bcc",
				"Content-Type", "Content-Transfer-Encoding", "Priority",
				"Reply-To", "X-Priority", "Importance", "Sender", "Message-Id"
			};
			NameValueCollection header = new NameValueCollection() {
				{ "MIME-Version", "1.0" },
				{ "Date", DateTime.Now.ToString("R") },
				{ "Priority", PriorityMap[m.Priority] },
				{ "Importance", ImportanceMap[m.Priority] }
			};
			if (m.From == null)
				throw new InvalidOperationException("The From property must not be null");
			header.Add("From", m.From.To822Address());
			if (m.Subject != null)
				header.Add("Subject", m.Subject.IsASCII() ? m.Subject : QEncode(m.Subject));
			foreach (MailAddress a in m.To)
				header.Add("To", a.To822Address());
			foreach (MailAddress a in m.CC)
				header.Add("Cc", a.To822Address());
			foreach (MailAddress a in m.Bcc)
				header.Add("Bcc", a.To822Address());
			bool multipart = m.AlternateViews.Count > 0 || m.Attachments.Count > 0;
			if (!multipart) {
				string contentType = m.IsBodyHtml ? "text/html" : "text/plain";
				if (m.BodyEncoding != null)
					contentType = contentType + "; charset=" + m.BodyEncoding.WebName;
				header.Add("Content-Type", contentType);
				if (m.Body != null && !m.Body.IsASCII())
					header.Add("Content-Transfer-Encoding", "base64");
			} else {
				string contentType = (m.Attachments.Count == 0 ?
					"multipart/alternative" : "multipart/mixed") + "; boundary=" +
					GenerateContentBoundary();
				header.Add("Content-Type", contentType);
			}
			// Add any custom headers added by user
			foreach (string key in m.Headers) {
				if (ignore.Contains(key, StringComparer.OrdinalIgnoreCase))
					continue;
				header.Add(key, m.Headers.GetValues(key)[0]);
			}
			return header;
		}

		/// <summary>
		/// A map for mapping the values of the MailPriority enumeration to
		/// their corresponding MIME priority values as defined in
		/// RFC2156.
		/// </summary>
		static Dictionary<MailPriority, string> PriorityMap =
			new Dictionary<MailPriority, string>() {
				{ MailPriority.Low, "non-urgent" },
				{ MailPriority.Normal, "normal" },
				{ MailPriority.High, "urgent" }
			};

		/// <summary>
		/// A map for mapping the values of the MailPriority enumeration to
		/// their corresponding MIME importance values as defined in
		/// RFC2156.
		/// </summary>
		static Dictionary<MailPriority, string> ImportanceMap =
			new Dictionary<MailPriority, string>() {
				{ MailPriority.Low, "low" },
				{ MailPriority.Normal, "normal" },
				{ MailPriority.High, "high" }
			};

		/// <summary>
		/// Takes a unicode string and encodes it using Q-encoding.
		/// </summary>
		/// <param name="s">The string to encode</param>
		/// <returns>The input string encoded as Q-encoded string containing
		/// only ASCII characters.</returns>
		static string QEncode(string s) {
			StringBuilder builder = new StringBuilder("=?UTF-8?Q?");
			char[] chars = Encoding.Unicode.GetChars(Encoding.Unicode.GetBytes(s));
			foreach (char c in chars) {
				if (c < 32 || c == '=' || c == '_' || c == '?' || c > 126)
					builder.Append(string.Format("={0:X2}", (int)c));
				else if (c == ' ')
					builder.Append('_');
				else
					builder.Append(c);
			}
			return builder.ToString() + "?=";
		}

		/// <summary>
		/// Creates an address string from the specified MailAddress instance in
		/// compliance with the address specification as outlined in RFC2822 under
		/// section 3.4
		/// </summary>
		/// <param name="address">The MailAddress instance to create the address
		/// string from.</param>
		/// <returns>An address string as is used in RFC822 mail headers</returns>
		static string To822Address(this MailAddress address) {
			if (!String.IsNullOrEmpty(address.DisplayName)) {
				string name = address.DisplayName.IsASCII() ?
					address.DisplayName : QEncode(address.DisplayName);
				return name + " <" + address.Address + ">";
			}
			return address.Address;
		}

		/// <summary>
		/// Generates a unique sequence of characters for indicating a boundary
		/// between parts in a multipart message.
		/// </summary>
		/// <returns>A unique content boundary string</returns>
		static string GenerateContentBoundary() {
			return Guid.NewGuid().ToString("N");
		}

		/// <summary>
		/// Builds an RFC822/MIME-compliant mail body from the specified
		/// MailMessage instance and returns it as a formatted string.
		/// </summary>
		/// <param name="m">The MailMessage instance to build the mail body
		/// from.</param>
		/// <param name="header">The RFC822/MIME mail header to use for
		/// constructing the mail body.</param>
		/// <returns>An RFC822/MIME-compliant mail body as a string.
		/// </returns>
		/// <remarks>According to RFC2822 each line of a mail message should
		/// at max be 78 characters in length excluding carriage return and
		/// newline characters. This method accounts for that and ensures
		/// line breaks are inserted to meet this requirement.</remarks>
		static string BuildBody(MailMessage m, NameValueCollection header) {
			StringBuilder builder = new StringBuilder();
			bool multipart = header["Content-Type"].Contains("boundary");
			// Just a regular RFC822 mail w/o any MIME parts
			if (!multipart) {
				AddBody(builder, m, header);
				return builder.ToString();
			}
			Match match = Regex.Match(header["Content-Type"], @"boundary=(\w+)");
			string boundary = match.Groups[1].Value;
			// Start boundary
			builder.AppendLine("--" + boundary);
			bool nestParts = m.AlternateViews.Count > 0 && m.Attachments.Count > 0;
			if (nestParts) {
				AddNestedAlternative(builder, m, header);
				builder.AppendLine("--" + boundary);
				AddNestedMixed(builder, m);
			} else {
				AddBody(builder, m, header, true);
				foreach (AlternateView v in m.AlternateViews) {
					builder.AppendLine("--" + boundary);
					AddAttachment(builder, v);
				}
				foreach (Attachment a in m.Attachments) {
					builder.AppendLine("--" + boundary);
					AddAttachment(builder, a);
				}
			}
			// End boundary
			builder.AppendLine("--" + boundary + "--");
			return builder.ToString();
		}

		/// <summary>
		/// Adds a body part to the specified Stringbuilder object composed from
		/// the Body and BodyEncoding properties of the MailMessage class.
		/// </summary>
		/// <param name="builder">The Stringbuilder to append the body part to.</param>
		/// <param name="m">The MailMessage instance to build the body part from.</param>
		/// <param name="header">The RFC822/MIME mail header to use for
		/// constructing the mail body.</param>
		/// <param name="addHeaders">Set to true to append body headers before
		/// adding the actual body part content.</param>
		static void AddBody(StringBuilder builder, MailMessage m,
			NameValueCollection header, bool addHeaders = false) {
			bool base64 = header["Content-Transfer-Encoding"] == "base64";
			if (addHeaders) {
				string contentType = m.IsBodyHtml ? "text/html" : "text/plain";
				if (m.BodyEncoding != null)
					contentType = contentType + "; charset=" + m.BodyEncoding.WebName;
				builder.AppendLine("Content-Type: " + contentType);
				if (m.Body != null && !m.Body.IsASCII()) {
					builder.AppendLine("Content-Transfer-Encoding: base64");
					base64 = true;
				}
				builder.AppendLine();
			}
			string body = m.Body;
			if (base64) {
				byte[] bytes = m.BodyEncoding.GetBytes(m.Body);
				body = Convert.ToBase64String(bytes);
			}
			StringReader reader = new StringReader(body);
			char[] line = new char[76];
			int read;
			while ((read = reader.Read(line, 0, line.Length)) > 0)
				builder.AppendLine(new string(line, 0, read));
		}

		/// <summary>
		/// Creates a MIME body part from an entry of the AlternateView or
		/// Attachments collection of a MailMessage instance and appends it
		/// to the specified Stringbuilder instance.
		/// </summary>
		/// <param name="builder">The Stringbuilder instance to append the
		/// body part to.</param>
		/// <param name="view">An entry from either the AlternateView or the
		/// Attachments collection of a MailMessage instance.</param>
		static void AddAttachment(StringBuilder builder, AttachmentBase view) {
			// Append the MIME headers for this body part
			string contentType = "Content-Type: " + view.ContentType.MediaType;
			foreach (string key in view.ContentType.Parameters.Keys) {
				contentType = contentType + "; " + key + "=" +
					view.ContentType.Parameters[key];
			}
			builder.AppendLine(contentType);
			builder.AppendLine("Content-Transfer-Encoding: base64");
			if (!String.IsNullOrEmpty(view.ContentId))
				builder.AppendLine("Content-Id: <" + view.ContentId + ">");
			if (view is Attachment)
				builder.AppendLine("Content-Disposition: attachment");
			builder.AppendLine();
			// Append the actual body part contents encoded as Base64
			using (MemoryStream memstream = new MemoryStream()) {
				int bytesRead;
				byte[] buffer = new byte[4096];
				while ((bytesRead =
					view.ContentStream.Read(buffer, 0, buffer.Length)) > 0) {
					memstream.Write(buffer, 0, bytesRead);
				}
				string str = Convert.ToBase64String(memstream.ToArray());
				StringReader reader = new StringReader(str);
				char[] line = new char[76];
				int read;
				while ((read = reader.Read(line, 0, line.Length)) > 0)
					builder.AppendLine(new string(line, 0, read));
			}
			// Rewind the stream if it supports seeking
			if (view.ContentStream.CanSeek)
				view.ContentStream.Seek(0, SeekOrigin.Begin);
		}

		/// <summary>
		/// Creates a nested multipart/alternative part which contains all entries
		/// from the AlternateViews collection of the specified MailMessage instance
		/// as well as the body part for the Body and BodyEncoding properties of the
		/// specified MailMessage instance.
		/// </summary>
		/// <param name="builder">The StringBuilder instance to append to.</param>
		/// <param name="m">The MailMessage instance whose AlternateView collection
		/// will be added to the nested multipart/alternative part.</param>
		/// <param name="header">The RFC822/MIME mail header to use for
		/// constructing the mail body.</param>
		/// <remarks>This is used if the MailMessage instance contains both alternative
		/// views and attachments. In this case the created RFC822/MIME mail message will
		/// contain nested body parts.</remarks>
		static void AddNestedAlternative(StringBuilder builder, MailMessage m,
			NameValueCollection header) {
			string boundary = GenerateContentBoundary();
			builder.AppendLine("Content-Type: multipart/alternative; boundary=" + boundary);
			builder.AppendLine();
			// Add the body parts to the nested multipart/alternative part
			builder.AppendLine("--" + boundary);
			AddBody(builder, m, header, true);
			foreach (AlternateView v in m.AlternateViews) {
				builder.AppendLine("--" + boundary);
				AddAttachment(builder, v);
			}
			builder.AppendLine("--" + boundary + "--");
		}

		/// <summary>
		/// Creates a nested multipart/mixed part which contains all entries
		/// from the Attachments collection of the specified MailMessage instance.
		/// </summary>
		/// <param name="builder">The StringBuilder instance to append to.</param>
		/// <param name="m">The MailMessage instance whose Attachments collection
		/// will be added to the nested multipart/mixed part.</param>
		/// <remarks>This is used if the MailMessage instance contains both alternative
		/// views and attachments. In this case the created RFC822/MIME mail message will
		/// contain nested body parts.</remarks>
		static void AddNestedMixed(StringBuilder builder, MailMessage m) {
			string boundary = GenerateContentBoundary();
			builder.AppendLine("Content-Type: multipart/mixed; boundary=" + boundary);
			builder.AppendLine();
			// Add the body parts to the nested multipart/mixed part
			foreach (Attachment a in m.Attachments) {
				builder.AppendLine("--" + boundary);
				AddAttachment(builder, a);
			}
			builder.AppendLine("--" + boundary + "--");
		}
	}
}
