using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class MergedCellElement : WorksheetElementBase
	{
		public MergedCellElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.reference = base.RegisterAttribute<ConvertedOpenXmlAttribute<Ref>>(new ConvertedOpenXmlAttribute<Ref>("ref", XlsxConverters.RefConverter, true));
		}

		public Ref Reference
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

		public override string ElementName
		{
			get
			{
				return "mergeCell";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorksheetExportContext context, MergedCellInfo mergedCell)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<MergedCellInfo>(mergedCell, "mergedCell");
			this.Reference = new Ref(mergedCell.CellRange);
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			context.ApplyMergeCellInfo(new MergedCellInfo
			{
				CellRange = this.Reference.ToCellRange()
			});
		}

		readonly ConvertedOpenXmlAttribute<Ref> reference;
	}
}
