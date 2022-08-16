using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class CellValuesPdfLayer : PdfLayerBase
	{
		public CellValuesPdfLayer()
		{
			this.textRenderer = new PdfTextRenderer();
			this.cellValuesLayer = new CellValuesLayer(this.textRenderer);
		}

		public override string Name
		{
			get
			{
				return "CellValues";
			}
		}

		public override void UpdateRenderOverride(PdfPrintWorksheetRenderUpdateContext updateContext, FixedContentEditor editor)
		{
			this.textRenderer.Editor = editor;
			this.cellValuesLayer.UpdateRender(updateContext);
		}

		readonly PdfTextRenderer textRenderer;

		readonly CellValuesLayer cellValuesLayer;
	}
}
