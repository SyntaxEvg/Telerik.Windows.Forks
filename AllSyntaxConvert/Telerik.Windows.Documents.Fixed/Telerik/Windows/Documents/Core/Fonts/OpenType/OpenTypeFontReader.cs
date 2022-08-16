using System;
using System.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType
{
	class OpenTypeFontReader : ReaderBase
	{
		public OpenTypeFontReader(byte[] data)
			: base(data)
		{
		}

		public sbyte ReadChar()
		{
			return (sbyte)this.Read();
		}

		public ushort ReadUShort()
		{
			byte[] array = new byte[2];
			base.ReadBE(array, 2);
			return BitConverter.ToUInt16(array, 0);
		}

		public short ReadShort()
		{
			byte[] array = new byte[2];
			base.ReadBE(array, 2);
			return BitConverter.ToInt16(array, 0);
		}

		public uint ReadULong()
		{
			byte[] array = new byte[4];
			base.ReadBE(array, 4);
			return BitConverter.ToUInt32(array, 0);
		}

		public int ReadLong()
		{
			byte[] array = new byte[4];
			base.ReadBE(array, 4);
			return BitConverter.ToInt32(array, 0);
		}

		public long ReadLongDateTime()
		{
			byte[] array = new byte[8];
			base.ReadBE(array, 8);
			return BitConverter.ToInt64(array, 0);
		}

		public float ReadFixed()
		{
			float num = (float)this.ReadShort();
			return num + (float)this.ReadUShort() / 65536f;
		}

		public float Read2Dot14()
		{
			return (float)this.ReadShort() / 16384f;
		}

		public string ReadString()
		{
			byte b = this.Read();
			byte[] array = new byte[(int)b];
			for (int i = 0; i < (int)b; i++)
			{
				array[i] = this.Read();
			}
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}
	}
}
