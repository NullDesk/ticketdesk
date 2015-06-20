using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace S22.Imap {
	/// <summary>
	/// A static utility class containing methods for decoding encoded
	/// non-ASCII data as is often used in mail messages as well as
	/// extension methods for some existing classes.
	/// </summary>
	internal static class Util {

		/// <summary>
		/// Returns true if the string contains only ASCII characters.
		/// </summary>
		/// <param name="s">Extension method for the String class.</param>
		/// <returns>Returns true if the string contains only ASCII characters,
		/// otherwise false is returned.</returns>
		internal static bool IsASCII(this string s) {
			return s.ToCharArray().All(c => c < 127);
		}

		/// <summary>
		/// Decodes a string composed of one or several MIME 'encoded-words'.
		/// </summary>
		/// <param name="words">A string to composed of one or several MIME
		/// 'encoded-words'</param>
		/// <exception cref="FormatException">Thrown when an unknown encoding
		/// (other than Q-Encoding or Base64) is encountered.</exception>
		/// <returns>A concatenation of all enconded-words in the passed
		/// string</returns>
		public static string DecodeWords(string words) {
			MatchCollection matches = Regex.Matches(words,
				@"(=\?[A-Za-z0-9\-]+\?[BbQq]\?[^\?]+\?=)");
			string decoded = String.Empty;
			foreach (Match m in matches)
				decoded = decoded + DecodeWord(m.ToString());
			return decoded;
		}

		/// <summary>
		/// Decodes a MIME 'encoded-word' string.
		/// </summary>
		/// <param name="word">The encoded word to decode</param>
		/// <exception cref="FormatException">Thrown when an unknown encoding
		/// (other than Q-Encoding or Base64) is encountered.</exception>
		/// <returns>A decoded string</returns>
		/// <remarks>MIME encoded-word syntax is a way to encode strings that
		/// contain non-ASCII data. Commonly used encodings for the encoded-word
		/// sytax are Q-Encoding and Base64. For an in-depth description, refer
		/// to RFC 2047</remarks>
		internal static string DecodeWord(string word) {
			Match m = Regex.Match(word,
					@"=\?([A-Za-z0-9\-]+)\?([BbQq])\?(.+)\?=");
			if (!m.Success)
				return word;
			Encoding encoding = Util.GetEncoding(m.Groups[1].Value);
			string type = m.Groups[2].Value.ToUpper();
			string text = m.Groups[3].Value;
			switch (type) {
				case "Q":
					return Util.QDecode(text, encoding);
				case "B":
					return encoding.GetString(Util.Base64Decode(text));
				default:
					throw new FormatException("Encoding not recognized " +
						"in encoded word: " + word);
			}
		}

		/// <summary>
		/// Takes a Q-encoded string and decodes it using the specified
		/// encoding.
		/// </summary>
		/// <param name="value">The Q-encoded string to decode</param>
		/// <param name="encoding">The encoding to use for encoding the
		/// returned string</param>
		/// <exception cref="FormatException">Thrown if the string is
		/// not a valid Q-encoded string.</exception>
		/// <returns>A Q-decoded string</returns>
		internal static string QDecode(string value, Encoding encoding) {
			try {
				using (MemoryStream m = new MemoryStream()) {
					for (int i = 0; i < value.Length; i++) {
						if (value[i] == '=') {
							string hex = value.Substring(i + 1, 2);
							m.WriteByte(Convert.ToByte(hex, 16));
							i = i + 2;
						} else if (value[i] == '_') {
							m.WriteByte(Convert.ToByte(' '));
						} else {
							m.WriteByte(Convert.ToByte(value[i]));
						}
					}
					return encoding.GetString(m.ToArray());
				}
			} catch {
				throw new FormatException("value is not a valid Q-encoded " +
					"string");
			}
		}

		/// <summary>
		/// Takes a quoted-printable encoded string and decodes it using
		/// the specified encoding.
		/// </summary>
		/// <param name="value">The quoted-printable-encoded string to
		/// decode</param>
		/// <param name="encoding">The encoding to use for encoding the
		/// returned string</param>
		/// <exception cref="FormatException">Thrown if the string is
		/// not a valid quoted-printable encoded string.</exception>
		/// <returns>A quoted-printable decoded string</returns>
		internal static string QPDecode(string value, Encoding encoding) {
			try {
				using (MemoryStream m = new MemoryStream()) {
					for (int i = 0; i < value.Length; i++) {
						if (value[i] == '=') {
							string hex = value.Substring(i + 1, 2);
							m.WriteByte(Convert.ToByte(hex, 16));
							i = i + 2;
						} else {
							m.WriteByte(Convert.ToByte(value[i]));
						}
					}
					return encoding.GetString(m.ToArray());
				}
			} catch {
				throw new FormatException("value is not a valid quoted-printable " +
					"encoded string");
			}
		}

		/// <summary>
		/// Takes a Base64-encoded string and decodes it.
		/// </summary>
		/// <param name="value">The Base64-encoded string to decode</param>
		/// <returns>A byte array containing the Base64-decoded bytes
		/// of the input string.</returns>
		internal static byte[] Base64Decode(string value) {
			return Convert.FromBase64String(value);
		}

		/// <summary>
		/// This just wraps Encoding.GetEncoding in a try-catch block to
		/// ensure it never fails. If the encoding can not be determined
		/// ASCII is returned as a default.
		/// </summary>
		/// <param name="name">The code page name of the preferred encoding.
		/// Any value returned by System.Text.Encoding.WebName is a valid
		/// input.</param>
		/// <returns>The System.Text.Encoding associated with the specified
		/// code page or Encoding.ASCII if the specified code page could not
		/// be resolved.</returns>
		internal static Encoding GetEncoding(string name) {
			Encoding encoding;
			try {
				encoding = Encoding.GetEncoding(name);
			} catch {
				encoding = Encoding.ASCII;
			}
			return encoding;
		}
	}
}
