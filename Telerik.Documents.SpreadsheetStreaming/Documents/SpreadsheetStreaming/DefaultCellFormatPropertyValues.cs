using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model;

namespace Telerik.Documents.SpreadsheetStreaming
{
	class DefaultCellFormatPropertyValues
	{
		public static readonly SpreadBorder Border = new SpreadBorder(SpreadBorderStyle.None, new SpreadThemableColor(new SpreadColor(211, 211, 211)));

		public static readonly ISpreadFill Fill = NoneFill.Instance;

		public static readonly string NumberFormat = string.Empty;

		public static readonly SpreadThemableFontFamily FontFamily = new SpreadThemableFontFamily(SpreadThemeFontType.Minor);

		public static readonly int FontSize = 11;

		public static readonly SpreadThemableColor ForeColor = new SpreadThemableColor(SpreadThemeColorType.Dark1);

		public static readonly SpreadHorizontalAlignment HorizontalAlignment = SpreadHorizontalAlignment.General;

		public static readonly int Indent = 0;

		public static readonly bool IsBold = false;

		public static readonly bool IsItalic = false;

		public static readonly SpreadUnderlineType Underline = SpreadUnderlineType.None;

		public static readonly SpreadVerticalAlignment VerticalAlignment = SpreadVerticalAlignment.Bottom;

		public static readonly bool WrapText = false;
	}
}
