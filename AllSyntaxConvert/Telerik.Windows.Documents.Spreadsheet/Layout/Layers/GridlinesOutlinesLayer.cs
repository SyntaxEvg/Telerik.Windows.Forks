using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class GridlinesOutlinesLayer : WorksheetLayerBase
	{
		public GridlinesOutlinesLayer(IRenderer<LineRenderable> lineRenderer)
		{
			this.lineRenderer = lineRenderer;
		}

		public override string Name
		{
			get
			{
				return "GridlinesOutline";
			}
		}

		protected override void UpdateRenderOverride(WorksheetRenderUpdateContext updateContext)
		{
			Rect rect = new Rect(0.0, 0.0, updateContext.SheetViewport.Width * updateContext.ScaleFactor.Width, updateContext.SheetViewport.Height * updateContext.ScaleFactor.Height);
			this.lineRenderer.Render(new LineRenderable
			{
				Stroke = Colors.Black,
				StrokeThickness = 2.0,
				X1 = rect.Left,
				Y1 = rect.Top,
				X2 = rect.Left,
				Y2 = rect.Bottom
			}, ViewportPaneType.Scrollable);
			this.lineRenderer.Render(new LineRenderable
			{
				Stroke = Colors.Black,
				StrokeThickness = 2.0,
				X1 = rect.Left,
				Y1 = rect.Top,
				X2 = rect.Right,
				Y2 = rect.Top
			}, ViewportPaneType.Scrollable);
			this.lineRenderer.Render(new LineRenderable
			{
				Stroke = Colors.Black,
				StrokeThickness = 2.0,
				X1 = rect.Right,
				Y1 = rect.Top,
				X2 = rect.Right,
				Y2 = rect.Bottom
			}, ViewportPaneType.Scrollable);
			this.lineRenderer.Render(new LineRenderable
			{
				Stroke = Colors.Black,
				StrokeThickness = 2.0,
				X1 = rect.Left,
				Y1 = rect.Bottom,
				X2 = rect.Right,
				Y2 = rect.Bottom
			}, ViewportPaneType.Scrollable);
		}

		readonly IRenderer<LineRenderable> lineRenderer;
	}
}
