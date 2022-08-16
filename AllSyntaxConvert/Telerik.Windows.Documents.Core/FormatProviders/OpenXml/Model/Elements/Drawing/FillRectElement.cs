using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class FillRectElement : DrawingElementBase
	{
		public FillRectElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "fillRect";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}
	}
}
