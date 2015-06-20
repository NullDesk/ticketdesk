### Introduction

This repository contains a .NET assembly which adds a couple of extension methods to
the MailMessage class of the System.Net.Mail namespace. It also contains a serializable
replica of the MailMessage class.

### Usage & Examples

To use the library add the S22.Mail.dll assembly to your project references in Visual Studio.
The **SerializableMailMessage** class implements conversion operators to allow for implicit
conversion between SerializableMailMessage and MailMessage objects.

	using System;
	using System.IO;
	using System.Net.Mail;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Binary;
	using S22.Mail;

	namespace Test {
		class Program {
			static void Main() {
				MailMessage msg = MyMailMessage();

				IFormatter formatter = new BinaryFormatter();
				using(MemoryStream s = new MemoryStream()) {
					// Serialize MailMessage to memory stream
					formatter.Serialize(s, (SerializableMailMessage)message);

					// Rewind stream and deserialize MailMessage
					s.Seek(0, SeekOrigin.Begin);
					MailMessage Tmp = (SerializableMailMessage)formatter.Deserialize(s)

					Console.WriteLine(Tmp.Subject);
					Console.WriteLine(Tmp.Body);
				}
			}

			static MailMessage MyMailMessage() {
				MailMessage m = new MailMessage("John@Doe.com", "Jane@Doe.com");

				m.Subject = "Hello World";
				m.Body = "This is just a test";
				m.Attachments.Add(new Attachment("Test.cs""));

				return m;
			}
		}
	}

### Extension Methods

*System.Net.Mail.MailMessage*
- *Load(Stream stream)*
- *Load(string name)*
- *Save(Stream stream)*
- *Save(string name)*

*System.Net.Mail.Attachment*
- *SaveAs(string name)*


### Credits

This library is copyright © 2012 Torben Könke.


### License

This library is released under the [MIT license](https://github.com/smiley22/S22.Imap/blob/master/License.md).


### Bug reports

Please send your bug reports and questions to [smileytwentytwo@gmail.com](mailto:smileytwentytwo@gmail.com) or create a new
issue on the GitHub project homepage.
