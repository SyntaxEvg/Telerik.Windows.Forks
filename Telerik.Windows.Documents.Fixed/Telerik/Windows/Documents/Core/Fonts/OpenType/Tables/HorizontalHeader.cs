using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class HorizontalHeader : TrueTypeTableBase
	{
		public HorizontalHeader(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		internal override uint Tag
		{
			get
			{
				return Tags.HHEA_TABLE;
			}
		}

		public short Ascender { get; set; }

		public short Descender { get; set; }

		public short LineGap { get; set; }

		public ushort NumberOfHMetrics { get; set; }

		public override void Read(OpenTypeFontReader reader)
		{
			reader.ReadFixed();
			this.Ascender = reader.ReadShort();
			this.Descender = reader.ReadShort();
			this.LineGap = reader.ReadShort();
			reader.ReadUShort();
			reader.ReadShort();
			reader.ReadShort();
			reader.ReadShort();
			reader.ReadShort();
			reader.ReadShort();
			reader.ReadShort();
			reader.ReadShort();
			reader.ReadShort();
			reader.ReadShort();
			reader.ReadShort();
			reader.ReadShort();
			this.NumberOfHMetrics = reader.ReadUShort();
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteShort(this.Ascender);
			writer.WriteShort(this.Descender);
			writer.WriteShort(this.LineGap);
			writer.WriteUShort(this.NumberOfHMetrics);
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			this.Ascender = reader.ReadShort();
			this.Descender = reader.ReadShort();
			this.LineGap = reader.ReadShort();
			this.NumberOfHMetrics = reader.ReadUShort();
		}
	}
}
