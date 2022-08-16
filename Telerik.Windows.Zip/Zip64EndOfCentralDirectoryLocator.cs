using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class Zip64EndOfCentralDirectoryLocator : ISpecData
	{
		public uint NumberOfTheDiskWithTheStartOfTheZip64EndOfCentralDirectory { get; set; }

		public ulong RelativeOffsetOfTheZip64EndOfCentralDirectoryRecord { get; set; }

		public uint NumberOfDisks { get; set; }

		public bool TryReadBlock(BinaryReader reader)
		{
			if (reader.ReadUInt32() != 117853008U)
			{
				return false;
			}
			this.NumberOfTheDiskWithTheStartOfTheZip64EndOfCentralDirectory = reader.ReadUInt32();
			this.RelativeOffsetOfTheZip64EndOfCentralDirectoryRecord = reader.ReadUInt64();
			this.NumberOfDisks = reader.ReadUInt32();
			return true;
		}

		public void WriteBlock(BinaryWriter writer)
		{
			writer.Write(117853008U);
			writer.Write(this.NumberOfTheDiskWithTheStartOfTheZip64EndOfCentralDirectory);
			writer.Write(this.RelativeOffsetOfTheZip64EndOfCentralDirectoryRecord);
			writer.Write(this.NumberOfDisks);
		}

		public const uint Signature = 117853008U;

		public const int StaticBlockLength = 16;
	}
}
