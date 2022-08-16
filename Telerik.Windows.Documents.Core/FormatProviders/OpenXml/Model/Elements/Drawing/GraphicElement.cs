using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Shapes;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class GraphicElement : DrawingElementBase
	{
		public GraphicElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.graphicData = base.RegisterChildElement<GraphicDataElement>("graphicData");
		}

		public override string ElementName
		{
			get
			{
				return "graphic";
			}
		}

		public GraphicDataElement GraphicDataElement
		{
			get
			{
				return this.graphicData.Element;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, ShapeBase shape)
		{
			base.CreateElement(this.graphicData);
			this.GraphicDataElement.CopyPropertiesFrom(context, shape);
		}

		public ShapeBase CreateShapeAndCopyPropertiesTo(IOpenXmlImportContext context)
		{
			ShapeBase result = null;
			if (this.GraphicDataElement != null)
			{
				result = this.GraphicDataElement.CreateShapeAndCopyPropertiesTo(context);
				base.ReleaseElement(this.graphicData);
			}
			return result;
		}

		readonly OpenXmlChildElement<GraphicDataElement> graphicData;
	}
}
