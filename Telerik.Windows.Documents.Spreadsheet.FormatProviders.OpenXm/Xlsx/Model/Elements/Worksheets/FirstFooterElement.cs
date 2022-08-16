using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class FirstFooterElement : HeaderFooterChildElementBase
	{
		public FirstFooterElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "firstFooter";
			}
		}
	}
}
