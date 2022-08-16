using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class CentralDirectoryHeader : FileHeaderBase, ISpecData
	{
		public byte VersionMadeBy { get; set; }

		public byte OsCompatibility { get; set; }

		public ushort DiskNumberStart { get; set; }

		public ushort InternalAttributes { get; set; }

		public uint ExternalAttributes { get; set; }

		public uint LocalHeaderOffset { get; set; }

		public byte[] FileComment { get; set; }

		public bool TryReadBlock(BinaryReader reader)
		{
			if (reader.ReadUInt32() != 33639248U)
			{
				return false;
			}
			this.VersionMadeBy = reader.ReadByte();
			this.OsCompatibility = reader.ReadByte();
			this.ReadFields(reader);
			int count = (int)reader.ReadInt16();
			this.DiskNumberStart = reader.ReadUInt16();
			this.InternalAttributes = reader.ReadUInt16();
			this.ExternalAttributes = reader.ReadUInt32();
			this.LocalHeaderOffset = reader.ReadUInt32();
			base.ReadExtraData(reader);
			this.FileComment = reader.ReadBytes(count);
			return true;
		}

		public void WriteBlock(BinaryWriter writer)
		{
			writer.Write(33639248U);
			writer.Write(this.VersionMadeBy);
			writer.Write(this.OsCompatibility);
			this.WriteFields(writer);
			writer.Write((ushort)((this.FileComment != null) ? this.FileComment.Length : 0));
			writer.Write(this.DiskNumberStart);
			writer.Write(this.InternalAttributes);
			writer.Write(this.ExternalAttributes);
			writer.Write(this.LocalHeaderOffset);
			base.WriteExtraData(writer);
			if (this.FileComment != null)
			{
				writer.Write(this.FileComment);
			}
		}

		protected override FileHeaderInfoBase GetHeaderInfo()
		{
			return new CentralDirectoryHeaderInfo(this);
		}

		public const uint Signature = 33639248U;

		public const int StaticBlockLength = 42;
	}
}
