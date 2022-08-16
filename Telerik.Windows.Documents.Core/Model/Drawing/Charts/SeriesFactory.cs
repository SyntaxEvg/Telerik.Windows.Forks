using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Model.Drawing.Theming;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	static class SeriesFactory
	{
		static SeriesFactory()
		{
			SeriesFactory.seriesConstructors.Add(SeriesType.Bar, () => new BarSeries());
			SeriesFactory.seriesConstructors.Add(SeriesType.Line, () => new LineSeries());
			SeriesFactory.seriesConstructors.Add(SeriesType.Pie, () => new PieSeries());
			SeriesFactory.seriesConstructors.Add(SeriesType.Area, () => new AreaSeries());
			SeriesFactory.seriesConstructors.Add(SeriesType.Scatter, () => new ScatterSeries
			{
				Outline = 
				{
					Fill = new NoFill()
				}
			});
			SeriesFactory.seriesConstructors.Add(SeriesType.Bubble, () => new BubbleSeries());
		}

		internal static SeriesBase GetSeries(SeriesType seriesType)
		{
			if (!SeriesFactory.seriesConstructors.ContainsKey(seriesType))
			{
				throw new NotSupportedException("This type of chart is not supported.");
			}
			return SeriesFactory.seriesConstructors[seriesType]();
		}

		internal static SeriesBase GetSeries(SeriesType seriesType, IChartData xValuesData, IChartData valuesData, IChartData bubbleSizesData, Title title = null)
		{
			SeriesBase series = SeriesFactory.GetSeries(seriesType);
			series.Title = title;
			if (seriesType == SeriesType.Bubble)
			{
				BubbleSeries bubbleSeries = (BubbleSeries)series;
				bubbleSeries.XValues = xValuesData;
				bubbleSeries.YValues = valuesData;
				bubbleSeries.BubbleSizes = bubbleSizesData;
			}
			else
			{
				series.VerticalSeriesData = valuesData;
				series.HorizontalSeriesData = xValuesData;
			}
			return series;
		}

		static Dictionary<SeriesType, Func<SeriesBase>> seriesConstructors = new Dictionary<SeriesType, Func<SeriesBase>>();
	}
}
