using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class MergedCellsElement : WorksheetElementBase
	{
		public MergedCellsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "mergeCells";
			}
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			return context.GetMergedCells().Any<MergedCellInfo>() || base.ShouldExport(context);
		}

		protected override IEnumerable<XlsxElementBase> EnumerateChildElements(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			foreach (MergedCellInfo mergedCell in context.GetMergedCells())
			{
				MergedCellElement mergedCellElement = base.CreateElement<MergedCellElement>("mergeCell");
				mergedCellElement.CopyPropertiesFrom(context, mergedCell);
				yield return mergedCellElement;
			}
			yield break;
		}
	}
}
