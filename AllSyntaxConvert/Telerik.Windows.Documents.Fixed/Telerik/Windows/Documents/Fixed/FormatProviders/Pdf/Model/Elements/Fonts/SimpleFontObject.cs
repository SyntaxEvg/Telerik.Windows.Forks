using System;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	abstract class SimpleFontObject : FontObject
	{
		public SimpleFontObject()
		{
			this.fontDescriptor = base.RegisterReferenceProperty<FontDescriptor>(new PdfPropertyDescriptor("FontDescriptor", true, PdfPropertyRestrictions.MustBeIndirectReference));
			this.encoding = base.RegisterReferenceProperty<SimpleFontEncoding>(new PdfPropertyDescriptor("Encoding", false, PdfPropertyRestrictions.MustBeIndirectReference));
			this.firstChar = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("FirstChar", true));
			this.lastChar = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("LastChar", true));
			this.widths = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Widths", true));
		}

		public PdfInt FirstChar
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

		public PdfInt LastChar
		{
			get
			{
				return this.lastChar.GetValue();
			}
			set
			{
				this.lastChar.SetValue(value);
			}
		}

		public PdfArray Widths
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

		public SimpleFontEncoding Encoding
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

		public FontDescriptor FontDescriptor
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

		protected abstract bool TryCreateSimpleFontFromFontFile(out SimpleFont font);

		protected abstract SimpleFont CreateSimpleFontWithoutFontFile();

		protected override bool TryGetFontFamily(out FontFamily fontFamily)
		{
			fontFamily = null;
			if (this.FontDescriptor.FontFamily != null)
			{
				fontFamily = new FontFamily(this.FontDescriptor.FontFamily.ToString());
			}
			return fontFamily != null;
		}

		protected override bool TryCreateFontFromFontFile(out FontBase font)
		{
			SimpleFont simpleFont;
			if (this.TryCreateSimpleFontFromFontFile(out simpleFont))
			{
				font = simpleFont;
				return true;
			}
			font = null;
			return false;
		}

		protected override FontBase CreateFontWithoutFontFile()
		{
			return this.CreateSimpleFontWithoutFontFile();
		}

		protected override void CopyPropertiesFromOverride(IPdfExportContext context, FontBase font)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			base.CopyPropertiesFromOverride(context, font);
			SimpleFont simpleFont = (SimpleFont)font;
			if (font.Type != FontType.Standard || font.HasFontDescriptorProperties)
			{
				this.FontDescriptor = new FontDescriptor();
				this.FontDescriptor.CopyPropertiesFrom(context, font);
			}
			this.Encoding = simpleFont.Encoding.ToPrimitive(delegate(SimpleEncoding encoding)
			{
				SimpleFontEncoding simpleFontEncoding = new SimpleFontEncoding();
				simpleFontEncoding.BaseEncoding = encoding.PredefinedEncoding.ToPrimitive((string name) => new PdfName(name), null);
				simpleFontEncoding.Differences = encoding.Differences.ToPrimitive((DifferencesRanges differencesRanges) => SimpleFontObject.CreateDifferencesArray(differencesRanges), null);
				return simpleFontEncoding;
			}, null);
			if (!this.TryCopyWidthsFromFont(simpleFont))
			{
				this.CalculateUsedGlyphsWidths(context, font);
			}
		}

		protected override void CopyPropertiesToFont(FontBase font, PostScriptReader reader, IPdfImportContext context)
		{
			SimpleFont simpleFont = font as SimpleFont;
			if (simpleFont != null)
			{
				this.FirstChar.CopyToProperty(simpleFont.FirstChar, (PdfInt firstChar) => firstChar.Value);
				this.LastChar.CopyToProperty(simpleFont.LastChar, (PdfInt lastChar) => lastChar.Value);
				this.Widths.CopyToProperty(simpleFont.Widths, (PdfArray widths) => widths.ToIntArray());
				this.Encoding.CopyToProperty(simpleFont.Encoding, delegate(SimpleFontEncoding encoding)
				{
					SimpleEncoding simpleEncoding = new SimpleEncoding();
					encoding.BaseEncoding.CopyToProperty(simpleEncoding.PredefinedEncoding, (PdfName name) => name.Value);
					encoding.Differences.CopyToProperty(simpleEncoding.Differences, (PdfArray array) => SimpleFontObject.CreateDifferencesRanges(array));
					return simpleEncoding;
				});
			}
			base.CopyFontDescriptorPropertiesToFont(this.FontDescriptor, font);
			base.CopyPropertiesToFont(font, reader, context);
		}

		static PdfArray CreateDifferencesArray(DifferencesRanges differencesRanges)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (object obj in differencesRanges)
			{
				DifferencesRange differencesRange = (DifferencesRange)obj;
				pdfArray.Add(new PdfInt((int)differencesRange.StartCharCode));
				foreach (string initialValue in differencesRange.Differences)
				{
					pdfArray.Add(new PdfName(initialValue));
				}
			}
			return pdfArray;
		}

		static DifferencesRanges CreateDifferencesRanges(PdfArray array)
		{
			DifferencesRanges differencesRanges = new DifferencesRanges();
			DifferencesRange differencesRange = null;
			foreach (PdfPrimitive pdfPrimitive in array)
			{
				PdfElementType type = pdfPrimitive.Type;
				if (type != PdfElementType.PdfInt)
				{
					if (type != PdfElementType.PdfName)
					{
						throw new InvalidOperationException(string.Format("Unexpected element type: {0}", pdfPrimitive.Type));
					}
					PdfName pdfName = (PdfName)pdfPrimitive;
					differencesRange.AddNextDifference(pdfName.Value);
				}
				else
				{
					PdfInt pdfInt = (PdfInt)pdfPrimitive;
					differencesRange = differencesRanges.AddDifferencesRange((byte)pdfInt.Value);
				}
			}
			return differencesRanges;
		}

		bool TryCopyWidthsFromFont(SimpleFont font)
		{
			bool flag = font.Widths.HasValue && font.FirstChar.HasValue && font.LastChar.HasValue;
			if (flag)
			{
				this.Widths = font.Widths.ToPrimitive((int[] widths) => widths.ToPdfArray(), null);
				this.FirstChar = font.FirstChar.Value.ToPdfInt();
				this.LastChar = font.LastChar.Value.ToPdfInt();
			}
			return flag;
		}

		void CalculateUsedGlyphsWidths(IPdfExportContext context, FontBase font)
		{
			if (font.Type != FontType.Standard)
			{
				CharInfo[] array = (from g in context.GetUsedCharacters(font)
					orderby g.CharCode.Code
					select g).ToArray<CharInfo>();
				if (array.Length > 0)
				{
					this.FirstChar = new PdfInt(array[0].CharCode.Code);
					this.LastChar = new PdfInt(array[array.Length - 1].CharCode.Code);
					PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
					for (int i = this.FirstChar.Value; i <= this.LastChar.Value; i++)
					{
						pdfArray.Add(new PdfInt((int)font.GetWidth(i)));
					}
					this.Widths = pdfArray;
				}
			}
		}

		readonly DirectProperty<PdfInt> firstChar;

		readonly DirectProperty<PdfInt> lastChar;

		readonly DirectProperty<PdfArray> widths;

		readonly ReferenceProperty<FontDescriptor> fontDescriptor;

		readonly ReferenceProperty<SimpleFontEncoding> encoding;
	}
}
