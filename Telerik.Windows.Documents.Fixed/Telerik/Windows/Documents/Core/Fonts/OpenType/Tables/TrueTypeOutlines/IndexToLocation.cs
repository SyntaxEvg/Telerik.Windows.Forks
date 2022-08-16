using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.TrueTypeOutlines
{
	class IndexToLocation : TrueTypeTableBase
	{
		public IndexToLocation(OpenTypeFontSource fontFile)
			: base(fontFile)
		{
		}

		internal override uint Tag
		{
			get
			{
				return Tags.LOCA_TABLE;
			}
		}

		public long GetOffset(ushort index)
		{
			if (this.offsets == null || (int)index >= this.offsets.Length || ((int)index < this.offsets.Length - 1 && this.offsets[(int)(index + 1)] == this.offsets[(int)index]))
			{
				return -1L;
			}
			return (long)((ulong)this.offsets[(int)index]);
		}

		public override void Read(OpenTypeFontReader reader)
		{
			this.offsets = new uint[(int)(base.FontSource.GlyphCount + 1)];
			for (int i = 0; i < this.offsets.Length; i++)
			{
				if (base.FontSource.Head.IndexToLocFormat == 0)
				{
					this.offsets[i] = (uint)(2 * reader.ReadUShort());
				}
				else
				{
					this.offsets[i] = reader.ReadULong();
				}
			}
		}

		uint[] offsets;
	}
}
