using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.Model.Drawing.Shapes;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class NonVisualGraphicFramePropertiesElement : DrawingElementBase
	{
		public NonVisualGraphicFramePropertiesElement(OpenXmlPartsManager partsManager, OpenXmlNamespace ns)
			: base(partsManager)
		{
			this.ns = ns;
			this.nonVisualDrawingProperties = base.RegisterChildElement<NonVisualDrawingPropertiesElement>("cNvPr");
			this.nonVisualGraphicFrameDrawingProperties = base.RegisterChildElement<NonVisualGraphicFrameDrawingPropertiesElement>("cNvGraphicFramePr");
		}

		public override string ElementName
		{
			get
			{
				return "nvGraphicFramePr";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return this.ns;
			}
		}

		public NonVisualDrawingPropertiesElement NonVisualDrawingPropertiesElement
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

		public NonVisualGraphicFrameDrawingPropertiesElement NonVisualGraphicFrameDrawingPropertiesElement
		{
			get
			{
				return this.nonVisualGraphicFrameDrawingProperties.Element;
			}
			set
			{
				this.nonVisualGraphicFrameDrawingProperties.Element = value;
			}
		}

		public void CopyPropertiesFrom(ShapeBase shape)
		{
			base.CreateElement(this.nonVisualDrawingProperties);
			this.NonVisualDrawingPropertiesElement.Id = shape.Id;
			this.NonVisualDrawingPropertiesElement.Name = shape.Name;
			base.CreateElement(this.nonVisualGraphicFrameDrawingProperties);
		}

		public void CopyPropertiesTo(ShapeBase shape)
		{
			if (this.NonVisualDrawingPropertiesElement != null)
			{
				shape.Id = this.NonVisualDrawingPropertiesElement.Id;
				shape.Name = this.NonVisualDrawingPropertiesElement.Name;
				base.ReleaseElement(this.nonVisualDrawingProperties);
			}
		}

		readonly OpenXmlNamespace ns;

		readonly OpenXmlChildElement<NonVisualDrawingPropertiesElement> nonVisualDrawingProperties;

		readonly OpenXmlChildElement<NonVisualGraphicFrameDrawingPropertiesElement> nonVisualGraphicFrameDrawingProperties;
	}
}
