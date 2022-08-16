using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class ScatterChartElement : ChartGroupWithAxesElementBase<ScatterSeriesGroup>
	{
		public ScatterChartElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.scatterStyle = base.RegisterChildElement<ScatterStyleElement>("scatterStyle");
		}

		public override string ElementName
		{
			get
			{
				return "scatterChart";
			}
		}

		public ScatterStyleElement ScatterStyleElement
		{
			get
			{
				return this.scatterStyle.Element;
			}
		}

		public void CopyPropertiesFrom(ScatterSeriesGroup seriesGroup)
		{
			base.SeriesGroup = seriesGroup;
			base.CreateElement(this.scatterStyle);
			this.ScatterStyleElement.Value = seriesGroup.ScatterStyle;
		}

		readonly OpenXmlChildElement<ScatterStyleElement> scatterStyle;
	}
}
