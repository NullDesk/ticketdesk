using System;
using System.Collections.ObjectModel;
#pragma warning disable 1591
namespace S22.Mail {
	[Serializable]
	public class SerializableAttachmentCollection : Collection<SerializableAttachment>, IDisposable {
		public void Dispose() { }
	}
}
#pragma warning restore 1591
