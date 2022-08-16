using System;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class MajorFontElement : FontSchemeElementBase
	{
		public MajorFontElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "majorFont";
			}
		}

		protected override ThemeFontType ThemeFontType
		{
			get
			{
				return ThemeFontType.Major;
			}
		}
	}
}
