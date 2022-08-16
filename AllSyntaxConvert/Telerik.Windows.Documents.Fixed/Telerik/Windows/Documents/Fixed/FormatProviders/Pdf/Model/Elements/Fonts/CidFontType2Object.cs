using System;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class CidFontType2Object : CidFontObject
	{
		internal override bool TryCreateFontFromFontFile(out FontBase font)
		{
			FontStream fontStream = null;
			if (base.FontDescriptor != null)
			{
				fontStream = base.FontDescriptor.FontFile2 ?? base.FontDescriptor.FontFile3;
			}
			if (fontStream != null)
			{
				FontSource fontSource = fontStream.ToFontSource(FontType.CidType2Font);
				font = new CidType2Font(base.BaseFont.Value, fontSource);
				font.FontFileInfo.Value = fontStream.ToFileInfo();
				return true;
			}
			font = null;
			return false;
		}

		internal override FontBase CreateFontWithoutFontFile()
		{
			return new CidType2Font(base.BaseFont.Value, EmptyFontSource.Instance);
		}

		internal override bool TryCreateFontFromFontProperties(FontProperties fontProperties, out FontBase font)
		{
			return FontsRepository.TryCreateFont(fontProperties.FontFamily, fontProperties.FontStyle, fontProperties.FontWeight, out font);
		}
	}
}
