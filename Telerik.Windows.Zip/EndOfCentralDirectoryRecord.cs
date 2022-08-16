using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class EndOfCentralDirectoryRecord : ISpecData
	{
		public ushort NumberOfThisDisk { get; set; }

		public ushort NumberOfTheDiskWithTheStartOfTheCentralDirectory { get; set; }

		public ushort NumberOfEntriesInTheCentralDirectoryOnThisDisk { get; set; }

		public ushort NumberOfEntriesInTheCentralDirectory { get; set; }

		public uint SizeOfCentralDirectory { get; set; }

		public uint OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber { get; set; }

		public byte[] ArchiveComment { get; set; }

		public bool TryReadBlock(BinaryReader reader)
		{
			if (reader.ReadUInt32() != 101010256U)
			{
				return false;
			}
			this.NumberOfThisDisk = reader.ReadUInt16();
			this.NumberOfTheDiskWithTheStartOfTheCentralDirectory = reader.ReadUInt16();
			this.NumberOfEntriesInTheCentralDirectoryOnThisDisk = reader.ReadUInt16();
			this.NumberOfEntriesInTheCentralDirectory = reader.ReadUInt16();
			this.SizeOfCentralDirectory = reader.ReadUInt32();
			this.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber = reader.ReadUInt32();
			this.ArchiveComment = reader.ReadBytes((int)reader.ReadUInt16());
			return true;
		}

		public void WriteBlock(BinaryWriter writer)
		{
			writer.Write(101010256U);
			writer.Write(this.NumberOfThisDisk);
			writer.Write(this.NumberOfTheDiskWithTheStartOfTheCentralDirectory);
			writer.Write(this.NumberOfEntriesInTheCentralDirectoryOnThisDisk);
			writer.Write(this.NumberOfEntriesInTheCentralDirectory);
			writer.Write(this.SizeOfCentralDirectory);
			writer.Write(this.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber);
			ushort value = ((ushort)((this.ArchiveComment != null) ? ((ushort)this.ArchiveComment.Length) : 0));
			writer.Write(value);
			if (this.ArchiveComment != null)
			{
				writer.Write(this.ArchiveComment);
			}
		}

		public const uint Signature = 101010256U;

		public const int StaticBlockLength = 18;
	}
}
