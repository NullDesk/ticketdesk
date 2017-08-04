using System;
using System.Net.Mail;
#pragma warning disable 1591

namespace S22.Mail {
	[Serializable]
	public class SerializableMailAddress {

        public static implicit operator MailAddress(SerializableMailAddress address) {
			if (address == null)
				return null;
			return new MailAddress(address.Address, address.DisplayName);
		}

		public static implicit operator SerializableMailAddress(MailAddress address) {
			if (address == null)
				return null;
			return new SerializableMailAddress(address);
		}

		private SerializableMailAddress(MailAddress address) {
			Address = address.Address;
			DisplayName = address.DisplayName;
		}

		public string Address { get; private set; }
		public string DisplayName { get; private set; }
	}
}
#pragma warning restore 1591

