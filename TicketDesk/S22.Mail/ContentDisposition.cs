using System;
using System.Collections.Generic;

namespace S22.Imap {
	/// <summary>
	/// Represents the content disposition as is presented in the BODYSTRUCTURE
	/// response by the IMAP server.
	/// </summary>
	[Serializable]
	public class ContentDisposition {
		/// <summary>
		/// Initializes a new instance of the ContentDisposition class with
		/// default values.
		/// </summary>
		public ContentDisposition() {
			Attributes = new Dictionary<string, string>();
			Type = ContentDispositionType.Unknown;
			Filename = "";
		}

		/// <summary>
		/// The content disposition specifies the presentation style.
		/// </summary>
		public ContentDispositionType Type { get; set; }

		/// <summary>
		/// Additional attribute fields for specifying the name of a file, the creation
		/// date and modification date, which can be used by the reader's mail user agent
		/// to store the attachment.
		/// </summary>
		public Dictionary<string, string> Attributes { get; set; }

		/// <summary>
		/// Contains the name of the transmitted file if the content-disposition is of type
		/// "Attachment" and if the name value was provided as part of the header information.
		/// This field may be empty.
		/// </summary>
		public string Filename { get; set; }

		/// <summary>
		/// Maps MIME content disposition string values to their corresponding
		/// counter-parts of the ContentDispositionType enumeration.
		/// </summary>
		static internal Dictionary<string, ContentDispositionType> Map =
			new Dictionary<string, ContentDispositionType>(StringComparer.OrdinalIgnoreCase) {
			{ "Inline", ContentDispositionType.Inline },
			{ "Attachment",	ContentDispositionType.Attachment }
		};
	}

	/// <summary>
	/// Possible values for the content disposition type which determines the presentation
	/// style
	/// </summary>
	public enum ContentDispositionType {
		/// <summary>
		/// The content disposition could not be determined.
		/// </summary>
		Unknown,
		/// <summary>
		/// An inline content disposition means that the content should be automatically
		/// displayed when the message is displayed.
		/// </summary>
		Inline,
		/// <summary>
		/// An attachment content disposition means that the content should not be displayed
		/// automatically and requires some form of action from the user to open it.
		/// </summary>
		Attachment
	}

	internal static class ContentDispositionTypeMap {
		public static ContentDispositionType fromString(string disposition) {
			Dictionary<string, ContentDispositionType> Map =
			new Dictionary<string, ContentDispositionType>
				(StringComparer.OrdinalIgnoreCase) {
				{ "Inline", ContentDispositionType.Inline },
				{ "Attachment",	ContentDispositionType.Attachment }
			};
			try {
				return Map[disposition];
			} catch {
				return ContentDispositionType.Unknown;
			}
		}
	}
}
