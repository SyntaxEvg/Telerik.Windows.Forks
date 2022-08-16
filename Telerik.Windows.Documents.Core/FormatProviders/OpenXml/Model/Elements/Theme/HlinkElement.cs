using System;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class HlinkElement : ColorSchemeElementBase
	{
		public HlinkElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "hlink";
			}
		}

		public override ThemeColorType ThemeColorType
		{
			get
			{
				return ThemeColorType.Hyperlink;
			}
		}
	}
}
