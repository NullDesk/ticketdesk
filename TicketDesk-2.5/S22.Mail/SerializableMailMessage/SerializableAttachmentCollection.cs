using System;
using System.Collections.ObjectModel;

namespace S22.Mail {
	[Serializable]
	public class SerializableAttachmentCollection : Collection<SerializableAttachment>, IDisposable {
		public void Dispose() { }
	}
}
