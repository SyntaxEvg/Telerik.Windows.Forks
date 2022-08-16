using System;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class BubbleChartElement : ChartGroupWithAxesElementBase<BubbleSeriesGroup>
	{
		public BubbleChartElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "bubbleChart";
			}
		}

		public void CopyPropertiesFrom(BubbleSeriesGroup seriesGroup)
		{
			base.SeriesGroup = seriesGroup;
		}
	}
}
