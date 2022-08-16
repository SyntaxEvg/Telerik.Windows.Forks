using System;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class Ligature : TableBase
	{
		public Ligature(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		public ushort LigatureGlyphId { get; set; }

		public int Length
		{
			get
			{
				return this.componentGlyphIds.Length;
			}
		}

		public bool IsMatch(GlyphsSequence glyphIDs, int startIndex)
		{
			for (int i = 0; i < this.componentGlyphIds.Length; i++)
			{
				if (i + startIndex >= glyphIDs.Count || this.componentGlyphIds[i] != glyphIDs[i + startIndex].GlyphId)
				{
					return false;
				}
			}
			return true;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			this.LigatureGlyphId = reader.ReadUShort();
			ushort num = (ushort)(reader.ReadUShort() - 1);
			this.componentGlyphIds = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.componentGlyphIds[i] = reader.ReadUShort();
			}
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort(this.LigatureGlyphId);
			writer.WriteUShort((ushort)(this.componentGlyphIds.Length + 1));
			for (int i = 0; i < this.componentGlyphIds.Length; i++)
			{
				writer.WriteUShort(this.componentGlyphIds[i]);
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			this.Read(reader);
		}

		ushort[] componentGlyphIds;
	}
}
