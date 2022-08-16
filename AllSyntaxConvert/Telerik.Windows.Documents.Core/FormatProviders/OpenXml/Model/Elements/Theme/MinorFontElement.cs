using System;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class MinorFontElement : FontSchemeElementBase
	{
		public MinorFontElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "minorFont";
			}
		}

		protected override ThemeFontType ThemeFontType
		{
			get
			{
				return ThemeFontType.Minor;
			}
		}
	}
}
