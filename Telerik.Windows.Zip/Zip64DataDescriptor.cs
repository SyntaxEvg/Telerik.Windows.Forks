using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class Zip64DataDescriptor : Zip64DataDescriptorBase, ISpecData
	{
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
			this.WriteFields(writer);
		}

		public const uint Signature = 134695760U;
	}
}
