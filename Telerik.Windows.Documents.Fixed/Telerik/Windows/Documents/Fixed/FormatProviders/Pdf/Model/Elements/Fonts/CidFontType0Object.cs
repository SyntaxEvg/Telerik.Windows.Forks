using System;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class CidFontType0Object : CidFontObject
	{
		internal override bool TryCreateFontFromFontFile(out FontBase font)
		{
			FontStream fontFile = base.FontDescriptor.FontFile3;
			if (fontFile != null)
			{
				FontSource fontSource = fontFile.ToFontSource(FontType.CidType0Font);
				font = new CidType0Font(base.BaseFont.Value, fontSource);
				font.FontFileInfo.Value = fontFile.ToFileInfo();
				return true;
			}
			font = null;
			return false;
		}

		internal override FontBase CreateFontWithoutFontFile()
		{
			return new CidType0Font(base.BaseFont.Value, EmptyFontSource.Instance);
		}

		internal override bool TryCreateFontFromFontProperties(FontProperties fontProperties, out FontBase font)
		{
			font = null;
			return false;
		}
	}
}
