using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class EastAsianFontElement : FontElementBase
	{
		public EastAsianFontElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "ea";
			}
		}
	}
}
