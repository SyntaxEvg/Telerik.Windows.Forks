using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class ValueAxisElement : AxisElementBase
	{
		public ValueAxisElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "valAx";
			}
		}
	}
}
