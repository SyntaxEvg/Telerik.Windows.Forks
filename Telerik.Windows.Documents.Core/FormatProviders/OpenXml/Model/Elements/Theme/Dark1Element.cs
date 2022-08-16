using System;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class Dark1Element : ColorSchemeElementBase
	{
		public Dark1Element(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "dk1";
			}
		}

		public override ThemeColorType ThemeColorType
		{
			get
			{
				return ThemeColorType.Text1;
			}
		}
	}
}
