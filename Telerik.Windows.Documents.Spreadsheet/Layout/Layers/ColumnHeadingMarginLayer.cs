using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class ColumnHeadingMarginLayer : ColumnBasedLayerBase
	{
		public ColumnHeadingMarginLayer(IRenderer<RowColumnHeadingRenderable<ColumnLayoutBox>> columnHeadingRenderer)
		{
			this.columnHeadingRenderer = columnHeadingRenderer;
		}

		public override string Name
		{
			get
			{
				return "ColumnHeadingMargin";
			}
		}

		public double MaxHeight { get; set; }

		internal IRenderer<RowColumnHeadingRenderable<ColumnLayoutBox>> ColumnHeadingRenderer
		{
			get
			{
				return this.columnHeadingRenderer;
			}
		}

		internal static bool ShouldCreateColumnHeadingElementForBox(ViewportPaneType viewportPaneType, WorksheetRenderUpdateContext updateContext)
		{
			SheetViewport sheetViewport = updateContext.SheetViewport;
			return (!sheetViewport[ViewportPaneType.Fixed].IsEmpty || !sheetViewport[ViewportPaneType.VerticalScrollable].IsEmpty || sheetViewport[ViewportPaneType.HorizontalScrollable].IsEmpty || viewportPaneType != ViewportPaneType.Scrollable) && (sheetViewport[ViewportPaneType.Fixed].IsEmpty || sheetViewport[ViewportPaneType.Scrollable].IsEmpty || (viewportPaneType != ViewportPaneType.VerticalScrollable && viewportPaneType != ViewportPaneType.Scrollable));
		}

		protected override void UpdateRenderOverride(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			this.MaxHeight = SpreadsheetDefaultValues.RowColumnHeadingMinimumSize.Height;
			base.UpdateRenderOverride(worksheetUpdateContext);
		}

		protected override IRenderable CreateRenderableElementForBox(ViewportPaneType viewportPaneType, ColumnLayoutBox box, WorksheetRenderUpdateContext updateContext)
		{
			if (!ColumnHeadingMarginLayer.ShouldCreateColumnHeadingElementForBox(viewportPaneType, updateContext))
			{
				return null;
			}
			Size columnHeadingSize = updateContext.Layout.GetColumnHeadingSize(box.ColumnIndex);
			this.MaxHeight = Math.Max(this.MaxHeight, columnHeadingSize.Height);
			Worksheet worksheet = updateContext.Worksheet;
			CellRange visibleRange = updateContext.SheetViewport[viewportPaneType].VisibleRange;
			return new RowColumnHeadingRenderable<ColumnLayoutBox>
			{
				LayoutBox = box,
				Text = updateContext.Worksheet.HeaderNameRenderingConverter.ConvertColumnIndexToName(new HeaderNameRenderingConverterContext(worksheet, visibleRange), box.ColumnIndex),
				Rectangle = new Rect(new Point(box.Left, 0.0), columnHeadingSize)
			};
		}

		protected override void TranslateAndScale(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			foreach (object obj in Enum.GetValues(typeof(ViewportPaneType)))
			{
				ViewportPaneType viewportPaneType = (ViewportPaneType)obj;
				foreach (IRenderable renderable in base.ContainerManager.GetElementsForViewportPane(viewportPaneType))
				{
					RowColumnHeadingRenderable<ColumnLayoutBox> rowColumnHeadingRenderable = (RowColumnHeadingRenderable<ColumnLayoutBox>)renderable;
					Rect rect = base.Translate(rowColumnHeadingRenderable.Rectangle, viewportPaneType, worksheetUpdateContext);
					rowColumnHeadingRenderable.Rectangle = new Rect(rect.Left, 0.0, rect.Width, this.MaxHeight);
					rowColumnHeadingRenderable.ScaleFactor = worksheetUpdateContext.ScaleFactor;
					this.columnHeadingRenderer.Render(rowColumnHeadingRenderable, viewportPaneType);
				}
			}
		}

		readonly IRenderer<RowColumnHeadingRenderable<ColumnLayoutBox>> columnHeadingRenderer;
	}
}
