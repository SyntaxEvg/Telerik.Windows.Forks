using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class PresetGeometryElement : DrawingElementBase
	{
		public PresetGeometryElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.presetShape = base.RegisterAttribute<string>("prst", true);
		}

		public override string ElementName
		{
			get
			{
				return "prstGeom";
			}
		}

		public string PresetShape
		{
			get
			{
				return this.presetShape.Value;
			}
			set
			{
				this.presetShape.Value = value;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, ShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeBase>(shape, "shape");
			this.PresetShape = "rect";
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, ShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeBase>(shape, "shape");
		}

		readonly OpenXmlAttribute<string> presetShape;
	}
}
