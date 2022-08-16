using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class SheetViewElement : WorksheetElementBase
	{
		public SheetViewElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.paneChildElement = base.RegisterChildElement<PaneElement>("pane");
			this.selectionChildElement = base.RegisterChildElement<SelectionElement>("selection");
			this.zoomScale = base.RegisterAttribute<double>("zoomScale", 100.0, false);
			this.topLeftCell = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRef>>(new ConvertedOpenXmlAttribute<CellRef>("topLeftCell", XlsxConverters.CellRefConverter, new CellRef(0, 0), false));
			this.workbookViewId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("workbookViewId", 0, true));
			this.showGridLines = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("showGridLines", true, false));
			this.showRowColHeaders = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("showRowColHeaders", true, false));
		}

		public double ZoomScale
		{
			get
			{
				return this.zoomScale.Value;
			}
			set
			{
				this.zoomScale.Value = value;
			}
		}

		public CellRef TopLeftCell
		{
			get
			{
				return this.topLeftCell.Value;
			}
			set
			{
				this.topLeftCell.Value = value;
			}
		}

		public int WorkbookViewId
		{
			get
			{
				return this.workbookViewId.Value;
			}
			set
			{
				this.workbookViewId.Value = value;
			}
		}

		public bool ShowGridLines
		{
			get
			{
				return this.showGridLines.Value;
			}
			set
			{
				this.showGridLines.Value = value;
			}
		}

		public bool ShowRowColHeaders
		{
			get
			{
				return this.showRowColHeaders.Value;
			}
			set
			{
				this.showRowColHeaders.Value = value;
			}
		}

		public SelectionElement SelectionElement
		{
			get
			{
				return this.selectionChildElement.Element;
			}
			set
			{
				this.selectionChildElement.Element = value;
			}
		}

		public PaneElement PaneElement
		{
			get
			{
				return this.paneChildElement.Element;
			}
			set
			{
				this.paneChildElement.Element = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "sheetView";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			ISheet worksheet = context.Worksheet;
			WorksheetViewState worksheetViewState = (WorksheetViewState)worksheet.ViewState;
			worksheetViewState.TopLeftCellIndex = this.TopLeftCell.ToCellIndex();
			worksheetViewState.ShowGridLines = this.ShowGridLines;
			worksheetViewState.ShowRowColHeaders = this.ShowRowColHeaders;
			if (this.SelectionElement != null)
			{
				CellIndex activeCellIndex = ((this.SelectionElement.ActiveCell != null) ? this.SelectionElement.ActiveCell.ToCellIndex() : new CellIndex(0, 0));
				ViewportPaneType pane = this.SelectionElement.Pane;
				IEnumerable<CellRange> selectedRanges = new CellRange[]
				{
					new CellRange(0, 0, 0, 0)
				};
				if (this.SelectionElement.SqRef != null)
				{
					selectedRanges = this.SelectionElement.SqRef.CellRanges;
				}
				SelectionState selectionState = new SelectionState(selectedRanges, activeCellIndex, pane);
				worksheetViewState.SelectionState = selectionState;
				worksheetViewState.ScaleFactor = this.GetScaleFactor();
			}
			if (this.PaneElement != null)
			{
				Pane pane2 = this.PaneElement.ToPane();
				worksheetViewState.Pane = pane2;
			}
			base.ReleaseElement(this.paneChildElement);
			base.ReleaseElement(this.selectionChildElement);
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			CellIndex topLeftCellIndex = context.WorksheetViewState.TopLeftCellIndex;
			this.TopLeftCell = new CellRef(topLeftCellIndex.RowIndex, topLeftCellIndex.ColumnIndex);
			this.ZoomScale = Math.Floor(context.WorksheetViewState.ScaleFactor.Height * 100.0);
			this.ShowGridLines = context.WorksheetViewState.ShowGridLines;
			this.ShowRowColHeaders = context.WorksheetViewState.ShowRowColHeaders;
			base.CreateElement(this.paneChildElement);
			base.CreateElement(this.selectionChildElement);
		}

		Size GetScaleFactor()
		{
			double val = this.ZoomScale / 100.0;
			double width;
			double height = (width = Math.Max(0.5, val));
			return new Size(width, height);
		}

		readonly OpenXmlChildElement<PaneElement> paneChildElement;

		readonly OpenXmlChildElement<SelectionElement> selectionChildElement;

		readonly OpenXmlAttribute<double> zoomScale;

		readonly ConvertedOpenXmlAttribute<CellRef> topLeftCell;

		readonly IntOpenXmlAttribute workbookViewId;

		readonly BoolOpenXmlAttribute showGridLines;

		readonly BoolOpenXmlAttribute showRowColHeaders;
	}
}
