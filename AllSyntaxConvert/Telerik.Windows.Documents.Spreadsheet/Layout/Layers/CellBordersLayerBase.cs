using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class CellBordersLayerBase : WorksheetLayerBase
	{
		public bool SupportDiagonalBorders
		{
			get
			{
				return this.supportDiagonalBorders;
			}
		}

		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		internal IRenderer<LineRenderable> LineRenderer
		{
			get
			{
				return this.lineRenderer;
			}
		}

		public CellBordersLayerBase(IRenderer<LineRenderable> lineRenderer, bool shouldShowTopLeftMostBorders, bool supportDiagonalBorders, Func<CellBorder, bool> shouldShowBorder, string name)
		{
			this.shouldShowTopLeftMostBorders = shouldShowTopLeftMostBorders;
			this.supportDiagonalBorders = supportDiagonalBorders;
			this.shouldShowBorder = shouldShowBorder;
			this.lineRenderer = lineRenderer;
			this.name = name;
		}

		protected override void UpdateRenderOverride(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			this.UpdateHorizontalBorders(worksheetUpdateContext);
			this.UpdateVerticalBorders(worksheetUpdateContext);
			if (this.SupportDiagonalBorders)
			{
				this.UpdateDiagonalBorders(worksheetUpdateContext);
			}
		}

		private void UpdateHorizontalBorders(WorksheetRenderUpdateContext updateContext)
		{
			foreach (ViewportPane viewportPane in updateContext.SheetViewport.ViewportPanes)
			{
				CellRange visibleRange = viewportPane.VisibleRange;
				int rowIndex3 = visibleRange.FromIndex.RowIndex;
				int columnIndex3 = visibleRange.FromIndex.ColumnIndex;
				int rowIndex2 = visibleRange.ToIndex.RowIndex;
				int columnIndex2 = visibleRange.ToIndex.ColumnIndex;
				int rowIndex;
				for (rowIndex = rowIndex3; rowIndex <= rowIndex2; rowIndex++)
				{
					CellBordersLayerBase.c__DisplayClass10 CS_8__locals3 = new CellBordersLayerBase.c__DisplayClass10();
					CS_8__locals3.borders = new CompressedList<CellBorder>((long)columnIndex3, (long)columnIndex2, null);
					int columnIndex;
					for (columnIndex = columnIndex3; columnIndex <= columnIndex2; columnIndex++)
					{
						this.AddToBorders(updateContext, delegate(CellBorder border)
						{
							CS_8__locals3.borders.SetValue((long)columnIndex, border);
						}, (CellRange mergedRange) => rowIndex < mergedRange.ToIndex.RowIndex, () => BorderRenderHelper.GetBottomBorderToDisplay(updateContext, rowIndex, columnIndex), false, rowIndex, columnIndex);
					}
					this.UpdateBordersInRow(viewportPane.ViewportPaneType, rowIndex, CS_8__locals3.borders, updateContext, (Rect boundingRect) => boundingRect.Bottom);
					if (rowIndex == rowIndex3 && this.shouldShowTopLeftMostBorders)
					{
						CS_8__locals3.borders = new CompressedList<CellBorder>((long)columnIndex3, (long)columnIndex2, null);
						for (columnIndex = columnIndex3; columnIndex <= columnIndex2; columnIndex++)
						{
							this.AddToBorders(updateContext, delegate(CellBorder border)
							{
								CS_8__locals3.borders.SetValue((long)columnIndex, border);
							}, (CellRange mergedRange) => rowIndex >= mergedRange.FromIndex.RowIndex && rowIndex < mergedRange.ToIndex.RowIndex, () => updateContext.GetTopBorder(WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex)), false, rowIndex, columnIndex);
						}
						this.UpdateBordersInRow(viewportPane.ViewportPaneType, rowIndex, CS_8__locals3.borders, updateContext, (Rect boundingRect) => boundingRect.Top);
					}
				}
			}
		}

		private void UpdateBordersInRow(ViewportPaneType paneType, int rowIndex, ICompressedList<CellBorder> borders, WorksheetRenderUpdateContext updateContext, Func<Rect, double> getYCoordinate)
		{
			foreach (Range<long, CellBorder> range in borders)
			{
				if (!(range.Value == null))
				{
					CellLayoutBox visibleCellBox = updateContext.GetVisibleCellBox(rowIndex, (int)range.Start);
					CellLayoutBox visibleCellBox2 = updateContext.GetVisibleCellBox(rowIndex, (int)range.End);
					double num = 0.0;
					CellBorder rightBorderToDisplay;
					if (visibleCellBox.ColumnIndex > 0)
					{
						rightBorderToDisplay = BorderRenderHelper.GetRightBorderToDisplay(updateContext, visibleCellBox.RowIndex, visibleCellBox.ColumnIndex - 1);
						num = rightBorderToDisplay.Thickness / 2.0;
					}
					rightBorderToDisplay = BorderRenderHelper.GetRightBorderToDisplay(updateContext, visibleCellBox2.RowIndex, visibleCellBox2.ColumnIndex);
					double num2 = rightBorderToDisplay.Thickness / 2.0;
					LineRenderable lineRenderable = new LineRenderable();
					CellBordersLayerBase.SetLinePropertiesForBorder(lineRenderable, range.Value, updateContext);
					lineRenderable.X1 = visibleCellBox.BoundingRectangle.Left - num;
					lineRenderable.X2 = visibleCellBox2.BoundingRectangle.Right + num2;
					lineRenderable.Y1 = getYCoordinate(visibleCellBox.BoundingRectangle);
					lineRenderable.Y2 = lineRenderable.Y1;
					base.ContainerManager.Add(lineRenderable, paneType);
				}
			}
		}

		private void UpdateVerticalBorders(WorksheetRenderUpdateContext updateContext)
		{
			HashSet<long> bordersToSkip = this.GetBordersToSkip(updateContext);
			this.UpdateVerticalBordersForVisibleRanges(updateContext, bordersToSkip);
		}

		private void UpdateVerticalBordersForVisibleRanges(WorksheetRenderUpdateContext updateContext, HashSet<long> verticalBordersToSkip)
		{
			foreach (ViewportPane viewportPane in updateContext.SheetViewport.ViewportPanes)
			{
				this.UpdateVerticalBordersForCellRange(updateContext, viewportPane, verticalBordersToSkip);
			}
		}

		private void UpdateVerticalBordersForCellRange(WorksheetRenderUpdateContext updateContext, ViewportPane viewportPane, HashSet<long> verticalBordersToSkip)
		{
			CellBordersLayerBase.c__DisplayClass24 CS_8__locals1 = new CellBordersLayerBase.c__DisplayClass24();
			CS_8__locals1.updateContext = updateContext;
			CellRange visibleRange = viewportPane.VisibleRange;
			int rowIndex3 = visibleRange.FromIndex.RowIndex;
			int columnIndex3 = visibleRange.FromIndex.ColumnIndex;
			int rowIndex2 = visibleRange.ToIndex.RowIndex;
			int columnIndex2 = visibleRange.ToIndex.ColumnIndex;
			int columnIndex;
			for (columnIndex = columnIndex3; columnIndex <= columnIndex2; columnIndex++)
			{
				CellBordersLayerBase.c__DisplayClass2a CS_8__locals3 = new CellBordersLayerBase.c__DisplayClass2a();
				CS_8__locals3.borders = new CompressedList<CellBorder>((long)rowIndex3, (long)rowIndex2, null);
				int rowIndex;
				for (rowIndex = rowIndex3; rowIndex <= rowIndex2; rowIndex++)
				{
					this.AddToBorders(CS_8__locals1.updateContext, delegate(CellBorder border)
					{
						CS_8__locals3.borders.SetValue((long)rowIndex, border);
					}, (CellRange mergedRange) => columnIndex < mergedRange.ToIndex.ColumnIndex, () => BorderRenderHelper.GetRightBorderToDisplay(CS_8__locals1.updateContext, rowIndex, columnIndex), verticalBordersToSkip.Contains(WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex)), rowIndex, columnIndex);
				}
				this.UpdateBordersInColumn(viewportPane.ViewportPaneType, columnIndex, CS_8__locals3.borders, CS_8__locals1.updateContext, (Rect boundingRect) => boundingRect.Right);
				if (columnIndex == columnIndex3 && this.shouldShowTopLeftMostBorders)
				{
					CS_8__locals3.borders = new CompressedList<CellBorder>((long)rowIndex3, (long)rowIndex2, null);
					for (rowIndex = rowIndex3; rowIndex <= rowIndex2; rowIndex++)
					{
						long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
						this.AddToBorders(CS_8__locals1.updateContext, delegate(CellBorder border)
						{
							CS_8__locals3.borders.SetValue((long)rowIndex, border);
						}, (CellRange mergedRange) => columnIndex >= mergedRange.FromIndex.ColumnIndex && columnIndex < mergedRange.ToIndex.ColumnIndex, () => CS_8__locals1.updateContext.GetLeftBorder(index), verticalBordersToSkip.Contains(index), rowIndex, columnIndex);
					}
					this.UpdateBordersInColumn(viewportPane.ViewportPaneType, columnIndex, CS_8__locals3.borders, CS_8__locals1.updateContext, (Rect boundingRect) => boundingRect.Left);
				}
			}
		}

		private void AddToBorders(WorksheetRenderUpdateContext updateContext, Action<CellBorder> addToBorders, Func<CellRange, bool> testIsInsideMergedRange, Func<CellBorder> getCellBorder, bool shouldSkip, int rowIndex, int columnIndex)
		{
			Worksheet worksheet = updateContext.Worksheet;
			bool flag = false;
			CellRange arg;
			if (worksheet.Cells.TryGetContainingMergedRange(rowIndex, columnIndex, out arg))
			{
				flag = testIsInsideMergedRange(arg);
			}
			if (!flag && !shouldSkip)
			{
				CellBorder cellBorder = getCellBorder();
				if (this.ShouldShowBorder(cellBorder))
				{
					addToBorders(cellBorder);
				}
			}
		}

		private HashSet<long> GetBordersToSkip(WorksheetRenderUpdateContext updateContext)
		{
			HashSet<long> hashSet = new HashSet<long>();
			foreach (ViewportPane viewportPane in updateContext.SheetViewport.ViewportPanes)
			{
				long[] bordersToSkipFromCellRange = this.GetBordersToSkipFromCellRange(viewportPane.VisibleRange, updateContext);
				hashSet.UnionWith(bordersToSkipFromCellRange);
			}
			return hashSet;
		}

		private long[] GetBordersToSkipFromCellRange(CellRange range, WorksheetRenderUpdateContext updateContext)
		{
			List<long> list = new List<long>();
			int rowIndex = range.FromIndex.RowIndex;
			int columnIndex = range.FromIndex.ColumnIndex;
			int rowIndex2 = range.ToIndex.RowIndex;
			int columnIndex2 = range.ToIndex.ColumnIndex;
			for (int i = rowIndex; i <= rowIndex2; i++)
			{
				for (int j = columnIndex; j <= columnIndex2; j++)
				{
					long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(i, j);
					CellLayoutBox visibleCellBox = updateContext.GetVisibleCellBox(index);
					if (visibleCellBox.MergeState != CellMergeState.NonTopLeftCellInMergedRange)
					{
						ICellValue cellValue = updateContext.GetCellValue(index);
						if (cellValue != EmptyCellValue.EmptyValue)
						{
							Rect rect = CellBoxRenderHelper.CalculateCellClippedBoundingRectangle(visibleCellBox, updateContext);
							rect.X += 0.1;
							rect.Y = visibleCellBox.Top;
							rect.Width = Math.Max(0.0, rect.Width - SpreadsheetDefaultValues.RightCellMargin);
							CellLayoutBox cellLayoutBox;
							CellLayoutBox cellLayoutBox2;
							updateContext.GetPreviousNextNonEmptyNonMergedCells(visibleCellBox.LongIndex, out cellLayoutBox, out cellLayoutBox2);
							for (int k = visibleCellBox.ColumnIndex - 1; k >= cellLayoutBox.ColumnIndex; k--)
							{
								CellLayoutBox visibleCellBox2 = updateContext.GetVisibleCellBox(i, k);
								if (!rect.IntersectsWith(visibleCellBox2.BoundingRectangle))
								{
									break;
								}
								list.Add(visibleCellBox2.LongIndex);
							}
							for (int l = visibleCellBox.ColumnIndex + 1; l <= cellLayoutBox2.ColumnIndex; l++)
							{
								CellLayoutBox visibleCellBox3 = updateContext.GetVisibleCellBox(i, l);
								if (!rect.IntersectsWith(visibleCellBox3.BoundingRectangle))
								{
									break;
								}
								list.Add(WorksheetPropertyBagBase.ConvertCellIndexToLong(i, l - 1));
							}
						}
					}
				}
			}
			return list.ToArray();
		}

		private void UpdateBordersInColumn(ViewportPaneType paneType, int columnIndex, ICompressedList<CellBorder> borders, WorksheetRenderUpdateContext updateContext, Func<Rect, double> getXCoordinate)
		{
			foreach (Range<long, CellBorder> range in borders)
			{
				if (!(range.Value == null))
				{
					CellLayoutBox visibleCellBox = updateContext.GetVisibleCellBox((int)range.Start, columnIndex);
					CellLayoutBox visibleCellBox2 = updateContext.GetVisibleCellBox((int)range.End, columnIndex);
					LineRenderable lineRenderable = new LineRenderable();
					CellBordersLayerBase.SetLinePropertiesForBorder(lineRenderable, range.Value, updateContext);
					double num = 0.0;
					CellBorder bottomBorderToDisplay;
					if (visibleCellBox.RowIndex > 0)
					{
						bottomBorderToDisplay = BorderRenderHelper.GetBottomBorderToDisplay(updateContext, visibleCellBox.RowIndex - 1, visibleCellBox.ColumnIndex);
						num = bottomBorderToDisplay.Thickness / 2.0;
					}
					bottomBorderToDisplay = BorderRenderHelper.GetBottomBorderToDisplay(updateContext, visibleCellBox2.RowIndex, visibleCellBox2.ColumnIndex);
					double num2 = bottomBorderToDisplay.Thickness / 2.0;
					lineRenderable.X1 = getXCoordinate(visibleCellBox.BoundingRectangle);
					lineRenderable.X2 = lineRenderable.X1;
					lineRenderable.Y1 = visibleCellBox.BoundingRectangle.Top - num;
					lineRenderable.Y2 = visibleCellBox2.BoundingRectangle.Bottom + num2;
					base.ContainerManager.Add(lineRenderable, paneType);
				}
			}
		}

		private void UpdateDiagonalBorders(WorksheetRenderUpdateContext updateContext)
		{
			foreach (ViewportPane viewportPane in updateContext.SheetViewport.ViewportPanes)
			{
				CellRange visibleRange = viewportPane.VisibleRange;
				int rowIndex = visibleRange.FromIndex.RowIndex;
				int columnIndex = visibleRange.FromIndex.ColumnIndex;
				int rowIndex2 = visibleRange.ToIndex.RowIndex;
				int columnIndex2 = visibleRange.ToIndex.ColumnIndex;
				for (int i = rowIndex; i <= rowIndex2; i++)
				{
					for (int j = columnIndex; j <= columnIndex2; j++)
					{
						CellLayoutBox visibleCellBox = updateContext.GetVisibleCellBox(i, j);
						if (visibleCellBox.MergeState != CellMergeState.NonTopLeftCellInMergedRange)
						{
							CellBorder diagonalUpBorder = updateContext.GetDiagonalUpBorder(visibleCellBox.LongIndex);
							if (diagonalUpBorder != CellBorder.Default)
							{
								LineRenderable lineRenderable = new LineRenderable();
								CellBordersLayerBase.SetLinePropertiesForBorder(lineRenderable, diagonalUpBorder, updateContext);
								lineRenderable.X1 = visibleCellBox.BoundingRectangle.Left;
								lineRenderable.X2 = visibleCellBox.BoundingRectangle.Right;
								lineRenderable.Y1 = visibleCellBox.BoundingRectangle.Bottom;
								lineRenderable.Y2 = visibleCellBox.BoundingRectangle.Top;
								base.ContainerManager.Add(lineRenderable, viewportPane.ViewportPaneType);
							}
							CellBorder diagonalDownBorder = updateContext.GetDiagonalDownBorder(visibleCellBox.LongIndex);
							if (diagonalDownBorder != CellBorder.Default)
							{
								LineRenderable lineRenderable2 = new LineRenderable();
								CellBordersLayerBase.SetLinePropertiesForBorder(lineRenderable2, diagonalDownBorder, updateContext);
								lineRenderable2.X1 = visibleCellBox.BoundingRectangle.Left;
								lineRenderable2.X2 = visibleCellBox.BoundingRectangle.Right;
								lineRenderable2.Y1 = visibleCellBox.BoundingRectangle.Top;
								lineRenderable2.Y2 = visibleCellBox.BoundingRectangle.Bottom;
								base.ContainerManager.Add(lineRenderable2, viewportPane.ViewportPaneType);
							}
						}
					}
				}
			}
		}

		public bool ShouldShowBorder(CellBorder border)
		{
			return this.shouldShowBorder(border);
		}

		private static void SetLinePropertiesForBorder(LineRenderable line, CellBorder border, WorksheetRenderUpdateContext updateContext)
		{
			line.Stroke = border.Color.GetActualValue(updateContext.CurrentTheme.ColorScheme);
			line.CellBorderStyle = border.Style;
			line.StrokeThickness = border.Thickness;
			int zindex = border.GetZIndex(updateContext.CurrentTheme.ColorScheme);
			line.ZIndex = zindex;
		}

		protected override void TranslateAndScale(WorksheetRenderUpdateContext updateContext)
		{
			ScaleTransform scaleTransform = new ScaleTransform
			{
				ScaleX = updateContext.ScaleFactor.Width,
				ScaleY = updateContext.ScaleFactor.Height
			};
			foreach (object obj in Enum.GetValues(typeof(ViewportPaneType)))
			{
				ViewportPaneType viewportPaneType = (ViewportPaneType)obj;
				foreach (IRenderable renderable in base.ContainerManager.GetElementsForViewportPane(viewportPaneType))
				{
					LineRenderable lineRenderable = (LineRenderable)renderable;
					Rect rect = this.Translate(lineRenderable, viewportPaneType, updateContext);
					Point point = new Point(rect.Left, rect.Top);
					point = scaleTransform.Transform(point);
					Point point2 = scaleTransform.Transform(new Point(rect.Width, rect.Height));
					rect.Width = point2.X;
					rect.Height = point2.Y;
					lineRenderable.X2 = point.X + (double)Math.Sign(lineRenderable.X2 - lineRenderable.X1) * rect.Width;
					lineRenderable.X1 = point.X;
					lineRenderable.Y2 = point.Y + (double)Math.Sign(lineRenderable.Y2 - lineRenderable.Y1) * rect.Height;
					lineRenderable.Y1 = point.Y;
					this.LineRenderer.Render(lineRenderable, viewportPaneType);
				}
			}
		}

		private Rect Translate(LineRenderable line, ViewportPaneType viewportPaneType, RenderUpdateContext updateContext)
		{
			Rect rect = new Rect(line.X1, line.Y1, Math.Abs(line.X2 - line.X1), Math.Abs(line.Y2 - line.Y1));
			return base.Translate(rect, viewportPaneType, updateContext);
		}

		[CompilerGenerated]
		private static double UpdateHorizontalBordersb__3(Rect boundingRect)
		{
			return boundingRect.Bottom;
		}

		[CompilerGenerated]
		private static double UpdateHorizontalBordersb__7(Rect boundingRect)
		{
			return boundingRect.Top;
		}

		[CompilerGenerated]
		private static double UpdateVerticalBordersForCellRangeb__1d(Rect boundingRect)
		{
			return boundingRect.Right;
		}

		[CompilerGenerated]
		private static double UpdateVerticalBordersForCellRangeb__21(Rect boundingRect)
		{
			return boundingRect.Left;
		}

		private readonly bool shouldShowTopLeftMostBorders;

		private readonly bool supportDiagonalBorders;

		private readonly string name;

		private readonly Func<CellBorder, bool> shouldShowBorder;

		private readonly IRenderer<LineRenderable> lineRenderer;

		[CompilerGenerated]
		private static Func<Rect, double> CS_9__CachedAnonymousMethodDelegate8;

		[CompilerGenerated]
		private static Func<Rect, double> CS_9__CachedAnonymousMethodDelegate9;

		[CompilerGenerated]
		private static Func<Rect, double> CS_9__CachedAnonymousMethodDelegate22;

		[CompilerGenerated]
		private static Func<Rect, double> CS_9__CachedAnonymousMethodDelegate23;

		[CompilerGenerated]
		private sealed class c__DisplayClassa
		{
			public c__DisplayClassa()
			{
			}

			public WorksheetRenderUpdateContext updateContext;
		}

		[CompilerGenerated]
		private sealed class c__DisplayClasse
		{
			public c__DisplayClasse()
			{
			}

			public bool UpdateHorizontalBordersb__1(CellRange mergedRange)
			{
				return this.rowIndex < mergedRange.ToIndex.RowIndex;
			}

			public bool UpdateHorizontalBordersb__5(CellRange mergedRange)
			{
				return this.rowIndex >= mergedRange.FromIndex.RowIndex && this.rowIndex < mergedRange.ToIndex.RowIndex;
			}

			public CellBordersLayerBase.c__DisplayClassa CS_8__localsb;

			public int rowIndex;
		}

		[CompilerGenerated]
		private sealed class c__DisplayClass10
		{
			public c__DisplayClass10()
			{
			}

			public ICompressedList<CellBorder> borders;
		}

		[CompilerGenerated]
		private sealed class c__DisplayClass14
		{
			public c__DisplayClass14()
			{
			}

			public void UpdateHorizontalBorders_b__0(CellBorder border)
			{
				this.CS_8__locals11.borders.SetValue((long)this.columnIndex, border);
			}

			public CellBorder UpdateHorizontalBorders_b__2()
			{
				return BorderRenderHelper.GetBottomBorderToDisplay(this.CS_8__localsb.updateContext, this.CS_8__localsf.rowIndex, this.columnIndex);
			}

			public CellBordersLayerBase.c__DisplayClass10 CS_8__locals11;

			public CellBordersLayerBase.c__DisplayClasse CS_8__localsf;

			public CellBordersLayerBase.c__DisplayClassa CS_8__localsb;

			public int columnIndex;
		}

		[CompilerGenerated]
		private sealed class c__DisplayClass18
		{
			public c__DisplayClass18()
			{
			}

			public void UpdateHorizontalBorders_b__4(CellBorder border)
			{
				this.CS_8__locals11.borders.SetValue((long)this.columnIndex, border);
			}

			public CellBorder UpdateHorizontalBorders_b__6()
			{
				return this.CS_8__localsb.updateContext.GetTopBorder(WorksheetPropertyBagBase.ConvertCellIndexToLong(this.CS_8__localsf.rowIndex, this.columnIndex));
			}

			public CellBordersLayerBase.c__DisplayClass10 CS_8__locals11;

			public CellBordersLayerBase.c__DisplayClasse CS_8__localsf;

			public CellBordersLayerBase.c__DisplayClassa CS_8__localsb;

			public int columnIndex;
		}

		[CompilerGenerated]
		private sealed class c__DisplayClass24
		{
			public c__DisplayClass24()
			{
			}

			public WorksheetRenderUpdateContext updateContext;
		}

		[CompilerGenerated]
		private sealed class c__DisplayClass28
		{
			public c__DisplayClass28()
			{
			}

			public bool UpdateVerticalBordersForCellRange_b__1b(CellRange mergedRange)
			{
				return this.columnIndex < mergedRange.ToIndex.ColumnIndex;
			}

			public bool UpdateVerticalBordersForCellRange_b__1f(CellRange mergedRange)
			{
				return this.columnIndex >= mergedRange.FromIndex.ColumnIndex && this.columnIndex < mergedRange.ToIndex.ColumnIndex;
			}

			public CellBordersLayerBase.c__DisplayClass24 CS_8__locals25;

			public int columnIndex;
		}

		[CompilerGenerated]
		private sealed class c__DisplayClass2a
		{
			public c__DisplayClass2a()
			{
			}

			public ICompressedList<CellBorder> borders;
		}

		[CompilerGenerated]
		private sealed class c__DisplayClass2e
		{
			public c__DisplayClass2e()
			{
			}

			public void UpdateVerticalBordersForCellRange_b__1a(CellBorder border)
			{
				this.CS_8__locals2b.borders.SetValue((long)this.rowIndex, border);
			}

			public CellBorder UpdateVerticalBordersForCellRange_b__1c()
			{
				return BorderRenderHelper.GetRightBorderToDisplay(this.CS_8__locals25.updateContext, this.rowIndex, this.CS_8__locals29.columnIndex);
			}

			public CellBordersLayerBase.c__DisplayClass2a CS_8__locals2b;

			public CellBordersLayerBase.c__DisplayClass28 CS_8__locals29;

			public CellBordersLayerBase.c__DisplayClass24 CS_8__locals25;

			public int rowIndex;
		}

		[CompilerGenerated]
		private sealed class c__DisplayClass31
		{
			public c__DisplayClass31()
			{
			}

			public void UpdateVerticalBordersForCellRange_b__1e(CellBorder border)
			{
				this.CS_8__locals2b.borders.SetValue((long)this.rowIndex, border);
			}

			public CellBordersLayerBase.c__DisplayClass2a CS_8__locals2b;

			public CellBordersLayerBase.c__DisplayClass28 CS_8__locals29;

			public CellBordersLayerBase.c__DisplayClass24 CS_8__locals25;

			public int rowIndex;
		}

		[CompilerGenerated]
		private sealed class c__DisplayClass33
		{
			public c__DisplayClass33()
			{
			}

			public CellBorder UpdateVerticalBordersForCellRange_b__20()
			{
				return this.CS_8__locals25.updateContext.GetLeftBorder(this.index);
			}

			public CellBordersLayerBase.c__DisplayClass31 CS_8__locals32;

			public CellBordersLayerBase.c__DisplayClass2a CS_8__locals2b;

			public CellBordersLayerBase.c__DisplayClass28 CS_8__locals29;

			public CellBordersLayerBase.c__DisplayClass24 CS_8__locals25;

			public long index;
		}
	}
}
