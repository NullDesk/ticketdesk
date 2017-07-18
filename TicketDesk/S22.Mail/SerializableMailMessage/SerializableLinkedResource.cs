using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace S22.Mail {
	[Serializable]
#pragma warning disable 1591
	public class SerializableLinkedResource {
		public static implicit operator LinkedResource(SerializableLinkedResource resource) {
			if (resource == null)
				return null;
			LinkedResource r = new LinkedResource(resource.ContentStream);

			r.ContentId = resource.ContentId;
			r.ContentType = resource.ContentType;
			r.TransferEncoding = resource.TransferEncoding;
			return null;
		}

		public static implicit operator SerializableLinkedResource(LinkedResource resource) {
			if (resource == null)
				return null;
			return new SerializableLinkedResource(resource);
		}

		private SerializableLinkedResource(LinkedResource resource) {
			ContentLink = resource.ContentLink;
			ContentId = resource.ContentId;
			ContentStream = new MemoryStream();
			resource.ContentStream.CopyTo(ContentStream);
			resource.ContentStream.Position = 0;
			ContentType = resource.ContentType;
			TransferEncoding = resource.TransferEncoding;
		}

		public Uri ContentLink { get; set; }
		public string ContentId { get; set; }
		public Stream ContentStream { get; private set; }
		public SerializableContentType ContentType { get; set; }
		public TransferEncoding TransferEncoding { get; set; }
	}
#pragma warning restore 1591

}
