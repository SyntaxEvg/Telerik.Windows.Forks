using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Pictures;
using Telerik.Windows.Documents.Model.Drawing.Charts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class GraphicDataElement : DrawingElementBase
	{
		public GraphicDataElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.uri = base.RegisterAttribute<string>("uri", true);
			this.picture = base.RegisterChildElement<PictureElement>("pic");
			this.chartReference = base.RegisterChildElement<ChartReferenceElement>("chart", "chartReference");
		}

		public override string ElementName
		{
			get
			{
				return "graphicData";
			}
		}

		public string Uri
		{
			get
			{
				return this.uri.Value;
			}
			set
			{
				this.uri.Value = value;
			}
		}

		public PictureElement PictureElement
		{
			get
			{
				return this.picture.Element;
			}
		}

		public ChartReferenceElement ChartReferenceElement
		{
			get
			{
				return this.chartReference.Element;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, ShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeBase>(shape, "shape");
			Image image = shape as Image;
			if (image != null)
			{
				base.CreateElement(this.picture);
				this.Uri = this.PictureElement.Namespace.Value;
				this.PictureElement.CopyPropertiesFrom(context, image);
			}
			ChartShape chartShape = shape as ChartShape;
			if (chartShape != null)
			{
				base.CreateElement(this.chartReference);
				this.Uri = this.ChartReferenceElement.Namespace.Value;
				this.ChartReferenceElement.CopyPropertiesFrom(context, chartShape);
			}
		}

		public ShapeBase CreateShapeAndCopyPropertiesTo(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			ShapeBase result = null;
			if (this.PictureElement != null)
			{
				Image image = new Image();
				this.PictureElement.CopyPropertiesTo(context, image);
				base.ReleaseElement(this.picture);
				result = image;
			}
			if (this.ChartReferenceElement != null)
			{
				ChartShape chartShape = new ChartShape();
				chartShape.Chart = new DocumentChart();
				this.ChartReferenceElement.CopyPropertiesTo(context, chartShape);
				base.ReleaseElement(this.chartReference);
				result = chartShape;
			}
			return result;
		}

		readonly OpenXmlAttribute<string> uri;

		readonly OpenXmlChildElement<PictureElement> picture;

		readonly OpenXmlChildElement<ChartReferenceElement> chartReference;
	}
}
