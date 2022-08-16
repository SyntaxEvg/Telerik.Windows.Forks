using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class CoverageFormat1 : Coverage
	{
		public CoverageFormat1(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		public override void Read(OpenTypeFontReader reader)
		{
		}

		public override int GetCoverageIndex(ushort glyphIndex)
		{
			return -1;
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort(1);
		}
	}
}
