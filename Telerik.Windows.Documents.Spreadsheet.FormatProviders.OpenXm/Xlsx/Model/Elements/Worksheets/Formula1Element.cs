using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class Formula1Element : DataValidationFormulaElementBase
	{
		public Formula1Element(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "formula1";
			}
		}
	}
}
