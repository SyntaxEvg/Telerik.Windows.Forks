using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class ChartElement : ChartElementBase
	{
		public ChartElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.plotArea = base.RegisterChildElement<PlotAreaElement>("plotArea");
			this.title = base.RegisterChildElement<TitleElement>("title");
			this.legend = base.RegisterChildElement<LegendElement>("legend");
		}

		public override string ElementName
		{
			get
			{
				return "chart";
			}
		}

		public PlotAreaElement PlotAreaElement
		{
			get
			{
				return this.plotArea.Element;
			}
			set
			{
				this.plotArea.Element = value;
			}
		}

		public TitleElement TitleElement
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

		public LegendElement LegendElement
		{
			get
			{
				return this.legend.Element;
			}
			set
			{
				this.legend.Element = value;
			}
		}

		public void CopyPropertiesFrom(DocumentChart chart)
		{
			if (chart.Title != null)
			{
				base.CreateElement(this.title);
				this.TitleElement.CopyPropertiesFrom(chart.Title);
			}
			if (chart.Legend != null)
			{
				base.CreateElement(this.legend);
				this.LegendElement.CopyPropertiesFrom(chart.Legend);
			}
			base.CreateElement(this.plotArea);
			this.PlotAreaElement.CopyPropertiesFrom(chart);
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, DocumentChart chart)
		{
			if (this.TitleElement != null)
			{
				chart.Title = this.TitleElement.CreateChartTitle(context);
				base.ReleaseElement(this.title);
			}
			if (this.LegendElement != null)
			{
				chart.Legend = new Legend();
				this.LegendElement.CopyPropertiesTo(chart.Legend);
				base.ReleaseElement(this.legend);
			}
			if (this.PlotAreaElement != null)
			{
				this.PlotAreaElement.CopyPropertiesTo(context, chart);
				base.ReleaseElement(this.plotArea);
			}
		}

		readonly OpenXmlChildElement<PlotAreaElement> plotArea;

		readonly OpenXmlChildElement<TitleElement> title;

		readonly OpenXmlChildElement<LegendElement> legend;
	}
}
