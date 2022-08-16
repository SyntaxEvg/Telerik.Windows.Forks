using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class EvenHeaderElement : HeaderFooterChildElementBase
	{
		public EvenHeaderElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "evenHeader";
			}
		}
	}
}
