using System;
using System.Collections.Specialized;
using System.Net.Mime;
#pragma warning disable 1591

namespace S22.Mail {
	[Serializable]
	public class SerializableContentDisposition {
		public static implicit operator ContentDisposition(SerializableContentDisposition disposition) {
			if (disposition == null)
				return null;
			ContentDisposition d = new ContentDisposition();

			d.CreationDate = disposition.CreationDate;
			d.DispositionType = disposition.DispositionType;
			d.FileName = disposition.FileName;
			d.Inline = disposition.Inline;
			d.ModificationDate = disposition.ModificationDate;
			foreach (string k in disposition.Parameters.Keys)
				d.Parameters.Add(k, disposition.Parameters[k]);
			d.ReadDate = disposition.ReadDate;
			d.Size = disposition.Size;
			return d;
		}

		public static implicit operator SerializableContentDisposition(ContentDisposition disposition) {
			if (disposition == null)
				return null;
			return new SerializableContentDisposition(disposition);
		}

		private SerializableContentDisposition(ContentDisposition disposition) {
			CreationDate = disposition.CreationDate;
			DispositionType = disposition.DispositionType;
			FileName = disposition.FileName;
			Inline = disposition.Inline;
			ModificationDate = disposition.ModificationDate;
			Parameters = new StringDictionary();
			foreach (string k in disposition.Parameters.Keys)
				Parameters.Add(k, disposition.Parameters[k]);
			ReadDate = disposition.ReadDate;
			Size = disposition.Size;
		}

		public DateTime CreationDate { get; set; }
		public string DispositionType { get; set; }
		public string FileName { get; set; }
		public bool Inline { get; set; }
		public DateTime ModificationDate { get; set; }
		public StringDictionary Parameters { get; private set; }
		public DateTime ReadDate { get; set; }
		public long Size { get; set; }
	}
}
#pragma warning restore 1591
