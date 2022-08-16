using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	[PdfClass(TypeName = "Font", SubtypeProperty = "Subtype", SubtypeValue = "Type1")]
	class Type1FontOld : SimpleFontOld
	{
		public Type1FontOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.fontFileType = Type1FontFileType.Undefined;
		}

		protected override FontSource FontSource
		{
			get
			{
				switch (this.fontFileType)
				{
				case Type1FontFileType.Type1:
				case Type1FontFileType.Missing:
				case Type1FontFileType.StandardFont:
					return this.Type1FontSource;
				case Type1FontFileType.CFF:
					return this.CFFFontSource;
				default:
					return null;
				}
			}
		}

		Type1FontFileType FontFileType
		{
			get
			{
				if (this.fontFileType == Type1FontFileType.Undefined)
				{
					if (base.FontDescriptor != null)
					{
						if (base.FontDescriptor.FontFile3 != null)
						{
							this.fontFileType = Type1FontFileType.CFF;
						}
						else if (base.FontDescriptor.FontFile != null)
						{
							this.fontFileType = Type1FontFileType.Type1;
						}
						else if (base.BaseFont != null && FontsManagerBase.IsStandardFontName(base.BaseFont.Value))
						{
							this.fontFileType = Type1FontFileType.StandardFont;
						}
						else
						{
							this.fontFileType = Type1FontFileType.Missing;
						}
					}
					else if (base.BaseFont != null && FontsManagerBase.IsStandardFontName(base.BaseFont.Value))
					{
						this.fontFileType = Type1FontFileType.StandardFont;
					}
					else
					{
						this.fontFileType = Type1FontFileType.Missing;
					}
				}
				return this.fontFileType;
			}
		}

		CFFFontSource CFFFontSource
		{
			get
			{
				if (this.ccfFontSource == null && this.FontFileType == Type1FontFileType.CFF)
				{
					this.ccfFontSource = base.FontDescriptor.GetCFFFontSource();
				}
				return this.ccfFontSource;
			}
		}

		Type1FontSource Type1FontSource
		{
			get
			{
				if (this.type1FontSource == null)
				{
					if (this.FontFileType == Type1FontFileType.Type1)
					{
						byte[] data = base.ContentManager.ReadData(base.FontDescriptor.FontFile.Reference);
						this.type1FontSource = new Type1FontSource(data);
					}
					else if (this.FontFileType == Type1FontFileType.StandardFont)
					{
						this.type1FontSource = base.FontsManager.SystemFontsManager.GetStandardFontSource(base.BaseFont.Value);
					}
					else if (this.FontFileType == Type1FontFileType.Missing)
					{
						this.type1FontSource = base.FontsManager.SystemFontsManager.GetType1FallbackFontSource(base.BaseFont.Value);
					}
				}
				return this.type1FontSource;
			}
		}

		protected override void RenderGlyph(PdfContext context, GlyphOld glyph)
		{
			if (base.FontDescriptor == null && base.BaseFont != null && FontsManagerBase.IsStandardFontName(base.BaseFont.Value))
			{
				StandardFontDescriptor standardFontDescriptor = FontsManagerBase.GetStandardFontDescriptor(base.BaseFont.Value);
				if (standardFontDescriptor != null)
				{
					glyph.Ascent = standardFontDescriptor.Ascent;
					glyph.Descent = standardFontDescriptor.Descent;
				}
			}
			base.RenderGlyph(context, glyph);
		}

		protected override IEnumerable<GlyphOld> GetGlyphs(PdfStringOld str)
		{
			switch (this.FontFileType)
			{
			case Type1FontFileType.Type1:
			case Type1FontFileType.Missing:
			case Type1FontFileType.StandardFont:
				return this.GetGlyphsFromType1FontSource(str);
			case Type1FontFileType.CFF:
				return this.GetGlyphsFromCFFFontSource(str);
			default:
				return base.GetGlyphs(str);
			}
		}

		IEnumerable<GlyphOld> GetGlyphsFromCFFFontSource(PdfStringOld str)
		{
			foreach (byte b in str.Value)
			{
				GlyphOld g = new GlyphOld();
				Type1Font.InitializeCoreGlyph(g, this.CFFFontSource, base.Encoding, b);
				g.CharId = new CharCodeOld(b);
				yield return g;
			}
			yield break;
		}

		IEnumerable<GlyphOld> GetGlyphsFromType1FontSource(PdfStringOld str)
		{
			foreach (byte b in str.Value)
			{
				GlyphOld g = new GlyphOld();
				Type1Font.InitializeCoreGlyph(g, this.Type1FontSource, base.Encoding, b);
				g.CharId = new CharCodeOld(b);
				yield return g;
			}
			yield break;
		}

		Type1FontFileType fontFileType;

		CFFFontSource ccfFontSource;

		Type1FontSource type1FontSource;
	}
}
