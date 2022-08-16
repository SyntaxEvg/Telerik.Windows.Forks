using System;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class Accent1Element : ColorSchemeElementBase
	{
		public Accent1Element(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "accent1";
			}
		}

		public override ThemeColorType ThemeColorType
		{
			get
			{
				return ThemeColorType.Accent1;
			}
		}
	}
}
