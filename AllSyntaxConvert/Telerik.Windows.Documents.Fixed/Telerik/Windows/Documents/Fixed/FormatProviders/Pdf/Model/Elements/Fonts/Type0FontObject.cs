using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.CMaps;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class Type0FontObject : FontObject
	{
		public Type0FontObject()
		{
			this.cidFont = null;
			this.encoding = base.RegisterReferenceProperty<EncodingBaseObject>(new PdfPropertyDescriptor("Encoding", true));
			this.descendantFonts = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("DescendantFonts", true, PdfPropertyRestrictions.ContainsOnlyIndirectReferences));
		}

		public EncodingBaseObject Encoding
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

		public PdfArray DescendantFonts
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

		CidFontObject CidFont
		{
			get
			{
				return this.cidFont;
			}
		}

		protected override IEnumerable<CharInfo> GetCharactersOverride(PdfString str)
		{
			if (this.Encoding != null && this.CidFont != null)
			{
				List<CharInfo> list = new List<CharInfo>();
				foreach (CharCode charCode in this.Encoding.BuildCharCodes(str))
				{
					list.Add(new CharInfo(this.GetToUnicode(charCode), charCode));
				}
				return list;
			}
			return base.GetCharactersOverride(str);
		}

		protected override void CopyPropertiesFromOverride(IPdfExportContext context, FontBase font)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			base.CopyPropertiesFromOverride(context, font);
			this.DescendantFonts = new PdfArray(new PdfPrimitive[0]);
			this.Encoding = EncodingBaseObject.IdentityH;
			switch (font.Type)
			{
			case FontType.CidType0Font:
			{
				CidFontType0Object cidFontType0Object = new CidFontType0Object();
				cidFontType0Object.CopyPropertiesFrom(context, font);
				this.DescendantFonts.Add(cidFontType0Object);
				base.BaseFont = new PdfName(cidFontType0Object.BaseFont.Value);
				return;
			}
			case FontType.CidType2Font:
			{
				CidFontType2Object cidFontType2Object = new CidFontType2Object();
				cidFontType2Object.CopyPropertiesFrom(context, font);
				this.DescendantFonts.Add(cidFontType2Object);
				base.BaseFont = new PdfName(cidFontType2Object.BaseFont.Value);
				return;
			}
			default:
				throw new InvalidOperationException(string.Format("Cannot create CIDFont from font type: {0}", font.Type));
			}
		}

		protected override void CopyPropertiesToFont(FontBase font, PostScriptReader reader, IPdfImportContext context)
		{
			CidFontBase cidFontBase = font as CidFontBase;
			if (cidFontBase != null)
			{
				this.Encoding.CopyToProperty(cidFontBase.Encoding, (EncodingBaseObject cMap) => cMap.Encoding);
				this.CidFont.CopyPropertiesToFont(cidFontBase, reader, context);
			}
			base.CopyFontDescriptorPropertiesToFont(this.CidFont.FontDescriptor, font);
			base.CopyPropertiesToFont(font, reader, context);
		}

		protected override bool TryGetFontFamily(out FontFamily fontFamily)
		{
			return this.CidFont.TryGetFontFamily(out fontFamily);
		}

		protected override bool TryCreateFontFromFontFile(out FontBase font)
		{
			return this.CidFont.TryCreateFontFromFontFile(out font);
		}

		protected override FontBase CreateFontWithoutFontFile()
		{
			return this.CidFont.CreateFontWithoutFontFile();
		}

		protected override void PrepareForImport(PostScriptReader reader, IPdfImportContext context)
		{
			CidFontObject cidFontObject;
			if (this.DescendantFonts.TryGetElement<CidFontObject>(reader, context, 0, out cidFontObject))
			{
				this.cidFont = cidFontObject;
				base.PrepareForImport(reader, context);
				return;
			}
			throw new InvalidOperationException("Cannot find descendant CidFont!");
		}

		protected override bool TryCreateFontFromFontProperties(FontProperties fontProperties, out FontBase font)
		{
			return this.CidFont.TryCreateFontFromFontProperties(fontProperties, out font);
		}

		readonly ReferenceProperty<EncodingBaseObject> encoding;

		readonly ReferenceProperty<PdfArray> descendantFonts;

		CidFontObject cidFont;
	}
}
