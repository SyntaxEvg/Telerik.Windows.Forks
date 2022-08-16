using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class MaxProfile : TrueTypeTableBase
	{
		public MaxProfile(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		internal override uint Tag
		{
			get
			{
				return Tags.MAXP_TABLE;
			}
		}

		public float Version { get; set; }

		public ushort NumGlyphs { get; set; }

		public override void Read(OpenTypeFontReader reader)
		{
			this.Version = reader.ReadFixed();
			this.NumGlyphs = reader.ReadUShort();
		}
	}
}
