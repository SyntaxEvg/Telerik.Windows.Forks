using System;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class SingleSubstitutionFormat1 : SingleSubstitution
	{
		public SingleSubstitutionFormat1(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		public override GlyphsSequence Apply(GlyphsSequence glyphIDs)
		{
			GlyphsSequence glyphsSequence = new GlyphsSequence(glyphIDs.Count);
			for (int i = 0; i < glyphIDs.Count; i++)
			{
				ushort glyphId = glyphIDs[i].GlyphId;
				if (base.Coverage.GetCoverageIndex(glyphId) < 0)
				{
					glyphsSequence.Add(glyphIDs[i]);
				}
				else
				{
					glyphsSequence.Add(glyphId + this.deltaGlyphId);
				}
			}
			return glyphsSequence;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			base.Read(reader);
			this.deltaGlyphId = reader.ReadUShort();
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort(1);
			base.Write(writer);
			writer.WriteUShort(this.deltaGlyphId);
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			base.Import(reader);
			this.deltaGlyphId = reader.ReadUShort();
		}

		ushort deltaGlyphId;
	}
}
