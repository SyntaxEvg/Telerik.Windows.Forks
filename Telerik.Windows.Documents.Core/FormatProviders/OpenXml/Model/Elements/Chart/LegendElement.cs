using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class LegendElement : ChartElementBase
	{
		public LegendElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.legendPosition = base.RegisterChildElement<LegendPositionElement>("legendPos");
			this.overlay = base.RegisterChildElement<OverlayElement>("overlay");
		}

		public override string ElementName
		{
			get
			{
				return "legend";
			}
		}

		public LegendPositionElement LegendPositionElement
		{
			get
			{
				return this.legendPosition.Element;
			}
			set
			{
				this.legendPosition.Element = value;
			}
		}

		public OverlayElement OverlayElement
		{
			get
			{
				return this.overlay.Element;
			}
			set
			{
				this.overlay.Element = value;
			}
		}

		public void CopyPropertiesFrom(Legend chartLegend)
		{
			base.CreateElement(this.legendPosition);
			this.LegendPositionElement.Value = chartLegend.Position;
			base.CreateElement(this.overlay);
			this.OverlayElement.Value = false;
		}

		public void CopyPropertiesTo(Legend legend)
		{
			if (this.LegendPositionElement != null)
			{
				legend.Position = this.LegendPositionElement.Value;
				base.ReleaseElement(this.legendPosition);
			}
			if (this.OverlayElement != null)
			{
				base.ReleaseElement(this.overlay);
			}
		}

		readonly OpenXmlChildElement<LegendPositionElement> legendPosition;

		readonly OpenXmlChildElement<OverlayElement> overlay;
	}
}
