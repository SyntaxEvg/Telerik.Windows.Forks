using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public interface IFontProperties
	{
		FontFamily FontFamily { get; }

		double FontSize { get; }

		bool IsBold { get; }

		bool IsItalic { get; }

		UnderlineType Underline { get; }

		ThemableColor ForeColor { get; }

		FontWeight FontWeight { get; }

		FontStyle FontStyle { get; }

		bool IsMonospaced { get; }
	}
}
