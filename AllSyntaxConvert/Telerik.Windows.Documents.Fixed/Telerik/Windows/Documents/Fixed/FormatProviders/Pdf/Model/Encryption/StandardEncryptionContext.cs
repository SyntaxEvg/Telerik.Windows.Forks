using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption
{
	struct StandardEncryptionContext
	{
		public int ObjectNumber;

		public int GenerationNumber;

		public byte[] InputData;

		public byte[] EncryptionKey;

		public bool IsIdentityEncryption;

		public CryptFilterMethod CryptFilterMethod;
	}
}
