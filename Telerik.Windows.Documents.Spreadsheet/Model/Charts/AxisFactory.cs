using System;
using Telerik.Windows.Documents.Model.Drawing.Charts;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Charts
{
	static class AxisFactory
	{
		public static Axis GetHorizontalAxis(RangeParseResultInfo parseResult, Worksheet worksheet, ChartType[] chartTypes)
		{
			CellRange categoriesRange = parseResult.CategoriesRange;
			bool seriesRangesAreVertical = parseResult.SeriesRangesAreVertical;
			bool flag = categoriesRange != null && (seriesRangesAreVertical ? (categoriesRange.ColumnCount == 1) : (categoriesRange.RowCount == 1));
			bool flag2 = false;
			if (flag)
			{
				bool flag3 = false;
				bool flag4 = false;
				for (int i = categoriesRange.FromIndex.RowIndex; i <= categoriesRange.ToIndex.RowIndex; i++)
				{
					for (int j = categoriesRange.FromIndex.ColumnIndex; j <= categoriesRange.ToIndex.ColumnIndex; j++)
					{
						ICellValue value = worksheet.Cells[i, j].GetValue().Value;
						CellValueFormat value2 = worksheet.Cells[i, j].GetFormat().Value;
						bool flag5 = value.ValueType == CellValueType.Number && value2.FormatStringInfo.Category == FormatStringCategory.Date;
						if (!flag5 && value.ValueType != CellValueType.Empty)
						{
							flag3 = true;
						}
						if (flag5)
						{
							flag4 = true;
						}
					}
				}
				flag2 = !flag3 && flag4;
			}
			Axis axis;
			if (flag2)
			{
				axis = new DateAxis();
			}
			else
			{
				AxesType axesType = AxesType.CategoriesValues;
				foreach (ChartType seriesType in chartTypes)
				{
					AxesType axesType2 = ChartTypeToAxesTypeMapper.GetAxesType(seriesType);
					if (axesType2 != AxesType.CategoriesValues)
					{
						axesType = axesType2;
						break;
					}
				}
				if (axesType == AxesType.XValuesYValues)
				{
					axis = new ValueAxis();
				}
				else
				{
					axis = new CategoryAxis();
				}
			}
			if (categoriesRange != null)
			{
				CellValueFormat value3 = worksheet.Cells[categoriesRange.FromIndex].GetFormat().Value;
				axis.NumberFormat = value3.FormatString;
			}
			AxisFactory.AddDefaultShapePropertiesToAxis(axis);
			return axis;
		}

		public static Axis GetVerticalAxis(RangeParseResultInfo parseResult, Worksheet worksheet)
		{
			CellValueFormat value = worksheet.Cells[parseResult.DataRange.FromIndex].GetFormat().Value;
			Axis axis = new ValueAxis();
			axis.NumberFormat = value.FormatString;
			AxisFactory.AddDefaultShapePropertiesToAxis(axis);
			AxisFactory.AddDefaultGridlinesToAxis(axis);
			return axis;
		}

		static void AddDefaultShapePropertiesToAxis(Axis axis)
		{
			axis.Outline.Fill = SpreadsheetDefaultValues.ChartLinesDefaultFill;
			axis.Outline.Width = new double?(SpreadsheetDefaultValues.ChartLinesDefaultWidth);
		}

		static void AddDefaultGridlinesToAxis(Axis axis)
		{
			axis.MajorGridlines.Outline.Fill = SpreadsheetDefaultValues.ChartLinesDefaultFill;
			axis.MajorGridlines.Outline.Width = new double?(SpreadsheetDefaultValues.ChartLinesDefaultWidth);
		}
	}
}
