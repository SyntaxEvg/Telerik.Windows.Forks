using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class LocalFileHeader : FileHeaderBase, ISpecData
	{
		public LocalFileHeader()
		{
		}

		internal LocalFileHeader(FileHeaderBase fileHeader)
		{
			base.FromFileHeader(fileHeader);
		}

		public bool TryReadBlock(BinaryReader reader)
		{
			if (reader.ReadUInt32() != 67324752U)
			{
				return false;
			}
			this.ReadFields(reader);
			base.ReadExtraData(reader);
			return true;
		}

		public void WriteBlock(BinaryWriter writer)
		{
			writer.Write(67324752U);
			this.WriteFields(writer);
			base.WriteExtraData(writer);
		}

		protected override FileHeaderInfoBase GetHeaderInfo()
		{
			return new LocalFileHeaderInfo(this);
		}

		public const uint Signature = 67324752U;

		public const int StaticBlockLength = 26;
	}
}
