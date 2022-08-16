using System;

namespace Telerik.Documents.SpreadsheetStreaming.Utilities
{
	static class DefaultValues
	{
		public static readonly double MaxDigitWidthInNormalStyle = 7.433555555555556;

		public static readonly double DefaultColumnWidthInCharacters = 8.43;

		public static readonly double DefaultRowHeightInPoints = 15.0;

		public static readonly int RowCount = 1048576;

		public static readonly int ColumnCount = 16384;

		public static readonly double LeftCellMargin = 2.0 + DefaultValues.CellBorderDefaultThickness / 2.0;

		public static readonly double RightCellMargin = 2.0 + DefaultValues.CellBorderDefaultThickness / 2.0;

		static readonly double CellBorderDefaultThickness = 1.0;
	}
}
