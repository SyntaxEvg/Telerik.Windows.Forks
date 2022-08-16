using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class NumberReferenceElement : ReferenceDataElementBase
	{
		public NumberReferenceElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "numRef";
			}
		}
	}
}
