using System;
using System.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.Type1
{
	class CFFFontReader : ReaderBase
	{
		public CFFFontReader(byte[] data)
			: base(data)
		{
		}

		public byte ReadCard8()
		{
			return this.Read();
		}

		public ushort ReadCard16()
		{
			byte[] array = new byte[2];
			base.ReadBE(array, 2);
			return BitConverter.ToUInt16(array, 0);
		}

		public uint ReadCard24()
		{
			byte[] array = new byte[4];
			base.ReadBE(array, 3);
			return BitConverter.ToUInt32(array, 0);
		}

		public uint ReadCard32()
		{
			byte[] array = new byte[4];
			base.ReadBE(array, 4);
			return BitConverter.ToUInt32(array, 0);
		}

		public uint ReadOffset(byte offsetSize)
		{
			switch (offsetSize)
			{
			case 1:
				return (uint)this.ReadCard8();
			case 2:
				return (uint)this.ReadCard16();
			case 3:
				return this.ReadCard24();
			case 4:
				return this.ReadCard32();
			default:
				throw new NotSupportedException();
			}
		}

		public byte ReadOffSize()
		{
			return this.ReadCard8();
		}

		public ushort ReadSID()
		{
			return this.ReadCard16();
		}

		public string ReadString(int length)
		{
			byte[] array = new byte[length];
			base.Read(array, length);
			return global::System.Text.Encoding.UTF8.GetString(array, 0, array.Length);
		}
	}
}
