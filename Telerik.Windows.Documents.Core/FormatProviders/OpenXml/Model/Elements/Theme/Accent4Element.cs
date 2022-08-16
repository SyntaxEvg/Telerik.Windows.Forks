using System;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class Accent4Element : ColorSchemeElementBase
	{
		public Accent4Element(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "accent4";
			}
		}

		public override ThemeColorType ThemeColorType
		{
			get
			{
				return ThemeColorType.Accent4;
			}
		}
	}
}
