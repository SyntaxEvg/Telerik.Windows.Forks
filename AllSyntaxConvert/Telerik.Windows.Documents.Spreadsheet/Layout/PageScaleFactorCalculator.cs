using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	public static class PageScaleFactorCalculator
	{
		public static Size CalculateScaleAccordingToFitToPages(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			double scaleAccordingToFitToPages = PageScaleFactorCalculator.GetScaleAccordingToFitToPages(worksheet, worksheet.UsedCellRange);
			return new Size(scaleAccordingToFitToPages, scaleAccordingToFitToPages);
		}

		public static Size CalculateScaleAccordingToFitToPages(Worksheet worksheet, IEnumerable<CellRange> includedRanges)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<IEnumerable<CellRange>>(includedRanges, "includedRanges");
			double num = 1.0;
			foreach (CellRange range in includedRanges)
			{
				double scaleAccordingToFitToPages = PageScaleFactorCalculator.GetScaleAccordingToFitToPages(worksheet, range);
				num = System.Math.Min(scaleAccordingToFitToPages, num);
			}
			return new Size(num, num);
		}

		internal static Size CalculateRowHeadingsWidthColumnHeadingsHeight(Worksheet worksheet, CellRange cellRange, Size scaleFactor)
		{
			double num = 0.0;
			double num2 = 0.0;
			WorksheetPrintOptions printOptions = worksheet.WorksheetPageSetup.PrintOptions;
			if (printOptions.PrintRowColumnHeadings)
			{
				RadWorksheetLayout worksheetLayout = worksheet.Workbook.GetWorksheetLayout(worksheet, true);
				for (int i = cellRange.FromIndex.ColumnIndex; i <= cellRange.ToIndex.ColumnIndex; i++)
				{
					num2 = Math.Max(num2, scaleFactor.Height * worksheetLayout.GetColumnHeadingSize(i).Height);
				}
				if (!printOptions.IsDraftQuality)
				{
					for (int j = cellRange.FromIndex.RowIndex; j <= cellRange.ToIndex.RowIndex; j++)
					{
						num = Math.Max(num, scaleFactor.Width * worksheetLayout.GetRowHeadingSize(cellRange, j).Width);
					}
				}
			}
			return new Size(num, num2);
		}

		static double GetScaleAccordingToFitToPages(Worksheet worksheet, CellRange range)
		{
			RadWorksheetLayout worksheetLayout = worksheet.Workbook.GetWorksheetLayout(worksheet, true);
			WorksheetPageSetup worksheetPageSetup = worksheet.WorksheetPageSetup;
			int fitToPagesWide = worksheetPageSetup.FitToPagesWide;
			int fitToPagesTall = worksheetPageSetup.FitToPagesTall;
			Size size = PageScaleFactorCalculator.CalculateRowHeadingsWidthColumnHeadingsHeight(worksheet, range, new Size(1.0, 1.0));
			double availablePageLength = worksheetPageSetup.RotatedPageSize.Width - worksheetPageSetup.Margins.Left - worksheetPageSetup.Margins.Right - size.Width;
			double availablePageLength2 = worksheetPageSetup.RotatedPageSize.Height - worksheetPageSetup.Margins.Top - worksheetPageSetup.Margins.Bottom - size.Height;
			int columnIndex = range.FromIndex.ColumnIndex;
			int columnIndex2 = range.ToIndex.ColumnIndex;
			double totalLength = worksheetLayout.GetColumnLeft(columnIndex2) - worksheetLayout.GetColumnLeft(columnIndex) + worksheetLayout.GetColumnWidth(columnIndex2);
			int rowIndex = range.FromIndex.RowIndex;
			int rowIndex2 = range.ToIndex.RowIndex;
			double totalLength2 = worksheetLayout.GetRowTop(rowIndex2) - worksheetLayout.GetRowTop(rowIndex) + worksheetLayout.GetRowHeight(rowIndex2);
			double val = 1.0;
			double val2 = 1.0;
			IEnumerable<double> columnWidths = PageScaleFactorCalculator.GetColumnWidths(range, worksheetLayout);
			IEnumerable<double> rowHeights = PageScaleFactorCalculator.GetRowHeights(range, worksheetLayout);
			if (fitToPagesWide != 0 || fitToPagesTall != 0)
			{
				if (fitToPagesWide == 0)
				{
					val2 = PageScaleFactorCalculator.GetScaleAccordingToFitToPages(rowHeights, totalLength2, availablePageLength2, fitToPagesTall);
				}
				else if (fitToPagesTall == 0)
				{
					val = PageScaleFactorCalculator.GetScaleAccordingToFitToPages(columnWidths, totalLength, availablePageLength, fitToPagesWide);
				}
				else
				{
					val = PageScaleFactorCalculator.GetScaleAccordingToFitToPages(columnWidths, totalLength, availablePageLength, fitToPagesWide);
					val2 = PageScaleFactorCalculator.GetScaleAccordingToFitToPages(rowHeights, totalLength2, availablePageLength2, fitToPagesTall);
				}
			}
			double value = System.Math.Min(val, val2);
			return MathUtility.RoundDown(value, 2);
		}

		static IEnumerable<double> GetColumnWidths(CellRange range, RadWorksheetLayout layout)
		{
			for (int col = range.FromIndex.ColumnIndex; col <= range.ToIndex.ColumnIndex; col++)
			{
				yield return layout.GetColumnWidth(col);
			}
			yield break;
		}

		static IEnumerable<double> GetRowHeights(CellRange range, RadWorksheetLayout layout)
		{
			for (int row = range.FromIndex.RowIndex; row <= range.ToIndex.RowIndex; row++)
			{
				yield return layout.GetRowHeight(row);
			}
			yield break;
		}

		static double GetScaleAccordingToFitToPages(IEnumerable<double> lengths, double totalLength, double availablePageLength, int pagesCount)
		{
			if (pagesCount == 1)
			{
				double val = availablePageLength / totalLength;
				return Math.Max(Math.Min(1.0, val), SpreadsheetDefaultValues.MinPageScaleFactor);
			}
			double num = 1.0;
			double num2 = totalLength / (double)pagesCount;
			double num3 = num2;
			int num4 = 1;
			foreach (double num5 in lengths)
			{
				double num6 = num5;
				if (num4 != pagesCount && num3 < num6)
				{
					bool flag = num3 < num6 / 2.0;
					double num7 = (flag ? (num2 - num3) : (num6 + num2 - num3));
					num = System.Math.Min(num, availablePageLength / num7);
					num4++;
					num3 = (flag ? (num2 - num6) : num2);
				}
				else
				{
					num3 -= num6;
				}
			}
			num = System.Math.Min(num, availablePageLength / (num2 - num3));
			return num;
		}
	}
}
