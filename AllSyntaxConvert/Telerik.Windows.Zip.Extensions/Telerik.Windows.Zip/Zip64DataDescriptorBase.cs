using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class Zip64DataDescriptorBase
	{
		public uint Crc { get; set; }

		public ulong CompressedSize { get; set; }

		public ulong UncompressedSize { get; set; }

		protected virtual void ReadFields(BinaryReader reader)
		{
			this.Crc = reader.ReadUInt32();
			this.ReadSize(reader);
		}

		protected void ReadSize(BinaryReader reader)
		{
			this.CompressedSize = reader.ReadUInt64();
			this.UncompressedSize = reader.ReadUInt64();
		}

		protected virtual void WriteFields(BinaryWriter writer)
		{
			writer.Write(this.Crc);
			writer.Write(this.CompressedSize);
			writer.Write(this.UncompressedSize);
		}
	}
}
