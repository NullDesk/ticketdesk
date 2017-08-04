using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Text;
#pragma warning disable 1591

namespace S22.Mail {
	/// <summary>
	/// A serializable replication of the MailMessage class of the
	/// System.Net.Mail namespace. It implements conversion operators to allow for
	/// implicit conversion between SerializableMailMessage and MailMessage objects.
	/// </summary>
	[Serializable]
	public class SerializableMailMessage {
		public static implicit operator MailMessage(SerializableMailMessage message) {
			MailMessage m = new MailMessage();
			foreach (SerializableAlternateView a in message.AlternateViews)
				m.AlternateViews.Add(a);
			foreach (SerializableAttachment a in message.Attachments)
				m.Attachments.Add(a);
			foreach (SerializableMailAddress a in message.Bcc)
				m.Bcc.Add(a);
			m.Body = message.Body;
			m.BodyEncoding = message.BodyEncoding;
			foreach (SerializableMailAddress a in message.CC)
				m.CC.Add(a);
			m.DeliveryNotificationOptions = message.DeliveryNotificationOptions;
			m.From = message.From ?? new MailAddress("fake@example.com");//because conversion fails if this is null
			m.Headers.Add(message.Headers);
			m.HeadersEncoding = message.HeadersEncoding;
			m.IsBodyHtml = message.IsBodyHtml;
			m.Priority = message.Priority;
			m.ReplyTo = message.ReplyTo;
			foreach (SerializableMailAddress a in message.ReplyToList)
				m.ReplyToList.Add(a);
			m.Sender = message.Sender;
			m.Subject = message.Subject;
			m.SubjectEncoding = message.SubjectEncoding;
			foreach (SerializableMailAddress a in message.To)
				m.To.Add(a);
			return m;
		}

		public static implicit operator SerializableMailMessage(MailMessage message) {
			return new SerializableMailMessage(message);
		}

		private SerializableMailMessage(MailMessage m) {
			AlternateViews = new SerializableAlternateViewCollection();
			foreach (AlternateView a in m.AlternateViews)
				AlternateViews.Add(a);
			Attachments = new SerializableAttachmentCollection();
			foreach (Attachment a in m.Attachments)
				Attachments.Add(a);
			Bcc = new SerializableMailAddressCollection();
			foreach (MailAddress a in m.Bcc)
				Bcc.Add(a);
			Body = m.Body;
			BodyEncoding = m.BodyEncoding;
			CC = new SerializableMailAddressCollection();
			foreach (MailAddress a in m.CC)
				CC.Add(a);
			DeliveryNotificationOptions = m.DeliveryNotificationOptions;
			From = m.From;
			Headers = new NameValueCollection();
			Headers.Add(m.Headers);
			HeadersEncoding = m.HeadersEncoding;
			IsBodyHtml = m.IsBodyHtml;
			Priority = m.Priority;
			ReplyTo = m.ReplyTo;
			ReplyToList = new SerializableMailAddressCollection();
			foreach (MailAddress a in m.ReplyToList)
				ReplyToList.Add(a);
			Sender = m.Sender;
			Subject = m.Subject;
			SubjectEncoding = m.SubjectEncoding;
			To = new SerializableMailAddressCollection();
			foreach (MailAddress a in m.To)
				To.Add(a);
		}

		public SerializableAlternateViewCollection AlternateViews { get; private set; }
		public SerializableAttachmentCollection Attachments { get; private set; }
		public SerializableMailAddressCollection Bcc { get; private set; }
		public string Body { get; set; }
		public Encoding BodyEncoding { get; set; }
		public SerializableMailAddressCollection CC { get; private set; }
		public DeliveryNotificationOptions DeliveryNotificationOptions { get; set; }
		public SerializableMailAddress From { get; set; }
		public NameValueCollection Headers { get; private set; }
		public Encoding HeadersEncoding { get; set; }
		public bool IsBodyHtml { get; set; }
		public MailPriority Priority { get; set; }
		public SerializableMailAddress ReplyTo { get; set; }
		public SerializableMailAddressCollection ReplyToList { get; private set; }
		public SerializableMailAddress Sender { get; set; }
		public string Subject { get; set; }
		public Encoding SubjectEncoding { get; set; }
		public SerializableMailAddressCollection To { get; private set; }
	}
}
#pragma warning restore 1591
