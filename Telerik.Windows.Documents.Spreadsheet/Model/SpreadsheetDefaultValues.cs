using System;
using System.Windows;
using Telerik.Windows.Documents.Model.Drawing.Theming;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public static class SpreadsheetDefaultValues
	{
		public const double DefaultScaleFactor = 1.0;

		public const int MinFitToPagesCount = 0;

		public const int MaxFitToPagesCount = 32767;

		public const int MinFirstPageNumber = -32765;

		public const int MaxFirstPageNumber = 32767;

		internal const double RowColumnHeadingSymbolWidth = 6.5;

		public static readonly double DefaultColumnWidth = 65.0;

		public static readonly double DefaultRowHeight = 20.0;

		public static readonly double MinColumnWidth = 0.0;

		public static readonly double MaxColumnWidth = 2000.0;

		public static readonly double MinRowHeight = 0.0;

		public static readonly double MaxRowHeight = 600.0;

		public static readonly int RowCount = 1048576;

		public static readonly int ColumnCount = 16384;

		public static readonly long CellCount = (long)SpreadsheetDefaultValues.RowCount * (long)SpreadsheetDefaultValues.ColumnCount;

		public static readonly int MaxIndent = 250;

		public static readonly double IndentStep = 10.0;

		public static readonly double LeftCellMargin = 2.0 + CellBorder.Default.Thickness / 2.0;

		public static readonly double RightCellMargin = 2.0 + CellBorder.Default.Thickness / 2.0;

		public static readonly double TotalHorizontalCellMargin = SpreadsheetDefaultValues.LeftCellMargin + SpreadsheetDefaultValues.RightCellMargin;

		public static readonly CellRange TotalWorksheetCellRange = new CellRange(0, 0, SpreadsheetDefaultValues.RowCount - 1, SpreadsheetDefaultValues.ColumnCount - 1);

		public static readonly ThemableColor DefaultForeColor = new ThemableColor(ThemeColorType.Text1, null);

		public static readonly ThemableFontFamily DefaultFontFamily = new ThemableFontFamily(ThemeFontType.Minor);

		public static readonly double DefaultFontSizeInPoints = 11.0;

		public static readonly double DefaultFontSize = UnitHelper.PointToDip(SpreadsheetDefaultValues.DefaultFontSizeInPoints);

		public static readonly string DefaultStyleName = "Normal";

		public static readonly string HyperlinkStyleName = "Hyperlink";

		public static readonly double MinScaleFactor = 0.5;

		public static readonly double MaxScaleFactor = 4.0;

		public static readonly Size DefaultScaleFactorSize = new Size(1.0, 1.0);

		public static readonly double MinPageScaleFactor = 0.1;

		public static readonly double MaxPageScaleFactor = 4.0;

		internal static readonly Size RowColumnHeadingMinimumSize = new Size(25.0, 20.0);

		internal static readonly Thickness RowColumnHeadingsPadding = new Thickness(3.0, 2.0, 3.0, 2.0);

		internal static readonly SizeI VisibleSize = new SizeI(SpreadsheetDefaultValues.ColumnCount, SpreadsheetDefaultValues.RowCount);

		internal static readonly Size ChartDefaultSize = new Size(480.0, 288.0);

		internal static readonly int DoughnutDefaultHoleSize = 75;

		internal static readonly double ChartLinesDefaultWidth = 0.75;

		internal static readonly SolidFill ChartLinesDefaultFill = new SolidFill(new ThemableColor(ThemeColorType.Text1, 0.85));

		internal static readonly SolidFill ChartDefaultFill = new SolidFill(new ThemableColor(ThemeColorType.Background1, null));
	}
}
