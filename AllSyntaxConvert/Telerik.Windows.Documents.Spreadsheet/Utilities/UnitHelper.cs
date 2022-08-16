using System;
using Telerik.Windows.Documents.Spreadsheet.Measurement;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	public static class UnitHelper
	{
		public static double DipToPoint(double value)
		{
			return value * UnitHelper.PointToInch / UnitHelper.DPI;
		}

		public static int DipToPointI(double value)
		{
			return (int)Math.Round(UnitHelper.DipToPoint(value));
		}

		public static double DipToPica(double value)
		{
			return value * UnitHelper.PicaToInch / UnitHelper.DPI;
		}

		public static double DipToCm(double value)
		{
			return value * UnitHelper.CmToInch / UnitHelper.DPI;
		}

		public static double DipToMm(double value)
		{
			return UnitHelper.DipToCm(value * 10.0);
		}

		public static double DipToInch(double value)
		{
			return value / UnitHelper.DPI;
		}

		public static double DipToTwip(double value)
		{
			return value * UnitHelper.TwipToInch / UnitHelper.DPI;
		}

		public static double DipToEmu(double value)
		{
			return value * UnitHelper.EmuToInch / UnitHelper.DPI;
		}

		public static int DipToTwipI(double value)
		{
			return (int)Math.Round(UnitHelper.DipToTwip(value));
		}

		public static float DipToTwipF(double value)
		{
			return (float)UnitHelper.DipToTwip(value);
		}

		public static float TwipToDipF(double value)
		{
			return (float)UnitHelper.TwipToDip(value);
		}

		public static int TwipToDipI(double value)
		{
			return (int)UnitHelper.TwipToDip(value);
		}

		public static double DipToUnit(double value, UnitType type)
		{
			switch (type)
			{
			case UnitType.Dip:
				return value;
			case UnitType.Point:
				return UnitHelper.DipToPoint(value);
			case UnitType.Pica:
				return UnitHelper.DipToPica(value);
			case UnitType.Inch:
				return UnitHelper.DipToInch(value);
			case UnitType.Mm:
				return UnitHelper.DipToMm(value);
			case UnitType.Cm:
				return UnitHelper.DipToCm(value);
			case UnitType.Twip:
				return UnitHelper.DipToTwip(value);
			default:
				throw new ArgumentException("Unknown UnitType");
			}
		}

		public static double PointToDip(double value)
		{
			return value * UnitHelper.DPI / UnitHelper.PointToInch;
		}

		public static double PicaToDip(double value)
		{
			return value * UnitHelper.DPI / UnitHelper.PicaToInch;
		}

		public static double EmuToDip(double value)
		{
			return value * UnitHelper.DPI / UnitHelper.EmuToInch;
		}

		public static double CmToDip(double value)
		{
			return value * UnitHelper.DPI / UnitHelper.CmToInch;
		}

		public static double MmToDip(double value)
		{
			return UnitHelper.CmToDip(value / 10.0);
		}

		public static double InchToDip(double value)
		{
			return value * UnitHelper.DPI;
		}

		public static double TwipToDip(double value)
		{
			return value * UnitHelper.DPI / UnitHelper.TwipToInch;
		}

		public static double UnitToDip(double value, UnitType type)
		{
			switch (type)
			{
			case UnitType.Dip:
				return value;
			case UnitType.Point:
				return UnitHelper.PointToDip(value);
			case UnitType.Pica:
				return UnitHelper.PicaToDip(value);
			case UnitType.Inch:
				return UnitHelper.InchToDip(value);
			case UnitType.Mm:
				return UnitHelper.MmToDip(value);
			case UnitType.Cm:
				return UnitHelper.CmToDip(value);
			case UnitType.Twip:
				return UnitHelper.TwipToDip(value);
			default:
				throw new ArgumentException("Unknown UnitType");
			}
		}

		static double GetMaxDigitWidthInNormalStyle(Workbook workbook)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			CellStyle cellStyle = workbook.Styles["Normal"];
			FontProperties fontProperties = default(FontProperties);
			fontProperties.FontFamily = cellStyle.FontFamily.GetActualValue(workbook.Theme);
			fontProperties.FontSize = cellStyle.FontSize;
			fontProperties.IsBold = cellStyle.IsBold;
			fontProperties.IsItalic = cellStyle.IsItalic;
			double num = 0.0;
			for (int i = 0; i <= 9; i++)
			{
				num = Math.Max(num, RadTextMeasurer.Measure(i.ToString(), fontProperties, null).Size.Width);
			}
			return num;
		}

		static double PixelsToCharacterCount(double pixels, double maxDigitWidth)
		{
			double num = SpreadsheetDefaultValues.LeftCellMargin + SpreadsheetDefaultValues.RightCellMargin;
			return Math.Floor((pixels - num) / maxDigitWidth * 100.0 + 0.5) / 100.0;
		}

		static double CharacterCountToExcelColumnWidth(double charCount, double maxDigitWidth)
		{
			double num = SpreadsheetDefaultValues.LeftCellMargin + SpreadsheetDefaultValues.RightCellMargin;
			return Math.Floor((charCount * maxDigitWidth + num) / maxDigitWidth * 256.0) / 256.0;
		}

		public static double PixelWidthToExcelColumnWidth(Workbook workbook, double pixels)
		{
			double maxDigitWidth = Math.Floor(UnitHelper.GetMaxDigitWidthInNormalStyle(workbook));
			double charCount = UnitHelper.PixelsToCharacterCount(pixels, maxDigitWidth);
			return UnitHelper.CharacterCountToExcelColumnWidth(charCount, maxDigitWidth);
		}

		public static double ExcelColumnWidthToPixelWidth(Workbook workbook, double width)
		{
			double num = Math.Floor(UnitHelper.GetMaxDigitWidthInNormalStyle(workbook));
			return Math.Floor((256.0 * width + Math.Floor(128.0 / num)) / 256.0 * num);
		}

		static readonly double DPI = 96.0;

		static readonly double CmToInch = 2.54;

		static readonly double PointToInch = 72.0;

		static readonly double PicaToInch = 6.0;

		static readonly double TwipToInch = 1440.0;

		static readonly double EmuToInch = 914400.0;
	}
}
