using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Pfb;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format
{
	class PfbFontReader
	{
		public void ReadHeaders()
		{
			long num = 0L;
			this.headers = new PfbHeader[4];
			for (int i = 0; i < 3; i++)
			{
				this.position = num;
				PfbHeader pfbHeader = new PfbHeader(num, 6);
				pfbHeader.Read(this);
				this.headers[i] = pfbHeader;
				num += (long)pfbHeader.HeaderLength + (long)((ulong)pfbHeader.NextHeaderOffset);
			}
			this.headers[3] = new PfbHeader((long)(this.data.Length - 2), 2);
		}

		public uint ReadUInt()
		{
			byte[] array = new byte[4];
			this.ReadLE(array, array.Length);
			return BitConverter.ToUInt32(array, 0);
		}

		public byte Read()
		{
			byte[] array = this.data;
			long num;
			this.position = (num = this.position) + 1L;
			return array[(int)(checked((IntPtr)num))];
		}

		public byte[] ReadData(byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			this.data = data;
			this.ReadHeaders();
			List<byte> list = new List<byte>(data.Length);
			for (int i = 0; i < this.data.Length; i++)
			{
				if (!this.IsPositionInHeader((long)i))
				{
					list.Add(this.data[i]);
				}
			}
			return list.ToArray();
		}

		bool ReadLE(byte[] buffer, int count)
		{
			for (int i = 0; i < count; i++)
			{
				try
				{
					buffer[i] = this.Read();
				}
				catch (ArgumentOutOfRangeException)
				{
					return false;
				}
			}
			return true;
		}

		bool IsPositionInHeader(long position)
		{
			return this.headers.Any((PfbHeader h) => h.IsPositionInside(position));
		}

		const int HeadersCount = 4;

		const int LastHeader = 3;

		byte[] data;

		PfbHeader[] headers;

		long position;
	}
}
