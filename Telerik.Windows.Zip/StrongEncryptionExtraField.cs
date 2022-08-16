using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class StrongEncryptionExtraField : ExtraFieldBase
	{
		public ushort Format { get; set; }

		public ushort AlgorithmId { get; set; }

		public ushort KeyLength { get; set; }

		public ushort Flags { get; set; }

		protected override ushort HeaderId
		{
			get
			{
				return 23;
			}
		}

		protected override byte[] GetBlock()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(this.Format);
					binaryWriter.Write(this.AlgorithmId);
					binaryWriter.Write(this.KeyLength);
					binaryWriter.Write(this.Flags);
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		protected override void ParseBlock(byte[] extraData)
		{
			this.Format = BitConverter.ToUInt16(extraData, 0);
			this.AlgorithmId = BitConverter.ToUInt16(extraData, 2);
			this.KeyLength = BitConverter.ToUInt16(extraData, 4);
			this.Flags = BitConverter.ToUInt16(extraData, 6);
		}
	}
}
