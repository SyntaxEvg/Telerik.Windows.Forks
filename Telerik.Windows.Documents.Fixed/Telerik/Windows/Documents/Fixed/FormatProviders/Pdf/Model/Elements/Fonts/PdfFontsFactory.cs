using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	static class PdfFontsFactory
	{
		static PdfFontsFactory()
		{
			PdfFontsFactory.Register(FontType.Standard, () => new Type1FontObject());
			PdfFontsFactory.Register(FontType.Type1, () => new Type1FontObject());
			PdfFontsFactory.Register(FontType.TrueType, () => new TrueTypeFontObject());
			PdfFontsFactory.Register(FontType.CidType2Font, () => new Type0FontObject());
			PdfFontsFactory.Register(FontType.CidType0Font, () => new Type0FontObject());
		}

		public static FontObject CreateFont(FontType fontType)
		{
			return PdfFontsFactory.fontTypeToCreateFontFunctionMapping[fontType]();
		}

		public static FontObject CreateInstance(PdfName typeName)
		{
			Guard.ThrowExceptionIfNull<PdfName>(typeName, "typeName");
			string value;
			if ((value = typeName.Value) != null)
			{
				if (value == "Type0")
				{
					return new Type0FontObject();
				}
				if (value == "MMType1" || value == "Type1")
				{
					return new Type1FontObject();
				}
				if (value == "TrueType")
				{
					return new TrueTypeFontObject();
				}
			}
			throw new NotSupportedException("Font is not supported.");
		}

		static void Register(FontType fontType, Func<FontObject> font)
		{
			PdfFontsFactory.fontTypeToCreateFontFunctionMapping.Add(fontType, font);
		}

		public const string Type0Font = "Type0";

		public const string Type1Font = "Type1";

		public const string MMType1Font = "MMType1";

		public const string TrueTypeFont = "TrueType";

		static readonly Dictionary<FontType, Func<FontObject>> fontTypeToCreateFontFunctionMapping = new Dictionary<FontType, Func<FontObject>>();
	}
}
