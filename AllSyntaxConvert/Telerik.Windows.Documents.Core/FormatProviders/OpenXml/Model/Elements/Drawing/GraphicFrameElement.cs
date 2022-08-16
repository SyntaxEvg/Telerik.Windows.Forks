using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Shapes;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class GraphicFrameElement : DrawingElementBase
	{
		public GraphicFrameElement(OpenXmlPartsManager partsManager, OpenXmlNamespace ns)
			: base(partsManager)
		{
			this.ns = ns;
			this.nonVisualDrawingProperties = base.RegisterChildElement<NonVisualGraphicFramePropertiesElement>("nvGraphicFramePr");
			this.transform = base.RegisterChildElement<TransformElement>("xfrm", "xdr:xfrm");
			this.graphic = base.RegisterChildElement<GraphicElement>("graphic");
		}

		public override string ElementName
		{
			get
			{
				return "graphicFrame";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return this.ns;
			}
		}

		public NonVisualGraphicFramePropertiesElement NonVisualDrawingPropertiesElement
		{
			get
			{
				return this.nonVisualDrawingProperties.Element;
			}
			set
			{
				this.nonVisualDrawingProperties.Element = value;
			}
		}

		public TransformElement TransformElement
		{
			get
			{
				return this.transform.Element;
			}
			set
			{
				this.transform.Element = value;
			}
		}

		public GraphicElement GraphicElement
		{
			get
			{
				return this.graphic.Element;
			}
			set
			{
				this.graphic.Element = value;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, ChartShape chartShape)
		{
			base.CreateElement(this.nonVisualDrawingProperties);
			this.NonVisualDrawingPropertiesElement.CopyPropertiesFrom(chartShape);
			base.CreateElement(this.transform);
			this.TransformElement.CopyPropertiesFrom(context, chartShape);
			base.CreateElement(this.graphic);
			this.GraphicElement.CopyPropertiesFrom(context, chartShape);
		}

		public ShapeBase CreateShapeAndCopyPropertiesTo(IOpenXmlImportContext context)
		{
			ShapeBase shapeBase = null;
			if (this.GraphicElement != null)
			{
				shapeBase = this.GraphicElement.CreateShapeAndCopyPropertiesTo(context);
				base.ReleaseElement(this.graphic);
			}
			if (this.NonVisualDrawingPropertiesElement != null && shapeBase != null)
			{
				this.NonVisualDrawingPropertiesElement.CopyPropertiesTo(shapeBase);
				base.ReleaseElement(this.nonVisualDrawingProperties);
			}
			if (this.TransformElement != null && shapeBase != null)
			{
				this.TransformElement.CopyPropertiesTo(context, shapeBase);
				base.ReleaseElement(this.transform);
			}
			return shapeBase;
		}

		readonly OpenXmlNamespace ns;

		OpenXmlChildElement<NonVisualGraphicFramePropertiesElement> nonVisualDrawingProperties;

		OpenXmlChildElement<TransformElement> transform;

		OpenXmlChildElement<GraphicElement> graphic;
	}
}
