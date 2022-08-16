using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class AesEncryptionExtraField : ExtraFieldBase
	{
		public ushort VendorVersion { get; set; }

		public ushort Signature { get; set; }

		public byte KeyLength { get; set; }

		public ushort Method { get; set; }

		protected override ushort HeaderId
		{
			get
			{
				return 39169;
			}
		}

		protected override byte[] GetBlock()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(this.VendorVersion);
					binaryWriter.Write(this.Signature);
					binaryWriter.Write(this.KeyLength);
					binaryWriter.Write(this.Method);
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		protected override void ParseBlock(byte[] extraData)
		{
			this.VendorVersion = BitConverter.ToUInt16(extraData, 0);
			this.Signature = BitConverter.ToUInt16(extraData, 2);
			this.KeyLength = extraData[4];
			this.Method = BitConverter.ToUInt16(extraData, 5);
		}
	}
}
