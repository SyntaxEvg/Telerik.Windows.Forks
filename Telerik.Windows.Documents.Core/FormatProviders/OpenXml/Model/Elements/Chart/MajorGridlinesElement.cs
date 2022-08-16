using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.Model.Drawing.Shapes;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class MajorGridlinesElement : ChartElementBase
	{
		public MajorGridlinesElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.shapeProperties = base.RegisterChildElement<ShapePropertiesElement>("spPr", "c:spPr");
		}

		public override string ElementName
		{
			get
			{
				return "majorGridlines";
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

		public void CopyPropertiesFrom(IOpenXmlExportContext context, ChartLine majorGridlines)
		{
			base.CreateElement(this.shapeProperties);
			this.ShapePropertiesElement.CopyPropertiesFrom(context, majorGridlines);
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, ChartLine majorGridlines)
		{
			if (this.ShapePropertiesElement != null)
			{
				this.ShapePropertiesElement.CopyPropertiesTo(context, majorGridlines);
				base.ReleaseElement(this.shapeProperties);
			}
		}

		readonly OpenXmlChildElement<ShapePropertiesElement> shapeProperties;
	}
}
