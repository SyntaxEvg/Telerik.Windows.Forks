using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class ChartSpaceElement : OpenXmlPartRootElementBase
	{
		public ChartSpaceElement(OpenXmlPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
			this.roundedCorners = base.RegisterChildElement<RoundedCornersElement>("roundedCorners");
			this.chart = base.RegisterChildElement<ChartElement>("chart", "c:chart");
			this.shapeProperties = base.RegisterChildElement<ShapePropertiesElement>("spPr", "c:spPr");
		}

		public override string ElementName
		{
			get
			{
				return "chartSpace";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.ChartDrawingMLNamespace;
			}
		}

		public RoundedCornersElement RoundedCornersElement
		{
			get
			{
				return this.roundedCorners.Element;
			}
			set
			{
				this.roundedCorners.Element = value;
			}
		}

		public ChartElement ChartElement
		{
			get
			{
				return this.chart.Element;
			}
			set
			{
				this.chart.Element = value;
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

		protected override void OnBeforeWrite(IOpenXmlExportContext context)
		{
			ChartShape chartForChartPart = context.GetChartForChartPart(base.Part as ChartPart);
			base.CreateElement(this.chart);
			this.ChartElement.CopyPropertiesFrom(chartForChartPart.Chart);
			base.CreateElement(this.roundedCorners);
			this.RoundedCornersElement.Value = false;
			if (chartForChartPart.Fill != null || chartForChartPart.Outline != null)
			{
				base.CreateElement(this.shapeProperties);
				this.ShapePropertiesElement.CopyPropertiesFrom(context, chartForChartPart);
			}
		}

		protected override void OnAfterRead(IOpenXmlImportContext context)
		{
			ChartShape chartForChartPart = context.GetChartForChartPart(base.Part as ChartPart);
			if (chartForChartPart == null)
			{
				return;
			}
			if (this.ChartElement != null)
			{
				this.ChartElement.CopyPropertiesTo(context, chartForChartPart.Chart);
				base.ReleaseElement(this.chart);
			}
			if (this.RoundedCornersElement != null)
			{
				base.ReleaseElement(this.roundedCorners);
			}
			if (this.ShapePropertiesElement != null)
			{
				this.ShapePropertiesElement.CopyPropertiesTo(context, chartForChartPart);
				base.ReleaseElement(this.shapeProperties);
			}
		}

		readonly OpenXmlChildElement<RoundedCornersElement> roundedCorners;

		readonly OpenXmlChildElement<ChartElement> chart;

		readonly OpenXmlChildElement<ShapePropertiesElement> shapeProperties;
	}
}
