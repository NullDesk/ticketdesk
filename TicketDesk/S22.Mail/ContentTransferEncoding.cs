using System;
using System.Collections.Generic;

namespace S22.Imap {
	/// <summary>
	/// Possible values for the "Encoding" property of the Bodypart class. The content
	/// transfer encoding indicates whether or not a binary-to-text encoding
	/// scheme has been used on top of the original encoding as specified within the
	/// Content-Type header.
	/// </summary>
	public enum ContentTransferEncoding {
		/// <summary>
		/// The content tranfer encoding could not be determined or is unknown.
		/// </summary>
		Unknown,
		/// <summary>
		/// Up to 998 bytes per line of the code range 1 - 127 with CR and LF only
		/// allowed to appear as part of a CRLF line ending.
		/// </summary>
		Bit7,
		/// <summary>
		/// Up to 998 bytes per line with CR and LF only allowed to appear as part
		/// of a CRLF line ending.
		/// </summary>
		Bit8,
		/// <summary>
		/// Any sequence of bytes.
		/// </summary>
		Binary,
		/// <summary>
		/// Byte sequence is encoded using the quoted-printable encoding.
		/// </summary>
		QuotedPrintable,
		/// <summary>
		/// Byte sequence is encoded using Base64 encoding.
		/// </summary>
		Base64
	}

	internal static class ContentTransferEncodingMap {
		public static ContentTransferEncoding fromString(string transferEncoding) {
			Dictionary<string, ContentTransferEncoding> Map =
			new Dictionary<string, ContentTransferEncoding>
				(StringComparer.OrdinalIgnoreCase) {
				{ "7Bit", ContentTransferEncoding.Bit7 },
				{ "8Bit",	ContentTransferEncoding.Bit8 },
				{ "Binary",	ContentTransferEncoding.Binary },
				{ "Quoted-Printable", ContentTransferEncoding.QuotedPrintable },
				{ "Base64", ContentTransferEncoding.Base64 }
			};
			try {
				return Map[transferEncoding];
			} catch {
				return ContentTransferEncoding.Unknown;
			}
		}
	}
}
