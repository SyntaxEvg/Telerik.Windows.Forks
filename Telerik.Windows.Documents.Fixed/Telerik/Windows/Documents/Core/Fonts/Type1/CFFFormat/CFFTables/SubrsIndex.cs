using System;
using System.IO;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class SubrsIndex : Index
	{
		public SubrsIndex(ICFFFontFile fontFile, int charstringType, long offset)
			: base(fontFile, offset)
		{
			this.charstringType = charstringType;
		}

		public byte[] this[int index]
		{
			get
			{
				return this.GetSubr(index + (int)this.bias);
			}
		}

		byte[] ReadSubr(CFFFontReader reader, uint offset, int length)
		{
			reader.BeginReadingBlock();
			long offset2 = base.DataOffset + (long)((ulong)offset);
			reader.Seek(offset2, SeekOrigin.Begin);
			byte[] array = new byte[length];
			reader.Read(array, length);
			reader.EndReadingBlock();
			return array;
		}

		byte[] GetSubr(int index)
		{
			if (this.subrs[index] == null)
			{
				this.subrs[index] = this.ReadSubr(base.Reader, base.Offsets[index], base.GetDataLength(index));
			}
			return this.subrs[index];
		}

		public override void Read(CFFFontReader reader)
		{
			base.Read(reader);
			if (base.Count == 0)
			{
				return;
			}
			this.subrs = new byte[(int)base.Count][];
			ushort count = base.Count;
			if (this.charstringType == 1)
			{
				this.bias = 0;
				return;
			}
			if (count < 1240)
			{
				this.bias = 107;
				return;
			}
			if (count < 33900)
			{
				this.bias = 1131;
				return;
			}
			this.bias = 32768;
		}

		readonly int charstringType;

		byte[][] subrs;

		ushort bias;
	}
}
