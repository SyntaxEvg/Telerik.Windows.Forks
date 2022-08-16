using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class SelectionState
	{
		public IEnumerable<CellRange> SelectedRanges
		{
			get
			{
				return this.selectedRanges;
			}
		}

		public CellIndex ActiveCellIndex
		{
			get
			{
				return this.activeCellIndex;
			}
		}

		public ViewportPaneType Pane
		{
			get
			{
				return this.pane;
			}
		}

		internal IEnumerable<FloatingShapeBase> SelectedShapes
		{
			get
			{
				return this.selectedShapes;
			}
		}

		internal bool IsCellSelection
		{
			get
			{
				return this.isCellSelection;
			}
		}

		internal SelectionState()
			: this(new CellRange[]
			{
				new CellRange(0, 0, 0, 0)
			}, new CellIndex(0, 0), ViewportPaneType.Scrollable, new List<FloatingShapeBase>(), true)
		{
		}

		public SelectionState(IEnumerable<CellRange> selectedRanges, CellIndex activeCellIndex, ViewportPaneType pane)
			: this(selectedRanges, activeCellIndex, pane, new List<FloatingShapeBase>(), true)
		{
		}

		internal SelectionState(IEnumerable<CellRange> selectedRanges, CellIndex activeCellIndex, ViewportPaneType pane, IEnumerable<FloatingShapeBase> selectedShapes, bool isCellSelection)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<CellRange>>(selectedRanges, "selectedRanges");
			Guard.ThrowExceptionIfNull<CellIndex>(activeCellIndex, "activeCellIndex");
			Guard.ThrowExceptionIfNull<IEnumerable<FloatingShapeBase>>(selectedShapes, "selectedShapes");
			this.selectedShapes = new List<FloatingShapeBase>(selectedShapes);
			this.selectedRanges = new List<CellRange>(selectedRanges);
			this.activeCellIndex = activeCellIndex;
			this.pane = pane;
			this.isCellSelection = isCellSelection;
		}

		public SelectionState Clone()
		{
			return new SelectionState(this.SelectedRanges, this.ActiveCellIndex, this.Pane, this.SelectedShapes, this.IsCellSelection);
		}

		public override string ToString()
		{
			return string.Format("Pane: {0}; ActiveCellIndex: {1}; SelectedRanges: {2}", this.Pane, this.ActiveCellIndex, string.Join<CellRange>("\n", this.SelectedRanges));
		}

		readonly List<CellRange> selectedRanges;

		readonly CellIndex activeCellIndex;

		readonly ViewportPaneType pane;

		readonly List<FloatingShapeBase> selectedShapes;

		readonly bool isCellSelection;
	}
}
