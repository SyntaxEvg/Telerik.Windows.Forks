using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class NonVisualGraphicFrameDrawingPropertiesElement : DrawingElementBase
	{
		public NonVisualGraphicFrameDrawingPropertiesElement(OpenXmlPartsManager partsManager, OpenXmlNamespace ns)
			: base(partsManager)
		{
			this.ns = ns;
		}

		public override string ElementName
		{
			get
			{
				return "cNvGraphicFramePr";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return this.ns;
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		readonly OpenXmlNamespace ns;
	}
}
