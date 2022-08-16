using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Utils;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder
{
	class JpegDataProcessor
	{
		public JpegDataProcessor(IJpegReader owner)
		{
			this.owner = owner;
			this.bitsReader = new BitsReader();
		}

		public byte Read4()
		{
			return (byte)this.ReadBit(4);
		}

		public byte Read8()
		{
			return this.owner.Read();
		}

		public ushort Read16()
		{
			byte[] array = new byte[2];
			this.ReadBE(array, 2);
			return BitConverter.ToUInt16(array, 0);
		}

		int ReadBE(byte[] buffer, int count)
		{
			int num = 0;
			for (int i = count - 1; i >= 0; i--)
			{
				if (this.owner.Position >= this.owner.Length)
				{
					return num;
				}
				buffer[i] = this.owner.Read();
				num++;
			}
			return num;
		}

		public int ReadBit()
		{
			return this.ReadBit(1);
		}

		public int Receive(int length)
		{
			int num = 0;
			while (length > 0)
			{
				int num2 = this.owner.ReadBit();
				num = (num << 1) | num2;
				length--;
			}
			return num;
		}

		public int ReceiveAndExtend(int length)
		{
			int num = this.Receive(length);
			if (num >= 1 << length - 1)
			{
				return num;
			}
			return num + (-1 << length) + 1;
		}

		public IEnumerable<T> ReadJpegTables<T>() where T : JpegTable, new()
		{
			long position = this.owner.Position;
			ushort length = this.owner.Read16();
			while (position + (long)((ulong)length) > this.owner.Position)
			{
				T table = Activator.CreateInstance<T>();
				table.Read(this.owner);
				yield return table;
			}
			yield break;
		}

		public JpegMarker ReadNextJpegMarker()
		{
			this.owner.Restart();
			byte b;
			while (this.ReadJpegByte(out b))
			{
			}
			this.owner.Read();
			return JpegMarker.GetMarker(this.owner.Read());
		}

		bool ReadJpegByte(out byte bits)
		{
			bits = 0;
			if (this.owner.Peek(0) == 255)
			{
				if (this.owner.Peek(1) != 0)
				{
					return false;
				}
				bits = this.owner.Read();
				this.owner.Read();
			}
			else
			{
				bits = this.owner.Read();
			}
			return true;
		}

		int ReadBit(int count)
		{
			if (this.bitsReader.IsEmpty)
			{
				this.bitsReader.Data = this.owner.Read();
			}
			return this.bitsReader.ReadBits(count);
		}

		readonly BitsReader bitsReader;

		readonly IJpegReader owner;
	}
}
