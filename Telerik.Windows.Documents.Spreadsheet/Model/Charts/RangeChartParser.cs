using System;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Charts
{
	static class RangeChartParser
	{
		public static RangeParseResultInfo ParseRange(Worksheet worksheet, CellRange range, bool? seriesAreVertical = null)
		{
			CellRange dataRangeFromContent;
			if (!RangeChartParser.TryGetDataRangeFromDataPositioning(worksheet, range, out dataRangeFromContent))
			{
				dataRangeFromContent = RangeChartParser.GetDataRangeFromContent(worksheet, range);
			}
			if (seriesAreVertical == null)
			{
				seriesAreVertical = new bool?(RangeChartParser.DetermineWhetherSeriesAreVertical(dataRangeFromContent));
			}
			CellRange categoriesRange;
			CellRange seriesTitlesRange;
			RangeChartParser.GetCategoriesAndSeriesTitlesRangesFromKnownDataRange(range, dataRangeFromContent, out categoriesRange, out seriesTitlesRange, seriesAreVertical.Value);
			return new RangeParseResultInfo
			{
				DataRange = dataRangeFromContent,
				SeriesTitlesRange = seriesTitlesRange,
				CategoriesRange = categoriesRange,
				SeriesRangesAreVertical = seriesAreVertical.Value
			};
		}

		static CellRange CombineRanges(CellRange categoriesRange, CellRange dataRange)
		{
			if (categoriesRange == null)
			{
				return dataRange;
			}
			int fromRowIndex = System.Math.Min(categoriesRange.FromIndex.RowIndex, dataRange.FromIndex.RowIndex);
			int fromColumnIndex = System.Math.Min(categoriesRange.FromIndex.ColumnIndex, dataRange.FromIndex.ColumnIndex);
			int toRowIndex = Math.Max(categoriesRange.ToIndex.RowIndex, dataRange.ToIndex.RowIndex);
			int toColumnIndex = Math.Max(categoriesRange.ToIndex.ColumnIndex, dataRange.ToIndex.ColumnIndex);
			return new CellRange(fromRowIndex, fromColumnIndex, toRowIndex, toColumnIndex);
		}

		static bool TryGetDataRangeFromDataPositioning(Worksheet worksheet, CellRange range, out CellRange dataRange)
		{
			dataRange = null;
			if (worksheet.Cells[range.FromIndex].GetValue().Value.ValueType != CellValueType.Empty)
			{
				return false;
			}
			int lastRowLastTextCellColumnIndex;
			int lastColumnLastTextCellRowIndex;
			RangeChartParser.GetCellIndexDeterminingEmptyRangeInTopLeftCorner(worksheet, range, out lastRowLastTextCellColumnIndex, out lastColumnLastTextCellRowIndex);
			dataRange = RangeChartParser.ConstructDataRange(range, lastRowLastTextCellColumnIndex, lastColumnLastTextCellRowIndex);
			return true;
		}

		static void GetCellIndexDeterminingEmptyRangeInTopLeftCorner(Worksheet worksheet, CellRange range, out int resultColumnIndex, out int resultRowIndex)
		{
			int rowIndex = range.FromIndex.RowIndex;
			int columnIndex = range.FromIndex.ColumnIndex;
			int rowIndex2 = range.ToIndex.RowIndex;
			int columnIndex2 = range.ToIndex.ColumnIndex;
			resultColumnIndex = columnIndex;
			resultRowIndex = rowIndex;
			for (int i = columnIndex + 1; i <= columnIndex2; i++)
			{
				if (worksheet.Cells[rowIndex, i].GetValue().Value.ValueType != CellValueType.Empty)
				{
					resultColumnIndex = i - 1;
					break;
				}
			}
			for (int j = rowIndex + 1; j <= rowIndex2; j++)
			{
				if (worksheet.Cells[j, resultColumnIndex].GetValue().Value.ValueType != CellValueType.Empty)
				{
					resultRowIndex = j - 1;
					return;
				}
			}
		}

		static CellRange GetDataRangeFromContent(Worksheet worksheet, CellRange range)
		{
			int lastRowLastTextCellColumnIndex;
			int lastColumnLastTextCellRowIndex;
			RangeChartParser.GetLastTitleCellsInLastRowAndLastColumnOfRange(worksheet, range, out lastRowLastTextCellColumnIndex, out lastColumnLastTextCellRowIndex);
			return RangeChartParser.ConstructDataRange(range, lastRowLastTextCellColumnIndex, lastColumnLastTextCellRowIndex);
		}

		static void GetLastTitleCellsInLastRowAndLastColumnOfRange(Worksheet worksheet, CellRange range, out int lastRowLastTitleCellColumnIndex, out int lastColumnLastTitleCellRowIndex)
		{
			bool datesAreConsideredTitles = !RangeChartParser.SelectionContainsDates(worksheet.Cells[range.ToIndex]);
			int rowIndex = range.FromIndex.RowIndex;
			int columnIndex = range.FromIndex.ColumnIndex;
			int rowIndex2 = range.ToIndex.RowIndex;
			int columnIndex2 = range.ToIndex.ColumnIndex;
			lastRowLastTitleCellColumnIndex = columnIndex - 1;
			lastColumnLastTitleCellRowIndex = rowIndex - 1;
			for (int i = rowIndex2; i >= rowIndex; i--)
			{
				if (RangeChartParser.CellIsNonData(worksheet, i, columnIndex2, datesAreConsideredTitles))
				{
					lastColumnLastTitleCellRowIndex = i;
					break;
				}
			}
			for (int j = columnIndex2; j >= columnIndex; j--)
			{
				if (RangeChartParser.CellIsNonData(worksheet, rowIndex2, j, datesAreConsideredTitles))
				{
					lastRowLastTitleCellColumnIndex = j;
					return;
				}
			}
		}

		static bool CellIsNonData(Worksheet worksheet, int rowIndex, int columnIndex, bool datesAreConsideredTitles)
		{
			ICellValue cellValue = worksheet.Cells[rowIndex, columnIndex].GetValue().Value;
			if (cellValue.ValueType == CellValueType.Formula)
			{
				cellValue = (cellValue as FormulaCellValue).GetResultValueAsCellValue();
			}
			return cellValue is TextCellValue || (datesAreConsideredTitles && RangeChartParser.SelectionContainsDates(worksheet.Cells[rowIndex, columnIndex]));
		}

		static bool SelectionContainsDates(CellSelection selection)
		{
			ICellValue cellValue = selection.GetValue().Value;
			if (cellValue.ValueType == CellValueType.Formula)
			{
				cellValue = (cellValue as FormulaCellValue).GetResultValueAsCellValue();
			}
			return cellValue.ValueType == CellValueType.Number && selection.GetFormat().Value.FormatStringInfo.Category == FormatStringCategory.Date;
		}

		static CellRange ConstructDataRange(CellRange range, int lastRowLastTextCellColumnIndex, int lastColumnLastTextCellRowIndex)
		{
			int fromColumnIndex = System.Math.Min(lastRowLastTextCellColumnIndex + 1, range.ToIndex.ColumnIndex);
			int fromRowIndex = System.Math.Min(lastColumnLastTextCellRowIndex + 1, range.ToIndex.RowIndex);
			return new CellRange(fromRowIndex, fromColumnIndex, range.ToIndex.RowIndex, range.ToIndex.ColumnIndex);
		}

		static bool DetermineWhetherSeriesAreVertical(CellRange dataRange)
		{
			return dataRange.RowCount > dataRange.ColumnCount;
		}

		static void GetCategoriesAndSeriesTitlesRangesFromKnownDataRange(CellRange range, CellRange dataRange, out CellRange categoriesRange, out CellRange seriesTitlesRange, bool seriesAreVertical)
		{
			CellRange topRange;
			CellRange leftRange;
			RangeChartParser.GetTopAndLeftRanges(range, dataRange, out topRange, out leftRange);
			RangeChartParser.DetermineCategoriesAndSeriesTitlesRangesPosition(topRange, leftRange, seriesAreVertical, out categoriesRange, out seriesTitlesRange);
		}

		static void GetTopAndLeftRanges(CellRange range, CellRange dataRange, out CellRange topRange, out CellRange leftRange)
		{
			int num = dataRange.FromIndex.RowIndex - range.FromIndex.RowIndex;
			int num2 = dataRange.FromIndex.ColumnIndex - range.FromIndex.ColumnIndex;
			if (num == 0)
			{
				topRange = null;
			}
			else
			{
				int fromColumnIndex = ((num2 == 0) ? range.FromIndex.ColumnIndex : (range.FromIndex.ColumnIndex + num2));
				topRange = new CellRange(range.FromIndex.RowIndex, fromColumnIndex, range.FromIndex.RowIndex + num - 1, range.ToIndex.ColumnIndex);
			}
			if (num2 == 0)
			{
				leftRange = null;
				return;
			}
			int fromRowIndex = ((num == 0) ? range.FromIndex.RowIndex : (range.FromIndex.RowIndex + num));
			leftRange = new CellRange(fromRowIndex, range.FromIndex.ColumnIndex, range.ToIndex.RowIndex, range.FromIndex.ColumnIndex + num2 - 1);
		}

		static void DetermineCategoriesAndSeriesTitlesRangesPosition(CellRange topRange, CellRange leftRange, bool seriesAreVertical, out CellRange categoriesRange, out CellRange seriesTitlesRange)
		{
			if (seriesAreVertical)
			{
				seriesTitlesRange = topRange;
				categoriesRange = leftRange;
				return;
			}
			seriesTitlesRange = leftRange;
			categoriesRange = topRange;
		}
	}
}
