using System;
using System.Collections.ObjectModel;

namespace S22.Mail {
	[Serializable]
	public class SerializableLinkedResourceCollection : Collection<SerializableLinkedResource>, IDisposable {
		public void Dispose() { }
	}
}
