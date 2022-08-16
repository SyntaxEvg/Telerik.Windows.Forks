using System;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class PieChartElement : ChartGroupElementBase<PieSeriesGroup>
	{
		public PieChartElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "pieChart";
			}
		}
	}
}
