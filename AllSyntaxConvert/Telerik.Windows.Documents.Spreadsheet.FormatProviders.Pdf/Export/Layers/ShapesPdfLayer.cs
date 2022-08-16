using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class ShapesPdfLayer : PdfLayerBase
	{
		public ShapesPdfLayer()
		{
			this.shapeRenderer = new PdfShapeRenderer();
			ShapeSourceFactory cache = new ShapeSourceFactory(new ImageSourceCache(), new ChartPdfSourceFactory());
			this.shapesLayer = new ShapesLayer(this.shapeRenderer, cache);
		}

		public override string Name
		{
			get
			{
				return "Shapes";
			}
		}

		public override void UpdateRenderOverride(PdfPrintWorksheetRenderUpdateContext updateContext, FixedContentEditor editor)
		{
			this.shapeRenderer.ChartRenderer = updateContext.ChartRenderer;
			this.shapeRenderer.Editor = editor;
			this.shapesLayer.UpdateRender(updateContext);
		}

		readonly PdfShapeRenderer shapeRenderer;

		readonly ShapesLayer shapesLayer;
	}
}
