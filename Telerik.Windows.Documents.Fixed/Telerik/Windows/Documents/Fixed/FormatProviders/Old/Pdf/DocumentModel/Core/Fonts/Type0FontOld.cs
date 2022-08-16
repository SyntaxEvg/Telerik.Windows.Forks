using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	[PdfClass(TypeName = "Font", SubtypeProperty = "Subtype", SubtypeValue = "Type0")]
	class Type0FontOld : FontBaseOld
	{
		public Type0FontOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.encoding = base.CreateInstantLoadProperty<CMapStream>(new PdfPropertyDescriptor
			{
				Name = "Encoding",
				IsRequired = true
			}, Converters.CMapStreamConverter);
			this.descendantFonts = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "DescendantFonts",
				IsRequired = true
			});
			this.baseFont = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "BaseFont",
				IsRequired = true
			});
		}

		public PdfNameOld BaseFont
		{
			get
			{
				return this.baseFont.GetValue();
			}
			set
			{
				this.baseFont.SetValue(value);
			}
		}

		public CMapStream Encoding
		{
			get
			{
				return this.encoding.GetValue();
			}
			set
			{
				this.encoding.SetValue(value);
			}
		}

		public PdfArrayOld DescendantFonts
		{
			get
			{
				return this.descendantFonts.GetValue();
			}
			set
			{
				this.descendantFonts.SetValue(value);
			}
		}

		public CIDFontOld CIDFont
		{
			get
			{
				if (this.cidFont == null)
				{
					this.cidFont = this.DescendantFonts.GetElement<CIDFontOld>(0, Converters.CIDFontConverter);
				}
				return this.cidFont;
			}
		}

		protected override FontSource FontSource
		{
			get
			{
				return this.CIDFont.FontSource;
			}
		}

		public override double GetGlyphWidth(GlyphOld glyph)
		{
			this.LoadWidths();
			double result;
			if (this.widths == null || !this.widths.TryGetValue(glyph.CharId, out result))
			{
				if (this.defaultWidth != null)
				{
					result = this.defaultWidth.Value;
				}
				else if (this.FontSource != null)
				{
					result = base.GetGlyphWidth(glyph);
				}
				else
				{
					result = Type0FontOld.DefaultGlyphWidth;
				}
			}
			return result;
		}

		protected override IEnumerable<GlyphOld> GetGlyphs(PdfStringOld str)
		{
			if (this.Encoding != null)
			{
				CMapOld type0FontEncoding = base.FontsManager.GetType0FontEncoding(this);
				IEnumerable<CharCodeOld> charIds = type0FontEncoding.GetCharIds(str.Value);
				IEnumerable<GlyphOld> glyphs = this.CIDFont.GetGlyphs(charIds);
				if (glyphs != null)
				{
					return glyphs;
				}
			}
			return base.GetGlyphs(str);
		}

		protected override void RenderGlyph(PdfContext context, GlyphOld glyph)
		{
			this.CIDFont.RenderGlyph(context, glyph);
			if (this.CIDFont.FontDescriptor == null)
			{
				if (this.BaseFont == null)
				{
					return;
				}
				if (FontsManagerBase.IsStandardFontName(this.BaseFont.Value))
				{
					StandardFontDescriptor standardFontDescriptor = FontsManagerBase.GetStandardFontDescriptor(this.BaseFont.Value);
					glyph.Ascent = standardFontDescriptor.Ascent;
					glyph.Descent = standardFontDescriptor.Descent;
				}
				else if (this.FontSource != null)
				{
					glyph.Ascent = this.FontSource.Ascent;
					glyph.Descent = this.FontSource.Descent;
				}
			}
			else
			{
				glyph.Ascent = this.CIDFont.FontDescriptor.Ascent.Value;
				glyph.Descent = this.CIDFont.FontDescriptor.Descent.Value;
			}
			base.RenderGlyph(context, glyph);
		}

		void LoadWidths()
		{
			if (this.CIDFont.DefaultWidth != null)
			{
				this.defaultWidth = new double?((double)this.CIDFont.DefaultWidth.Value / 1000.0);
			}
			if (this.widths == null && this.CIDFont.Widths != null)
			{
				this.widths = new Dictionary<CharCodeOld, double>();
				int num = 0;
				while (num + 1 < this.CIDFont.Widths.Count)
				{
					PdfArrayOld pdfArrayOld = this.CIDFont.Widths[num + 1] as PdfArrayOld;
					if (pdfArrayOld != null)
					{
						int num2;
						this.CIDFont.Widths.TryGetInt(num, out num2);
						ushort num3 = 0;
						while ((int)num3 < pdfArrayOld.Count)
						{
							double num4;
							if (pdfArrayOld.TryGetReal((int)num3, out num4))
							{
								this.widths[new CharCodeOld((ushort)(num2 + (int)num3))] = num4 / 1000.0;
							}
							num3 += 1;
						}
						num += 2;
					}
					else
					{
						int num2;
						this.CIDFont.Widths.TryGetInt(num, out num2);
						int num5;
						this.CIDFont.Widths.TryGetInt(num + 1, out num5);
						double num4;
						this.CIDFont.Widths.TryGetReal(num + 2, out num4);
						num4 /= 1000.0;
						ushort num6 = (ushort)num2;
						while ((int)num6 <= num5)
						{
							this.widths[new CharCodeOld(num6)] = num4;
							num6 += 1;
						}
						num += 3;
					}
				}
			}
		}

		static readonly double DefaultGlyphWidth = 1.0;

		readonly InstantLoadProperty<PdfNameOld> baseFont;

		readonly InstantLoadProperty<CMapStream> encoding;

		readonly InstantLoadProperty<PdfArrayOld> descendantFonts;

		Dictionary<CharCodeOld, double> widths;

		double? defaultWidth;

		CIDFontOld cidFont;
	}
}
