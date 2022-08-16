using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Core;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	abstract class PdfLayerBase : INamedObject
	{
		public PdfLayerBase()
		{
			this.ShouldRender = true;
		}

		public abstract string Name { get; }

		public bool ShouldRender { get; set; }

		public virtual bool ClipToSheetViewport
		{
			get
			{
				return true;
			}
		}

		public void UpdateRender(PdfPrintWorksheetRenderUpdateContext updateContext, FixedContentEditor editor)
		{
			if (!this.ShouldRender)
			{
				return;
			}
			if (this.ClipToSheetViewport)
			{
				double width = updateContext.SheetViewport.Width * updateContext.ScaleFactor.Width;
				double height = updateContext.SheetViewport.Height * updateContext.ScaleFactor.Height;
				editor.PushTransformedClipping(new Rect(0.0, 0.0, width, height));
				this.UpdateRenderOverride(updateContext, editor);
				editor.PopClipping();
				return;
			}
			this.UpdateRenderOverride(updateContext, editor);
		}

		public abstract void UpdateRenderOverride(PdfPrintWorksheetRenderUpdateContext updateContext, FixedContentEditor editor);
	}
}
