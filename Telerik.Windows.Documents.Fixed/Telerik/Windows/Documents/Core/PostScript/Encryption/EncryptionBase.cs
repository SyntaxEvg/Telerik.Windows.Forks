using System;

namespace Telerik.Windows.Documents.Core.PostScript.Encryption
{
	abstract class EncryptionBase
	{
		public EncryptionBase(PostScriptReader reader, ushort r, int n)
		{
			this.reader = reader;
			this.r = r;
			this.randomBytesCount = n;
		}

		public int RandomBytesCount
		{
			get
			{
				return this.randomBytesCount;
			}
		}

		protected PostScriptReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		public abstract void Initialize();

		public byte Decrypt(byte cipher)
		{
			byte result = (byte)((int)cipher ^ (this.r >> 8));
			this.r = ((ushort)cipher + this.r) * 52845 + 22719;
			return result;
		}

		const ushort C1 = 52845;

		const ushort C2 = 22719;

		readonly int randomBytesCount;

		readonly PostScriptReader reader;

		ushort r;
	}
}
