using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption
{
	class RC4
	{
		public RC4()
		{
			this.state = new byte[256];
		}

		public void PrepareKey(byte[] key)
		{
			Guard.ThrowExceptionIfNull<byte[]>(key, "key");
			this.PrepareKey(key, 0, key.Length);
		}

		public void PrepareKey(byte[] key, int offset, int length)
		{
			Guard.ThrowExceptionIfNull<byte[]>(key, "key");
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 256; i++)
			{
				this.state[i] = (byte)i;
			}
			for (int j = 0; j < 256; j++)
			{
				num2 = ((int)(key[num + offset] + this.state[j]) + num2) & 255;
				byte b = this.state[j];
				this.state[j] = this.state[num2];
				this.state[num2] = b;
				num = (num + 1) % length;
			}
		}

		public void Encrypt(byte[] inputData, byte[] outputData)
		{
			Guard.ThrowExceptionIfNull<byte[]>(inputData, "inputData");
			Guard.ThrowExceptionIfNull<byte[]>(outputData, "outputData");
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < inputData.Length; i++)
			{
				num = (num + 1) & 255;
				num2 = ((int)this.state[num] + num2) & 255;
				byte b = this.state[num];
				this.state[num] = this.state[num2];
				this.state[num2] = b;
				outputData[i] = inputData[i] ^ this.state[(int)((this.state[num] + this.state[num2]) & byte.MaxValue)];
			}
		}

		readonly byte[] state;
	}
}
