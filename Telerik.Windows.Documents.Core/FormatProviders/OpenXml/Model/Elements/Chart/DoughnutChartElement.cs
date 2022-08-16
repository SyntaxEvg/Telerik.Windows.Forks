using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class DoughnutChartElement : ChartGroupElementBase<DoughnutSeriesGroup>
	{
		public DoughnutChartElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.holeSize = base.RegisterChildElement<HoleSizeElement>("holeSize");
		}

		public override string ElementName
		{
			get
			{
				return "doughnutChart";
			}
		}

		public HoleSizeElement HoleSizeElement
		{
			get
			{
				return this.holeSize.Element;
			}
			set
			{
				this.holeSize.Element = value;
			}
		}

		public void CopyPropertiesFrom()
		{
			base.CreateElement(this.holeSize);
			this.HoleSizeElement.Value = base.SeriesGroup.HoleSizePercent;
		}

		public override void CopyPropertiesTo(IOpenXmlImportContext context, SeriesGroup seriesGroup)
		{
			base.CopyPropertiesTo(context, seriesGroup);
			DoughnutSeriesGroup doughnutSeriesGroup = seriesGroup as DoughnutSeriesGroup;
			if (this.HoleSizeElement != null)
			{
				doughnutSeriesGroup.HoleSizePercent = this.HoleSizeElement.Value;
				base.ReleaseElement(this.holeSize);
			}
		}

		readonly OpenXmlChildElement<HoleSizeElement> holeSize;
	}
}
