using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class StringReferenceElement : ReferenceDataElementBase
	{
		public StringReferenceElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "strRef";
			}
		}
	}
}
