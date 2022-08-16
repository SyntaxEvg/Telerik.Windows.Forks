using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class DimensionElement : WorksheetElementBase
	{
		public DimensionElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.reference = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRefRange>>(new ConvertedOpenXmlAttribute<CellRefRange>("ref", XlsxConverters.CellRefRangeConverter, true));
		}

		public override string ElementName
		{
			get
			{
				return "dimension";
			}
		}

		public CellRefRange Reference
		{
			get
			{
				return this.reference.Value;
			}
			set
			{
				this.reference.Value = value;
			}
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			CellRange usedCellRange = context.GetUsedCellRange();
			this.Reference = new CellRefRange(usedCellRange);
		}

		readonly ConvertedOpenXmlAttribute<CellRefRange> reference;
	}
}
