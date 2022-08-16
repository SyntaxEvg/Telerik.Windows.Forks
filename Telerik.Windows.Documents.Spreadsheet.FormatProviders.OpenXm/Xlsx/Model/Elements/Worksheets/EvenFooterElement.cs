using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class EvenFooterElement : HeaderFooterChildElementBase
	{
		public EvenFooterElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "evenFooter";
			}
		}
	}
}
