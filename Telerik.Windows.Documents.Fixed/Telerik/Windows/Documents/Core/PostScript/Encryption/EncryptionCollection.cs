using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Core.PostScript.Encryption
{
	class EncryptionCollection
	{
		public EncryptionCollection()
		{
			this.store = new List<EncryptionBase>();
		}

		public bool HasEncryption
		{
			get
			{
				return this.store.Count != 0;
			}
		}

		public void PushEncryption(EncryptionBase encryption)
		{
			this.store.Add(encryption);
		}

		public void PopEncryption()
		{
			this.store.Remove(this.store.Last<EncryptionBase>());
		}

		public byte Decrypt(byte b)
		{
			byte b2 = this.store[0].Decrypt(b);
			for (int i = 1; i < this.store.Count; i++)
			{
				EncryptionBase encryptionBase = this.store[i];
				b2 = encryptionBase.Decrypt(b2);
			}
			return b2;
		}

		readonly List<EncryptionBase> store;
	}
}
