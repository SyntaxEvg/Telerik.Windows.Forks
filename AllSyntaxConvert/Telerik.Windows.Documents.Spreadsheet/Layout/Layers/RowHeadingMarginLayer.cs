using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class RowHeadingMarginLayer : RowBasedLayerBase
	{
		public RowHeadingMarginLayer(IRenderer<RowColumnHeadingRenderable<RowLayoutBox>> rowHeadingRenderer)
		{
			this.rowHeadingRenderer = rowHeadingRenderer;
		}

		public override string Name
		{
			get
			{
				return "RowHeadingMargin";
			}
		}

		public double MaxWidth { get; set; }

		internal IRenderer<RowColumnHeadingRenderable<RowLayoutBox>> RowHeadingRenderer
		{
			get
			{
				return this.rowHeadingRenderer;
			}
		}

		internal static bool ShouldCreateRowHeadingElementForBox(ViewportPaneType viewportPaneType, WorksheetRenderUpdateContext updateContext)
		{
			SheetViewport sheetViewport = updateContext.SheetViewport;
			return (!sheetViewport[ViewportPaneType.Fixed].IsEmpty || !sheetViewport[ViewportPaneType.HorizontalScrollable].IsEmpty || sheetViewport[ViewportPaneType.VerticalScrollable].IsEmpty || viewportPaneType != ViewportPaneType.Scrollable) && (sheetViewport[ViewportPaneType.Fixed].IsEmpty || sheetViewport[ViewportPaneType.Scrollable].IsEmpty || (viewportPaneType != ViewportPaneType.HorizontalScrollable && viewportPaneType != ViewportPaneType.Scrollable));
		}

		protected override void UpdateRenderOverride(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			this.MaxWidth = SpreadsheetDefaultValues.RowColumnHeadingMinimumSize.Width;
			base.UpdateRenderOverride(worksheetUpdateContext);
		}

		protected override IRenderable CreateRenderableElementForBox(ViewportPaneType viewportPaneType, RowLayoutBox box, WorksheetRenderUpdateContext updateContext)
		{
			if (!RowHeadingMarginLayer.ShouldCreateRowHeadingElementForBox(viewportPaneType, updateContext))
			{
				return null;
			}
			CellRange visibleRange = updateContext.SheetViewport[viewportPaneType].VisibleRange;
			Size rowHeadingSize = updateContext.Layout.GetRowHeadingSize(visibleRange, box.RowIndex);
			this.MaxWidth = Math.Max(this.MaxWidth, rowHeadingSize.Width);
			Worksheet worksheet = updateContext.Worksheet;
			return new RowColumnHeadingRenderable<RowLayoutBox>
			{
				LayoutBox = box,
				Text = updateContext.Worksheet.HeaderNameRenderingConverter.ConvertRowIndexToName(new HeaderNameRenderingConverterContext(worksheet, visibleRange), box.RowIndex),
				Rectangle = new Rect(new Point(0.0, box.Top), rowHeadingSize)
			};
		}

		protected override void TranslateAndScale(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			foreach (object obj in Enum.GetValues(typeof(ViewportPaneType)))
			{
				ViewportPaneType viewportPaneType = (ViewportPaneType)obj;
				foreach (IRenderable renderable in base.ContainerManager.GetElementsForViewportPane(viewportPaneType))
				{
					RowColumnHeadingRenderable<RowLayoutBox> rowColumnHeadingRenderable = (RowColumnHeadingRenderable<RowLayoutBox>)renderable;
					Rect rect = base.Translate(rowColumnHeadingRenderable.Rectangle, viewportPaneType, worksheetUpdateContext);
					rowColumnHeadingRenderable.Rectangle = new Rect(0.0, rect.Top, this.MaxWidth, rect.Height);
					rowColumnHeadingRenderable.ScaleFactor = worksheetUpdateContext.ScaleFactor;
					this.rowHeadingRenderer.Render(rowColumnHeadingRenderable, viewportPaneType);
				}
			}
		}

		readonly IRenderer<RowColumnHeadingRenderable<RowLayoutBox>> rowHeadingRenderer;
	}
}
