using System;
using System.Windows;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class FontsHelper
	{
		internal static void GetFontFamily(string fontName, out string fontFamily, out FontWeight fontWeight, out FontStyle fontStyle)
		{
			Guard.ThrowExceptionIfNullOrEmpty(fontName, "fontName");
			if (!PredefinedFontFamilies.TryGetFontFamily(fontName, out fontFamily))
			{
				fontFamily = FontsHelper.StripFontName(fontName);
			}
			string text = FontsHelper.GetFontStylesFromFontName(fontName).ToLower();
			fontWeight = FontWeights.Normal;
			if (FontsHelper.IsBold(text))
			{
				fontWeight = FontWeights.Bold;
			}
			else if (text.Contains("black"))
			{
				fontWeight = FontWeights.Black;
			}
			fontStyle = FontStyles.Normal;
			if (FontsHelper.IsItalic(text))
			{
				fontStyle = FontStyles.Italic;
			}
		}

		internal static string StripFontName(string fontName)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNullOrEmpty(fontName, "fontName");
			int num = fontName.IndexOf("+");
			if (num > 0)
			{
				return fontName.Substring(num + 1).Split(global::Telerik.Windows.Documents.Core.PostScript.Data.Characters.FontFamilyDelimiters)[0];
			}
			return fontName.Split(global::Telerik.Windows.Documents.Core.PostScript.Data.Characters.FontFamilyDelimiters)[0];
		}

		internal static bool IsBold(string styles)
		{
			return !string.IsNullOrEmpty(styles) && (styles.Contains("bold") || styles.Contains("bd"));
		}

		internal static bool IsItalic(string styles)
		{
			return !string.IsNullOrEmpty(styles) && (styles.Contains("it") || styles.Contains("obl"));
		}

		private static string GetFontStylesFromFontName(string fontName)
		{
			string[] array = fontName.Split(global::Telerik.Windows.Documents.Core.PostScript.Data.Characters.FontFamilyDelimiters);
			if (array.Length > 1)
			{
				return array[1];
			}
			return string.Empty;
		}
	}
}
