using System;
using System.Collections.Generic;
using System.Text;

namespace S22.Imap {
	/// <summary>
	/// Represents a MIME body part of a mail message that has multiple
	/// parts.
	/// </summary>
	[Serializable]
	public class Bodypart {
		/// <summary>
		/// Initializes a new instance of the Bodypart class with default
		/// values.
		/// </summary>
		/// <param name="partNumber">The part number as is expected by the
		/// IMAP FETCH command.</param>
		internal Bodypart(string partNumber) {
			// Initialize all fields with default values
			PartNumber = partNumber;
			Type = ContentType.Other;
			Subtype = Id = Description = Md5 = Language = Location = "";
			Parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			Disposition = new ContentDisposition();
			Encoding = ContentTransferEncoding.Unknown;
			Size = Lines = 0;
		}
		/// <summary>
		/// The body part number which acts as part specifier for
		/// the FETCH BODY command.
		/// </summary>
		internal string PartNumber { get; set; }
		/// <summary>
		/// The MIME content-type of this body part. The content-type is
		/// used to declare the general type of data.
		/// </summary>
		public ContentType Type { get; set; }
		/// <summary>
		/// The MIME content-subtype of this body part. The subtype
		/// specifies a specific format for the type of data.
		/// </summary>
		public string Subtype { get; set; }
		/// <summary>
		/// Parameter values present in the MIME content-type header
		/// of this body part (for instance, 'charset').
		/// </summary>
		public Dictionary<string, string> Parameters { get; set; }
		/// <summary>
		/// The MIME content-id of this body part, if any. This value
		/// may be used for uniquely identifying MIME entities in
		/// several contexts.
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// The MIME content-description of this body part. This value
		/// may contain some descriptive information on the body part.
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// The MIME content-transfer-encoding mechanism used for
		/// encoding this body part's data.
		/// </summary>
		public ContentTransferEncoding Encoding { get; set; }
		/// <summary>
		/// The size of this body part in bytes. Note that this size
		/// is the size in its transfer encoding and not the resulting
		/// size after any decoding.
		/// </summary>
		public Int64 Size { get; set; }
		/// <summary>
		/// The size of the body in text lines. This field is only
		/// present in body parts with a content-type of text.
		/// </summary>
		public Int64 Lines { get; set; }
		/// <summary>
		/// The computed MD5-Hash of the body part. This field is not
		/// mandatory and may be empty.
		/// </summary>
		public string Md5 { get; set; }
		/// <summary>
		/// The MIME content-disposition for this body part. This field
		/// is not mandatory and may be empty.
		/// </summary>
		public ContentDisposition Disposition { get; set; }
		/// <summary>
		/// A string giving the body language. This field is not mandatory
		/// and may be empty.
		/// </summary>
		public string Language { get; set; }
		/// <summary>
		/// A string list giving the body content URI. This field is not
		/// mandatory and may be empty.
		/// </summary>
		public string Location { get; set; }
		/// <summary>
		/// Returns a detailed description listing all properties of this
		/// Bodypart instance.
		/// </summary>
		/// <returns>A string describing this instance of the Bodypart class</returns>
		public override string ToString() {
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("Part Number: " + PartNumber);
			builder.AppendLine("Type: " + Type);
			builder.AppendLine("Subtype: " + Subtype);
			foreach (string key in Parameters.Keys)
				builder.AppendLine("Parameters[" + key + "]: " + Parameters[key]);
			builder.AppendLine("Id: " + Id);
			builder.AppendLine("Description: " + Description);
			builder.AppendLine("Encoding: " + Encoding);
			builder.AppendLine("Size: " + Size);
			builder.AppendLine("Lines: " + Lines);
			builder.AppendLine("Md5: " + Md5);
			builder.AppendLine("Disposition: " + Disposition.Type);
			foreach (string key in Disposition.Attributes.Keys)
				builder.AppendLine("Disposition[" + key + "]: " + Disposition.Attributes[key]);
			builder.AppendLine("Language: " + Language);
			builder.AppendLine("Location: " + Location);
			return builder.ToString();
		}
	}
}
