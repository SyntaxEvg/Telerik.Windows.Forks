using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.TrueTypeOutlines
{
	class GlyphData : TrueTypeTableBase
	{
		public static GlyphData ReadGlyf(OpenTypeFontSourceBase fontFile, ushort glyphIndex)
		{
			short num = fontFile.Reader.ReadShort();
			GlyphData glyphData;
			if (num == 0)
			{
				glyphData = new GlyphData(fontFile, glyphIndex);
			}
			else if (num > 0)
			{
				glyphData = new SimpleGlyph(fontFile, glyphIndex);
			}
			else
			{
				glyphData = new CompositeGlyph(fontFile, glyphIndex);
			}
			glyphData.NumberOfContours = num;
			glyphData.BoundingRect = new Rect(new Point((double)fontFile.Reader.ReadShort(), (double)fontFile.Reader.ReadShort()), new Point((double)fontFile.Reader.ReadShort(), (double)fontFile.Reader.ReadShort()));
			glyphData.Read(fontFile.Reader);
			return glyphData;
		}

		public GlyphData(OpenTypeFontSourceBase fontFile, ushort glyphIndex)
			: base(fontFile)
		{
			this.glyphIndex = glyphIndex;
			this.BoundingRect = Rect.Empty;
		}

		internal virtual IEnumerable<OutlinePoint[]> Contours
		{
			get
			{
				return Enumerable.Empty<OutlinePoint[]>();
			}
		}

		internal override uint Tag
		{
			get
			{
				return Tags.GLYF_TABLE;
			}
		}

		internal short NumberOfContours { get; set; }

		public ushort GlyphIndex
		{
			get
			{
				return this.glyphIndex;
			}
		}

		public Rect BoundingRect { get; set; }

		public override void Read(OpenTypeFontReader reader)
		{
		}

		readonly ushort glyphIndex;
	}
}
