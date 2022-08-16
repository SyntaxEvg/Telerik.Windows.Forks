using System;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class FollowedHyperlinkElement : ColorSchemeElementBase
	{
		public FollowedHyperlinkElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "folHlink";
			}
		}

		public override ThemeColorType ThemeColorType
		{
			get
			{
				return ThemeColorType.FollowedHyperlink;
			}
		}
	}
}
