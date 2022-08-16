using System;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class Type1FontObject : SimpleFontObject
	{
		protected override bool TryCreateSimpleFontFromFontFile(out SimpleFont font)
		{
			FontStream fontStream = null;
			if (base.FontDescriptor != null)
			{
				fontStream = base.FontDescriptor.FontFile ?? base.FontDescriptor.FontFile3;
			}
			if (fontStream != null)
			{
				FontSource fontSource = fontStream.ToFontSource(FontType.Type1);
				font = new Type1Font(base.BaseFont.Value, fontSource);
				font.FontFileInfo.Value = fontStream.ToFileInfo();
				return true;
			}
			font = null;
			return false;
		}

		protected override SimpleFont CreateSimpleFontWithoutFontFile()
		{
			return new Type1Font(base.BaseFont.Value, EmptyFontSource.Instance);
		}

		protected override bool TryCreateFontFromFontName(out FontBase font)
		{
			StandardFont standardFont;
			if (FontsRepository.TryCreateStandardFont(base.BaseFont.Value, out standardFont))
			{
				font = standardFont;
				return true;
			}
			return base.TryCreateFontFromFontName(out font);
		}

		protected override bool TryCreateFontFromFontProperties(FontProperties fontProperties, out FontBase font)
		{
			font = null;
			return false;
		}
	}
}
