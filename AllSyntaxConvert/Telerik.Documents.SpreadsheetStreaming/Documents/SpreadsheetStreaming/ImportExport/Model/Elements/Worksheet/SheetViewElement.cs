using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;
using Telerik.Documents.SpreadsheetStreaming.Model;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet
{
	class SheetViewElement : DirectElementBase<SheetView>
	{
		public SheetViewElement()
		{
			this.zoomScale = base.RegisterAttribute<double>("zoomScale", 100.0, false);
			this.topLeftCell = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRef>>(new ConvertedOpenXmlAttribute<CellRef>("topLeftCell", Converters.CellRefConverter, new CellRef(0, 0), false));
			this.workbookViewId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("workbookViewId", 0, true));
			this.showGridLines = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("showGridLines", true, false));
			this.showRowColHeaders = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("showRowColHeaders", true, false));
		}

		public override string ElementName
		{
			get
			{
				return "sheetView";
			}
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

		protected override void OnBeforeWrite(SheetView value)
		{
			value.PrepareForExport();
		}

		protected override void InitializeAttributesOverride(SheetView value)
		{
			if (value.ScaleFactor != null)
			{
				this.ZoomScale = (double)value.ScaleFactor.Value;
			}
			int rowIndex;
			int columnIndex;
			value.GetTopLeftCell(SpreadPane.TopLeft, out rowIndex, out columnIndex);
			this.TopLeftCell = new CellRef(rowIndex, columnIndex);
			if (value.ShouldShowGridLines != null)
			{
				this.ShowGridLines = value.ShouldShowGridLines.Value;
			}
			if (value.ShouldShowRowColumnHeaders != null)
			{
				this.ShowRowColHeaders = value.ShouldShowRowColumnHeaders.Value;
			}
		}

		protected override void WriteChildElementsOverride(SheetView value)
		{
			if (value.HasFreezePanes)
			{
				PaneElement paneElement = base.CreateChildElement<PaneElement>();
				paneElement.Write(value.Pane);
				if (value.IsFirstColumnFreeze || (!value.IsFirstColumnFreeze && !value.IsFirstRowFreeze))
				{
					SelectionElement selectionElement = base.CreateChildElement<SelectionElement>();
					selectionElement.Write(value.TopRightSelection);
				}
				if (value.IsFirstRowFreeze || (!value.IsFirstColumnFreeze && !value.IsFirstRowFreeze))
				{
					SelectionElement selectionElement2 = base.CreateChildElement<SelectionElement>();
					selectionElement2.Write(value.BottomLeftSelection);
				}
				if (!value.IsFirstColumnFreeze && !value.IsFirstRowFreeze)
				{
					SelectionElement selectionElement3 = base.CreateChildElement<SelectionElement>();
					selectionElement3.Write(value.BottomRightSelection);
					return;
				}
			}
			else
			{
				SelectionElement selectionElement4 = base.CreateChildElement<SelectionElement>();
				selectionElement4.Write(value.TopLeftSelection);
			}
		}

		readonly OpenXmlAttribute<double> zoomScale;

		readonly ConvertedOpenXmlAttribute<CellRef> topLeftCell;

		readonly IntOpenXmlAttribute workbookViewId;

		readonly BoolOpenXmlAttribute showGridLines;

		readonly BoolOpenXmlAttribute showRowColHeaders;
	}
}
