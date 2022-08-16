using System;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class TrueTypeFontObject : SimpleFontObject
	{
		protected override bool TryCreateSimpleFontFromFontFile(out SimpleFont font)
		{
			FontStream fontStream = null;
			if (base.FontDescriptor != null)
			{
				fontStream = base.FontDescriptor.FontFile2 ?? base.FontDescriptor.FontFile3;
			}
			if (fontStream != null)
			{
				FontSource fontSource = fontStream.ToFontSource(FontType.TrueType);
				font = new TrueTypeFont(base.BaseFont.Value, fontSource);
				font.FontFileInfo.Value = fontStream.ToFileInfo();
				return true;
			}
			font = null;
			return false;
		}

		protected override SimpleFont CreateSimpleFontWithoutFontFile()
		{
			return new TrueTypeFont(base.BaseFont.Value, EmptyFontSource.Instance);
		}

		protected override bool TryCreateFontFromFontProperties(FontProperties fontProperties, out FontBase font)
		{
			OpenTypeFontSource openTypeFontSource = FontsRepository.FontsManager.GetOpenTypeFontSource(fontProperties);
			font = ((openTypeFontSource != null) ? new TrueTypeFont(base.BaseFont.Value, openTypeFontSource) : null);
			return font != null;
		}
	}
}
