using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class OddHeaderElement : HeaderFooterChildElementBase
	{
		public OddHeaderElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "oddHeader";
			}
		}
	}
}
