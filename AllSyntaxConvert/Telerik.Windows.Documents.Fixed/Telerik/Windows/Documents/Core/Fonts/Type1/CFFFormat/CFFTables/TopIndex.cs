using System;
using System.IO;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class TopIndex : Index
	{
		public TopIndex(ICFFFontFile file, long offset)
			: base(file, offset)
		{
		}

		public Top this[int index]
		{
			get
			{
				return this.GetTop(index);
			}
		}

		Top ReadTop(CFFFontReader reader, uint offset, int length)
		{
			reader.BeginReadingBlock();
			long offset2 = base.DataOffset + (long)((ulong)offset);
			reader.Seek(offset2, SeekOrigin.Begin);
			Top top = new Top(base.File, offset2, length);
			top.Read(reader);
			reader.EndReadingBlock();
			return top;
		}

		Top GetTop(int index)
		{
			if (this.tops[index] == null)
			{
				this.tops[index] = this.ReadTop(base.Reader, base.Offsets[index], base.GetDataLength(index));
			}
			return this.tops[index];
		}

		public override void Read(CFFFontReader reader)
		{
			base.Read(reader);
			if (base.Count == 0)
			{
				return;
			}
			this.tops = new Top[(int)base.Count];
		}

		Top[] tops;
	}
}
