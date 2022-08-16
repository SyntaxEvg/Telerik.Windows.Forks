using System;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class Type1FontStream : Type2FontStream
	{
		public Type1FontStream()
		{
			this.length2 = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Length2", true));
			this.length3 = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Length3", true));
		}

		public PdfInt Length2
		{
			get
			{
				return this.length2.GetValue();
			}
			set
			{
				this.length2.SetValue(value);
			}
		}

		public PdfInt Length3
		{
			get
			{
				return this.length3.GetValue();
			}
			set
			{
				this.length3.SetValue(value);
			}
		}

		public override FontFileInfo ToFileInfo()
		{
			FontFileInfo fontFileInfo = base.ToFileInfo();
			fontFileInfo.FileType.Value = FontFileType.FontFile;
			if (this.Length2 != null)
			{
				fontFileInfo.Length2.Value = this.Length2.Value;
			}
			if (this.Length3 != null)
			{
				fontFileInfo.Length3.Value = this.Length3.Value;
			}
			return fontFileInfo;
		}

		public override FontSource ToFontSource(FontType fontType)
		{
			switch (fontType)
			{
			case FontType.Type1:
			case FontType.Standard:
				return new Type1FontSource(base.Data);
			default:
				throw new InvalidOperationException(string.Format("Cannot create {0} font type from FontFile!", fontType));
			}
		}

		readonly DirectProperty<PdfInt> length2;

		readonly DirectProperty<PdfInt> length3;
	}
}
