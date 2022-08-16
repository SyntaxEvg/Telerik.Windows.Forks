using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Internal;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	abstract class SimpleFontOld : FontBaseOld
	{
		public SimpleFontOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.firstChar = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "FirstChar",
				IsRequired = true
			});
			this.widths = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Widths",
				IsRequired = true
			});
			this.fontDescriptor = base.CreateInstantLoadProperty<FontDescriptorOld>(new PdfPropertyDescriptor
			{
				Name = "FontDescriptor"
			});
			this.encoding = base.CreateInstantLoadProperty<EncodingOld>(new PdfPropertyDescriptor
			{
				Name = "Encoding"
			}, Converters.EncodingConverter);
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

		public PdfIntOld FirstChar
		{
			get
			{
				return this.firstChar.GetValue();
			}
			set
			{
				this.firstChar.SetValue(value);
			}
		}

		public PdfArrayOld Widths
		{
			get
			{
				return this.widths.GetValue();
			}
			set
			{
				this.widths.SetValue(value);
			}
		}

		public FontDescriptorOld FontDescriptor
		{
			get
			{
				return this.fontDescriptor.GetValue();
			}
			set
			{
				this.fontDescriptor.SetValue(value);
			}
		}

		public EncodingOld Encoding
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

		public override double GetGlyphWidth(GlyphOld glyph)
		{
			if (this.Widths != null)
			{
				int num = glyph.CharId.IntValue - this.FirstChar.Value;
				if (0 <= num && num < this.Widths.Count)
				{
					double num2;
					this.Widths.TryGetReal(num, out num2);
					num2 /= 1000.0;
					return num2;
				}
				if (this.FontDescriptor != null)
				{
					return (double)this.FontDescriptor.MissingWidth.Value / 1000.0;
				}
			}
			return base.GetGlyphWidth(glyph);
		}

		protected override void RenderGlyph(PdfContext context, GlyphOld glyph)
		{
			if (this.FontDescriptor != null)
			{
				glyph.Ascent = this.FontDescriptor.Ascent.Value;
				glyph.Descent = this.FontDescriptor.Descent.Value;
			}
			this.LoadFontFamily();
			glyph.FontFamily = this.fontFamily;
			glyph.FontStyle = this.fontStyle;
			glyph.FontWeight = this.fontWeight;
			base.RenderGlyph(context, glyph);
		}

		void LoadFontFamily()
		{
			if (!this.fontFamilyLoaded)
			{
				if (this.FontDescriptor == null || (!this.FontDescriptor.GetFontFamily(out this.fontFamily, out this.fontWeight, out this.fontStyle) && this.BaseFont.Value != null))
				{
					FontsHelper.GetFontFamily(this.BaseFont.Value, out this.fontFamily, out this.fontWeight, out this.fontStyle);
				}
				this.fontFamilyLoaded = true;
			}
		}

		readonly InstantLoadProperty<PdfNameOld> baseFont;

		readonly InstantLoadProperty<PdfIntOld> firstChar;

		readonly InstantLoadProperty<PdfArrayOld> widths;

		readonly InstantLoadProperty<FontDescriptorOld> fontDescriptor;

		readonly InstantLoadProperty<EncodingOld> encoding;

		bool fontFamilyLoaded;

		string fontFamily;

		FontStyle fontStyle;

		FontWeight fontWeight;
	}
}
