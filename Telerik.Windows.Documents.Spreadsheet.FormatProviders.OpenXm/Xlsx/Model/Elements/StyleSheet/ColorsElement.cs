using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class ColorsElement : StyleSheetElementBase
	{
		public ColorsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			base.RegisterChildElement<IndexedColorsElement>("indexedColors");
		}

		public override string ElementName
		{
			get
			{
				return "colors";
			}
		}
	}
}
