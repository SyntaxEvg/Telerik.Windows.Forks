using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder
{
	class JpegWriter : WriterBase
	{
		public JpegWriter()
		{
			this.bitsWriter = new BitsWriter();
			this.bufferBitsWriter = new BitsWriter();
		}

		public void Flush()
		{
			if (!this.bufferBitsWriter.IsFull && !this.bufferBitsWriter.IsEmpty)
			{
				this.bufferBitsWriter.WriteBits(0, this.bufferBitsWriter.BitsLeft);
			}
			this.WriteJpegByte(this.bufferBitsWriter.Data);
			this.bufferBitsWriter.Clear();
		}

		public void Write4(byte value)
		{
			this.bitsWriter.WriteBits(value, 4);
			if (this.bitsWriter.IsFull)
			{
				base.Write(this.bitsWriter.Data);
				this.bitsWriter.Clear();
			}
		}

		public void Write8(byte value)
		{
			base.Write(value);
		}

		public void Write16(ushort value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			base.WriteBE(bytes, 2);
		}

		public void WriteJpegTables<T>(IEnumerable<T> tables, ushort extraBytesCount) where T : JpegTable
		{
			ushort value = (ushort)(tables.Sum((T t) => (int)t.Length) + (int)extraBytesCount);
			this.Write16(value);
			foreach (T t2 in tables)
			{
				t2.Write(this);
			}
		}

		public void WriteBits(int n, int bits)
		{
			if (n > 0)
			{
				if (this.bufferBitsWriter.BitsLeft >= n)
				{
					this.bufferBitsWriter.WriteBits((byte)bits, n);
				}
				else
				{
					bits <<= 32 - n;
					while (n > 0)
					{
						int num = System.Math.Min(n, this.bufferBitsWriter.BitsLeft);
						byte value = (byte)(bits >> 32 - num);
						this.bufferBitsWriter.WriteBits(value, num);
						bits <<= num;
						n -= num;
						if (this.bufferBitsWriter.IsFull)
						{
							this.WriteJpegByte(this.bufferBitsWriter.Data);
							this.bufferBitsWriter.Clear();
						}
					}
				}
			}
			if (this.bufferBitsWriter.IsFull)
			{
				this.WriteJpegByte(this.bufferBitsWriter.Data);
				this.bufferBitsWriter.Clear();
			}
		}

		public void WriteJpegMarker(JpegMarker marker)
		{
			this.Write(byte.MaxValue);
			this.Write(marker.Code);
		}

		public void Restart()
		{
			this.bufferBitsWriter.Clear();
		}

		void WriteJpegByte(byte bits)
		{
			this.Write(bits);
			if (bits == 255)
			{
				this.Write(0);
			}
		}

		readonly BitsWriter bitsWriter;

		readonly BitsWriter bufferBitsWriter;
	}
}
