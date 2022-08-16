using System;

namespace Telerik.Windows.Documents.Core.PostScript.Encryption
{
	class EExecEncryption : EncryptionBase
	{
		public EExecEncryption(PostScriptReader reader)
			: base(reader, 55665, 4)
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
