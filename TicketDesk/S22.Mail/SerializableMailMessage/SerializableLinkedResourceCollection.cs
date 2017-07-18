using System;
using System.Collections.ObjectModel;

namespace S22.Mail {
	[Serializable]
#pragma warning disable 1591
	public class SerializableLinkedResourceCollection : Collection<SerializableLinkedResource>, IDisposable {
		public void Dispose() { }
	}
#pragma warning restore 1591

}
