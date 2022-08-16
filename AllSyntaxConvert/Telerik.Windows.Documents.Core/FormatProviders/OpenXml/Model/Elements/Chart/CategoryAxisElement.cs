using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class CategoryAxisElement : AxisElementBase
	{
		public CategoryAxisElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "catAx";
			}
		}
	}
}
