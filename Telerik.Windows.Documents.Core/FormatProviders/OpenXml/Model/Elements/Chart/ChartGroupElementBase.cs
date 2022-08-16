using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	abstract class ChartGroupElementBase : ChartElementBase
	{
		public ChartGroupElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.importedSeriesInfos = new List<SeriesInfo>();
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public DocumentChart Chart
		{
			get
			{
				return this.chart;
			}
			set
			{
				this.chart = value;
			}
		}

		public SeriesGroup SeriesGroup
		{
			get
			{
				return this.seriesGroup;
			}
			set
			{
				this.seriesGroup = value;
			}
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IOpenXmlExportContext context)
		{
			foreach (SeriesBase series in this.SeriesGroup.Series)
			{
				int seriesIndex = this.GetSeriesIndex(series);
				SeriesElement seriesElement = base.CreateElement<SeriesElement>("ser");
				seriesElement.CopyPropertiesFrom(context, series, seriesIndex);
				yield return seriesElement;
			}
			yield break;
		}

		int GetSeriesIndex(SeriesBase series)
		{
			int num = 0;
			foreach (SeriesGroup seriesGroup in this.Chart.SeriesGroups)
			{
				foreach (SeriesBase seriesBase in seriesGroup.Series)
				{
					if (seriesBase == series)
					{
						return num;
					}
					num++;
				}
			}
			throw new InvalidOperationException("Series does not belong to this chart.");
		}

		protected override void OnAfterReadChildElement(IOpenXmlImportContext context, OpenXmlElementBase childElement)
		{
			if (childElement.ElementName == "ser")
			{
				SeriesElement seriesElement = childElement as SeriesElement;
				SeriesInfo seriesInfo = new SeriesInfo();
				seriesElement.CopyPropertiesTo(context, seriesInfo);
				this.importedSeriesInfos.Add(seriesInfo);
			}
		}

		public virtual void CopyPropertiesTo(IOpenXmlImportContext context, SeriesGroup seriesGroup)
		{
			foreach (SeriesInfo seriesInfo in this.importedSeriesInfos)
			{
				SeriesBase seriesBase = seriesGroup.Series.Add();
				seriesBase.Title = seriesInfo.Title;
				seriesBase.HorizontalSeriesData = seriesInfo.Categories;
				seriesBase.VerticalSeriesData = seriesInfo.Values;
				seriesBase.Outline = seriesInfo.Outline;
				BubbleSeries bubbleSeries = seriesBase as BubbleSeries;
				if (bubbleSeries != null)
				{
					bubbleSeries.BubbleSizes = seriesInfo.BubbleSizes;
				}
				ISupportMarker supportMarker = seriesBase as ISupportMarker;
				if (supportMarker != null)
				{
					supportMarker.Marker = seriesInfo.Marker;
				}
				ISupportSmooth supportSmooth = seriesBase as ISupportSmooth;
				if (supportSmooth != null)
				{
					supportSmooth.IsSmooth = seriesInfo.IsSmooth;
				}
			}
		}

		protected override void ClearOverride()
		{
			this.importedSeriesInfos.Clear();
			this.chart = null;
			this.seriesGroup = null;
		}

		readonly List<SeriesInfo> importedSeriesInfos;

		DocumentChart chart;

		SeriesGroup seriesGroup;
	}
}
