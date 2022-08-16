using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class PaneElement : WorksheetElementBase
	{
		public PaneElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.xSplit = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("xSplit", 0, false));
			this.ySplit = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("ySplit", 0, false));
			this.topLeftCell = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRef>>(new ConvertedOpenXmlAttribute<CellRef>("topLeftCell", XlsxConverters.CellRefConverter, true));
			this.activePane = base.RegisterAttribute<MappedOpenXmlAttribute<ViewportPaneType>>(new MappedOpenXmlAttribute<ViewportPaneType>("activePane", null, TypeMappers.ViewportTypeMapper, false));
			this.state = base.RegisterAttribute<MappedOpenXmlAttribute<PaneState>>(new MappedOpenXmlAttribute<PaneState>("state", null, TypeMappers.ViewportStateMapper, false));
		}

		public override string ElementName
		{
			get
			{
				return "pane";
			}
		}

		int XSplit
		{
			get
			{
				return this.xSplit.Value;
			}
			set
			{
				this.xSplit.Value = value;
			}
		}

		int YSplit
		{
			get
			{
				return this.ySplit.Value;
			}
			set
			{
				this.ySplit.Value = value;
			}
		}

		CellRef TopLeftCell
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

		ViewportPaneType ActivePane
		{
			get
			{
				return this.activePane.Value;
			}
			set
			{
				this.activePane.Value = value;
			}
		}

		PaneState State
		{
			get
			{
				return this.state.Value;
			}
			set
			{
				this.state.Value = value;
			}
		}

		public Pane ToPane()
		{
			CellIndex topLeftCellIndex = this.TopLeftCell.ToCellIndex();
			return new Pane(topLeftCellIndex, this.XSplit, this.YSplit, this.ActivePane, this.State);
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			if (context.WorksheetViewState.Pane == null)
			{
				return false;
			}
			Pane pane = context.WorksheetViewState.Pane;
			return pane.XSplit != 0 || pane.YSplit != 0;
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			if (context.WorksheetViewState.Pane != null)
			{
				Pane pane = context.WorksheetViewState.Pane;
				this.ActivePane = ViewportPaneHelper.GetActivePaneType(context.WorksheetViewState.Pane);
				this.TopLeftCell = new CellRef(pane.TopLeftCellIndex.RowIndex, pane.TopLeftCellIndex.ColumnIndex);
				this.XSplit = pane.XSplit;
				this.YSplit = pane.YSplit;
				this.State = pane.State;
			}
		}

		readonly IntOpenXmlAttribute xSplit;

		readonly IntOpenXmlAttribute ySplit;

		readonly ConvertedOpenXmlAttribute<CellRef> topLeftCell;

		readonly MappedOpenXmlAttribute<PaneState> state;

		readonly MappedOpenXmlAttribute<ViewportPaneType> activePane;
	}
}
