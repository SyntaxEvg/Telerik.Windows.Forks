using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Charts
{
	static class DocumentChartFactory
	{
		public static DocumentChart CreateChartFromRangeOfType(Worksheet worksheet, CellRange initialRange, SeriesRangesOrientation seriesOrientation, params ChartType[] chartTypes)
		{
			bool? seriesAreVertical;
			switch (seriesOrientation)
			{
			case SeriesRangesOrientation.Automatic:
				seriesAreVertical = null;
				break;
			case SeriesRangesOrientation.Vertical:
				seriesAreVertical = new bool?(true);
				break;
			case SeriesRangesOrientation.Horizontal:
				seriesAreVertical = new bool?(false);
				break;
			default:
				throw new NotSupportedException();
			}
			RangeParseResultInfo parseResult = RangeChartParser.ParseRange(worksheet, initialRange, seriesAreVertical);
			return DocumentChartFactory.CreateChartFromParsedRange(worksheet, parseResult, chartTypes);
		}

		static DocumentChart CreateChartFromParsedRange(Worksheet worksheet, RangeParseResultInfo parseResult, params ChartType[] chartTypes)
		{
			DocumentChart documentChart = new DocumentChart();
			DocumentChartFactory.PopulateSeriesGroups(documentChart, chartTypes);
			DocumentChartFactory.PopulateSeries(worksheet, documentChart, parseResult);
			DocumentChartFactory.PopulateAxes(documentChart, parseResult, worksheet, chartTypes);
			DocumentChartFactory.PopulateLegend(documentChart);
			return documentChart;
		}

		static void PopulateSeriesGroups(DocumentChart newChart, params ChartType[] chartTypes)
		{
			if (chartTypes.Length == 0)
			{
				newChart.SeriesGroups.Add(SeriesGroupFactory.GetSeriesGroup(ChartType.Bar));
			}
			foreach (ChartType chartType in chartTypes)
			{
				newChart.SeriesGroups.Add(SeriesGroupFactory.GetSeriesGroup(chartType));
			}
		}

		static void PopulateAxes(DocumentChart newChart, RangeParseResultInfo parseResult, Worksheet worksheet, ChartType[] chartTypes)
		{
			if (!newChart.SeriesGroups.Any((SeriesGroup c) => c is ISupportAxes))
			{
				return;
			}
			Axis horizontalAxis = AxisFactory.GetHorizontalAxis(parseResult, worksheet, chartTypes);
			Axis verticalAxis = AxisFactory.GetVerticalAxis(parseResult, worksheet);
			AxisGroup primaryAxes = new AxisGroup(horizontalAxis, verticalAxis);
			newChart.PrimaryAxes = primaryAxes;
			foreach (SeriesGroup seriesGroup in newChart.SeriesGroups)
			{
				ISupportAxes supportAxes = seriesGroup as ISupportAxes;
				if (supportAxes != null)
				{
					supportAxes.AxisGroupName = AxisGroupName.Primary;
				}
			}
		}

		static void PopulateSeries(Worksheet worksheet, DocumentChart newChart, RangeParseResultInfo parseResult)
		{
			int num = (parseResult.SeriesRangesAreVertical ? parseResult.DataRange.FromIndex.ColumnIndex : parseResult.DataRange.FromIndex.RowIndex);
			int num2 = (parseResult.SeriesRangesAreVertical ? parseResult.DataRange.ToIndex.ColumnIndex : parseResult.DataRange.ToIndex.RowIndex);
			int i = num;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			List<SeriesGroup> list = newChart.SeriesGroups.ToList<SeriesGroup>();
			while (i <= num2)
			{
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					SeriesGroup seriesGroup = list[j];
					int dataRangesPerSeries = DocumentChartFactory.GetDataRangesPerSeries(parseResult, seriesGroup);
					i += dataRangesPerSeries;
					if (i > num2 + 1)
					{
						i -= dataRangesPerSeries;
					}
					else
					{
						if (!dictionary.ContainsKey(j))
						{
							dictionary[j] = 0;
						}
						Dictionary<int, int> dictionary2;
						int key;
						(dictionary2 = dictionary)[key = j] = dictionary2[key] + 1;
						flag = true;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			i = num;
			for (int k = 0; k < list.Count; k++)
			{
				SeriesGroup seriesGroup2 = list[k];
				int dataRangesPerSeries2 = DocumentChartFactory.GetDataRangesPerSeries(parseResult, seriesGroup2);
				int num3 = 1;
				if (dictionary.ContainsKey(k))
				{
					num3 = dictionary[k];
				}
				for (int l = 0; l < num3; l++)
				{
					DocumentChartFactory.AddSeries(worksheet, parseResult, i, seriesGroup2);
					i += dataRangesPerSeries2;
				}
			}
		}

		static void AddSeries(Worksheet worksheet, RangeParseResultInfo parseResult, int seriesIndex, SeriesGroup seriesGroup)
		{
			int dataRangesPerSeries = DocumentChartFactory.GetDataRangesPerSeries(parseResult, seriesGroup);
			CellRange[] array = new CellRange[dataRangesPerSeries];
			bool seriesRangesAreVertical = parseResult.SeriesRangesAreVertical;
			for (int i = 0; i < dataRangesPerSeries; i++)
			{
				int num = seriesIndex + i;
				CellIndex fromIndex = parseResult.DataRange.FromIndex;
				CellIndex toIndex = parseResult.DataRange.ToIndex;
				CellRange cellRange;
				if (seriesRangesAreVertical)
				{
					cellRange = new CellRange(fromIndex.RowIndex, num, toIndex.RowIndex, num);
				}
				else
				{
					cellRange = new CellRange(num, fromIndex.ColumnIndex, num, toIndex.ColumnIndex);
				}
				if (parseResult.DataRange.Contains(cellRange))
				{
					array[i] = cellRange;
				}
			}
			CellRange cellRange2 = null;
			CellRange seriesTitlesRange = parseResult.SeriesTitlesRange;
			if (seriesTitlesRange != null)
			{
				CellIndex fromIndex2 = seriesTitlesRange.FromIndex;
				CellIndex toIndex2 = seriesTitlesRange.ToIndex;
				if (seriesRangesAreVertical)
				{
					cellRange2 = new CellRange(fromIndex2.RowIndex, seriesIndex, toIndex2.RowIndex, seriesIndex);
				}
				else
				{
					cellRange2 = new CellRange(seriesIndex, fromIndex2.ColumnIndex, seriesIndex, toIndex2.ColumnIndex);
				}
			}
			CellRange cellRange3 = parseResult.CategoriesRange;
			if (seriesGroup.HasCategories && parseResult.CategoriesRange == null && parseResult.SeriesTitlesRange != null)
			{
				cellRange3 = parseResult.SeriesTitlesRange;
			}
			CellRange cellRange4 = array.First<CellRange>();
			if (cellRange3 != null)
			{
				if (!seriesRangesAreVertical && cellRange3.ColumnCount < cellRange4.ColumnCount)
				{
					cellRange3 = null;
				}
				else if (seriesRangesAreVertical && cellRange3.RowCount < cellRange4.RowCount)
				{
					cellRange3 = null;
				}
			}
			IChartData chartData = ((cellRange3 == null) ? null : new WorkbookFormulaChartData(worksheet, new CellRange[] { cellRange3 }));
			Title title = ((cellRange2 != null) ? new FormulaTitle(new WorkbookFormulaChartData(worksheet, new CellRange[] { cellRange2 })) : null);
			IChartData valuesData = null;
			IChartData bubbleSizesData = null;
			if (seriesGroup.HasCategories && chartData == null)
			{
				chartData = DocumentChartFactory.GenerateIncreasingAutoValues(parseResult, seriesRangesAreVertical);
			}
			if (array.Length == 1)
			{
				valuesData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[0] });
			}
			else if (array.Length == 2 && chartData == null)
			{
				if (array[0] != null && array[1] != null)
				{
					chartData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[0] });
					valuesData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[1] });
				}
				else if (array[0] != null && array[1] == null)
				{
					chartData = DocumentChartFactory.GenerateIncreasingAutoValues(parseResult, seriesRangesAreVertical);
					valuesData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[0] });
				}
			}
			else if (array.Length == 2 && chartData != null)
			{
				if (array[0] != null && array[1] != null)
				{
					valuesData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[0] });
					chartData = DocumentChartFactory.GenerateIncreasingAutoValues(parseResult, seriesRangesAreVertical);
					bubbleSizesData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[1] });
				}
				else if (array[0] != null && array[1] == null)
				{
					valuesData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[0] });
					chartData = DocumentChartFactory.GenerateIncreasingAutoValues(parseResult, seriesRangesAreVertical);
					bubbleSizesData = DocumentChartFactory.GenerateConstantAutoValues(parseResult, seriesRangesAreVertical, 1.0);
				}
			}
			else if (array.Length == 3)
			{
				if (array[0] != null && array[1] != null && array[2] != null)
				{
					chartData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[0] });
					valuesData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[1] });
					bubbleSizesData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[2] });
				}
				else if (array[0] != null && array[1] == null && array[2] == null)
				{
					chartData = DocumentChartFactory.GenerateIncreasingAutoValues(parseResult, seriesRangesAreVertical);
					valuesData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[0] });
					bubbleSizesData = DocumentChartFactory.GenerateConstantAutoValues(parseResult, seriesRangesAreVertical, 1.0);
				}
				else if (array[0] != null && array[1] != null && array[2] == null)
				{
					chartData = DocumentChartFactory.GenerateIncreasingAutoValues(parseResult, seriesRangesAreVertical);
					valuesData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[0] });
					bubbleSizesData = new WorkbookFormulaChartData(worksheet, new CellRange[] { array[1] });
				}
			}
			seriesGroup.Series.AddOverride(chartData, valuesData, bubbleSizesData, title);
		}

		static IChartData GenerateIncreasingAutoValues(RangeParseResultInfo parseResult, bool areSeriesVertical)
		{
			CellRange dataRange = parseResult.DataRange;
			int count = (areSeriesVertical ? dataRange.RowCount : dataRange.ColumnCount);
			return new NumericChartData(from p in Enumerable.Range(1, count)
				select (double)p);
		}

		static IChartData GenerateConstantAutoValues(RangeParseResultInfo parseResult, bool areSeriesVertical, double value)
		{
			CellRange dataRange = parseResult.DataRange;
			int count = (areSeriesVertical ? dataRange.RowCount : dataRange.ColumnCount);
			return new NumericChartData(Enumerable.Repeat<double>(value, count));
		}

		static int GetDataRangesPerSeries(RangeParseResultInfo parseResult, SeriesGroup seriesGroup)
		{
			int num = seriesGroup.DataRangesPerSeries;
			if (!seriesGroup.HasCategories && parseResult.CategoriesRange != null)
			{
				num--;
			}
			return num;
		}

		static void PopulateLegend(DocumentChart newChart)
		{
			newChart.Legend = new Legend();
			newChart.Legend.Position = LegendPosition.Bottom;
		}
	}
}
