using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class SelectionElement : WorksheetElementBase
	{
		public SelectionElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.activeCell = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRef>>(new ConvertedOpenXmlAttribute<CellRef>("activeCell", XlsxConverters.CellRefConverter, true));
			this.sqref = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRefRangeSequence>>(new ConvertedOpenXmlAttribute<CellRefRangeSequence>("sqref", XlsxConverters.CellRefRangeSequenceConverter, true));
			this.pane = base.RegisterAttribute<MappedOpenXmlAttribute<ViewportPaneType>>(new MappedOpenXmlAttribute<ViewportPaneType>("pane", null, TypeMappers.ViewportTypeMapper, false));
		}

		public CellRef ActiveCell
		{
			get
			{
				return this.activeCell.Value;
			}
			set
			{
				this.activeCell.Value = value;
			}
		}

		public CellRefRangeSequence SqRef
		{
			get
			{
				return this.sqref.Value;
			}
			set
			{
				this.sqref.Value = value;
			}
		}

		public ViewportPaneType Pane
		{
			get
			{
				return this.pane.Value;
			}
			set
			{
				this.pane.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "selection";
			}
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			if (context.WorksheetViewState.Pane != null)
			{
				return true;
			}
			SelectionState selectionState = context.WorksheetViewState.SelectionState;
			bool flag = selectionState.SelectedRanges.Count<CellRange>() > 1;
			if (flag)
			{
				return true;
			}
			CellIndex right = new CellIndex(0, 0);
			CellRange cellRange = selectionState.SelectedRanges.First<CellRange>();
			return (this.ActiveCell != null && this.ActiveCell.ToCellIndex() != right) || !cellRange.IsSingleCell;
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			SelectionState selectionState = context.WorksheetViewState.SelectionState;
			if (context.WorksheetViewState.Pane != null)
			{
				this.Pane = ViewportPaneHelper.GetActivePaneType(context.WorksheetViewState.Pane);
			}
			this.ActiveCell = new CellRef(selectionState.ActiveCellIndex.RowIndex, selectionState.ActiveCellIndex.ColumnIndex);
			this.RetrieveAndSetCellReferenceSequence(selectionState.SelectedRanges, this);
		}

		void RetrieveAndSetCellReferenceSequence(IEnumerable<CellRange> cellRanges, SelectionElement selectionElement)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (CellRange cellRange in cellRanges)
			{
				stringBuilder.Append(NameConverter.ConvertCellRangeToName(cellRange.FromIndex, cellRange.ToIndex));
				stringBuilder.Append(' ');
			}
			selectionElement.SqRef = new CellRefRangeSequence(stringBuilder.ToString());
		}

		readonly ConvertedOpenXmlAttribute<CellRef> activeCell;

		readonly ConvertedOpenXmlAttribute<CellRefRangeSequence> sqref;

		readonly MappedOpenXmlAttribute<ViewportPaneType> pane;
	}
}
