using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing
{
	class InlineElement : DrawingElementBase
	{
		public InlineElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.extent = base.RegisterChildElement<SizeElement>("extent");
			this.effectExtent = base.RegisterChildElement<EffectExtentElement>("effectExtent");
			this.drawingObjectNonVisualProperties = base.RegisterChildElement<NonVisualDrawingPropertiesElement>("docPr");
			this.graphics = base.RegisterChildElement<GraphicElement>("graphic");
		}

		public override string ElementName
		{
			get
			{
				return "inline";
			}
		}

		public NonVisualDrawingPropertiesElement DrawingObjectNonVisualProperties
		{
			get
			{
				return this.drawingObjectNonVisualProperties.Element;
			}
		}

		public GraphicElement GraphicElement
		{
			get
			{
				return this.graphics.Element;
			}
		}

		public SizeElement ExtentElement
		{
			get
			{
				return this.extent.Element;
			}
		}

		public EffectExtentElement EffectExtentElement
		{
			get
			{
				return this.effectExtent.Element;
			}
		}

		public void CopyPropertiesFrom(IDocxExportContext context, ShapeInlineBase shape)
		{
			base.CreateElement(this.drawingObjectNonVisualProperties);
			this.DrawingObjectNonVisualProperties.Id = shape.Shape.Id;
			this.DrawingObjectNonVisualProperties.Name = shape.Shape.Name;
			base.CreateElement(this.graphics);
			this.GraphicElement.CopyPropertiesFrom(context, shape.Shape);
			base.CreateElement(this.extent);
			this.ExtentElement.ExtentWidth = (double)((int)shape.Shape.Width);
			this.ExtentElement.ExtentHeight = (double)((int)shape.Shape.Height);
			base.CreateElement(this.effectExtent);
			this.EffectExtentElement.CopyPropertiesFrom(context, shape);
		}

		public ShapeInlineBase CreateShapeInline(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			ShapeBase shapeBase = null;
			if (this.GraphicElement != null)
			{
				shapeBase = this.GraphicElement.CreateShapeAndCopyPropertiesTo(context);
				base.ReleaseElement(this.graphics);
			}
			if (this.ExtentElement != null)
			{
				if (shapeBase != null)
				{
					shapeBase.Width = this.ExtentElement.ExtentWidth;
					shapeBase.Height = this.ExtentElement.ExtentHeight;
				}
				base.ReleaseElement(this.extent);
			}
			if (this.EffectExtentElement != null)
			{
				base.ReleaseElement(this.effectExtent);
			}
			return InlineElement.CreateShapeInlineFromShape(context, shapeBase);
		}

		static ShapeInlineBase CreateShapeInlineFromShape(IDocxImportContext context, ShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Image image = shape as Image;
			if (image != null)
			{
				return new ImageInline(context.Document, image);
			}
			return null;
		}

		readonly OpenXmlChildElement<NonVisualDrawingPropertiesElement> drawingObjectNonVisualProperties;

		readonly OpenXmlChildElement<SizeElement> extent;

		readonly OpenXmlChildElement<GraphicElement> graphics;

		readonly OpenXmlChildElement<EffectExtentElement> effectExtent;
	}
}
