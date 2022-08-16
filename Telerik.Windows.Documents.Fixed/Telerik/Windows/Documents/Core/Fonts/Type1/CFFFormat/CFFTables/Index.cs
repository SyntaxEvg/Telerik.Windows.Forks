using System;
using System.Linq;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class Index : CFFTable
	{
		public Index(ICFFFontFile file, long offset)
			: base(file, offset)
		{
		}

		public long SkipOffset
		{
			get
			{
				if (this.Offsets == null)
				{
					return this.DataOffset;
				}
				return this.DataOffset + (long)((ulong)this.Offsets.Last<uint>());
			}
		}

		public ushort Count { get; set; }

		protected uint[] Offsets { get; set; }

		protected long DataOffset { get; set; }

		protected int GetDataLength(int index)
		{
			return (int)(this.Offsets[index + 1] - this.Offsets[index]);
		}

		public override void Read(CFFFontReader reader)
		{
			this.Count = reader.ReadCard16();
			if (this.Count == 0)
			{
				this.DataOffset = reader.Position;
				return;
			}
			byte offsetSize = reader.ReadOffSize();
			ushort num = this.Count + 1;
			this.Offsets = new uint[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.Offsets[i] = reader.ReadOffset(offsetSize);
			}
			this.DataOffset = reader.Position - 1L;
		}
	}
}
