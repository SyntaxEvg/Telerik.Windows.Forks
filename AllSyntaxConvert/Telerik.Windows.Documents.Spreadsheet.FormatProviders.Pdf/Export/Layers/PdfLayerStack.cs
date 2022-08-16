using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Core;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class PdfLayerStack : NamedObjectList<PdfLayerBase>
	{
		public void UpdateRender(PdfPrintWorksheetRenderUpdateContext updateContext, FixedContentEditor editor)
		{
			foreach (PdfLayerBase pdfLayerBase in this)
			{
				pdfLayerBase.UpdateRender(updateContext, editor);
			}
		}
	}
}
