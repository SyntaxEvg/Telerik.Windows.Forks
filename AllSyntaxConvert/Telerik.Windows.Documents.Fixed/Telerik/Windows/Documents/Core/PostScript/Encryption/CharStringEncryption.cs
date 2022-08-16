using System;

namespace Telerik.Windows.Documents.Core.PostScript.Encryption
{
	class CharStringEncryption : EncryptionBase
	{
		public CharStringEncryption(PostScriptReader reader)
			: base(reader, 4330, 4)
		{
		}

		public override void Initialize()
		{
			for (int i = 0; i < base.RandomBytesCount; i++)
			{
				base.Reader.Read();
			}
		}
	}
}
