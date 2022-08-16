using System;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class SubRule : TableBase
	{
		public SubRule(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		public bool IsMatch(GlyphsSequence glyphIDs, int startIndex)
		{
			for (int i = 0; i < this.input.Length; i++)
			{
				if (glyphIDs[startIndex + i + 1].GlyphId != this.input[i])
				{
					return false;
				}
			}
			return true;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			ushort num2 = reader.ReadUShort();
			this.input = new ushort[(int)num];
			this.substitutions = new SubstLookupRecord[(int)num2];
			for (int i = 0; i < (int)num; i++)
			{
				this.input[i] = reader.ReadUShort();
			}
			for (int j = 0; j < (int)num2; j++)
			{
				this.substitutions[j] = new SubstLookupRecord(base.FontSource);
				this.substitutions[j].Read(reader);
			}
		}

		ushort[] input;

		SubstLookupRecord[] substitutions;
	}
}
