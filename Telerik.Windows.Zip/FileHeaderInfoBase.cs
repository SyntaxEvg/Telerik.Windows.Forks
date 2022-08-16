using System;

namespace Telerik.Windows.Zip
{
	abstract class FileHeaderInfoBase
	{
		public FileHeaderInfoBase(FileHeaderBase localFileHeader)
		{
			this.ExtraFieldsData = localFileHeader.ExtraFieldsData;
			this.UncompressedSizeOverflow = localFileHeader.UncompressedSize == uint.MaxValue;
			this.CompressedSizeOverflow = localFileHeader.CompressedSize == uint.MaxValue;
		}

		public byte[] ExtraFieldsData { get; set; }

		public bool UncompressedSizeOverflow { get; set; }

		public bool CompressedSizeOverflow { get; set; }
	}
}
