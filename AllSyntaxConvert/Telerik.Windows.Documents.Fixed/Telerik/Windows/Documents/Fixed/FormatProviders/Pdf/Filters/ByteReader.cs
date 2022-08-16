using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	class ByteReader
	{
		public ByteReader(byte[] data)
		{
			this.data = data;
			this.pos = 0;
		}

		public int Remaining
		{
			get
			{
				return this.data.Length - this.pos;
			}
		}

		public byte ReadByte()
		{
			byte result = this.data[this.pos];
			this.pos++;
			return result;
		}

		public void Read(byte[] buffer)
		{
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = this.ReadByte();
			}
		}

		byte[] data;

		int pos;
	}
}
