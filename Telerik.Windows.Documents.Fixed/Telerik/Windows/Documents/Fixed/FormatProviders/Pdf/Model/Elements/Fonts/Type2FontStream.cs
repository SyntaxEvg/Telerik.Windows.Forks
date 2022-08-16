using System;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class Type2FontStream : FontStream
	{
		public Type2FontStream()
		{
			this.length1 = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Length1", true));
		}

		public PdfInt Length1
		{
			get
			{
				return this.length1.GetValue();
			}
			set
			{
				this.length1.SetValue(value);
			}
		}

		public override FontFileInfo ToFileInfo()
		{
			FontFileInfo fontFileInfo = new FontFileInfo();
			fontFileInfo.FileType.Value = FontFileType.FontFile2;
			if (this.Length1 != null)
			{
				fontFileInfo.Length1.Value = this.Length1.Value;
			}
			if (base.Metadata != null)
			{
				fontFileInfo.Metadata.Value = base.Metadata.Data;
			}
			return fontFileInfo;
		}

		public override FontSource ToFontSource(FontType fontType)
		{
			switch (fontType)
			{
			case FontType.TrueType:
			case FontType.CidType2Font:
				return new OpenTypeFontSource(new OpenTypeFontReader(base.Data));
			}
			throw new InvalidOperationException(string.Format("Cannot create {0} font type from FontFile2!", fontType));
		}

		protected override void CopyPropertiesFromData()
		{
			Guard.ThrowExceptionIfNull<byte[]>(base.Data, "Data");
			this.Length1 = new PdfInt(base.Data.Length);
		}

		readonly DirectProperty<PdfInt> length1;
	}
}
