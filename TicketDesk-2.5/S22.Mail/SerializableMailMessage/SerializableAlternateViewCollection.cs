using System;
using System.Collections.ObjectModel;

namespace S22.Mail {
	[Serializable]
	public class SerializableAlternateViewCollection : Collection<SerializableAlternateView>, IDisposable {
		public void Dispose() { }
	}
}
