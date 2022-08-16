using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class DataDescriptor : DataDescriptorBase, ISpecData
	{
		public ulong CompressedSizeZip64 { get; set; }

		public ulong UncompressedSizeZip64 { get; set; }

		public bool TryReadBlock(BinaryReader reader)
		{
			byte[] array = new byte[4];
			ZipHelper.ReadBytes(reader.BaseStream, array, array.Length);
			uint num = BitConverter.ToUInt32(array, 0);
			if (num == 134695760U)
			{
				this.ReadFields(reader);
			}
			else
			{
				base.Crc = num;
				base.ReadSize(reader);
			}
			return true;
		}

		public void WriteBlock(BinaryWriter writer)
		{
			writer.Write(134695760U);
			if (!this.useZip64)
			{
				this.WriteFields(writer);
				return;
			}
			writer.Write(base.Crc);
			writer.Write(this.CompressedSizeZip64);
			writer.Write(this.UncompressedSizeZip64);
		}

		internal static DataDescriptor FromFileHeader(FileHeaderBase fileHeader)
		{
			DataDescriptor dataDescriptor = new DataDescriptor
			{
				CompressedSize = fileHeader.CompressedSize,
				Crc = fileHeader.Crc,
				UncompressedSize = fileHeader.UncompressedSize
			};
			Zip64ExtraField zip64ExtraField = null;
			foreach (ExtraFieldBase extraFieldBase in fileHeader.ExtraFields)
			{
				zip64ExtraField = extraFieldBase as Zip64ExtraField;
				if (zip64ExtraField != null)
				{
					break;
				}
			}
			if (zip64ExtraField != null)
			{
				dataDescriptor.useZip64 = true;
				if (zip64ExtraField.CompressedSize != null)
				{
					dataDescriptor.CompressedSizeZip64 = zip64ExtraField.CompressedSize.Value;
				}
				else
				{
					dataDescriptor.CompressedSizeZip64 = (ulong)fileHeader.CompressedSize;
				}
				if (zip64ExtraField.OriginalSize != null)
				{
					dataDescriptor.UncompressedSizeZip64 = zip64ExtraField.OriginalSize.Value;
				}
				else
				{
					dataDescriptor.UncompressedSizeZip64 = (ulong)fileHeader.UncompressedSize;
				}
			}
			return dataDescriptor;
		}

		public const uint Signature = 134695760U;

		public const int StaticBlockLength = 12;

		bool useZip64;
	}
}
