using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	public class SheetViewport
	{
		public double Width
		{
			get
			{
				return this[ViewportPaneType.VerticalScrollable].Rect.Width + this[ViewportPaneType.Scrollable].Rect.Width;
			}
		}

		public double Height
		{
			get
			{
				return this[ViewportPaneType.HorizontalScrollable].Rect.Height + this[ViewportPaneType.Scrollable].Rect.Height;
			}
		}

		public int ViewportPanesCount
		{
			get
			{
				return this.ViewportPanes.Count<ViewportPane>();
			}
		}

		public IEnumerable<ViewportPane> ViewportPanes
		{
			get
			{
				if (this.viewportPanes == null)
				{
					this.viewportPanes = (from p in this.paneToViewport.Values
						where !p.IsEmpty
						select p).ToArray<ViewportPane>();
				}
				return this.viewportPanes;
			}
		}

		CellRange[] VisibleRanges
		{
			get
			{
				if (this.visisbleRanges == null)
				{
					this.visisbleRanges = (from p in this.ViewportPanes
						select p.VisibleRange).ToArray<CellRange>();
				}
				return this.visisbleRanges;
			}
		}

		internal CellIndex TopLeftFixedCellIndex
		{
			get
			{
				ViewportPane viewportPane = this[ViewportPaneType.Fixed];
				ViewportPane viewportPane2 = this[ViewportPaneType.HorizontalScrollable];
				ViewportPane viewportPane3 = this[ViewportPaneType.VerticalScrollable];
				ViewportPane viewportPane4 = this[ViewportPaneType.Scrollable];
				CellIndex fromIndex = viewportPane.VisibleRange.FromIndex;
				CellIndex fromIndex2 = viewportPane3.VisibleRange.FromIndex;
				CellIndex fromIndex3 = viewportPane2.VisibleRange.FromIndex;
				CellIndex fromIndex4 = viewportPane4.VisibleRange.FromIndex;
				if (this.worksheetLayout == null)
				{
					return fromIndex4;
				}
				Func<CellIndex, bool> func = (CellIndex cellIndex) => this.worksheetLayout.GetColumnWidth(cellIndex.ColumnIndex) == 0.0 || this.worksheetLayout.GetRowHeight(cellIndex.RowIndex) == 0.0;
				if (!viewportPane.IsEmpty || func(fromIndex))
				{
					return fromIndex;
				}
				if (!viewportPane3.IsEmpty || func(fromIndex2))
				{
					return fromIndex2;
				}
				if (!viewportPane2.IsEmpty || func(fromIndex3))
				{
					return fromIndex3;
				}
				return fromIndex4;
			}
		}

		internal CellIndex TopLeftScrollableCellIndex
		{
			get
			{
				return this[ViewportPaneType.Scrollable].VisibleRange.FromIndex;
			}
		}

		internal CellIndex FrozenCellIndex
		{
			get
			{
				if (this.frozenCellIndex == null)
				{
					this.frozenCellIndex = new CellIndex(0, 0);
				}
				return this.frozenCellIndex;
			}
		}

		internal Size DisplaySize
		{
			get
			{
				return this.displaySize;
			}
		}

		internal Size ScaleFactor
		{
			get
			{
				return this.scaleFactor;
			}
		}

		public ViewportPane this[ViewportPaneType pane]
		{
			get
			{
				return this.paneToViewport[pane];
			}
			set
			{
				this.paneToViewport[pane] = value;
			}
		}

		internal SheetViewport()
		{
			this.scaleFactor = new Size(1.0, 1.0);
			this.BeforeInitialize();
			this.AfterInitialize();
		}

		internal SheetViewport(RadWorksheetLayout worksheetLayout, Size displaySize, Size scaleFactor, Point fixedTopLeft, Point scrollableTopLeft, CellIndex frozenCellIndex, Point maxBottomRight)
		{
			this.worksheetLayout = worksheetLayout;
			this.displaySize = displaySize;
			this.scaleFactor = scaleFactor;
			this.fixedTopLeft = fixedTopLeft;
			this.horizontalTopLeft = new Point(scrollableTopLeft.X, fixedTopLeft.Y);
			this.verticalTopLeft = new Point(fixedTopLeft.X, scrollableTopLeft.Y);
			this.scrollableTopLeft = scrollableTopLeft;
			this.maxBottomRight = maxBottomRight;
			double rowTop = this.worksheetLayout.GetRowTop(frozenCellIndex.RowIndex);
			double columnLeft = this.worksheetLayout.GetColumnLeft(frozenCellIndex.ColumnIndex);
			this.frozenPoint = new Point(columnLeft, rowTop);
			this.frozenCellIndex = frozenCellIndex;
			this.BeforeInitialize();
			this.InitializeViewPorts();
			this.InitializeVisibleRanges();
			this.AfterInitialize();
		}

		void InitializeViewPorts()
		{
			double num = this.displaySize.Width / this.scaleFactor.Width;
			double num2 = this.displaySize.Height / this.scaleFactor.Height;
			double num3 = Math.Max(this.frozenPoint.X - this.fixedTopLeft.X, 0.0);
			num3 = System.Math.Min(num3, num);
			double val = Math.Max(0.0, this.maxBottomRight.X - this.fixedTopLeft.X);
			num3 = System.Math.Min(val, num3);
			double num4 = Math.Max(this.frozenPoint.Y - this.fixedTopLeft.Y, 0.0);
			num4 = System.Math.Min(num4, num2);
			double val2 = Math.Max(0.0, this.maxBottomRight.Y - this.fixedTopLeft.Y);
			num4 = System.Math.Min(val2, num4);
			double num5 = num - num3;
			double val3 = Math.Max(0.0, this.maxBottomRight.X - this.horizontalTopLeft.X);
			num5 = System.Math.Min(val3, num5);
			double height = num4;
			double width = num3;
			double num6 = num2 - num4;
			double val4 = Math.Max(0.0, this.maxBottomRight.Y - this.verticalTopLeft.Y);
			num6 = System.Math.Min(val4, num6);
			double width2 = num5;
			double height2 = num6;
			this[ViewportPaneType.Fixed].Rect = new Rect(this.fixedTopLeft.X, this.fixedTopLeft.Y, num3, num4);
			this[ViewportPaneType.HorizontalScrollable].Rect = new Rect(this.horizontalTopLeft.X, this.horizontalTopLeft.Y, num5, height);
			this[ViewportPaneType.VerticalScrollable].Rect = new Rect(this.verticalTopLeft.X, this.verticalTopLeft.Y, width, num6);
			this[ViewportPaneType.Scrollable].Rect = new Rect(this.scrollableTopLeft.X, this.scrollableTopLeft.Y, width2, height2);
		}

		void BeforeInitialize()
		{
			this.paneToViewport = new Dictionary<ViewportPaneType, ViewportPane>();
			foreach (ViewportPaneType viewportPaneType in SheetViewport.PaneTypes)
			{
				this[viewportPaneType] = new ViewportPane(viewportPaneType);
			}
		}

		void InitializeVisibleRanges()
		{
			foreach (ViewportPane viewportPane in this.ViewportPanes)
			{
				if (!viewportPane.IsEmpty)
				{
					Rect rect = viewportPane.Rect;
					Point topLeftPoint = new Point(rect.Left, rect.Top);
					Point bottomRightPoint = new Point(rect.Right, rect.Bottom);
					ViewportPaneType viewportPaneType = viewportPane.ViewportPaneType;
					CellIndex paneTopLeftCellIndex = this.GetPaneTopLeftCellIndex(viewportPaneType, topLeftPoint);
					CellIndex paneBottomRightCellIndex = this.GetPaneBottomRightCellIndex(viewportPaneType, bottomRightPoint);
					viewportPane.VisibleRange = new CellRange(paneTopLeftCellIndex, paneBottomRightCellIndex);
					viewportPane.TopLeftPoint = this.worksheetLayout.GetPointFromCellIndexAndOffset(paneTopLeftCellIndex, 0.0, 0.0);
				}
			}
		}

		CellIndex GetPaneTopLeftCellIndex(ViewportPaneType paneType, Point topLeftPoint)
		{
			int num;
			int num2;
			this.worksheetLayout.GetCellIndexFromPoint(topLeftPoint.X, topLeftPoint.Y, false, false, out num, out num2, false, false);
			if (this.frozenCellIndex != null)
			{
				if (paneType == ViewportPaneType.HorizontalScrollable || paneType == ViewportPaneType.Scrollable)
				{
					num2 = Math.Max(num2, this.frozenCellIndex.ColumnIndex);
				}
				if (paneType == ViewportPaneType.VerticalScrollable || paneType == ViewportPaneType.Scrollable)
				{
					num = Math.Max(num, this.frozenCellIndex.RowIndex);
				}
			}
			return new CellIndex(num, num2);
		}

		CellIndex GetPaneBottomRightCellIndex(ViewportPaneType paneType, Point bottomRightPoint)
		{
			int num;
			int num2;
			this.worksheetLayout.GetCellIndexFromPoint(bottomRightPoint.X, bottomRightPoint.Y, true, true, out num, out num2, true, true);
			if (this.frozenCellIndex != null)
			{
				if (paneType == ViewportPaneType.HorizontalScrollable || paneType == ViewportPaneType.Fixed)
				{
					num = System.Math.Min(num, Math.Max(this.frozenCellIndex.RowIndex - 1, 0));
				}
				if (paneType == ViewportPaneType.VerticalScrollable || paneType == ViewportPaneType.Fixed)
				{
					num2 = System.Math.Min(num2, Math.Max(this.frozenCellIndex.ColumnIndex - 1, 0));
				}
			}
			return new CellIndex(num, num2);
		}

		void AfterInitialize()
		{
			ViewportPane viewportPane = this[ViewportPaneType.Fixed];
			ViewportPane viewportPane2 = this[ViewportPaneType.HorizontalScrollable];
			ViewportPane viewportPane3 = this[ViewportPaneType.VerticalScrollable];
			ViewportPane viewportPane4 = this[ViewportPaneType.Scrollable];
			viewportPane.BoundingRect = new Rect(0.0, 0.0, viewportPane.Rect.Width * this.scaleFactor.Width, viewportPane.Rect.Height * this.scaleFactor.Height);
			viewportPane2.BoundingRect = new Rect(viewportPane.BoundingRect.Width, 0.0, viewportPane2.Rect.Width * this.scaleFactor.Width, viewportPane2.Rect.Height * this.scaleFactor.Height);
			viewportPane3.BoundingRect = new Rect(0.0, viewportPane.BoundingRect.Height, viewportPane3.Rect.Width * this.scaleFactor.Width, viewportPane3.Rect.Height * this.scaleFactor.Height);
			viewportPane4.BoundingRect = new Rect(viewportPane.BoundingRect.Width, viewportPane.BoundingRect.Height, viewportPane4.Rect.Width * this.scaleFactor.Width, viewportPane4.Rect.Height * this.scaleFactor.Height);
		}

		ViewportPane GetViewportPaneFromViewportPoint(Point point)
		{
			ViewportPane viewportPane = this[ViewportPaneType.Fixed];
			Point crossPoint = new Point(viewportPane.Rect.Width, viewportPane.Rect.Height);
			return this.GetViewportPaneForPointRelativeToCrossPoint(point, crossPoint);
		}

		public ViewportPane GetViewportPaneFromDocumentPoint(Point point)
		{
			return this.GetViewportPaneForPointRelativeToCrossPoint(point, this.frozenPoint);
		}

		ViewportPane GetViewportPaneForPointRelativeToCrossPoint(Point point, Point crossPoint)
		{
			ViewportPaneType viewportPaneType;
			if (point.X < crossPoint.X)
			{
				if (point.Y < crossPoint.Y)
				{
					viewportPaneType = ViewportPaneType.Fixed;
				}
				else
				{
					viewportPaneType = ViewportPaneType.VerticalScrollable;
				}
			}
			else if (point.Y < crossPoint.Y)
			{
				viewportPaneType = ViewportPaneType.HorizontalScrollable;
			}
			else
			{
				viewportPaneType = ViewportPaneType.Scrollable;
			}
			if (viewportPaneType == ViewportPaneType.Fixed && this[viewportPaneType].IsEmpty)
			{
				if (this[ViewportPaneType.HorizontalScrollable].IsEmpty)
				{
					viewportPaneType = ViewportPaneType.VerticalScrollable;
				}
				else
				{
					viewportPaneType = ViewportPaneType.HorizontalScrollable;
				}
			}
			if (this[viewportPaneType].IsEmpty)
			{
				viewportPaneType = ViewportPaneType.Scrollable;
			}
			return this[viewportPaneType];
		}

		public Point GetDocumentPointFromViewPoint(Point point)
		{
			point = new Point(point.X / this.scaleFactor.Width, point.Y / this.scaleFactor.Height);
			ViewportPane viewportPaneFromViewportPoint = this.GetViewportPaneFromViewportPoint(point);
			ViewportPaneType viewportPaneType = viewportPaneFromViewportPoint.ViewportPaneType;
			double num = ((viewportPaneType == ViewportPaneType.HorizontalScrollable || viewportPaneType == ViewportPaneType.Scrollable) ? this[ViewportPaneType.VerticalScrollable].Rect.Width : 0.0);
			double num2 = ((viewportPaneType == ViewportPaneType.VerticalScrollable || viewportPaneType == ViewportPaneType.Scrollable) ? this[ViewportPaneType.HorizontalScrollable].Rect.Height : 0.0);
			Point result = new Point(point.X - num + viewportPaneFromViewportPoint.Rect.Left, point.Y - num2 + viewportPaneFromViewportPoint.Rect.Top);
			return result;
		}

		public Point GetViewPointFromDocumentPoint(Point point)
		{
			ViewportPane viewportPaneFromDocumentPoint = this.GetViewportPaneFromDocumentPoint(point);
			ViewportPaneType viewportPaneType = viewportPaneFromDocumentPoint.ViewportPaneType;
			ViewportPane viewportPane = this[ViewportPaneType.Fixed];
			ViewportPane viewportPane2 = this[ViewportPaneType.HorizontalScrollable];
			ViewportPane viewportPane3 = this[ViewportPaneType.VerticalScrollable];
			ViewportPane viewportPane4 = this[ViewportPaneType.Scrollable];
			double num = 0.0;
			double num2 = 0.0;
			switch (viewportPaneType)
			{
			case ViewportPaneType.Fixed:
				num = point.X - viewportPane.Rect.Left;
				num2 = point.Y - viewportPane.Rect.Top;
				break;
			case ViewportPaneType.HorizontalScrollable:
				num = point.X - viewportPane2.Rect.Left + viewportPane.Rect.Width;
				num2 = point.Y - viewportPane2.Rect.Top;
				break;
			case ViewportPaneType.VerticalScrollable:
				num = point.X - viewportPane3.Rect.Left;
				num2 = point.Y - viewportPane3.Rect.Top + viewportPane.Rect.Height;
				break;
			case ViewportPaneType.Scrollable:
				num = point.X - viewportPane4.Rect.Left + viewportPane3.Rect.Width;
				num2 = point.Y - viewportPane4.Rect.Top + viewportPane2.Rect.Height;
				break;
			}
			return new Point(num * this.scaleFactor.Width, num2 * this.scaleFactor.Height);
		}

		public Point Translate(Point point, ViewportPaneType pointContainingPaneType)
		{
			ViewportPane viewportPane = this[pointContainingPaneType];
			double x;
			if (pointContainingPaneType == ViewportPaneType.Fixed || pointContainingPaneType == ViewportPaneType.VerticalScrollable)
			{
				x = point.X - viewportPane.Rect.Left;
			}
			else
			{
				x = point.X - viewportPane.Rect.Left + this[ViewportPaneType.VerticalScrollable].Rect.Width;
			}
			double y;
			if (pointContainingPaneType == ViewportPaneType.Fixed || pointContainingPaneType == ViewportPaneType.HorizontalScrollable)
			{
				y = point.Y - viewportPane.Rect.Top;
			}
			else
			{
				y = point.Y - viewportPane.Rect.Top + this[ViewportPaneType.HorizontalScrollable].Rect.Height;
			}
			return new Point(x, y);
		}

		public bool Contains(Rect boundingRectangle)
		{
			foreach (ViewportPane viewportPane in this.ViewportPanes)
			{
				if (viewportPane.Rect.Contains(boundingRectangle))
				{
					return true;
				}
			}
			return false;
		}

		public bool Contains(Point point)
		{
			foreach (ViewportPane viewportPane in this.ViewportPanes)
			{
				if (viewportPane.Rect.Contains(point))
				{
					return true;
				}
			}
			return false;
		}

		public Point GetTopLeftPoint()
		{
			ViewportPane topLeftPane = this.GetTopLeftPane();
			return new Point(topLeftPane.Rect.Left, topLeftPane.Rect.Top);
		}

		public bool ContainsColumnIndex(int columnIndex)
		{
			for (int i = 0; i < this.VisibleRanges.Length; i++)
			{
				CellRange cellRange = this.VisibleRanges[i];
				if (cellRange.FromIndex.ColumnIndex <= columnIndex && cellRange.ToIndex.ColumnIndex >= columnIndex)
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsRowIndex(int rowIndex)
		{
			for (int i = 0; i < this.VisibleRanges.Length; i++)
			{
				CellRange cellRange = this.VisibleRanges[i];
				if (cellRange.FromIndex.RowIndex <= rowIndex && cellRange.ToIndex.RowIndex >= rowIndex)
				{
					return true;
				}
			}
			return false;
		}

		public bool Contains(CellIndex cellIndex)
		{
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			return this.Contains(cellIndex.RowIndex, cellIndex.ColumnIndex);
		}

		internal bool Contains(long index)
		{
			int rowIndex;
			int columnIndex;
			WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(index, out rowIndex, out columnIndex);
			return this.Contains(rowIndex, columnIndex);
		}

		public bool Contains(int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			for (int i = 0; i < this.VisibleRanges.Length; i++)
			{
				if (this.VisibleRanges[i].Contains(rowIndex, columnIndex))
				{
					return true;
				}
			}
			return false;
		}

		public CellRange GetCellRangeContainingCellIndex(CellIndex cellIndex)
		{
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			return this.GetCellRangeContainingCellIndex(cellIndex.RowIndex, cellIndex.ColumnIndex);
		}

		public CellRange GetCellRangeContainingCellIndex(int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			return this.GetViewportPaneContainingCellIndex(rowIndex, columnIndex).VisibleRange;
		}

		public ViewportPane GetViewportPaneContainingCellIndex(CellIndex index)
		{
			return this.GetViewportPaneContainingCellIndex(index.RowIndex, index.ColumnIndex);
		}

		internal ViewportPane GetViewportPaneContainingCellIndex(int rowIndex, int columnIndex)
		{
			ViewportPane viewportPane = this[ViewportPaneType.Fixed];
			ViewportPane viewportPane2 = this[ViewportPaneType.HorizontalScrollable];
			ViewportPane viewportPane3 = this[ViewportPaneType.VerticalScrollable];
			ViewportPane result = this[ViewportPaneType.Scrollable];
			if ((rowIndex <= viewportPane.VisibleRange.ToIndex.RowIndex && !viewportPane.IsEmpty) || (rowIndex <= viewportPane2.VisibleRange.ToIndex.RowIndex && !viewportPane2.IsEmpty))
			{
				if ((columnIndex <= viewportPane.VisibleRange.ToIndex.ColumnIndex && !viewportPane.IsEmpty) || (columnIndex <= viewportPane3.VisibleRange.ToIndex.ColumnIndex && !viewportPane3.IsEmpty))
				{
					return viewportPane;
				}
				return viewportPane2;
			}
			else
			{
				if ((columnIndex <= viewportPane.VisibleRange.ToIndex.ColumnIndex && !viewportPane.IsEmpty) || (columnIndex <= viewportPane3.VisibleRange.ToIndex.ColumnIndex && !viewportPane3.IsEmpty))
				{
					return viewportPane3;
				}
				return result;
			}
		}

		public ViewportPane GetTopMostViewportPaneContainingColumnIndex(int columnIndex)
		{
			IEnumerable<ViewportPane> source = from p in this.ViewportPanes
				where p.VisibleRange.FromIndex.ColumnIndex <= columnIndex && p.VisibleRange.ToIndex.ColumnIndex >= columnIndex
				orderby p.VisibleRange.FromIndex.RowIndex
				select p;
			return source.FirstOrDefault<ViewportPane>();
		}

		public ViewportPane GetLeftMostViewportPaneContainingRowIndex(int rowIndex)
		{
			IEnumerable<ViewportPane> source = from p in this.ViewportPanes
				where p.VisibleRange.FromIndex.RowIndex <= rowIndex && p.VisibleRange.ToIndex.RowIndex >= rowIndex
				orderby p.VisibleRange.FromIndex.ColumnIndex
				select p;
			return source.FirstOrDefault<ViewportPane>();
		}

		internal ViewportPane GetTopLeftPane()
		{
			ViewportPane viewportPane = this[ViewportPaneType.Fixed];
			if (viewportPane.IsEmpty)
			{
				viewportPane = this[ViewportPaneType.HorizontalScrollable];
			}
			if (viewportPane.IsEmpty)
			{
				viewportPane = this[ViewportPaneType.VerticalScrollable];
			}
			if (viewportPane.IsEmpty)
			{
				viewportPane = this[ViewportPaneType.Scrollable];
			}
			return viewportPane;
		}

		public override bool Equals(object obj)
		{
			SheetViewport sheetViewport = obj as SheetViewport;
			if (sheetViewport == null)
			{
				return false;
			}
			if (this.frozenPoint != sheetViewport.frozenPoint)
			{
				return false;
			}
			foreach (ViewportPaneType pane in SheetViewport.PaneTypes)
			{
				if (!this[pane].Equals(sheetViewport[pane]))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			ViewportPaneType pane = SheetViewport.PaneTypes[0];
			int num = this[pane].GetHashCodeOrZero();
			for (int i = 1; i < SheetViewport.PaneTypes.Count; i++)
			{
				ViewportPaneType pane2 = SheetViewport.PaneTypes[i];
				num = TelerikHelper.CombineHashCodes(num, this[pane2].GetHashCodeOrZero());
			}
			return num;
		}

		public static bool operator ==(SheetViewport first, SheetViewport second)
		{
			return TelerikHelper.EqualsOfT<SheetViewport>(first, second);
		}

		public static bool operator !=(SheetViewport first, SheetViewport second)
		{
			return !TelerikHelper.EqualsOfT<SheetViewport>(first, second);
		}

		RadWorksheetLayout worksheetLayout;

		readonly Size displaySize;

		readonly Point fixedTopLeft;

		readonly Point horizontalTopLeft;

		readonly Point verticalTopLeft;

		readonly Point scrollableTopLeft;

		readonly Point maxBottomRight;

		Point frozenPoint;

		CellIndex frozenCellIndex;

		readonly Size scaleFactor;

		Dictionary<ViewportPaneType, ViewportPane> paneToViewport;

		static readonly List<ViewportPaneType> PaneTypes = Enum.GetValues(typeof(ViewportPaneType)).Cast<ViewportPaneType>().ToList<ViewportPaneType>();

		ViewportPane[] viewportPanes;

		CellRange[] visisbleRanges;
	}
}
