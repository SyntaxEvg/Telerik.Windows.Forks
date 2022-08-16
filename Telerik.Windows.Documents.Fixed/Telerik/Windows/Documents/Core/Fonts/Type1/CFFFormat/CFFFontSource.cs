using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat
{
	class CFFFontSource : FontSource
	{
		public CFFFontSource(ICFFFontFile file, Top top)
		{
			this.file = file;
			this.top = top;
		}

		internal ICFFFontFile File
		{
			get
			{
				return this.file;
			}
		}

		internal CFFFontReader Reader
		{
			get
			{
				return this.file.Reader;
			}
		}

		public override string GetFontFamily()
		{
			return this.top.FamilyName;
		}

		public override double GetItalicAngle()
		{
			return 0.0;
		}

		public override bool GetIsItalic()
		{
			return false;
		}

		public override bool GetIsBold()
		{
			throw new NotImplementedException();
		}

		public override double GetAscent()
		{
			throw new NotImplementedException();
		}

		public override double GetDescent()
		{
			throw new NotImplementedException();
		}

		public override double GetLineGap()
		{
			throw new NotImplementedException();
		}

		public override double GetCapHeight()
		{
			throw new NotImplementedException();
		}

		public override double GetStemV()
		{
			throw new NotImplementedException();
		}

		public override Rect GetBoundingBox()
		{
			throw new NotImplementedException();
		}

		public override byte[] GetData()
		{
			return this.Reader.Data;
		}

		public virtual bool UsesCIDFontOperators
		{
			get
			{
				return this.top.UsesCIDFontOperators;
			}
		}

		public override double GetUnderlineThickness()
		{
			return this.top.UnderlineThickness;
		}

		public override double GetUnderlinePosition()
		{
			return this.top.UnderlinePosition;
		}

		public virtual bool TryGetGlyphId(string name, out ushort glyphId)
		{
			return this.top.TryGetGlyphIdByCharacterName(name, out glyphId);
		}

		public override bool TryGetGlyphId(int unicode, out ushort glyphId)
		{
			return this.top.TryGetGlyphIdByCharacterIdentifier((ushort)unicode, out glyphId);
		}

		public virtual string GetGlyphName(ushort cid)
		{
			return this.top.GetGlyphName(cid);
		}

		public override void GetAdvancedWidthOverride(Glyph glyph)
		{
			if (!this.top.UsesCIDFontOperators)
			{
				glyph.AdvancedWidth = (double)this.top.GetAdvancedWidth(glyph.GlyphId) / 1000.0;
			}
		}

		public override void InitializeGlyphOutlinesOverride(Glyph glyph, double fontSize)
		{
			this.top.GetGlyphOutlines(glyph, fontSize);
		}

		public override bool TryGetCharCode(int unicode, out int charCode)
		{
			throw new NotImplementedException();
		}

		public override double GetAdvancedWidthOverride(int charCode)
		{
			ushort glyphId;
			this.TryGetGlyphId((int)((ushort)charCode), out glyphId);
			return (double)this.top.GetAdvancedWidth(glyphId);
		}

		readonly ICFFFontFile file;

		readonly Top top;
	}
}
