using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class DataDescriptorBase
	{
		public uint Crc { get; set; }

		public uint CompressedSize { get; set; }

		public uint UncompressedSize { get; set; }

		protected virtual void ReadFields(BinaryReader reader)
		{
			this.Crc = reader.ReadUInt32();
			this.ReadSize(reader);
		}

		protected void ReadSize(BinaryReader reader)
		{
			this.CompressedSize = reader.ReadUInt32();
			this.UncompressedSize = reader.ReadUInt32();
		}

		protected virtual void WriteFields(BinaryWriter writer)
		{
			writer.Write(this.Crc);
			writer.Write(this.CompressedSize);
			writer.Write(this.UncompressedSize);
		}
	}
}
