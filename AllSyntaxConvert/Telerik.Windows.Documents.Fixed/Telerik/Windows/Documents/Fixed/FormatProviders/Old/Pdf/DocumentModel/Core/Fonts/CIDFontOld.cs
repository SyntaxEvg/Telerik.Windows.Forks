using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	abstract class CIDFontOld : PdfObjectOld
	{
		public CIDFontOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.subtype = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "Subtype",
				IsRequired = true
			});
			this.baseFont = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "BaseFont",
				IsRequired = true
			}, Converters.PdfNameConverter);
			this.fontDescriptor = base.CreateInstantLoadProperty<CIDFontDescriptorOld>(new PdfPropertyDescriptor
			{
				Name = "FontDescriptor",
				IsRequired = true,
				State = PdfPropertyState.MustBeIndirectReference
			});
			this.defaultWidth = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "DW"
			});
			this.widths = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "W"
			});
		}

		public abstract FontSource FontSource { get; }

		public PdfNameOld Subtype
		{
			get
			{
				return this.subtype.GetValue();
			}
			set
			{
				this.subtype.SetValue(value);
			}
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

		public CIDFontDescriptorOld FontDescriptor
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

		public PdfIntOld DefaultWidth
		{
			get
			{
				return this.defaultWidth.GetValue();
			}
			set
			{
				this.defaultWidth.SetValue(value);
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

		public static CIDFontOld CreateCIDFont(PdfContentManager contentManager, PdfDictionaryOld dict)
		{
			string value = dict.GetElement<PdfNameOld>("Subtype").Value;
			string a;
			if ((a = value) != null)
			{
				CIDFontOld cidfontOld;
				if (!(a == "CIDFontType0"))
				{
					if (!(a == "CIDFontType2"))
					{
						goto IL_44;
					}
					cidfontOld = new CIDFontType2Old(contentManager);
				}
				else
				{
					cidfontOld = new CIDFontType0Old(contentManager);
				}
				cidfontOld.Load(dict);
				return cidfontOld;
			}
			IL_44:
			throw new NotSupportedException();
		}

		public abstract IEnumerable<GlyphOld> GetGlyphs(IEnumerable<CharCodeOld> charIds);

		public virtual void RenderGlyph(PdfContext context, GlyphOld glyph)
		{
			this.LoadFontFamily();
			glyph.FontFamily = this.fontFamily;
			glyph.FontStyle = this.fontStyle;
			glyph.FontWeight = this.fontWeight;
		}

		void LoadFontFamily()
		{
			if (!this.fontFamilyLoaded)
			{
				if (this.FontDescriptor == null || !this.FontDescriptor.GetFontFamily(out this.fontFamily, out this.fontWeight, out this.fontStyle))
				{
					FontsHelper.GetFontFamily(this.BaseFont.Value, out this.fontFamily, out this.fontWeight, out this.fontStyle);
				}
				this.fontFamilyLoaded = true;
			}
		}

		readonly InstantLoadProperty<PdfNameOld> subtype;

		readonly InstantLoadProperty<PdfNameOld> baseFont;

		readonly InstantLoadProperty<PdfIntOld> defaultWidth;

		readonly InstantLoadProperty<CIDFontDescriptorOld> fontDescriptor;

		readonly InstantLoadProperty<PdfArrayOld> widths;

		bool fontFamilyLoaded;

		string fontFamily;

		FontStyle fontStyle;

		FontWeight fontWeight;
	}
}
