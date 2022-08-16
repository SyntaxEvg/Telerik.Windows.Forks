using System;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Core.Fonts.Type1;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class Type3FontStream : Type1FontStream
	{
		public Type3FontStream()
		{
			this.subtype = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("Subtype", true));
		}

		public PdfName Subtype
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

		FontFileSubType FileSubType
		{
			get
			{
				Guard.ThrowExceptionIfNull<PdfName>(this.Subtype, "Subtype");
				object obj = Enum.Parse(typeof(FontFileSubType), this.Subtype.Value, true);
				return (FontFileSubType)obj;
			}
		}

		public override FontFileInfo ToFileInfo()
		{
			FontFileInfo fontFileInfo = base.ToFileInfo();
			fontFileInfo.FileType.Value = FontFileType.FontFile3;
			if (this.Subtype != null)
			{
				fontFileInfo.FileSubType.Value = this.FileSubType;
			}
			return fontFileInfo;
		}

		public override FontSource ToFontSource(FontType fontType)
		{
			switch (fontType)
			{
			case FontType.Type1:
			case FontType.Standard:
			case FontType.CidType0Font:
			{
				CFFFontSource cfffontSource = this.CreateCffFontSource(fontType);
				return cfffontSource ?? EmptyFontSource.Instance;
			}
			case FontType.TrueType:
			case FontType.CidType2Font:
				return this.CreateOpenTypeFontSource(fontType);
			default:
				throw new NotSupportedException(string.Format("Not supported font type: {0}", fontType));
			}
		}

		OpenTypeFontSource CreateOpenTypeFontSource(FontType fontType)
		{
			FontFileSubType fileSubType = this.FileSubType;
			if (fileSubType == FontFileSubType.OpenType)
			{
				return new OpenTypeFontSource(new OpenTypeFontReader(base.Data));
			}
			throw new InvalidOperationException(string.Format("Cannot create {0} font type from FontFile3 with {1} subtype!", fontType, this.FileSubType));
		}

		CFFFontSource CreateCffFontSource(FontType fontType)
		{
			CFFFontSource result;
			switch (this.FileSubType)
			{
			case FontFileSubType.Type1C:
			case FontFileSubType.CIDFontType0C:
				result = this.GetFontSourceFromCffFontFile();
				break;
			case FontFileSubType.OpenType:
				result = this.GetFontSourceFromOpenTypeFontSource();
				break;
			default:
				throw new NotSupportedException(string.Format("Not supported file subtype: {0}", this.FileSubType));
			}
			return result;
		}

		CFFFontSource GetFontSourceFromCffFontFile()
		{
			CFFFontFile cfffontFile = new CFFFontFile(base.Data);
			return cfffontFile.FontSource;
		}

		CFFFontSource GetFontSourceFromOpenTypeFontSource()
		{
			OpenTypeFontSource openTypeFontSource = new OpenTypeFontSource(new OpenTypeFontReader(base.Data));
			if (openTypeFontSource.Outlines == Outlines.OpenType)
			{
				return openTypeFontSource.CFF;
			}
			return null;
		}

		readonly DirectProperty<PdfName> subtype;
	}
}
