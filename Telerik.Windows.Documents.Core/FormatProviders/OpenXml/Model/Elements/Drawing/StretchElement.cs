using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class StretchElement : DrawingElementBase
	{
		public StretchElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.fillRect = base.RegisterChildElement<FillRectElement>("fillRect");
		}

		public override string ElementName
		{
			get
			{
				return "stretch";
			}
		}

		public FillRectElement FillRectElement
		{
			get
			{
				return this.fillRect.Element;
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, ShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeBase>(shape, "shape");
			base.CreateElement(this.fillRect);
		}

		internal void CopyPropertiesTo(IOpenXmlImportContext context, ShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeBase>(shape, "shape");
			if (this.FillRectElement != null)
			{
				base.ReleaseElement(this.fillRect);
			}
		}

		readonly OpenXmlChildElement<FillRectElement> fillRect;
	}
}
