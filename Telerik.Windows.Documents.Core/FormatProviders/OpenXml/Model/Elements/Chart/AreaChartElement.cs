using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class AreaChartElement : ChartGroupWithAxesElementBase<AreaSeriesGroup>
	{
		public AreaChartElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.grouping = base.RegisterChildElement<GroupingElement>("grouping", "lineAreaGrouping");
		}

		public override string ElementName
		{
			get
			{
				return "areaChart";
			}
		}

		public GroupingElement GroupingElement
		{
			get
			{
				return this.grouping.Element;
			}
			set
			{
				this.grouping.Element = value;
			}
		}

		public void CopyPropertiesFrom()
		{
			base.CreateElement(this.grouping);
			this.GroupingElement.Value = base.SeriesGroup.Grouping;
		}

		public override void CopyPropertiesTo(IOpenXmlImportContext context, SeriesGroup seriesGroup)
		{
			base.CopyPropertiesTo(context, seriesGroup);
			AreaSeriesGroup areaSeriesGroup = seriesGroup as AreaSeriesGroup;
			if (this.GroupingElement != null)
			{
				areaSeriesGroup.Grouping = this.GroupingElement.Value;
				base.ReleaseElement(this.grouping);
			}
		}

		readonly OpenXmlChildElement<GroupingElement> grouping;
	}
}
