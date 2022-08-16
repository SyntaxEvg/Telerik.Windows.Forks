using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class BodyPropertiesElement : DrawingElementBase
	{
		public BodyPropertiesElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "bodyPr";
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
