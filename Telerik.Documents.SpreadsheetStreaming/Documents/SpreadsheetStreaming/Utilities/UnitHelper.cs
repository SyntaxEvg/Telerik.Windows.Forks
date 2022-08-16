using System;

namespace Telerik.Documents.SpreadsheetStreaming.Utilities
{
	static class UnitHelper
	{
		public static double PointToDip(double value)
		{
			return value * UnitHelper.DPI / UnitHelper.PointToInch;
		}

		public static double DipToPoint(double value)
		{
			return value * UnitHelper.PointToInch / UnitHelper.DPI;
		}

		public static double ExcelColumnWidthToPixelWidth(double width)
		{
			double num = Math.Floor(DefaultValues.MaxDigitWidthInNormalStyle);
			return Math.Floor((256.0 * width + Math.Floor(128.0 / num)) / 256.0 * num);
		}

		public static double PixelWidthToExcelColumnWidth(double pixels)
		{
			double maxDigitWidth = Math.Floor(DefaultValues.MaxDigitWidthInNormalStyle);
			double charCount = UnitHelper.PixelsToCharacterCount(pixels);
			return UnitHelper.CharacterCountToExcelColumnWidth(charCount, maxDigitWidth);
		}

		public static double CharactersCountToPixelWidth(double width)
		{
			double num = Math.Floor(DefaultValues.MaxDigitWidthInNormalStyle);
			return Math.Floor((256.0 * width + Math.Floor(128.0 / num)) / 256.0 * num);
		}

		internal static double PixelsToCharacterCount(double pixels)
		{
			double num = Math.Floor(DefaultValues.MaxDigitWidthInNormalStyle);
			double num2 = DefaultValues.LeftCellMargin + DefaultValues.RightCellMargin;
			return Math.Floor((pixels - num2) / num * 100.0 + 0.5) / 100.0;
		}

		static double CharacterCountToExcelColumnWidth(double charCount, double maxDigitWidth)
		{
			double num = DefaultValues.LeftCellMargin + DefaultValues.RightCellMargin;
			return Math.Floor((charCount * maxDigitWidth + num) / maxDigitWidth * 256.0) / 256.0;
		}

		static readonly double PointToInch = 72.0;

		static readonly double DPI = 96.0;
	}
}
