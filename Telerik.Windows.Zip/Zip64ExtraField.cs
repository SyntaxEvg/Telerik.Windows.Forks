using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class Zip64ExtraField : ExtraFieldBase
	{
		internal Zip64ExtraField(FileHeaderInfoBase headerInfo)
		{
			this.headerInfo = headerInfo;
		}

		public ulong? OriginalSize { get; set; }

		public ulong? CompressedSize { get; set; }

		public ulong? RelativeHeaderOffset { get; set; }

		public uint? DiskStartNumber { get; set; }

		protected override ushort HeaderId
		{
			get
			{
				return 1;
			}
		}

		protected override byte[] GetBlock()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					if (this.OriginalSize != null)
					{
						binaryWriter.Write(this.OriginalSize.Value);
					}
					if (this.CompressedSize != null)
					{
						binaryWriter.Write(this.CompressedSize.Value);
					}
					if (this.RelativeHeaderOffset != null)
					{
						binaryWriter.Write(this.RelativeHeaderOffset.Value);
					}
					if (this.DiskStartNumber != null)
					{
						binaryWriter.Write(this.DiskStartNumber.Value);
					}
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		protected override void ParseBlock(byte[] extraData)
		{
			int num = 0;
			if (this.headerInfo.UncompressedSizeOverflow)
			{
				this.OriginalSize = new ulong?(BitConverter.ToUInt64(extraData, num));
				num += 8;
			}
			if (this.headerInfo.CompressedSizeOverflow)
			{
				this.CompressedSize = new ulong?(BitConverter.ToUInt64(extraData, num));
				num += 8;
			}
			CentralDirectoryHeaderInfo centralDirectoryHeaderInfo = this.headerInfo as CentralDirectoryHeaderInfo;
			if (centralDirectoryHeaderInfo != null && centralDirectoryHeaderInfo.LocalHeaderOffsetOverflow)
			{
				this.RelativeHeaderOffset = new ulong?(BitConverter.ToUInt64(extraData, num));
				num += 8;
			}
			if (extraData.Length >= num + 24)
			{
				this.DiskStartNumber = new uint?(BitConverter.ToUInt32(extraData, 24));
			}
		}

		readonly FileHeaderInfoBase headerInfo;
	}
}
