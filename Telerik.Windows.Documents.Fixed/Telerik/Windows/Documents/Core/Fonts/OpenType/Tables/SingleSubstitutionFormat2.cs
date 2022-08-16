using System;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class SingleSubstitutionFormat2 : SingleSubstitution
	{
		public SingleSubstitutionFormat2(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		public override GlyphsSequence Apply(GlyphsSequence glyphIDs)
		{
			GlyphsSequence glyphsSequence = new GlyphsSequence(glyphIDs.Count);
			for (int i = 0; i < glyphIDs.Count; i++)
			{
				int coverageIndex = base.Coverage.GetCoverageIndex(glyphIDs[i].GlyphId);
				if (coverageIndex < 0)
				{
					glyphsSequence.Add(glyphIDs[i]);
				}
				else
				{
					glyphsSequence.Add(this.substitutes[coverageIndex]);
				}
			}
			return glyphsSequence;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			base.Read(reader);
			ushort num = reader.ReadUShort();
			this.substitutes = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.substitutes[i] = reader.ReadUShort();
			}
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort(2);
			base.Write(writer);
			writer.WriteUShort((ushort)this.substitutes.Length);
			for (int i = 0; i < this.substitutes.Length; i++)
			{
				writer.WriteUShort(this.substitutes[i]);
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			base.Import(reader);
			ushort num = reader.ReadUShort();
			this.substitutes = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.substitutes[i] = reader.ReadUShort();
			}
		}

		ushort[] substitutes;
	}
}
