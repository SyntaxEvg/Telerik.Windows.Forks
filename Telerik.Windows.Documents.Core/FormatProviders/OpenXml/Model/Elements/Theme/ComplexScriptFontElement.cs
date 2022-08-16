using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class ComplexScriptFontElement : FontElementBase
	{
		public ComplexScriptFontElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "cs";
			}
		}
	}
}
