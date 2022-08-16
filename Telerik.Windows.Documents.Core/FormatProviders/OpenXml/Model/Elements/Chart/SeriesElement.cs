using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class SeriesElement : ChartElementBase
	{
		public SeriesElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.index = base.RegisterChildElement<SeriesIndexElement>("idx");
			this.order = base.RegisterChildElement<SeriesOrderElement>("order");
			this.title = base.RegisterChildElement<TxElement>("tx", "seriestx");
			this.shapeProperties = base.RegisterChildElement<ShapePropertiesElement>("spPr", "c:spPr");
			this.marker = base.RegisterChildElement<MarkerElement>("marker");
			this.categories = base.RegisterChildElement<SeriesCategoriesElement>("cat");
			this.values = base.RegisterChildElement<SeriesValuesElement>("val");
			this.xValues = base.RegisterChildElement<SeriesXValuesElement>("xVal");
			this.yValues = base.RegisterChildElement<SeriesYValuesElement>("yVal");
			this.bubbleSize = base.RegisterChildElement<SeriesBubbleSizeElement>("bubbleSize");
			this.smooth = base.RegisterChildElement<SmoothElement>("smooth");
		}

		public override string ElementName
		{
			get
			{
				return "ser";
			}
		}

		public SeriesIndexElement Index
		{
			get
			{
				return this.index.Element;
			}
			set
			{
				this.index.Element = value;
			}
		}

		public SeriesOrderElement Order
		{
			get
			{
				return this.order.Element;
			}
			set
			{
				this.order.Element = value;
			}
		}

		public SeriesCategoriesElement SeriesCategoriesElement
		{
			get
			{
				return this.categories.Element;
			}
			set
			{
				this.categories.Element = value;
			}
		}

		public SeriesValuesElement SeriesValuesElement
		{
			get
			{
				return this.values.Element;
			}
			set
			{
				this.values.Element = value;
			}
		}

		public SeriesXValuesElement SeriesXValuesElement
		{
			get
			{
				return this.xValues.Element;
			}
			set
			{
				this.xValues.Element = value;
			}
		}

		public SeriesYValuesElement SeriesYValuesElement
		{
			get
			{
				return this.yValues.Element;
			}
			set
			{
				this.yValues.Element = value;
			}
		}

		public SeriesBubbleSizeElement SeriesBubbleSizeElement
		{
			get
			{
				return this.bubbleSize.Element;
			}
			set
			{
				this.bubbleSize.Element = value;
			}
		}

		public TxElement TxElement
		{
			get
			{
				return this.title.Element;
			}
			set
			{
				this.title.Element = value;
			}
		}

		public SmoothElement SmoothElement
		{
			get
			{
				return this.smooth.Element;
			}
			set
			{
				this.smooth.Element = value;
			}
		}

		public ShapePropertiesElement ShapePropertiesElement
		{
			get
			{
				return this.shapeProperties.Element;
			}
			set
			{
				this.shapeProperties.Element = value;
			}
		}

		public MarkerElement MarkerElement
		{
			get
			{
				return this.marker.Element;
			}
			set
			{
				this.marker.Element = value;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, SeriesBase series, int index)
		{
			base.CreateElement(this.index);
			this.Index.Value = index;
			base.CreateElement(this.order);
			this.Order.Value = index;
			base.CreateElement(this.shapeProperties);
			this.ShapePropertiesElement.CopyPropertiesFrom(context, series);
			if (series.Title != null)
			{
				base.CreateElement(this.title);
				this.TxElement.CopyPropertiesFrom(series.Title);
			}
			ISupportMarker supportMarker = series as ISupportMarker;
			if (supportMarker != null && supportMarker.Marker != null)
			{
				base.CreateElement(this.marker);
				this.MarkerElement.CopyPropertiesFrom(context, supportMarker.Marker);
			}
			this.CopyPropertiesFromCategorySeries(series);
			this.CopyPropertiesFromPointSeries(series);
			this.CopyPropertiesFromBubbleSeries(series);
			ISupportSmooth supportSmooth = series as ISupportSmooth;
			if (supportSmooth != null)
			{
				base.CreateElement(this.smooth);
				this.SmoothElement.Value = supportSmooth.IsSmooth;
			}
		}

		void CopyPropertiesFromCategorySeries(SeriesBase series)
		{
			if (!(series is CategorySeriesBase))
			{
				return;
			}
			if (series.HorizontalSeriesData != null)
			{
				base.CreateElement(this.categories);
				this.SeriesCategoriesElement.CopyPropertiesFrom(series.HorizontalSeriesData);
			}
			if (series.VerticalSeriesData != null)
			{
				base.CreateElement(this.values);
				this.SeriesValuesElement.CopyPropertiesFrom(series.VerticalSeriesData);
			}
		}

		void CopyPropertiesFromPointSeries(SeriesBase series)
		{
			if (!(series is PointSeriesBase))
			{
				return;
			}
			if (series.HorizontalSeriesData != null)
			{
				base.CreateElement(this.xValues);
				this.SeriesXValuesElement.CopyPropertiesFrom(series.HorizontalSeriesData);
			}
			if (series.VerticalSeriesData != null)
			{
				base.CreateElement(this.yValues);
				this.SeriesYValuesElement.CopyPropertiesFrom(series.VerticalSeriesData);
			}
		}

		void CopyPropertiesFromBubbleSeries(SeriesBase series)
		{
			BubbleSeries bubbleSeries = series as BubbleSeries;
			if (bubbleSeries == null)
			{
				return;
			}
			if (bubbleSeries.BubbleSizes != null)
			{
				base.CreateElement(this.bubbleSize);
				this.SeriesBubbleSizeElement.CopyPropertiesFrom(bubbleSeries.BubbleSizes);
			}
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, SeriesInfo seriesInfo)
		{
			if (this.Order != null)
			{
				base.ReleaseElement(this.order);
			}
			if (this.Index != null)
			{
				base.ReleaseElement(this.index);
			}
			if (this.TxElement != null)
			{
				Title title = this.TxElement.CreateChartTitle(context);
				seriesInfo.Title = title;
				base.ReleaseElement(this.title);
			}
			if (this.SeriesCategoriesElement != null)
			{
				IChartData chartData = this.SeriesCategoriesElement.CreateChartData(context);
				seriesInfo.Categories = chartData;
				base.ReleaseElement(this.categories);
			}
			if (this.SeriesValuesElement != null)
			{
				IChartData chartData2 = this.SeriesValuesElement.CreateChartData(context);
				seriesInfo.Values = chartData2;
				base.ReleaseElement(this.values);
			}
			if (this.MarkerElement != null)
			{
				seriesInfo.Marker = this.MarkerElement.CreateMarker(context);
				base.ReleaseElement(this.marker);
			}
			if (this.SmoothElement != null)
			{
				seriesInfo.IsSmooth = this.SmoothElement.Value;
				base.ReleaseElement(this.smooth);
			}
			if (this.SeriesXValuesElement != null)
			{
				seriesInfo.Categories = this.SeriesXValuesElement.CreateChartData(context);
				base.ReleaseElement(this.xValues);
			}
			if (this.SeriesYValuesElement != null)
			{
				seriesInfo.Values = this.SeriesYValuesElement.CreateChartData(context);
				base.ReleaseElement(this.yValues);
			}
			if (this.SeriesBubbleSizeElement != null)
			{
				seriesInfo.BubbleSizes = this.SeriesBubbleSizeElement.CreateChartData(context);
				base.ReleaseElement(this.bubbleSize);
			}
			if (this.ShapePropertiesElement != null)
			{
				this.ShapePropertiesElement.CopyPropertiesTo(context, seriesInfo);
				base.ReleaseElement(this.shapeProperties);
			}
		}

		readonly OpenXmlChildElement<SeriesIndexElement> index;

		readonly OpenXmlChildElement<SeriesOrderElement> order;

		readonly OpenXmlChildElement<SeriesCategoriesElement> categories;

		readonly OpenXmlChildElement<SeriesValuesElement> values;

		readonly OpenXmlChildElement<SeriesXValuesElement> xValues;

		readonly OpenXmlChildElement<SeriesYValuesElement> yValues;

		readonly OpenXmlChildElement<SeriesBubbleSizeElement> bubbleSize;

		readonly OpenXmlChildElement<TxElement> title;

		readonly OpenXmlChildElement<ShapePropertiesElement> shapeProperties;

		readonly OpenXmlChildElement<MarkerElement> marker;

		readonly OpenXmlChildElement<SmoothElement> smooth;
	}
}
