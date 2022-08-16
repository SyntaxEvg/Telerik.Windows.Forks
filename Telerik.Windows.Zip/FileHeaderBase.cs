using System;
using System.Collections.Generic;
using System.IO;

namespace Telerik.Windows.Zip
{
	abstract class FileHeaderBase : DataDescriptorBase
	{
		public FileHeaderBase()
		{
			this.ExtraFields = new List<ExtraFieldBase>();
		}

		public ushort VersionNeededToExtract { get; set; }

		public ushort GeneralPurposeBitFlag { get; set; }

		public ushort CompressionMethod { get; set; }

		public uint FileTime { get; set; }

		public byte[] FileName { get; set; }

		public byte[] ExtraFieldsData { get; set; }

		public List<ExtraFieldBase> ExtraFields { get; set; }

		internal void FromFileHeader(FileHeaderBase fileHeader)
		{
			base.CompressedSize = fileHeader.CompressedSize;
			this.CompressionMethod = fileHeader.CompressionMethod;
			base.UncompressedSize = fileHeader.UncompressedSize;
			base.Crc = fileHeader.Crc;
			if (fileHeader.ExtraFieldsData != null)
			{
				this.ExtraFieldsData = new byte[fileHeader.ExtraFieldsData.Length];
				Array.Copy(fileHeader.ExtraFieldsData, this.ExtraFieldsData, fileHeader.ExtraFieldsData.Length);
				this.ExtraFields = new List<ExtraFieldBase>(ExtraFieldBase.GetExtraFields(this.GetHeaderInfo()));
			}
			if (fileHeader.FileName != null)
			{
				this.FileName = new byte[fileHeader.FileName.Length];
				Array.Copy(fileHeader.FileName, this.FileName, fileHeader.FileName.Length);
			}
			this.FileTime = fileHeader.FileTime;
			this.GeneralPurposeBitFlag = fileHeader.GeneralPurposeBitFlag;
			this.VersionNeededToExtract = fileHeader.VersionNeededToExtract;
		}

		protected abstract FileHeaderInfoBase GetHeaderInfo();

		protected override void ReadFields(BinaryReader reader)
		{
			this.VersionNeededToExtract = reader.ReadUInt16();
			this.GeneralPurposeBitFlag = reader.ReadUInt16();
			this.CompressionMethod = reader.ReadUInt16();
			this.FileTime = reader.ReadUInt32();
			base.ReadFields(reader);
			this.fileNameLength = reader.ReadUInt16();
			this.extraFieldLength = reader.ReadUInt16();
		}

		protected void ReadExtraData(BinaryReader reader)
		{
			this.FileName = reader.ReadBytes((int)this.fileNameLength);
			this.ExtraFieldsData = reader.ReadBytes((int)this.extraFieldLength);
			this.ExtraFields = new List<ExtraFieldBase>(ExtraFieldBase.GetExtraFields(this.GetHeaderInfo()));
		}

		protected override void WriteFields(BinaryWriter writer)
		{
			writer.Write(this.VersionNeededToExtract);
			writer.Write(this.GeneralPurposeBitFlag);
			writer.Write(this.CompressionMethod);
			writer.Write(this.FileTime);
			base.WriteFields(writer);
			writer.Write((ushort)this.FileName.Length);
			if (this.ExtraFields != null)
			{
				this.ExtraFieldsData = ExtraFieldBase.GetExtraFieldsData(this.ExtraFields);
				writer.Write((ushort)this.ExtraFieldsData.Length);
				return;
			}
			writer.Write(0);
		}

		protected void WriteExtraData(BinaryWriter writer)
		{
			writer.Write(this.FileName);
			if (this.ExtraFieldsData != null)
			{
				writer.Write(this.ExtraFieldsData);
			}
		}

		ushort extraFieldLength;

		ushort fileNameLength;
	}
}
