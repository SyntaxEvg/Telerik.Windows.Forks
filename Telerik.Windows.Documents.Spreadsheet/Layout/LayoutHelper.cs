using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Measurement;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	public static class LayoutHelper
	{
		internal static Size DefaultTextMeasuringMethod(string text, FontProperties fontProperties, double? wrappingWidth)
		{
			return RadTextMeasurer.MeasureMultiline(text, fontProperties, wrappingWidth);
		}

		internal static double? GetWrappingWidth(double cellBoxWidth, double cellIndent, bool isWrapped)
		{
			double? result = null;
			if (isWrapped)
			{
				result = new double?(Math.Max(0.0, cellBoxWidth - cellIndent));
			}
			return result;
		}

		internal static bool ShouldRespectIndent(RadHorizontalAlignment horizontalAlignment)
		{
			return horizontalAlignment == RadHorizontalAlignment.Left || horizontalAlignment == RadHorizontalAlignment.Right || horizontalAlignment == RadHorizontalAlignment.Distributed;
		}

		internal static int CalculateIndent(RadHorizontalAlignment horizontalAlignment, int indent)
		{
			if (LayoutHelper.ShouldRespectIndent(horizontalAlignment))
			{
				return indent;
			}
			return 0;
		}

		public static Size CalculateCellContentSize(Worksheet worksheet, CellIndex cellIndex)
		{
			return LayoutHelper.CalculateCellContentSize(worksheet, cellIndex.RowIndex, cellIndex.ColumnIndex);
		}

		public static Size CalculateCellContentSize(Worksheet worksheet, int rowIndex, int columnIndex)
		{
			CellLayoutBox cellBox = LayoutHelper.CalculateCellLayoutBox(worksheet, rowIndex, columnIndex);
			bool value = worksheet.Cells[rowIndex, columnIndex].GetIsWrapped().Value;
			FontProperties fontProperties = worksheet.Cells.GetFontProperties(rowIndex, columnIndex, false);
			return LayoutHelper.CalculateCellContentSize(worksheet, cellBox, value, new FontProperties?(fontProperties), null);
		}

		public static CellLayoutBox CalculateCellLayoutBox(Worksheet worksheet, CellIndex cellIndex)
		{
			return LayoutHelper.CalculateCellLayoutBox(worksheet, cellIndex.RowIndex, cellIndex.ColumnIndex);
		}

		public static CellLayoutBox CalculateCellLayoutBox(Worksheet worksheet, int rowIndex, int columnIndex)
		{
			RadWorksheetLayout worksheetLayout = worksheet.Workbook.GetWorksheetLayout(worksheet, false);
			CellRange cellRange = null;
			if (worksheet.Cells.TryGetContainingMergedRange(rowIndex, columnIndex, out cellRange) && cellRange.FromIndex.RowIndex == rowIndex && cellRange.FromIndex.ColumnIndex == columnIndex)
			{
				return worksheetLayout.GetTopLeftMergedCellLayoutBox(cellRange);
			}
			return worksheetLayout.GetCellLayoutBox(rowIndex, columnIndex);
		}

		internal static Size CalculateCellContentSize(Worksheet worksheet, CellLayoutBox cellBox, bool isWrapped, FontProperties? fontProperties = null, Func<string, FontProperties, double?, Size> measureMultilineText = null)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<CellLayoutBox>(cellBox, "cellBox");
			if (fontProperties == null)
			{
				fontProperties = new FontProperties?(worksheet.Cells.GetFontProperties(cellBox.LongIndex, true));
			}
			measureMultilineText = measureMultilineText ?? new Func<string, FontProperties, double?, Size>(LayoutHelper.DefaultTextMeasuringMethod);
			CellsPropertyBag propertyBag = worksheet.Cells.PropertyBag;
			long longIndex = cellBox.LongIndex;
			RadHorizontalAlignment propertyValueRespectingStyle = propertyBag.GetPropertyValueRespectingStyle<RadHorizontalAlignment>(CellPropertyDefinitions.HorizontalAlignmentProperty, worksheet, longIndex);
			int propertyValueRespectingStyle2 = propertyBag.GetPropertyValueRespectingStyle<int>(CellPropertyDefinitions.IndentProperty, worksheet, longIndex);
			ICellValue propertyValueRespectingStyle3 = propertyBag.GetPropertyValueRespectingStyle<ICellValue>(CellPropertyDefinitions.ValueProperty, worksheet, longIndex);
			CellValueFormat propertyValueRespectingStyle4 = propertyBag.GetPropertyValueRespectingStyle<CellValueFormat>(CellPropertyDefinitions.FormatProperty, worksheet, longIndex);
			int num = LayoutHelper.CalculateIndent(propertyValueRespectingStyle, propertyValueRespectingStyle2);
			double cellIndent = (double)num * SpreadsheetDefaultValues.IndentStep;
			double? wrappingWidth = LayoutHelper.GetWrappingWidth(cellBox.Width, cellIndent, isWrapped);
			return LayoutHelper.CalculateCellContentSize(propertyValueRespectingStyle3, propertyValueRespectingStyle4, fontProperties.Value, cellIndent, wrappingWidth, measureMultilineText);
		}

		internal static Size CalculateCellContentSize(ICellValue cellValue, CellValueFormat format, FontProperties fontProperties, double cellIndent, double? wrappingWidth, Func<string, FontProperties, double?, Size> measureMultilineText)
		{
			Guard.ThrowExceptionIfNull<ICellValue>(cellValue, "cellValue");
			Guard.ThrowExceptionIfNull<FontProperties>(fontProperties, "fontProperties");
			Guard.ThrowExceptionIfNull<Func<string, FontProperties, double?, Size>>(measureMultilineText, "measureMultilineText");
			string resultValueStringIncludingInvisibleSymbols = LayoutHelper.GetResultValueStringIncludingInvisibleSymbols(cellValue, format);
			if (string.IsNullOrEmpty(resultValueStringIncludingInvisibleSymbols))
			{
				return new Size(0.0, 0.0);
			}
			Size result = measureMultilineText(resultValueStringIncludingInvisibleSymbols, fontProperties, wrappingWidth);
			result.Width += LayoutHelper.GetIndentPlusMargin(cellIndent);
			return result;
		}

		internal static string GetResultValueStringIncludingInvisibleSymbols(ICellValue value, CellValueFormat format)
		{
			string infosText;
			lock (format)
			{
				FormulaCellValue formulaCellValue = value as FormulaCellValue;
				if (formulaCellValue != null)
				{
					return LayoutHelper.GetResultValueStringIncludingInvisibleSymbols(formulaCellValue.GetResultValueAsCellValue(), format);
				}
				infosText = format.GetFormatResult(value).InfosText;
			}
			return infosText;
		}

		internal static ColumnWidth CalculateAutoColumnWidth(Worksheet worksheet, int columnIndex)
		{
			return LayoutHelper.CalculateAutoColumnWidth(worksheet, columnIndex, 0, SpreadsheetDefaultValues.RowCount - 1, false);
		}

		internal static ColumnWidth CalculateAutoColumnWidth(Worksheet worksheet, int columnIndex, int fromRowIndex, int toRowIndex, bool respectNumberValuesOnly = false)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			double? num = null;
			long fromIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(fromRowIndex, columnIndex);
			long toIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(toRowIndex, columnIndex);
			ICompressedList<ICellValue> propertyValueRespectingStyle = worksheet.Cells.PropertyBag.GetPropertyValueRespectingStyle<ICellValue>(CellPropertyDefinitions.ValueProperty, worksheet, fromIndex, toIndex);
			ICompressedList<int> propertyValueRespectingStyle2 = worksheet.Cells.PropertyBag.GetPropertyValueRespectingStyle<int>(CellPropertyDefinitions.IndentProperty, worksheet, fromIndex, toIndex);
			ICompressedList<bool> propertyValueRespectingStyle3 = worksheet.Cells.PropertyBag.GetPropertyValueRespectingStyle<bool>(CellPropertyDefinitions.IsWrappedProperty, worksheet, fromIndex, toIndex);
			ICompressedList<CellValueFormat> propertyValueRespectingStyle4 = worksheet.Cells.PropertyBag.GetPropertyValueRespectingStyle<CellValueFormat>(CellPropertyDefinitions.FormatProperty, worksheet, fromIndex, toIndex);
			bool flag = false;
			foreach (Range<long, ICellValue> range in propertyValueRespectingStyle.GetNonDefaultRanges())
			{
				if (range.Value.ValueType != CellValueType.Empty && (!respectNumberValuesOnly || range.Value.ResultValueType == CellValueType.Number))
				{
					for (long num2 = range.Start; num2 <= range.End; num2 += 1L)
					{
						CellIndex cellIndex = WorksheetPropertyBagBase.ConvertLongToCellIndex(num2);
						bool flag2 = false;
						bool flag3 = false;
						CellRange cellRange;
						if (worksheet.Cells.TryGetContainingMergedRange(cellIndex, out cellRange))
						{
							flag2 = true;
							flag3 = (cellRange.RowCount >= 1 && cellRange.ColumnCount == 1) || (cellRange.ColumnCount >= 1 && cellRange.RowCount == 1);
						}
						if (!flag2 || !flag3)
						{
							FontProperties fontProperties = worksheet.Cells.GetFontProperties(cellIndex, true);
							bool value = propertyValueRespectingStyle3.GetValue(num2);
							if (value)
							{
								flag = true;
							}
							else
							{
								double cellIndent = (double)propertyValueRespectingStyle2.GetValue(num2) * SpreadsheetDefaultValues.IndentStep;
								double width = LayoutHelper.CalculateCellContentSize(range.Value, propertyValueRespectingStyle4.GetValue(num2), fontProperties, cellIndent, null, new Func<string, FontProperties, double?, Size>(LayoutHelper.DefaultTextMeasuringMethod)).Width;
								num = new double?(Math.Max((num != null) ? num.Value : 0.0, width));
							}
						}
					}
				}
			}
			if (num != null)
			{
				num = new double?(Math.Max(num.Value, SpreadsheetDefaultValues.MinColumnWidth));
				num = new double?(Math.Min(num.Value, SpreadsheetDefaultValues.MaxColumnWidth));
			}
			else
			{
				if (!flag)
				{
					return null;
				}
				num = new double?(worksheet.Columns[columnIndex].GetWidth().Value.Value);
			}
			return new ColumnWidth(num.Value, false);
		}

		static double GetIndentPlusMargin(double cellIndent)
		{
			return cellIndent + SpreadsheetDefaultValues.TotalHorizontalCellMargin;
		}
	}
}
