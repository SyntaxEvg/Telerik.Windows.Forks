using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class BarChartElement : ChartGroupWithAxesElementBase<BarSeriesGroup>
	{
		public BarChartElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.barDirection = base.RegisterChildElement<BarDirectionElement>("barDir");
			this.grouping = base.RegisterChildElement<BarGroupingElement>("grouping", "barGrouping");
			this.overlap = base.RegisterChildElement<OverlapElement>("overlap");
		}

		public override string ElementName
		{
			get
			{
				return "barChart";
			}
		}

		public BarDirectionElement BarDirectionElement
		{
			get
			{
				return this.barDirection.Element;
			}
			set
			{
				this.barDirection.Element = value;
			}
		}

		public BarGroupingElement GroupingElement
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

		public OverlapElement OverlapElement
		{
			get
			{
				return this.overlap.Element;
			}
			set
			{
				this.overlap.Element = value;
			}
		}

		public void CopyPropertiesFrom()
		{
			base.CreateElement(this.barDirection);
			this.BarDirectionElement.Value = base.SeriesGroup.BarDirection;
			base.CreateElement(this.grouping);
			this.GroupingElement.Value = base.SeriesGroup.Grouping;
			if (base.SeriesGroup.Grouping == SeriesGrouping.Stacked || base.SeriesGroup.Grouping == SeriesGrouping.PercentStacked)
			{
				base.CreateElement(this.overlap);
				this.OverlapElement.Value = 100;
			}
		}

		public override void CopyPropertiesTo(IOpenXmlImportContext context, SeriesGroup seriesGroup)
		{
			base.CopyPropertiesTo(context, seriesGroup);
			BarSeriesGroup barSeriesGroup = seriesGroup as BarSeriesGroup;
			if (this.GroupingElement != null)
			{
				barSeriesGroup.Grouping = this.GroupingElement.Value;
				base.ReleaseElement(this.grouping);
			}
			if (this.BarDirectionElement != null)
			{
				barSeriesGroup.BarDirection = this.BarDirectionElement.Value;
				base.ReleaseElement(this.barDirection);
			}
		}

		readonly OpenXmlChildElement<BarDirectionElement> barDirection;

		readonly OpenXmlChildElement<BarGroupingElement> grouping;

		readonly OpenXmlChildElement<OverlapElement> overlap;
	}
}
