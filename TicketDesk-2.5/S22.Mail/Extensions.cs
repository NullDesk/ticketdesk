using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using S22.Imap;

namespace S22.Mail {
	/// <summary>
	/// Contains extension methods for the MailMessage class of the System.Net.Mail
	/// namespace.
	/// </summary>
	public static class MailExtension {
		/// <summary>
		/// Saves the MailMessage instance to the specified stream.
		/// </summary>
		/// <param name="m">Extension method for MailMessage class.</param>
		/// <param name="stream">The stream the MailMessage data will be
		/// written to.</param>
		/// <remarks>The mail message is saved as plain text in MIME
		/// format (known as .eml in many email clients)</remarks>
		public static void Save(this MailMessage m, Stream stream) {
			byte[] bytes = Encoding.ASCII.GetBytes(m.ToMIME822());
			stream.Write(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// Saves the MailMessage instance to the specified file.
		/// </summary>
		/// <param name="m">Extension method for MailMessage class</param>
		/// <param name="name">A relative or absolute path for the file the
		/// MailMessage will be saved to.</param>
		/// <remarks>The mail message is saved as plain text in MIME
		/// format (known as .eml in many email clients)</remarks> 
		public static void Save(this MailMessage m, string name) {
			using (FileStream s = new FileStream(name, FileMode.Create))
				m.Save(s);
		}

		/// <summary>
		/// Creates a MailMessage instance from the specified stream.
		/// </summary>
		/// <param name="stream">The stream the MailMessage will be constructed
		/// from.</param>
		/// <returns>An initialized MailMessage object</returns>
		public static MailMessage Load(Stream stream) {
			using (StreamReader r = new StreamReader(stream)) {
				return S22.Imap.MessageBuilder.FromMIME822(r.ReadToEnd());
			}
		}

		/// <summary>
		/// Creates a MailMessage instance from the specified file (*.eml).
		/// </summary>
		/// <param name="name">A relative or absolute path for the file the
		/// MailMessage will be saved to.</param>
		/// <returns>An initialized MailMessage object</returns>
		public static MailMessage Load(string name) {
			using (FileStream s = new FileStream(name, FileMode.Open))
				return Load(s);
		}

		/// <summary>
		/// Saves the contents of the attachment to the specified file.
		/// </summary>
		/// <param name="attachment">Extension method for Attachment class</param>
		/// <param name="name">The file to save the attachment to.</param>
		/// <exception cref="IOException">Thrown when an I/O error occurs during
		/// the save operation. Use the InnerException property to obtain the exception that
		/// led to the current exception.</exception>
		/// <remarks>If the file does not exist, it will be created. If the file already
		/// exists, it will be truncated and overwritten.</remarks>
		public static void SaveAs(this Attachment attachment, string name) {
			int count;
			byte[] buffer = new byte[4096];
			Stream stream = attachment.ContentStream;
			try {
				using (FileStream fs = new FileStream(name, FileMode.Create)) {
					while ((count = stream.Read(buffer, 0, buffer.Length)) != 0)
						fs.Write(buffer, 0, count);
				}
			} catch (Exception e) {
				throw new IOException(e.Message, e);
			} finally {
				if (stream.CanSeek)
					stream.Seek(0, SeekOrigin.Begin);
			}
		}
	}
}
