using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Charts
{
	static class SeriesGroupFactory
	{
		static SeriesGroupFactory()
		{
			SeriesGroupFactory.seriesGroupConstructors.Add(ChartType.Bar, () => new BarSeriesGroup
			{
				BarDirection = BarDirection.Bar
			});
			SeriesGroupFactory.seriesGroupConstructors.Add(ChartType.Column, () => new BarSeriesGroup
			{
				BarDirection = BarDirection.Column
			});
			SeriesGroupFactory.seriesGroupConstructors.Add(ChartType.Line, () => new LineSeriesGroup());
			SeriesGroupFactory.seriesGroupConstructors.Add(ChartType.Pie, () => new PieSeriesGroup());
			SeriesGroupFactory.seriesGroupConstructors.Add(ChartType.Area, () => new AreaSeriesGroup());
			SeriesGroupFactory.seriesGroupConstructors.Add(ChartType.Doughnut, () => new DoughnutSeriesGroup
			{
				HoleSizePercent = SpreadsheetDefaultValues.DoughnutDefaultHoleSize
			});
			SeriesGroupFactory.seriesGroupConstructors.Add(ChartType.Scatter, () => new ScatterSeriesGroup());
			SeriesGroupFactory.seriesGroupConstructors.Add(ChartType.Bubble, () => new BubbleSeriesGroup());
		}

		public static SeriesGroup GetSeriesGroup(ChartType chartType)
		{
			if (!SeriesGroupFactory.seriesGroupConstructors.ContainsKey(chartType))
			{
				throw new NotSupportedException("This chart type is not supported.");
			}
			return SeriesGroupFactory.seriesGroupConstructors[chartType]();
		}

		static Dictionary<ChartType, Func<SeriesGroup>> seriesGroupConstructors = new Dictionary<ChartType, Func<SeriesGroup>>();
	}
}
